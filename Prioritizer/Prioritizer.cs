using Dalamud.Game.ClientState.Conditions;
using Dalamud.Logging;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prioritizer
{
    class Prioritizer : IDalamudPlugin
    {
        public string Name => "Prioritizer";
        volatile bool run = true;

        public void Dispose()
        {
            run = false;
        }

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<Svc>();
            new Thread(() =>
            {
                var proc = Process.GetCurrentProcess();
                while (run)
                {
                    try
                    {
                        Thread.Sleep(2000);
                        proc.Refresh();
                        if (Svc.Condition[ConditionFlag.Crafting] == true)
                        {
                            if (proc.PriorityClass == ProcessPriorityClass.Normal) proc.PriorityClass = ProcessPriorityClass.High;
                            PluginLog.Debug("Setting priority to High");
                        }
                        else
                        {
                            if (proc.PriorityClass == ProcessPriorityClass.High) proc.PriorityClass = ProcessPriorityClass.Normal;
                            PluginLog.Debug("Setting priority to Normal");
                        }
                    }
                    catch (Exception e) 
                    {
                        PluginLog.Error($"Error: {e.Message}\n{e.StackTrace ?? ""}");
                    }
                }
                proc.PriorityClass = ProcessPriorityClass.Normal;
            }).Start();
        }
    }
}
