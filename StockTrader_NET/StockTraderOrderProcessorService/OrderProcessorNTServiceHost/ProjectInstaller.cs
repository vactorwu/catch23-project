using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace Trade.OrderProcessorNTServiceHost
{
    [RunInstaller(true)]
    public partial class ProjectOPSInstaller : System.Configuration.Install.Installer
    {
        public ProjectOPSInstaller()
        {
            InitializeComponent();
        }
    }
}
