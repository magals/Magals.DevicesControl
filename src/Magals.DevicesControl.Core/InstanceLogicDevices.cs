namespace Magals.DevicesControl.Core
{
	/// <summary>
	/// The main logic of loading dll and creating an instance based on SettingsDevices.Config
	/// </summary>
	public sealed class InstanceLogicDevices : IDisposable
	{
		private ILogger? logger;
		private readonly string pathfolderdrivers;

		private bool _createDefaultSettings = false;
		private bool _disposedValue;
		private Configure _configure = new(null, string.Empty);
		private List<IDevice> _instances = new();
		private AssemblyBuilder? _assemblyBuilder;
		private ModuleBuilder? _moduleBuilder;

		public delegate void SignalProblemsHandle(LoggerEventArgs mesg);
		public event SignalProblemsHandle? Notify;
		public List<IDevice> Instances { get => _instances; }
		public Configure Configure { get => _configure;}
		public bool CreateDefaultSettings { get => _createDefaultSettings; set => _createDefaultSettings = value; }

		public InstanceLogicDevices(string pathconfig = "DriversConfig.json", 
														string pathfolderdrivers = "drivers", 
														ILogger<InstanceLogicDevices>? logger = null)
		{
			try
			{
				_configure = new Configure(logger, pathconfig);
				this.logger = logger;
				if (!Directory.Exists(pathfolderdrivers))
				{
					Directory.CreateDirectory(pathfolderdrivers);
				}

				if (!File.Exists(pathconfig))
				{
					CreateDefaultSettings = true;
					logger?.LogWarning("File config: {pathconfig} not exist", pathconfig);
				}

				//if (fileStream.Length == 0)
				//{
				//	createDefaultSettings = true;
				//	logger?.LogWarning($"File config: {pathconfig} is empty");
				//}
			}
			catch (Exception ex)
			{
				var msg = new LoggerEventArgs(LogLevel.Error, $"Exception:{ex}{Environment.NewLine}" +
																	$"StackTrace:{ex.StackTrace}");

				this.logger?.LogError("Exception:{ex}", ex);
				Notify?.Invoke(msg);
			}

			this.pathfolderdrivers = pathfolderdrivers;
		}

		public void AddILogger(ILogger? logger)
		{
			this.logger = logger;
		}

		public void ParseConfig()
		{
			if (!CreateDefaultSettings)
			{
				_configure.ParseConfig();
			}
		}


		public static string RemoveSpecChars(string word)
		{
			return word.Replace("+", string.Empty)
				.Replace("&&", string.Empty)
				.Replace("||", string.Empty)
				.Replace("!", string.Empty)
				.Replace("(", string.Empty)
				.Replace(")", string.Empty)
				.Replace("{", string.Empty)
				.Replace("}", string.Empty)
				.Replace("[", string.Empty)
				.Replace("]", string.Empty)
				.Replace("^", string.Empty)
				.Replace("~", string.Empty)
				.Replace("*", string.Empty)
				.Replace("?", string.Empty)
				.Replace(":", string.Empty)
				.Replace("\\", string.Empty)
				.Replace("\"", string.Empty)
				.Replace("/", string.Empty);
		}

		public T GetNameDefaultInstance<T>()
		where T : IDevice
		{
			try
			{
				if (_configure.DevicesConfigModel.defaultlist.Any((x) => x.key != null && x.key.ToLower() == typeof(T).Name.ToLower()))
				{
					var name = _configure.DevicesConfigModel.defaultlist.First(x => x.key != null && string.Equals(x.key.ToLower(), typeof(T).Name.ToLower()));
					return GetAllInstancesByInterface<T>().First(x =>
					{
						if(x == null)
						{
							return false;
						}
						return string.Equals(x.GetType().Name, name.value);
					});
				}
				throw new KeyNotFoundException($"Not Found default {typeof(T).Name.ToLower()}" );
			}
			catch (Exception ex)
			{
				logger?.LogCritical("{ex}", ex);
				throw;
			}
		}

		public void CreateInstance()
		{
			try
			{
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					foreach (Type type in assembly.GetTypes())
					{
						try
						{

							var attribs = type.GetCustomAttributes(typeof(DriverAttribute), false);
							if (attribs != null && attribs.Length > 0)
							{
								var drvobj = (DriverAttribute)attribs.First();
								var drvcon = _configure.GetConfigsForDevice(RemoveSpecChars(drvobj.name));
								if (drvcon != null) // create all drivers by configs
								{
									foreach (var config in drvcon.configs)
									{
										CreateInstanceForConfig(type, config);
									}
								}

								//create default SettingsDevices for template config
								if (drvcon == null && CreateDefaultSettings)
								{
									var listsettings = type.GetCustomAttributes()
															 .Where(x => x.GetType() != typeof(DriverAttribute) &&
																									 x.GetType() != typeof(CustomSettingsAttribute))
															 .ToList();

									SettingsDevices settingsDevices = new()
									{
										name = drvobj.name,
										configs = new List<Config>()
									};

									foreach (var item in listsettings)
									{
										Config config = new()
										{
											name = "ExampleDefaultTemplate",
											type_connect = item.GetType().Name.ToLower().Replace("settingsattribute", string.Empty),
											config = item,
											customsettings = new ()
										};

										_configure.AddConfig(settingsDevices.name, config);
										settingsDevices.configs.Add(config);
									}

									if (settingsDevices.configs.Any())
									{
										_configure.SaveConfigs();
									}
								}
							}
						}
						catch (Exception ex)
						{
							logger?.LogCritical("{ex}", ex);
						}
					}
				}
			}
			catch (Exception ex)
			{
				logger?.LogCritical("{ex}", ex);
				//throw;
			}
		}

		public T GetInstanceByNameConfig<T>(string nameDevice, string nameConfig)
		where T : IDevice
		{
			return (T)_instances.First((x) =>
			{
				bool r_quals = false;
				if (x.GetType().GetCustomAttribute(typeof(DriverAttribute)) is DriverAttribute driverAttribute &&
						x.GetType().GetCustomAttribute(typeof(ConfigNameAttribute)) is ConfigNameAttribute configNameAttribute)
				{
					r_quals = string.Equals(driverAttribute.name, nameDevice) &&
										string.Equals(configNameAttribute.name, nameConfig) &&
										x.GetType().GetInterface(typeof(T).Name) != null;
				}

				return r_quals;
			});
		}
		public void LoadAllDrivers()
		{

			string[] pathdrivers = Directory.GetFiles(pathfolderdrivers, "*.dll", SearchOption.AllDirectories);
			if (!pathdrivers.Any())
			{
				throw new DirectoryNotFoundException($"No files found in:{pathfolderdrivers} directory");
			}

			var devicesdkStandart = new AssemblyLoadContext(name: "temp", isCollectible: true);
			var current = typeof(InstanceLogicDevices).Assembly.GetReferencedAssemblies()
																																.First(x => x.Name == "Magals.DevicesControl.SDKStandart");
			List<FileInfo> fileInfos = new();
			foreach (var file in pathdrivers)
			{
				FileInfo info = new(file);
				fileInfos.Add(info);
			}

			foreach (var item in fileInfos)
			{
				var context = new AssemblyLoadContext(name: item.Name, isCollectible: true);
				try
				{
					Assembly assembly = context.LoadFromAssemblyPath(item.FullName);
					// if true need unload
					bool FlagUnload = true;
					foreach (var item_assembly in assembly.GetReferencedAssemblies())
					{
						if (current.Version != null && item_assembly.Version != null)
						{
							var check = Equals(current.Name, item_assembly.Name) &&
								current.Version.Major == item_assembly.Version.Major;
							if (check)
							{
								FlagUnload = false;
								break;
							}
						}
					}


					if (FlagUnload)
					{ context.Unload(); }
				}
				catch (Exception ex)
				{
					logger?.LogWarning("{ex}", ex);
					context.Unload();
				}
			}
			devicesdkStandart.Unload();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}


		public DriverAttribute GetNameInstance(object instance)
		{
			var da = instance.GetType().GetCustomAttribute(typeof(DriverAttribute));
			if (da is DriverAttribute result)
			{
				return result;
			}
			throw new InvalidOperationException("Object is not DriverAttribute");
		}


		public SettingsDevices? GetSettingsByDevice(object instance)
		{
			var device = instance.GetType().GetCustomAttribute(typeof(DriverAttribute));
			ArgumentNullException.ThrowIfNull(device);
			return _configure.GetConfigsForDevice(((DriverAttribute)device).name);
		}

		public Config GetConfigByInstance(object instance)
		{
			var device = instance.GetType().GetCustomAttribute(typeof(ConfigNameAttribute));
			if (device is ConfigNameAttribute result)
			{
				return _configure.GetConfigByInstance(result.name);
			}
			throw new InvalidOperationException("Not found instance");
		}

		public List<InstanceAndConfig> GetAllConfigWithRoleT<T>()
		{
			var result = new List<InstanceAndConfig>();
			var allinstance = GetAllInstancesByInterface<T>();
			foreach (var instance in allinstance)
			{
				if (instance != null)
				{
					var config = GetConfigByInstance(instance);
					result.Add(new InstanceAndConfig
					{
						Instance = instance,
						config = config
					});
				}
			}
			return result;
		}

		public object[] GetRolesByDevice(object instance) => instance.GetType().GetInterfaces();

		public Type[] GetAllTypeSettings() => Configure.GetNameAllCustomAttributes()
														 .Select((x) => x.Value.AsType())
														 .Where(x => x.Name.IndexOf("CustomSettings") == -1)
														 .ToArray();

		public Type[] GetAllDeviceRoles() => _configure.GetAllDeviceRoles();


		public void SetConfigByInstance(object instance, Config config)
		{
			var tempType = instance.GetType().BaseType;
			ArgumentNullException.ThrowIfNull(tempType);

			Instances.Remove((IDevice)instance);
			CreateInstanceForConfig(tempType, config);

			if (tempType.GetCustomAttribute(typeof(DriverAttribute)) is DriverAttribute driverAttribute)
			{
				_configure.RemoveConfig(tempType.Name, config.name);
				_configure.AddConfig(tempType.Name, config);
				_configure.SaveConfigs();
			}
		}

		public void CreateInstanceForConfig(Type type, Config config)
		{
			var uniqueIdentifier = $"{RemoveSpecChars(type.Name)}_{RemoveSpecChars(config.name)}";
			var assemblyName = new AssemblyName(uniqueIdentifier);

			_assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
			_moduleBuilder = _assemblyBuilder.DefineDynamicModule(uniqueIdentifier);
			TypeBuilder _typeBuilder = _moduleBuilder.DefineType(uniqueIdentifier, TypeAttributes.Public);
			_typeBuilder.SetParent(type);

			var types = config.config.GetType()
									 .GetProperties()
									 .Where(x => !string.Equals(x.Name, "TypeId"))
									 .ToArray();

			object[] objproperty = new object[types.Length];
			int index = 0;
			foreach (var item in types)
			{
				objproperty[index] = config.config.GetType().GetProperty(item.Name)?.GetValue(config.config) ?? string.Empty;
				index++;
			}

			var attrCtorInfo = config.config.GetType().GetConstructor(types.Select((x) => x.PropertyType).ToArray());
			if (attrCtorInfo != null)
			{
				var attrBuilder = new CustomAttributeBuilder(attrCtorInfo, objproperty);
				_typeBuilder.SetCustomAttribute(attrBuilder);

				//CustomSettingsAttribute
				if (config.customsettings != null)
				{
					//array is object pair "Key"-"Value"
					var attrCtorInfo2 = typeof(CustomSettingsAttribute).GetConstructor(new Type[] { typeof(object[]) });

					List<object> asd = new();
					foreach (var item in config.customsettings)
					{
						asd.Add(item.key);
						asd.Add(item.value);
					}

					if (attrCtorInfo2 != null)
					{
						var customattr = new CustomAttributeBuilder(attrCtorInfo2, new object[] { asd.ToArray() });
						_typeBuilder.SetCustomAttribute(customattr);
					}
				}

				//Set ConfigName
				var confignameconstruct = typeof(ConfigNameAttribute).GetConstructor(new Type[] { config.name.GetType(),
																									config.enable.GetType() });
				if (confignameconstruct != null)
				{
					var configname = new CustomAttributeBuilder(confignameconstruct, new object[] { config.name,
																								config.enable});
					_typeBuilder.SetCustomAttribute(configname);

					var ti = _typeBuilder.CreateTypeInfo();
					if (ti != null)
					{
						var tempobj = Activator.CreateInstance(ti);
						if (tempobj != null)
						{
							var idevice = (IDevice)tempobj;
							idevice.LogMessage += MainLogic_LogMessage;
							if(!Instances.Any(x => string.Equals(x.GetType().Name, idevice.GetType().Name)))
								Instances.Add(idevice);
						}
					}
				}
			}
		}

		private void MainLogic_LogMessage(object? sender, LoggerEventArgs e)
		{
			logger?.Log(e.eventType, "Message:{e.message}{Environment.NewLine}", e.message, Environment.NewLine);
			if(e.eventType >= LogLevel.Warning)
			{
				Notify?.Invoke(e);
			}
		}

		
		private List<T> GetAllInstancesByInterface<T>() => _instances.Where(h => h.GetType()
																	 .GetInterfaces()
																     .Contains(typeof(T)))
																	 .Cast<T>()
																	 .ToList();

		

		public void Dispose() => Dispose(true);

		private void Dispose(bool _)
		{
			if (!_disposedValue)
			{
				_disposedValue = true;
				foreach (var item in Instances)
				{
					try
					{
                        item.Dispose();
                    }
					catch (Exception ex)
					{
                        logger?.LogCritical(ex, "Message:{ex.message}{Environment.NewLine}", ex.ToString(), Environment.NewLine);
                    }
					
				}
			}
		}
	}

}
