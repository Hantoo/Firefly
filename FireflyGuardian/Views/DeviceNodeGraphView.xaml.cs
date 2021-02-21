using FireflyGuardian.ServerResources;
using FireflyGuardian.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
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
//using Windows.UI.Xaml.Controls;

namespace FireflyGuardian.Views
{
    /// <summary>
    /// Interaction logic for DeviceNodeGraphView.xaml
    /// </summary>
    
    //ToDo: Look back into node draging for repositioning.

    public delegate void NotifyCanvasDragToggle();  // delegate
    public delegate void NotifyNodeDragToggle();  // delegate
    public delegate void NotifyCanvasNodeSelected();  // delegate
    public delegate void NotifyListUpdate();  // delegate
    public partial class DeviceNodeGraphView : UserControl
    {
        //ToDo: Implement Zoom Functionality For Graph
        

        public static event NotifyCanvasDragToggle UserToggledCanvasDrag;
        public static event NotifyNodeDragToggle UserToggledNodeDrag;
        public static event NotifyListUpdate AutoUpdateList;
        public static event Action<int> UserSelectedNode;
        public int scaleFactor = 60;
        private bool canvasDrag = false;
        private bool nodeDrag = false;
        private Point clickPosition;
        private Double prevX, prevY;
        public bool firstTime = false;
        public bool canRefreshImagesOnNodes = false;
        Thread UpdateImageLoopThread;
        BitmapImage mapImage;
        public DeviceNodeGraphView()
        {
            InitializeComponent();

            FireflyGuardian.ViewModels.DeviceNetworkViewModel.RefreshCanvas += refreshGraph;
            FireflyGuardian.ViewModels.DeviceNetworkViewModel.UserChangedNode += changeSelectedNode;
            FireflyGuardian.ViewModels.DeviceNetworkViewModel.DestroyCanvas += destoryGraph;
            canRefreshImagesOnNodes = true;
            ThreadStart childref = new ThreadStart(threadedUpdateImageLoop);
            Console.WriteLine("[AUTO UPDATE] - Created AutoUpdate Thread");
            UpdateImageLoopThread = new Thread(childref);
            //mapImage = new BitmapImage(new Uri(ServerManagement.settings.locationOfVenueDiagram));

            UpdateImageLoopThread.Start();


        }

        public void threadedUpdateImageLoop()
        {
            Console.WriteLine("[THREAD] UpdatingImages: "+ canRefreshImagesOnNodes);
            while (canRefreshImagesOnNodes)
            {
              
                updateImagesOnNodes();
                Thread.Sleep(250);
            }
            Console.WriteLine("[THREAD] Updating Node Images Thread Exit!");
        }

        

        public void setMapSize(double scaleMultipler, ref Image _img)
        {
            Console.WriteLine("resizeMap by:" + scaleMultipler);
           /* Image img = LayoutRootCanvas.FindName("VenueMap") as Image;
            img.Width =*/ _img.Width*=scaleMultipler;
             _img.Height*=scaleMultipler;
        }

        public void destoryGraph() //Removes all node assigned names when view is closed or graph refreshed
        {
            canRefreshImagesOnNodes = false;
            try
            {
                UnregisterName("VenueMap");
            }
            catch { }
            for (int i = 0; i < ServerManagement.devices.Count; i++)
            {
                UnregisterName("Node" + ServerManagement.devices[i].deviceID.ToString() + "_rect");
                UnregisterName("Node" + ServerManagement.devices[i].deviceID.ToString() + "_img");
                UnregisterName("Node" + ServerManagement.devices[i].deviceID.ToString());
            }
            FireflyGuardian.ViewModels.DeviceNetworkViewModel.RefreshCanvas -= refreshGraph;
            FireflyGuardian.ViewModels.DeviceNetworkViewModel.UserChangedNode -= changeSelectedNode;
            FireflyGuardian.ViewModels.DeviceNetworkViewModel.DestroyCanvas -= destoryGraph;
        }
        
        public void refreshGraph()
        {
            //ToDo: Image hides the slection outline
            //Set Blocks
            canRefreshImagesOnNodes = false;
            LayoutRootCanvas.Children.Clear();
            try{
                UnregisterName("VenueMap");
            }catch(Exception e){
                //Do nothing
                Console.WriteLine("'VenueMap' Does not exist - This is fine");
            }
            try
            {
                if (ServerManagement.settings.locationOfVenueDiagram != null)
                {
                    if (File.Exists(ServerManagement.settings.locationOfVenueDiagram))
                    {
                        Image imgMap = new Image();
                        //mapImage = new BitmapImage(new Uri(ServerManagement.settings.locationOfVenueDiagram));
                        imgMap.Source = BitmapFromUri(new Uri(ServerManagement.settings.locationOfVenueDiagram));
                        Console.WriteLine("RenderSize: " + imgMap.RenderSize.Width + "," + imgMap.RenderSize.Height);
                        imgMap.Name = "VenueMap";
                        imgMap.Width = GetImageSize(ServerManagement.settings.locationOfVenueDiagram).Width * ServerManagement.settings.VenueDiagramScaleMultipler;
                        imgMap.Height = GetImageSize(ServerManagement.settings.locationOfVenueDiagram).Height * ServerManagement.settings.VenueDiagramScaleMultipler;
                        RegisterName(imgMap.Name, imgMap);
                        LayoutRootCanvas.Children.Add(imgMap);
                        Canvas.SetZIndex(imgMap, 0);
                        Canvas.SetLeft(imgMap, -(imgMap.Width / 2));
                        Canvas.SetTop(imgMap, -(imgMap.Height / 2));
                        Console.WriteLine("Added Map Image");
                        Console.WriteLine("Map Image [" + imgMap.Width.ToString() + "," + imgMap.Height.ToString() + "]");
                        imgMap.MouseDown += nodeClicked;
                    }
                }
            }
            catch (Exception e)
            {
                //Do nothing
                Console.WriteLine(e);
            }
           
            Console.WriteLine("RefreshGraph");
            for (int i =0; i < ServerManagement.devices.Count; i++)
            {
                var grid = new Grid();
                
                Rectangle rect = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 50
                };
                rect.Width = (1* scaleFactor);
                rect.Height = (1* scaleFactor);

                rect.Fill = Brushes.Black;

                //Windows.UI.Xaml.Controls.Image img = new Windows.UI.Xaml.Controls.Image();
                Image img = new Image();
                img.Width = rect.Width;
                img.Height = rect.Height;
                rect.Width += 10;
                rect.Height += 10;
                img.Name = "Node" + ServerManagement.devices[i].deviceID.ToString() + "_img";
                img.Source = ServerManagement.mediaSlots[ServerManagement.devices[i].activeImageSlot].image;
                grid.Children.Add(rect);
                grid.Children.Add(img);
                TextBlock text = new TextBlock();
                text.Text = ServerManagement.devices[i].deviceID.ToString();
                rect.Name = "Node" + ServerManagement.devices[i].deviceID.ToString() + "_rect";
                
                
                text.FontSize = 1;
                grid.Children.Add(text);
                grid.Name = "Node" + ServerManagement.devices[i].deviceID.ToString();
                grid.Focusable = true;
               
                grid.MouseDown += nodeClicked;
                try
                {

                    RegisterName(rect.Name, rect);
                    RegisterName(grid.Name, grid);
                    RegisterName(img.Name, img);
                }
                catch(Exception e)
                {
                    
                    UnregisterName(rect.Name);
                    UnregisterName(grid.Name);
                    UnregisterName(img.Name);
                    RegisterName(rect.Name, rect);
                    RegisterName(grid.Name, grid);
                    RegisterName(img.Name, img);
                }
                /*grid.MouseMove += Node_MouseMove;
                //grid.MouseUp += Node_MouseUp;
                grid.PreviewMouseLeftButtonUp += Node_MouseUp;*/
                Canvas.SetLeft(grid, ServerManagement.devices[i].deviceXZLocationOnGrid.Item1* scaleFactor);
                Canvas.SetTop(grid, -ServerManagement.devices[i].deviceXZLocationOnGrid.Item2* scaleFactor);

                LayoutRootCanvas.Children.Add(grid);
                Canvas.SetZIndex(grid, 2);
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
                            line.Y1 = -(ServerManagement.devices[mainNodeIdx].deviceXZLocationOnGrid.Item2* scaleFactor) + (scaleFactor / 2);
                            line.X2 = (ServerManagement.devices[compareIdx].deviceXZLocationOnGrid.Item1* scaleFactor) + (scaleFactor / 2);
                            line.Y2 = -(ServerManagement.devices[compareIdx].deviceXZLocationOnGrid.Item2* scaleFactor) + (scaleFactor / 2);
                            line.StrokeThickness = 1;
                            if (ServerManagement.devices[compareIdx].flagEmergencyAtNode || ServerManagement.devices[mainNodeIdx].flagEmergencyAtNode)
                            {
                                line.Stroke = Brushes.Red;
                            }
                            else
                            {
                                line.Stroke = Brushes.White;
                            }
                                
                            LayoutRootCanvas.Children.Add(line);
                            Canvas.SetZIndex(line, 1);
                        }
                    }
                }
            }
            canRefreshImagesOnNodes = true;
            if (!UpdateImageLoopThread.IsAlive)
            {
                ThreadStart childref = new ThreadStart(threadedUpdateImageLoop);
                Console.WriteLine("[AUTO UPDATE] - Created AutoUpdate Thread");
                UpdateImageLoopThread = new Thread(childref);
                UpdateImageLoopThread.Start();
            }
        }
        Rectangle prevRect;
        Grid selectedDragNode;
        bool mouseDown = false;
        private string nodeIDMove;
        //stackoverflow.com/questions/60857830/finding-png-image-width-height-via-file-metadata-net-core-3-1-c-sharp
        public Size GetImageSize(string Filename)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(Filename));
            br.BaseStream.Position = 16;
            byte[] widthbytes = new byte[sizeof(int)];
            for (int i = 0; i < sizeof(int); i++) widthbytes[sizeof(int) - 1 - i] = br.ReadByte();
            int width = BitConverter.ToInt32(widthbytes, 0);
            byte[] heightbytes = new byte[sizeof(int)];
            for (int i = 0; i < sizeof(int); i++) heightbytes[sizeof(int) - 1 - i] = br.ReadByte();
            int height = BitConverter.ToInt32(heightbytes, 0);
            
            return new Size(width, height);
        }

        public static ImageSource BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }

        public void refreshImageOnNodes(object sender, MouseButtonEventArgs e)
        {
            updateImagesOnNodes();
        }

       

        public void updateImagesOnNodes()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {


                    try
                    {

                        for (int i = 0; i < ServerManagement.devices.Count; i++)
                        {
                            if (ServerManagement.devices[i] != null)
                            {
                                if (ServerManagement.mediaSlots.Count > 0)
                                {
                                    if (canRefreshImagesOnNodes)
                                    {
                                        string imgName = "Node" + ServerManagement.devices[i].deviceID.ToString() + "_img";
                                        Image img = this.FindName(imgName) as Image;
                                        if (img != null)
                                        {
                                            try
                                            {
                                                img.Source = ServerManagement.mediaSlots[ServerManagement.devices[i].activeImageSlot].image;
                                            }
                                            catch
                                            {
                                                ServerManagement.updateServermangementMediapool();
                                                img.Source = ServerManagement.mediaSlots[ServerManagement.devices[i].activeImageSlot].image;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        AutoUpdateList.Invoke();
                        
                    }
                    catch (Exception ex) { Console.WriteLine("DEBUG: " + ex); }
                });
            }
            catch { }
            

        }

        private void nodeClicked(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Mouse Captured");
            Grid node = sender as Grid;//LayoutRootCanvas.FindName(_id) as Grid;
            try
            {

                if (!canvasDrag)
                {
                    if (node != null)
                    {


                        int idNum;
                        int.TryParse(node.Name.Substring(4), out idNum);
                        UserSelectedNode(idNum);
                        changeSelectedNode(idNum);
                    }
             
                }
            }catch(Exception ex) { }


        }


        private void changeSelectedNode(int ID)
        {
            if (prevRect != null)
            {
                prevRect.Stroke = Brushes.Black;
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
