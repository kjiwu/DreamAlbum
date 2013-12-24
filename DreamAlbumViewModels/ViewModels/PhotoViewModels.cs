using DreamAlbumModels.Models;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Phone.Reactive;
using System.IO;
using System.Windows.Media.Imaging;

namespace DreamAlbumViewModels.ViewModels
{
    public class PhotoViewModels : ViewModelBase
    {
        public PhotoViewModels()
        {
            Observable.Start(LoadPhoto);
        }

        protected void LoadPhoto()
        {
            MediaLibrary mediaLib = new MediaLibrary();

            var pictures = mediaLib.Pictures.ToList();

            IObservable<Picture> obser = Observable.GenerateWithTime(0,
                                                         i => i < pictures.Count,
                                                         i => pictures[i],
                                                         i => TimeSpan.FromMilliseconds(100),
                                                         i => i + 1);

            obser.ObserveOnDispatcher().Subscribe(pic =>
            {
                Stream thumbnail = pic.GetThumbnail();
                BitmapImage biThumbnail = new BitmapImage()
                {
                    CreateOptions = BitmapCreateOptions.BackgroundCreation
                };
                biThumbnail.SetSource(thumbnail);

                PhotoInfo info = new PhotoInfo
                {
                    Name = pic.Name,
                    Album = pic.Album.Name,
                    Thumbnail = biThumbnail
                };

                Pictures.Add(info);
            });
        }

        private ObservableCollection<PhotoInfo> pictures = new ObservableCollection<PhotoInfo>();

        public ObservableCollection<PhotoInfo> Pictures
        {
            get
            {
                return pictures;
            }
            set
            {
                pictures = value;
                RaisePropertyChanged(() => Pictures);
            }
        }

        public void Reload()
        {
            Pictures.Clear();
            Observable.Start(LoadPhoto);
        }
    }
}
