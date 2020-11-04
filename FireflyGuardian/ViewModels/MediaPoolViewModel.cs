using Caliburn.Micro;
using FireflyGuardian.Models;
using FireflyGuardian.Properties;
using FireflyGuardian.ServerResources;
using FireflyGuardian.ServerResources.DataAccess;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FireflyGuardian.ViewModels
{
    class MediaPoolViewModel :Screen
    {
        public List<MediaSlotModel> mediaSlots { get; set; }
        private MediaSlotModel _SelectedImageSlot;
        public MediaSlotModel SelectedImageSlot { get { return _SelectedImageSlot; } set { _SelectedImageSlot = value; if (_SelectedImageSlot.slotID <= 10) { showMediaUploadButton = "Hidden"; } else { showMediaUploadButton = "Visible"; } NotifyOfPropertyChange(() => SelectedImageSlot); NotifyOfPropertyChange(() => showMediaUploadButton); } }
        public string showMediaUploadButton { get; set; }
        public MediaPoolViewModel()
        {
            
            //verifyMedia();
            RefreshMediaSlots();
        }

        public void RefreshMediaSlots()
        {
            int selectedidx = 0;
            bool updateSelected = false;
            if (SelectedImageSlot != null)
            {
                selectedidx = SelectedImageSlot.slotID;
                updateSelected = true;
            }
            mediaSlots = new List<MediaSlotModel>();
            
            if (ServerManagement.settings == null) { Console.WriteLine("No Settings"); return; }
            for (int i = 0; i < 255; i++)
            {
                MediaSlotModel slot = new MediaSlotModel();
                slot.slotID = i;
                if (File.Exists(ServerManagement.settings.absoluteLocationOfLocalisedMedia + "/" + i + ".png"))
                {
                    slot.image_name = i+".png";
                    slot.image = BitmapToImageSource(ServerManagement.settings.absoluteLocationOfLocalisedMedia + "/" + i + ".png");
                    //slot.image_source = ;
                }
                else
                {
                    
                    slot.image_name = "No Media";
                }
                mediaSlots.Add(slot);
            }
            if (updateSelected)
            {
                SelectedImageSlot = mediaSlots[selectedidx];
            }
            NotifyOfPropertyChange(() => mediaSlots);
        }

        BitmapImage BitmapToImageSource(string bitmaplocation)
        {

            Bitmap bitmap = new Bitmap(Image.FromFile(bitmaplocation));
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                bitmap.Dispose();
                return bitmapimage;
            }
            
        }

        public void SyncLocalToFTP()
        {
            FTPAccess.syncLocalisedMediaPoolToFTPServer();
        }

        public void replaceImage()
        {
            OpenFileDialog fileUpload = new OpenFileDialog();
            fileUpload.Filter = ".png Files | *.png";
            fileUpload.ShowDialog();
            Image upload = Image.FromFile(fileUpload.FileName);
            //If file is not correct size then rescale the image to be the correct size.
            if (upload.Width != 64 || upload.Height != 64)
            {
                Image img = Resize(Image.FromFile(fileUpload.FileName), 64, 64);
                img.Save(ServerManagement.settings.absoluteLocationOfLocalisedMedia + "/" + SelectedImageSlot.slotID + ".png");
            }
            else
            {
                File.Copy(fileUpload.FileName, ServerManagement.settings.absoluteLocationOfLocalisedMedia + "/" + SelectedImageSlot.slotID + ".png", true);
            }
            SelectedImageSlot.image_source = ServerManagement.settings.absoluteLocationOfLocalisedMedia + "/" + SelectedImageSlot.slotID + ".png";
            SelectedImageSlot.image_name = SelectedImageSlot.slotID + ".png";
            NotifyOfPropertyChange(() => SelectedImageSlot);
            RefreshMediaSlots();
        }

        //https://stackoverflow.com/questions/17441098/how-to-resize-image-with-different-resolution
        public Image Resize(Image originalImage, int w, int h)
        {
            //Original Image attributes
            int originalWidth = originalImage.Width;
            int originalHeight = originalImage.Height;

            // Figure out the ratio
            double ratioX = (double)w / (double)originalWidth;
            double ratioY = (double)h / (double)originalHeight;
            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // now we can get the new height and width
            int newHeight = Convert.ToInt32(originalHeight * ratio);
            int newWidth = Convert.ToInt32(originalWidth * ratio);

            Image thumbnail = new Bitmap(newWidth, newHeight);
            Graphics graphic = Graphics.FromImage(thumbnail);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            graphic.Clear(Color.Transparent);
            graphic.DrawImage(originalImage, 0, 0, newWidth, newHeight);

            return thumbnail;
        }
    }
}
