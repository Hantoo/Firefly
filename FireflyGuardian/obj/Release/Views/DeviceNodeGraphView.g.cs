﻿#pragma checksum "..\..\..\Views\DeviceNodeGraphView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F69B3200AD18FC3936ACA398932A16D2DE113E6CA0B83C4140BC10B0ECA31AD2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FireflyGuardian.Views;
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


namespace FireflyGuardian.Views {
    
    
    /// <summary>
    /// DeviceNodeGraphView
    /// </summary>
    public partial class DeviceNodeGraphView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\Views\DeviceNodeGraphView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid container;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Views\DeviceNodeGraphView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas LayoutRootCanvas;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\Views\DeviceNodeGraphView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ScaleTransform st;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Views\DeviceNodeGraphView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.TranslateTransform translate;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\Views\DeviceNodeGraphView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button moveButton;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\Views\DeviceNodeGraphView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RefreshMap;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\Views\DeviceNodeGraphView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button FitToPage;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FireflyGuardian;component/views/devicenodegraphview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\DeviceNodeGraphView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.container = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.LayoutRootCanvas = ((System.Windows.Controls.Canvas)(target));
            
            #line 25 "..\..\..\Views\DeviceNodeGraphView.xaml"
            this.LayoutRootCanvas.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Canvas_MouseDown);
            
            #line default
            #line hidden
            
            #line 26 "..\..\..\Views\DeviceNodeGraphView.xaml"
            this.LayoutRootCanvas.MouseMove += new System.Windows.Input.MouseEventHandler(this.Canvas_MouseMove);
            
            #line default
            #line hidden
            
            #line 27 "..\..\..\Views\DeviceNodeGraphView.xaml"
            this.LayoutRootCanvas.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.Canvas_MouseUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.st = ((System.Windows.Media.ScaleTransform)(target));
            return;
            case 4:
            this.translate = ((System.Windows.Media.TranslateTransform)(target));
            return;
            case 5:
            this.moveButton = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\..\Views\DeviceNodeGraphView.xaml"
            this.moveButton.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Canvas_Drag);
            
            #line default
            #line hidden
            return;
            case 6:
            this.RefreshMap = ((System.Windows.Controls.Button)(target));
            
            #line 78 "..\..\..\Views\DeviceNodeGraphView.xaml"
            this.RefreshMap.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.refreshImageOnNodes);
            
            #line default
            #line hidden
            return;
            case 7:
            this.FitToPage = ((System.Windows.Controls.Button)(target));
            
            #line 112 "..\..\..\Views\DeviceNodeGraphView.xaml"
            this.FitToPage.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Canvas_FitToPage);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 123 "..\..\..\Views\DeviceNodeGraphView.xaml"
            ((System.Windows.Controls.Button)(target)).PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Canvas_ZoomIn);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 134 "..\..\..\Views\DeviceNodeGraphView.xaml"
            ((System.Windows.Controls.Button)(target)).PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Canvas_ZoomOut);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

