namespace Magals.DevicesControl.Core.Models
{
#pragma warning disable IDE1006
	/// <summary>
	/// Model for Serializing and Deserializing сonfigurations from json-file
	/// </summary>
	public class DevicesConfigModel
	{
		public List<Defaultlist> defaultlist { get; set; } = new();
		public class Defaultlist
		{
			public string? key { get; set; }
			public string? value { get; set; }
		}

		public List<SettingsDevices> settingsdevices { get; set; } = new();

		public class SettingsDevices
		{
			public string name { get; set; } = Guid.NewGuid().ToString();

			public List<Config> configs { get; set; } = new();

			public bool enable { get; set; } = false;

			public class Config
			{
				public Config()
				{
					name = string.Empty;
					config = new List<Config>();
				}

				[JsonConstructor]
				public Config(string name,
											bool enable,
											string? type_connect,
											object config,
											string protocol,
											bool autoscan,
											string description,
											List<Customsettings> customsettings)
				{
					this.name = name;
					this.enable = enable;
					this.type_connect = type_connect;
					this.config = ConvertJsonElementToAttribute(config);
					this.protocol = protocol;
					this.autoscan = autoscan;
					this.description = description;
					this.customsettings = customsettings;
				}

				private object ConvertJsonElementToAttribute(object __config)
				{
					object result = new();
					var namesCustromAttr = Configure.GetNameAllCustomAttributes();
					if (type_connect != null && namesCustromAttr.ContainsKey(type_connect))
					{
						var type_s = namesCustromAttr[type_connect].AsType();
						var types = type_s.GetProperties()
																					.Where(x => !string.Equals(x.Name, "TypeId"))
																					.Select((x) => x)
																					.ToArray();

						object[] objproperty = new object[types.Length];
						int index = 0;
						var defaultclass = Activator.CreateInstance(type_s);
						foreach (var property in types)
						{
							var pname = property.Name;
							var prequared = property.CustomAttributes.Any(x => x.AttributeType == typeof(RequiredAttribute));

							((JsonElement)__config).TryGetProperty(pname, out JsonElement jelemtn);
							//{
							//	throw new ArgumentNullException(pname, $"No required value for config:" +
							//																					 $"{name}-config-{pname}");
							//}

							object valueconfig = new();
							if (string.IsNullOrEmpty(jelemtn.ToString())
								&& defaultclass != null)
							{
								valueconfig = defaultclass.GetType()?
																					.GetProperty(pname)?
																					.GetValue(defaultclass, null) ?? new object();
							}
							else
							{
								switch (jelemtn.ValueKind)
								{
									case JsonValueKind.String: valueconfig = jelemtn.ToString(); break;
									case JsonValueKind.Number: valueconfig = Int32.Parse(jelemtn.ToString()); break;
									case JsonValueKind.Undefined:
										break;
									case JsonValueKind.Object:
										break;
									case JsonValueKind.Array:
											valueconfig = jelemtn.Deserialize <int[]>();

                    break;
									case JsonValueKind.True:
										break;
									case JsonValueKind.False:
										break;
									case JsonValueKind.Null:
										break;
									default:
										throw new InvalidCastException($"Can not convert object :{jelemtn}, " +
																													$"because type not defined::{jelemtn.ValueKind}");
								}
							}

							objproperty[index] = valueconfig;
							index++;

						}
						var attrCtorInfo = type_s.GetConstructor(types.Select((x) => x.PropertyType).ToArray());
						if (attrCtorInfo != null)
						{
							var objectconfig = attrCtorInfo.Invoke(objproperty);
							result = (Attribute)objectconfig;
						}
					}
					return result;
				}

				public string name { get; set; }

				public bool enable { get; set; } = false;

				public string? type_connect { get; set; }

				public object config { get; set; }

				public string protocol { get; set; } = string.Empty;

				public bool autoscan { get; set; } = false;

				public string description { get; set; } = string.Empty;

				public List<Customsettings> customsettings { get; set; } = new();
				public class Customsettings
				{
					public string key { get; set; } = Guid.NewGuid().ToString();
					public string value { get; set; } = string.Empty;
				}
			}
		}

	}
#pragma warning restore IDE1006 
}

