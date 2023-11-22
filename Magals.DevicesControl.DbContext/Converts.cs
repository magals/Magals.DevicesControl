using Magals.DevicesControl.DbContext.DTOs;
using Magals.DevicesControl.DbContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext;
public static class Converts
{
    public static T ConvertDTOtoEntity<T>(object dto)
    {
        T result = default!;
        if (dto is SettingsDevicesDTO _sddto)
        {
            var configs = new List<ConfigsEntity>();
            foreach (var config in _sddto.configs)
            {
                configs.Add(new ConfigsEntity
                {
                    Name = config.name,
                    Enable = config.enable,
                    Description = config.description,
                    Autoscan = config.autoscan,
                    Protocol = config.protocol,
                    Type_Connect = config.type_connect,
                    Config = JsonSerializer.Serialize(config.config)
                });

                var custtomsettings = new List<CustomsettingsEntity>();
                if (config.customsettings != null)
                    foreach (var item in config.customsettings)
                    {
                        custtomsettings.Add(new CustomsettingsEntity
                        {
                            Key = item.key,
                            Value = item.value,
                        });
                    }
                configs.Last().Customsettings = custtomsettings;
            }

            var tempresult = (new SettingsDevicesEntity
            {
                Name = _sddto.name,
                Enable = _sddto.enable,
                Configs = configs
            });

            return (T)Convert.ChangeType(tempresult, typeof(T));
        }

        if (dto is ConfigsDTO _cdto)
        {
            var custtomsettings = new List<CustomsettingsEntity>();
            if (_cdto.customsettings != null)
                foreach (var item in _cdto.customsettings)
                {
                    custtomsettings.Add(new CustomsettingsEntity
                    {
                        Key = item.key,
                        Value = item.value,
                    });
                }

            var tempresult = new ConfigsEntity()
            {
                Name = _cdto.name,
                Enable = _cdto.enable,
                Description = _cdto.description,
                Autoscan = _cdto.autoscan,
                Protocol = _cdto.protocol,
                Type_Connect = _cdto.type_connect,
                Config = JsonSerializer.Serialize(_cdto.config),
                Customsettings = custtomsettings
            };
            return (T)Convert.ChangeType(tempresult, typeof(T));
        }

        if (dto is CustomsettingsDTO _csdto)
        {
            var tempresult = new CustomsettingsEntity
            {
                Key = _csdto.key,
                Value = _csdto.value,
            };
            return (T)Convert.ChangeType(tempresult, typeof(T));
        }

        throw new Exception("Unknow type argument:" + nameof(dto));
    }
    public static object? ConvertEntitytoDTO(object entity)
    {
        object result = default!;

        if (entity is SettingsDevicesEntity _sdentity)
        {
            var configstemp = new List<ConfigsDTO>();
            if(_sdentity.Configs != null &&
               _sdentity.Configs.Any())
            {
                foreach (var item in _sdentity.Configs)
                {
                    var temp = new List<CustomsettingsDTO>();
                    if (item.Customsettings != null && item.Customsettings.Any())
                    {
                        foreach (var customsettings in item.Customsettings)
                        {
                            temp.Add(new CustomsettingsDTO
                            {
                                key = customsettings.Key,
                                value = customsettings.Value,
                            });
                        }
                    }

                    configstemp.Add(new ConfigsDTO
                    {
                        name = item.Name,
                        enable = item.Enable,
                        autoscan = item.Autoscan,
                        description = item.Description,
                        protocol = item.Protocol,
                        type_connect = item.Type_Connect,
                        config = JsonSerializer.Deserialize<JsonElement>(item.Config),
                        customsettings = temp
                    });
                }
            }

            result = new SettingsDevicesDTO
            {
                name = _sdentity.Name,
                enable =  _sdentity.Enable,
                configs = configstemp
            };
        }

        if (entity is ConfigsEntity _centity)
        {
            var temp = new List<CustomsettingsDTO>();

            if (_centity.Customsettings  != null && _centity.Customsettings.Any())
            {
                foreach (var item in _centity.Customsettings)
                {
                    temp.Add(new CustomsettingsDTO
                    { 
                        key = item.Key,
                        value = item.Value,
                    });
                }
            }

            result = new ConfigsDTO
            {
                name = _centity.Name,
                enable = _centity.Enable,
                autoscan = _centity.Autoscan,
                description = _centity.Description,
                protocol = _centity.Protocol,
                type_connect = _centity.Type_Connect,
                config = JsonSerializer.Deserialize<JsonElement>(_centity.Config),
                customsettings = temp
            };
        }

        if (entity is CustomsettingsEntity _csentity)
        {
            result = new CustomsettingsDTO
            {
                key = _csentity.Key,
                value = _csentity.Value,
            };
        }

        return result;
    }
}
