namespace Magals.DevicesControl.Core.Models
{
    public class InstanceAndConfig
    {
        public string Name => Instance?.GetType().Name ?? string.Empty;
        public string Group => Instance?.GetType().Name.Split("_")[0] ?? string.Empty;
        public object? Instance { get; set; }
        public Config? config { get; set; }
    }
}
