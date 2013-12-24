using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DreamAlbumViewModels.ViewModels;
using DreamAlbumResources;
using Microsoft.Phone.Reactive;
using DreamAlbumModels.Models;
using System.Text;
using DreamAlbumResources.Resources;
using System.Diagnostics;

namespace DreamAlbum
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            LayoutRoot.DataContext = new PhotoViewModels();

            GetAppBar();

            var oEvent = Observable.FromEvent<RoutedEventArgs>(LayoutRoot, "Loaded");
            oEvent.ObserveOnDispatcher().Subscribe(args =>
            {
                ImageList.GridCellSize = new Size(GetThumbnailWidth(), GetThumbnailWidth());
            });
        }

        private void GetAppBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton btnChangeView = new ApplicationBarIconButton(new Uri("/Assets/AppBar/sync.png", UriKind.Relative));
            btnChangeView.Text = AppResources.AppBar_ChangeView;
            btnChangeView.Click += (s, args) =>
            {
                NavigationService.Navigate(new Uri("/SlidePage.xaml", UriKind.Relative));
            };

            ApplicationBar.Buttons.Add(btnChangeView);
        }

        private double GetThumbnailWidth()
        {
            var frame = App.Current.RootVisual as PhoneApplicationFrame;

            double screenWidth = App.Current.Host.Content.ActualWidth;
            double screenHeight = App.Current.Host.Content.ActualHeight;

            double thumbnailWidth = Constants.DefaultThumbnailWidth;

            if (null != frame)
            {
                switch (frame.Orientation)
                {
                    case PageOrientation.Landscape:
                    case PageOrientation.LandscapeLeft:
                    case PageOrientation.LandscapeRight:
                        thumbnailWidth = Math.Round(screenHeight / Constants.LandscapeThumbnailCount) - (Constants.DefaultThumbnailGap * (Constants.LandscapeThumbnailCount - 1));
                        break;
                    case PageOrientation.Portrait:
                    case PageOrientation.PortraitUp:
                    case PageOrientation.PortraitDown:
                        thumbnailWidth = Math.Round(screenWidth / Constants.PortraitThumbnailCount) - (Constants.DefaultThumbnailGap * (Constants.PortraitThumbnailCount - 1));
                        break;
                }
            }

            return thumbnailWidth;
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);

            ImageList.GridCellSize = new Size(GetThumbnailWidth(), GetThumbnailWidth());
        }

        private void ImageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (null != selector)
            {
                string name = (selector.SelectedItem as PhotoInfo).Name;
                byte[] buffer = Encoding.UTF8.GetBytes(name);
                string baseName = Convert.ToBase64String(buffer);

                string uri = String.Format("/ImageViewPage.xaml?name={0}", baseName);
                NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }
        }

        protected override void OnRemovedFromJournal(JournalEntryRemovedEventArgs e)
        {
            LayoutRoot.DataContext = null;

            base.OnRemovedFromJournal(e);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {                        
            base.OnBackKeyPress(e);
            Debug.WriteLine("Quit");

        }
    }
}