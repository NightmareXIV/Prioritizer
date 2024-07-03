using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace Prioritizer
{
    internal class Svc
    {
        [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; }
        [PluginService] internal static IClientState ClientState { get; private set; }
        [PluginService] internal static ICondition Condition { get; private set; }
        [PluginService] internal static IPluginLog Log { get; private set; }
    }
}