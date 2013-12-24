using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamAlbumModels.Models
{
    public class Utility
    {
        public static Stream GetMediaLibraryImageStream(string name)
        {
            MediaLibrary library = new MediaLibrary();
            var picture = (from pic in library.Pictures
                          where pic.Name.Equals(name)
                          select pic).First();
            if (null != picture)
            {
                return picture.GetImage();
            }

            return null;
        }
    }
}
