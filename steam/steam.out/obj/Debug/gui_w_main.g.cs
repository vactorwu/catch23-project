﻿#pragma checksum "..\..\gui_w_main.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "453272859A5F27C64C7AA3F25E91AC53"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Sybase.PowerBuilder.WPF.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace LYC.Steam.Gui {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    internal partial class w_main : Sybase.PowerBuilder.WPF.Controls.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 4 "..\..\gui_w_main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Sybase.PowerBuilder.WPF.Controls.Tab tab_1;
        
        #line default
        #line hidden
        
        
        #line 5 "..\..\gui_w_main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Sybase.PowerBuilder.WPF.Controls.TabPage tabpage_1;
        
        #line default
        #line hidden
        
        
        #line 6 "..\..\gui_w_main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Sybase.PowerBuilder.WPF.Controls.DataWindow dw_applist;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\gui_w_main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_input1;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\gui_w_main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Sybase.PowerBuilder.WPF.Controls.CommandButton cb_search;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\gui_w_main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Sybase.PowerBuilder.WPF.Controls.CommandButton cb_reset;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/steam;component/gui_w_main.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\gui_w_main.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.tab_1 = ((Sybase.PowerBuilder.WPF.Controls.Tab)(target));
            return;
            case 2:
            this.tabpage_1 = ((Sybase.PowerBuilder.WPF.Controls.TabPage)(target));
            return;
            case 3:
            this.dw_applist = ((Sybase.PowerBuilder.WPF.Controls.DataWindow)(target));
            return;
            case 4:
            this.txt_input1 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.cb_search = ((Sybase.PowerBuilder.WPF.Controls.CommandButton)(target));
            return;
            case 6:
            this.cb_reset = ((Sybase.PowerBuilder.WPF.Controls.CommandButton)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
