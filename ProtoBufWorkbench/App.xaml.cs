using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ProtoBufWorkbench.Properties;

namespace ProtoBufWorkbench
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (Settings.Default.RequiresUpgrade)
            {
                Settings.Default.Upgrade();
                Settings.Default.RequiresUpgrade = false;
                Settings.Default.Save();
            }
        }
    }
}
