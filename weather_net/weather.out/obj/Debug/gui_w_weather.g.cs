﻿#pragma checksum "..\..\gui_w_weather.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9100C74956E834599585DB670F33F53B"
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




[System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
internal partial class w_weather : Sybase.PowerBuilder.WPF.Controls.Window, System.Windows.Markup.IComponentConnector {
    
    
    #line 2 "..\..\gui_w_weather.xaml"
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    internal System.Windows.Controls.Grid l_grid;
    
    #line default
    #line hidden
    
    
    #line 7 "..\..\gui_w_weather.xaml"
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    internal Sybase.PowerBuilder.WPF.Controls.DataWindow dw_province;
    
    #line default
    #line hidden
    
    
    #line 8 "..\..\gui_w_weather.xaml"
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    internal Sybase.PowerBuilder.WPF.Controls.DataWindow dw_city;
    
    #line default
    #line hidden
    
    
    #line 9 "..\..\gui_w_weather.xaml"
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    internal Sybase.PowerBuilder.WPF.Controls.DataWindow dw_weather;
    
    #line default
    #line hidden
    
    
    #line 10 "..\..\gui_w_weather.xaml"
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    internal Sybase.PowerBuilder.WPF.Controls.CommandButton cb_1;
    
    #line default
    #line hidden
    
    
    #line 11 "..\..\gui_w_weather.xaml"
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    internal Sybase.PowerBuilder.WPF.Controls.HProgressBar hpb_retrieve;
    
    #line default
    #line hidden
    
    
    #line 12 "..\..\gui_w_weather.xaml"
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    internal Sybase.PowerBuilder.WPF.Controls.CommandButton cb_2;
    
    #line default
    #line hidden
    
    
    #line 13 "..\..\gui_w_weather.xaml"
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    internal Sybase.PowerBuilder.WPF.Controls.CommandButton cb_3;
    
    #line default
    #line hidden
    
    
    #line 14 "..\..\gui_w_weather.xaml"
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    internal Sybase.PowerBuilder.WPF.Controls.HTrackBar htb_1;
    
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
        System.Uri resourceLocater = new System.Uri("/weather;component/gui_w_weather.xaml", System.UriKind.Relative);
        
        #line 1 "..\..\gui_w_weather.xaml"
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
        this.l_grid = ((System.Windows.Controls.Grid)(target));
        return;
            case 2:
        this.dw_province = ((Sybase.PowerBuilder.WPF.Controls.DataWindow)(target));
        return;
            case 3:
        this.dw_city = ((Sybase.PowerBuilder.WPF.Controls.DataWindow)(target));
        return;
            case 4:
        this.dw_weather = ((Sybase.PowerBuilder.WPF.Controls.DataWindow)(target));
        return;
            case 5:
        this.cb_1 = ((Sybase.PowerBuilder.WPF.Controls.CommandButton)(target));
        return;
            case 6:
        this.hpb_retrieve = ((Sybase.PowerBuilder.WPF.Controls.HProgressBar)(target));
        return;
            case 7:
        this.cb_2 = ((Sybase.PowerBuilder.WPF.Controls.CommandButton)(target));
        return;
            case 8:
        this.cb_3 = ((Sybase.PowerBuilder.WPF.Controls.CommandButton)(target));
        return;
            case 9:
        this.htb_1 = ((Sybase.PowerBuilder.WPF.Controls.HTrackBar)(target));
        return;
            }
        this._contentLoaded = true;
    }
}

