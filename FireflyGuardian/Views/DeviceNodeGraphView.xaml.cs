using FireflyGuardian.ServerResources;
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
    /// Interaction logic for DeviceNodeGraphView.xaml
    /// </summary>
    
    //ToDo: Look back into node draging for repositioning.

    public delegate void NotifyCanvasDragToggle();  // delegate
    public delegate void NotifyNodeDragToggle();  // delegate
    public delegate void NotifyCanvasNodeSelected();  // delegate
    public partial class DeviceNodeGraphView : UserControl
    {
        //ToDo: Implement Zoom Functionality For Graph
        

        public static event NotifyCanvasDragToggle UserToggledCanvasDrag;
        public static event NotifyNodeDragToggle UserToggledNodeDrag;
        public static event Action<int> UserSelectedNode;
        public int scaleFactor = 60;
        private bool canvasDrag = false;
        private bool nodeDrag = false;
        private Point clickPosition;
        private Double prevX, prevY;

        public DeviceNodeGraphView()
        {
            InitializeComponent();
            FireflyGuardian.ViewModels.DeviceNetworkViewModel.RefreshCanvas += refreshGraph; 
            FireflyGuardian.ViewModels.DeviceNetworkViewModel.UserChangedNode += changeSelectedNode; 

            refreshGraph();
        }

        
        
        public void refreshGraph()
        {
            //Set Blocks
            LayoutRootCanvas.Children.Clear();

            for (int i =0; i < ServerManagement.devices.Count; i++)
            {
                var grid = new Grid();
                
                Rectangle rect = new Rectangle
                {
                    Stroke = Brushes.LightBlue,
                    StrokeThickness = 2
                };
                rect.Width = 1* scaleFactor;
                rect.Height = 1* scaleFactor;

                rect.Fill = Brushes.Red;
                grid.Children.Add(rect);
                TextBlock text = new TextBlock();
                text.Text = ServerManagement.devices[i].deviceID.ToString();
                rect.Name = "Node" + ServerManagement.devices[i].deviceID.ToString() + "_rect";
                RegisterName(rect.Name, rect);
               
                text.FontSize = 1;
                grid.Children.Add(text);
                grid.Name = "Node" + ServerManagement.devices[i].deviceID.ToString();
                grid.Focusable = true;
                RegisterName(grid.Name, grid);
                /*grid.PreviewMouseLeftButtonDown += nodeClicked;
                grid.MouseMove += Node_MouseMove;
                //grid.MouseUp += Node_MouseUp;
                grid.PreviewMouseLeftButtonUp += Node_MouseUp;*/
                Canvas.SetLeft(grid, ServerManagement.devices[i].deviceXZLocationOnGrid.Item1* scaleFactor);
                Canvas.SetTop(grid, ServerManagement.devices[i].deviceXZLocationOnGrid.Item2* scaleFactor);

                LayoutRootCanvas.Children.Add(grid);
            }

            //Add Connection Lines
            for (int mainNodeIdx = 0; mainNodeIdx < ServerManagement.devices.Count; mainNodeIdx++)
            {
                for (int nodeConnectionIdx = 0; nodeConnectionIdx < ServerManagement.devices[mainNodeIdx].deviceConnectionOutputIds.Count; nodeConnectionIdx++)
                {
                    for (int compareIdx = 0; compareIdx < ServerManagement.devices.Count; compareIdx++)
                    {
                        if(ServerManagement.devices[compareIdx].deviceID == ServerManagement.devices[mainNodeIdx].deviceConnectionOutputIds[nodeConnectionIdx])
                        {
                            Line line = new Line();
                            line.X1 = (ServerManagement.devices[mainNodeIdx].deviceXZLocationOnGrid.Item1* scaleFactor) + (scaleFactor/2);
                            line.Y1 = (ServerManagement.devices[mainNodeIdx].deviceXZLocationOnGrid.Item2* scaleFactor) + (scaleFactor / 2);
                            line.X2 = (ServerManagement.devices[compareIdx].deviceXZLocationOnGrid.Item1* scaleFactor) + (scaleFactor / 2);
                            line.Y2 = (ServerManagement.devices[compareIdx].deviceXZLocationOnGrid.Item2* scaleFactor) + (scaleFactor / 2);
                            line.StrokeThickness = 1;
                            line.Stroke = Brushes.White;
                            LayoutRootCanvas.Children.Add(line);
                        }
                    }
                }
            }
        
        }
        Rectangle prevRect;
        Grid selectedDragNode;
        bool mouseDown = false;
        private string nodeIDMove;
       

        private void nodeClicked(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Mouse Captured");
            Grid node = sender as Grid;//LayoutRootCanvas.FindName(_id) as Grid;
            if (!canvasDrag)
            {
                int idNum;
                int.TryParse(node.Name.Substring(4), out idNum);
                UserSelectedNode(idNum);
                changeSelectedNode(idNum);
                /*     UserSelectedNode.Invoke(idNum);
                     FireflyGuardian.ViewModels.DeviceNetworkViewModel.*/
            }
           /* if (nodeDrag)
            {
                
                if (node != null)
                {
                    selectedDragNode = node;
                    node.CaptureMouse();
                    mouseDown = true;
                    //nodeIDMove = _id;
                }
                clickPosition = e.GetPosition(this);

            }*/
        }
/*
        private void Node_MouseMove(object sender, MouseEventArgs e)
        {
            if (nodeDrag)
            {
                Grid _sender = sender as Grid;
                //Canvas layoutCanvas = _sender.Parent as Canvas;
                if (mouseDown)
                {
                    Point currentPosition = e.GetPosition(this.Parent as UIElement); //Current Pos Of Mouse

                    
                    Point currentPosOfGrid = _sender.TranslatePoint(new Point(0, 0), LayoutRootCanvas);
                    Console.WriteLine("CurrentPos: " + currentPosOfGrid.X + "," + currentPosOfGrid.Y + ", Click Pos: " + clickPosition.X + "," + clickPosition.Y + ", Calculated Pos: " + (clickPosition.X-currentPosOfGrid.X).ToString() + "," + (clickPosition.Y-currentPosOfGrid.Y).ToString() + ", Abs Pos: " + (currentPosOfGrid.X - (clickPosition.X - currentPosOfGrid.X)).ToString() + "," + (currentPosOfGrid.Y - (currentPosOfGrid.Y - clickPosition.Y)).ToString());

                    Matrix translateTransformPrev = _sender.RenderTransform.Value;
                    TranslateTransform translateTransform1 = new TranslateTransform(translateTransformPrev.OffsetX+(clickPosition.X - currentPosOfGrid.X), translateTransformPrev.OffsetY+(clickPosition.Y - currentPosOfGrid.Y));
                    _sender.RenderTransform = translateTransform1;
                    //layoutCanvas.
                    *//* 
                     if (prevX > 0)
                     {
                         translate.X += prevX;
                         translate.Y += prevY;
                     }*//*
                }
            }
        }

        private void Node_MouseUp(object sender, MouseButtonEventArgs e)
        {
            *//*prevX = translate.X;
            prevY = translate.Y;*//*
            Grid sendObj = sender as Grid;
            sendObj.ReleaseMouseCapture();
            Console.WriteLine("Mouse Released");
            mouseDown = false;

        }
*/

        private void changeSelectedNode(int ID)
        {
            if (prevRect != null)
            {
                prevRect.Stroke = Brushes.LightBlue;
            }
            Rectangle obj = LayoutRootCanvas.FindName("Node"+ID+"_rect") as Rectangle;
            if (obj != null)
            {
                
                obj.Stroke = Brushes.Yellow;
                prevRect = obj;
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LayoutRootCanvas.CaptureMouse();
            clickPosition = e.GetPosition(this);
        }


        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
           
            if (LayoutRootCanvas.IsMouseCaptured && canvasDrag)
            {
                Point currentPosition = e.GetPosition(this.Parent as UIElement);
                translate.X = currentPosition.X - clickPosition.X;
                translate.Y = currentPosition.Y - clickPosition.Y;
                if (prevX > 0)
                {
                    translate.X += prevX;
                    translate.Y += prevY;
                }
            }
            
        }


        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
                prevX = translate.X;
                prevY = translate.Y;
            LayoutRootCanvas.ReleaseMouseCapture();
         
        }

        private void Canvas_FitToPage(object sender, MouseButtonEventArgs e)
        {
            translate.X = container.ActualWidth/2;
            translate.Y = container.ActualHeight/2;
            prevX = translate.X;
            prevY = translate.Y;
            st.ScaleX = 1;
            st.ScaleY = 1;
          
        }
        private void Canvas_ZoomIn(object sender, MouseButtonEventArgs e)
        {
            st.ScaleX = st.ScaleX + 0.1;
            st.ScaleY = st.ScaleY + 0.1;  
        }
        private void Canvas_ZoomOut(object sender, MouseButtonEventArgs e)
        {
            st.ScaleX = st.ScaleX - 0.1;
            st.ScaleY = st.ScaleY - 0.1;
        } 
        private void Canvas_Drag(object sender, MouseButtonEventArgs e)
        {
            canvasDrag = !canvasDrag;
            nodeDrag = false;
            UserToggledCanvasDrag?.Invoke();
        }
        
        private void Node_Drag(object sender, MouseButtonEventArgs e)
        {
            canvasDrag = false;
            nodeDrag = !nodeDrag;
            UserToggledNodeDrag?.Invoke();
        }

    }
}
