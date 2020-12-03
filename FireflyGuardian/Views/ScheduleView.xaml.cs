using FireflyGuardian.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FireflyGuardian.Views
{
    /// <summary>
    /// Interaction logic for ScheduleView.xaml
    /// </summary>
    /// 
    public delegate void NotifyDelete();
    public delegate void NotifyNewHover();
    public delegate void NotifyTimeSlotHover();
    public delegate void NotifyDateTimeRefresh();
    public partial class ScheduleView : UserControl
    {
        public static event NotifyDelete DeletionEvent;
        public static event NotifyDateTimeRefresh DateTimeRefreshEvent;
        public static Action<object> HoverChanged;
        public static Action<object> TimeSlotHoverChanged;
        public ScheduleView()
        {
            InitializeComponent();
        }

        public int hoveredIndex = -1;

        private void TriggerSuggestedDelete_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
     
            DeletionEvent.Invoke();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            HoverChanged.Invoke(sender);
        }
        private void TimeSlot_MouseEnter(object sender, MouseEventArgs e)
        {
            TimeSlotHoverChanged.Invoke(sender);
        }
 

        void removeItem_Click(object sender, RoutedEventArgs e)
        {
            object i = ((FrameworkElement)sender).DataContext;
            (this.DataContext as ScheduleViewModel)?.RemoveItem(i);
        }

        private void TimeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTimeRefreshEvent.Invoke();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            cmb.ItemsSource = FireflyGuardian.ViewModels.ScheduleViewModel.localmediaslots;
        }
    }
}
