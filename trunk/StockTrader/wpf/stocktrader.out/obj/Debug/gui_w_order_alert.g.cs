﻿#pragma checksum "..\..\gui_w_order_alert.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1FDBBB72A0972F15ADB3E12CB55B2B49"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
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




/// <summary>
/// w_order_alert
/// </summary>
public partial class w_order_alert : Sybase.PowerBuilder.WPF.Controls.Window, System.Windows.Markup.IComponentConnector {
    
    
    #line 2 "..\..\gui_w_order_alert.xaml"
    internal System.Windows.Controls.Canvas PBClientArea;
    
    #line default
    #line hidden
    
    
    #line 3 "..\..\gui_w_order_alert.xaml"
    internal Sybase.PowerBuilder.WPF.Controls.StaticText st_label;
    
    #line default
    #line hidden
    
    
    #line 4 "..\..\gui_w_order_alert.xaml"
    internal Sybase.PowerBuilder.WPF.Controls.PictureButton pb_ok;
    
    #line default
    #line hidden
    
    
    #line 5 "..\..\gui_w_order_alert.xaml"
    internal Sybase.PowerBuilder.WPF.Controls.DataWindow dw_result;
    
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
        System.Uri resourceLocater = new System.Uri("/stocktrader;component/gui_w_order_alert.xaml", System.UriKind.Relative);
        
        #line 1 "..\..\gui_w_order_alert.xaml"
        System.Windows.Application.LoadComponent(this, resourceLocater);
        
        #line default
        #line hidden
    }
    
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
    void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
        this.PBClientArea = ((System.Windows.Controls.Canvas)(target));
        return;
            case 2:
        this.st_label = ((Sybase.PowerBuilder.WPF.Controls.StaticText)(target));
        return;
            case 3:
        this.pb_ok = ((Sybase.PowerBuilder.WPF.Controls.PictureButton)(target));
        return;
            case 4:
        this.dw_result = ((Sybase.PowerBuilder.WPF.Controls.DataWindow)(target));
        return;
            }
        this._contentLoaded = true;
    }
}
