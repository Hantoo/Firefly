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
    public enum statusState
    {
        Good,
        Warning,
        Error
    };

    class MediaPoolViewModel :Screen
    {
        public List<MediaSlotModel> mediaSlots { get; set; }
        private MediaSlotModel _SelectedImageSlot;
        public MediaSlotModel SelectedImageSlot { get { return _SelectedImageSlot; } set { _SelectedImageSlot = value; if (_SelectedImageSlot.slotID < 23) { showMediaUploadButton = "Hidden"; } else { showMediaUploadButton = "Visible"; } NotifyOfPropertyChange(() => SelectedImageSlot); NotifyOfPropertyChange(() => showMediaUploadButton); } }
        public string showMediaUploadButton { get; set; }
        public bool renameMediaButton { get; set; }
        public string mediaSlotName { get; set; }

        public MediaPoolViewModel()
        {
           
            RefreshMediaSlots();
        }

        #region statusBanner
        public bool showStatus { get; set; }
        public string statusMessage { get; set; }
        public FireflyGuardian.ViewModels.statusState statusStateIndicator { get; set; }
        public void changeStatusBackGround(statusState state)
        {
            statusStateIndicator = state;
            NotifyOfPropertyChange(() => statusStateIndicator);
           
        }

        public async void ShowStatus(string message, int timeShown, statusState state)
        {
            changeStatusBackGround(state);
            await PutTaskDelay(message, timeShown);
        }

        async Task PutTaskDelay(string msg,int timeout)
        {
            showStatus = true;
            NotifyOfPropertyChange(() => showStatus);
            statusMessage = msg;
            NotifyOfPropertyChange(() => statusMessage);
            await Task.Delay(timeout);
            showStatus = false;
            NotifyOfPropertyChange(() => showStatus);
            await Task.Delay(500);
            statusMessage = "";
            NotifyOfPropertyChange(() => statusMessage);
        }

        #endregion


        //Renaming Of Slot
        //Only changes the name locally stored within ServerManagement.Settings variable
        //Does not change the name of the actual file type as this could cause more work
        //When trying to download the files onto the node devices
        #region Rename Slot
        public void RenameImage()
        {
            if (SelectedImageSlot.slotID < 23)
            {
                return;
            }
            renameMediaButton = true;
            NotifyOfPropertyChange(() => renameMediaButton);
            
        }

        public void renameImageConfirm()
        {
            renameMediaButton = false;
            NotifyOfPropertyChange(() => renameMediaButton);
            ServerManagement.settings.slotNames[SelectedImageSlot.slotID] = mediaSlotName;
            mediaSlotName = "";
            NotifyOfPropertyChange(() => mediaSlotName);
            RefreshMediaSlots();
        }
        public void renameImageCancel()
        {
            renameMediaButton = false;
            NotifyOfPropertyChange(() => renameMediaButton);
            mediaSlotName = "";
            NotifyOfPropertyChange(() => mediaSlotName);
        }
        #endregion


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
            Console.WriteLine("Generate :" + ServerManagement.settings.absoluteLocationOfLocalisedMedia);
            for (int i = 0; i < 255; i++)
            {
                MediaSlotModel slot = new MediaSlotModel();
                slot.slotID = i;
                if (File.Exists(ServerManagement.settings.absoluteLocationOfLocalisedMedia + "/" + i + ".png"))
                {
                    if(i < 23)
                    {
                        slot.image_symbol = "\uE72E";
                    }
                    if (ServerManagement.settings.slotNames[i] != null)
                    {
                        slot.image_name = ServerManagement.settings.slotNames[i];
                    }
                    else
                    {
                        
                        slot.image_name = ServerManagement.settings.slotNames[i];
                    }
                    slot.image = utils.BitmapToImageSource(ServerManagement.settings.absoluteLocationOfLocalisedMedia + "/" + i + ".png");
                    //slot.image_source = ;
                }
                else
                {
                    ServerManagement.settings.slotNames[i] = "";
                    slot.image_name = "No Media";
                }
                mediaSlots.Add(slot);
            }
            if (updateSelected)
            {
                SelectedImageSlot = mediaSlots[selectedidx];
            }
            NotifyOfPropertyChange(() => mediaSlots);
            ServerManagement.mediaSlots = mediaSlots;
        }

        

        public void SyncLocalToFTP()
        {
            Console.WriteLine(ServerManagement.settings.ftpURL + ", " + ServerManagement.settings.ftpUsername + ", " + ServerManagement.settings.ftpPassword);
            if (FTPAccess.VerifyConnection(ServerManagement.settings.ftpURL, ServerManagement.settings.ftpUsername, ServerManagement.settings.ftpPassword))
            {
                Console.WriteLine("Connection Established");
                FTPAccess.syncLocalisedMediaPoolToFTPServer();
                ShowStatus("Synced To FTP", 3000, statusState.Good);
            }
            else
            {
                FTPAccess.syncLocalisedMediaPoolToFTPServer();
                ShowStatus("Can't Connect To FTP Server", 5000, statusState.Error);
            }
        }

        public void RequestPullToDevices()
        {
            ServerResources.UDP.UDPPreformattedMessages.RequestImagesFromFTPForAllDevices();
        }

        public void replaceImage()
        {
            OpenFileDialog fileUpload = new OpenFileDialog();
            fileUpload.Filter = ".png Files | *.png";
            fileUpload.ShowDialog();
            if (fileUpload.FileName != null && File.Exists(fileUpload.FileName))
            {
                
                Image upload = Image.FromFile(fileUpload.FileName);
                //If file is not correct size then rescale the image to be the correct size.
                if (upload.Width != 64 || upload.Height != 64)
                {
                    Image img = utils.Resize(Image.FromFile(fileUpload.FileName), 64, 64);

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
        }

        
    }
}
