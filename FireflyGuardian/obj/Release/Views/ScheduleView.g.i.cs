﻿#pragma checksum "..\..\..\Views\ScheduleView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "41B1872CAD24578E6E691B3F749057E582EFF3D36BE2E09BC4867EAA4C2281A7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Caliburn.Micro;
using FireflyGuardian.ViewModels;
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
using System.Windows.Interactivity;
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
    /// ScheduleView
    /// </summary>
    public partial class ScheduleView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 133 "..\..\..\Views\ScheduleView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddRoutine;
        
        #line default
        #line hidden
        
        
        #line 155 "..\..\..\Views\ScheduleView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView routines;
        
        #line default
        #line hidden
        
        
        #line 349 "..\..\..\Views\ScheduleView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock SelectedRoutine_routineName;
        
        #line default
        #line hidden
        
        
        #line 359 "..\..\..\Views\ScheduleView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox DaySelector;
        
        #line default
        #line hidden
        
        
        #line 372 "..\..\..\Views\ScheduleView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddTimeSlot;
        
        #line default
        #line hidden
        
        
        #line 648 "..\..\..\Views\ScheduleView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddSelectedDeviceToRoutine;
        
        #line default
        #line hidden
        
        
        #line 693 "..\..\..\Views\ScheduleView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddMediaSlot;
        
        #line default
        #line hidden
        
        
        #line 696 "..\..\..\Views\ScheduleView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView MediaList;
        
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
            System.Uri resourceLocater = new System.Uri("/FireflyGuardian;component/views/scheduleview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ScheduleView.xaml"
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
            this.AddRoutine = ((System.Windows.Controls.Button)(target));
            return;
            case 2:
            this.routines = ((System.Windows.Controls.ListView)(target));
            return;
            case 5:
            this.SelectedRoutine_routineName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.DaySelector = ((System.Windows.Controls.ComboBox)(target));
            
            #line 362 "..\..\..\Views\ScheduleView.xaml"
            this.DaySelector.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.TimeSelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.AddTimeSlot = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.AddSelectedDeviceToRoutine = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.AddMediaSlot = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.MediaList = ((System.Windows.Controls.ListView)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 3:
            
            #line 178 "..\..\..\Views\ScheduleView.xaml"
            ((System.Windows.Controls.Border)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.Border_MouseEnter);
            
            #line default
            #line hidden
            break;
            case 4:
            
            #line 314 "..\..\..\Views\ScheduleView.xaml"
            ((System.Windows.Controls.Button)(target)).PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.TriggerSuggestedDelete_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            break;
            case 8:
            
            #line 385 "..\..\..\Views\ScheduleView.xaml"
            ((System.Windows.Controls.StackPanel)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.TimeSlot_MouseEnter);
            
            #line default
            #line hidden
            break;
            case 12:
            
            #line 716 "..\..\..\Views\ScheduleView.xaml"
            ((System.Windows.Controls.ComboBox)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ComboBox_Loaded);
            
            #line default
            #line hidden
            break;
            case 13:
            
            #line 838 "..\..\..\Views\ScheduleView.xaml"
            ((System.Windows.Controls.Button)(target)).PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.removeItem_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
