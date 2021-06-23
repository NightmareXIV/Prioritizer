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
        DalamudPluginInterface pi;
        volatile bool run = true;

        public void Dispose()
        {
            run = false;
            pi.Dispose();
        }

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            pi = pluginInterface;
            new Thread(() =>
            {
                var proc = Process.GetCurrentProcess();
                while (run)
                {
                    try
                    {
                        Thread.Sleep(3000);
                        if (pi.ClientState?.Condition[Dalamud.Game.ClientState.ConditionFlag.Crafting] == true)
                        {
                            if (proc.PriorityClass == ProcessPriorityClass.Normal) proc.PriorityClass = ProcessPriorityClass.High;
                        }
                        else
                        {
                            if (proc.PriorityClass == ProcessPriorityClass.High) proc.PriorityClass = ProcessPriorityClass.Normal;
                        }
                    }
                    catch (Exception) { }
                }
            }).Start();
        }
    }
}
