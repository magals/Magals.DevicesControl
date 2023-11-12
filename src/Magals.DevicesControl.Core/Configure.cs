namespace Magals.DevicesControl.Core
{
	/// <summary>
	/// The class is responsible for saving and reading the settings from the json file. 
	/// If the file is missing, it will be created with all the base configurations based on the drivers found
	/// </summary>
	public class Configure
	{
		public DevicesConfigModel DevicesConfigModel { get; set; } = new DevicesConfigModel();
		private readonly ILogger? logger;
		private readonly string pathFile;

		public Configure(ILogger? logger,
										 string pathFile)
		{
			this.logger = logger;
			this.pathFile = pathFile;
		}

		public void RemoveConfig(string nameSettingsDevice, string configName)
		{
			var setting = DevicesConfigModel.settingsdevices.First(x => string.Equals(nameSettingsDevice, x.name));
			var config = setting.configs.First(y => string.Equals(y.name, configName));
			setting.configs.Remove(config);
		}

		public void AddConfig(string nameSettingsDevice, SettingsDevices.Config config)
		{
			if(DevicesConfigModel.settingsdevices.Any(x => string.Equals(x.name, nameSettingsDevice)))
			{
				var settings = DevicesConfigModel.settingsdevices.First(x => string.Equals(x.name, nameSettingsDevice));
				if (settings.configs.Any(y => string.Equals(y.name, config.name)))
				{
					var t_index = settings.configs.FindIndex(x => string.Equals(x.name, config.name));
					settings.configs[t_index] = config;
				}
				else
				{
					settings.configs.Add(config);
				}
			}
			else
			{
				DevicesConfigModel.settingsdevices.Add(new SettingsDevices
				{
					enable = true,
					name = nameSettingsDevice,
					configs = new List<Config>
					{
						config
					}
				});
			}
		}

		public void ParseConfig()
		{
			try
			{
				using var stream = new FileStream(pathFile, FileMode.OpenOrCreate, FileAccess.Read);
				DevicesConfigModel = JsonSerializer.Deserialize<DevicesConfigModel>(stream) ?? new();
			}
			catch (Exception ex)
			{
				logger?.LogError("Exception:{ex} {Environment.NewLine} Trace:{ex.StackTrace}", ex, Environment.NewLine, ex.StackTrace);
			}
		}

		public void SaveConfigs()
		{
			using var sw = new StreamWriter(pathFile);
			var text = JsonSerializer.Serialize(DevicesConfigModel, new JsonSerializerOptions()
			{
				IgnoreReadOnlyProperties = true,
				WriteIndented = true,
			});

			sw.Write(text);
			sw.Flush();
		}


		public SettingsDevices? GetConfigsForDevice(string name) => DevicesConfigModel.settingsdevices.FirstOrDefault(x => x.name == name);
		public Config GetConfigByInstance(string name) => DevicesConfigModel.settingsdevices.First(x => x.configs.Any(y => y.name == name)).configs.First(y => y.name == name);

		public static Dictionary<string, TypeInfo> GetNameAllCustomAttributes()
		{
			var temp = AppDomain.CurrentDomain.GetAssemblies().First(x => x.GetName().Name == "Magals.DevicesControl.SDKStandart.dll").DefinedTypes
										 .Where(x => x.BaseType == typeof(Attribute) && x.Name.IndexOf("Settings") != -1)
										 .ToDictionary(x => $"{x.Name}".Replace("SettingsAttribute", string.Empty).ToLower());
			return temp;
		}


		public Type[] GetAllDeviceRoles()
		{
			var temp = AppDomain.CurrentDomain.GetAssemblies().First(x => x.GetName().Name == "Magals.DevicesControl.SDKStandart.dll").DefinedTypes
													.Where(x => x.ImplementedInterfaces.Any(y => y.Name == typeof(IDevice).Name))
													.ToArray();
			return temp;
		}
	}
}
