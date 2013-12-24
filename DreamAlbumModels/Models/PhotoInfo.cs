using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DreamAlbumModels.Models
{
    public class PhotoInfo : ViewModelBase
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private string album;
        public string Album
        {
            get
            {
                return album;
            }
            set
            {
                album = value;
                RaisePropertyChanged(() => Album);
            }
        }

        private ImageSource thumbnail;

        public ImageSource Thumbnail
        {
            get
            {
                return thumbnail;
            }
            set
            {
                thumbnail = value;
                RaisePropertyChanged(() => Thumbnail);
            }
        }

        private string createTime;
        public string CreateTime
        {
            get
            {
                return createTime;
            }
            set
            {
                createTime = value;
                RaisePropertyChanged(() => CreateTime);
            }
        }

        private string remark;
        public string Remark
        {
            get
            {
                return remark;
            }
            set
            {
                remark = value;
                RaisePropertyChanged(() => Remark);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            if (null != Thumbnail)
            {
                Thumbnail = null;
            }
        }
    }
}
