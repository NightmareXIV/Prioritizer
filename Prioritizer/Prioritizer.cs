﻿using Dalamud.Game.ClientState.Conditions;
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
        volatile bool set = false;

        public void Dispose()
        {
            run = false;
        }

        public Prioritizer(DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<Svc>();
            PluginLog.Debug("Prioritizer loaded");
            new Thread(() =>
            {
                PluginLog.Debug("Begin");
                var proc = Process.GetCurrentProcess();
                while (run)
                {
                    try
                    {
                        Thread.Sleep(2000);
                        proc.Refresh();
                        if (Svc.Condition[ConditionFlag.Crafting] == true)
                        {
                            if (proc.PriorityClass == ProcessPriorityClass.Normal)
                            {
                                proc.PriorityClass = ProcessPriorityClass.High;
                                PluginLog.Debug("Setting priority to High");
                                set = true;
                            }
                        }
                        else
                        {
                            if (proc.PriorityClass == ProcessPriorityClass.High && set)
                            {
                                proc.PriorityClass = ProcessPriorityClass.Normal;
                                PluginLog.Debug("Setting priority to Normal");
                                set = false;
                            }
                        }
                    }
                    catch (Exception e) 
                    {
                        PluginLog.Error($"Error: {e.Message}\n{e.StackTrace ?? ""}");
                    }
                }
                proc.PriorityClass = ProcessPriorityClass.Normal;
                PluginLog.Debug("End");
            }).Start();
        }
    }
}
