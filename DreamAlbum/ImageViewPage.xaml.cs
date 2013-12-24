using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DreamAlbumResources;
using Microsoft.Xna.Framework.Media;
using System.Windows.Media.Imaging;
using System.Text;
using System.Diagnostics;
using DreamAlbumModels.Models;
using System.IO;

namespace DreamAlbum
{
    public partial class ImageViewPage : PhoneApplicationPage
    {
        public ImageViewPage()
        {
            InitializeComponent();
        }

        double imageWidth, imageHeight;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("name"))
            {
                string imageName = NavigationContext.QueryString["name"];
                byte[] buffer = Convert.FromBase64String(imageName);
                string name = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

                using (Stream stream = Utility.GetMediaLibraryImageStream(name))
                {
                    BitmapImage bi = new BitmapImage();
                    bi.SetSource(stream);

                    imageHeight = image.Height;
                    imageWidth = image.Width;

                    LayoutRoot.DataContext = bi;
                }
            }
        }

        protected override void OnRemovedFromJournal(JournalEntryRemovedEventArgs e)
        {
            LayoutRoot.DataContext = null;
            GC.Collect();

            NavigationService.RemoveBackEntry();

            base.OnRemovedFromJournal(e);
        }

        private void LayoutRoot_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            #region Scale
            if (e.DeltaManipulation.Scale.X > 0)
            {
                double scale = ImageScale.ScaleX * e.DeltaManipulation.Scale.X;
                if (scale > 2)
                {
                    scale = 2;
                }
                if (scale < 0.1)
                {
                    scale = 0.1;
                }
                if (Math.Abs(1 - scale) < 0.05)
                {
                    scale = 1;
                }

                ImageScale.ScaleX = scale;
                ImageScale.ScaleY = scale;

                Debug.WriteLine("Image size: ({0}, {1})", image.ActualWidth * scale, image.ActualHeight * scale);
            }
            #endregion

            #region Translate

            double transX = ImageTranslate.X + e.DeltaManipulation.Translation.X;
            double transY = ImageTranslate.Y + e.DeltaManipulation.Translation.Y;

            if (Math.Abs(transX) < 5)
            {
                transX = 0;
            }

            if (Math.Abs(transY) < 5)
            {
                transY = 0;
            }

            ImageTranslate.X = transX;
            ImageTranslate.Y = transY;          

            #endregion            
        }
    }
}