using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace Trade.BusinessServiceNTServiceHost
{
    [RunInstaller(true)]
    public partial class ProjectBSLInstaller : System.Configuration.Install.Installer
    {
        public ProjectBSLInstaller()
        {
            InitializeComponent();
        }
    }
}
