using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DreamAlbumViewModels.ViewModels;
using Microsoft.Phone.Reactive;
using System.Windows.Media.Imaging;
using DreamAlbumModels.Models;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace DreamAlbum
{
    public partial class SlidePage : PhoneApplicationPage
    {
        private PhotoViewModels viewModel;
        private int imageIndex = 0;

        public SlidePage()
        {
            InitializeComponent();

            viewModel = new PhotoViewModels();

            var oEvent = Observable.FromEvent<EventArgs>(viewModel, "LoadingPhoto").Take(1);

            oEvent.ObserveOnDispatcher().Subscribe(args =>
            {
                BitmapImage bi = new BitmapImage();
                if (null != viewModel.Pictures && viewModel.Pictures.Count > 0)
                {
                    PhotoInfo photo = viewModel.Pictures[imageIndex];
                    string imageName = photo.Name;
                    using (Stream stream = Utility.GetMediaLibraryImageStream(imageName))
                    {
                        bi.SetSource(stream);
                        LayoutRoot.DataContext = bi;
                    }

                    imageIndex++;
                }
            });
        }

        protected override void OnRemovedFromJournal(JournalEntryRemovedEventArgs e)
        {
            viewModel.Dispose();
            base.OnRemovedFromJournal(e);
        }
    }
}