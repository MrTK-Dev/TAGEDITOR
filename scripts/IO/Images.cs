using ID3_Tag_Editor.Scripts.User;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ID3_Tag_Editor.Scripts.IO
{
    public static class Images
    {
        public static string TEMP_Folder = @"resources\TEMP";

        public static void SaveBitMapToFile(Bitmap bitMap, string newPath, string fileName)
        {
            /*using (FileStream fileStream = new FileStream(newPath, FileMode.Create))
            {
                //bitMap = new Bitmap(fileStream);
                Bitmap copy = new Bitmap(bitMap);
                copy.Save(fileName, ImageFormat.Png);
                bitMap.Dispose();
                copy.Dispose();
            }*/

            Console.WriteLine(Path.Combine(newPath, fileName));

            Bitmap copy = new Bitmap(bitMap);
            copy.Save(newPath + @"\" + fileName + ".png", ImageFormat.Png);
            bitMap.Dispose();
            copy.Dispose();

            //bitMap.Save(newPath);
            //bitMap.Dispose();
        }

        public static string CacheBitMap(Bitmap bitMap, string fileName)
        {
            string newPath = Paths.GetFullPath(TEMP_Folder);

            SaveBitMapToFile(bitMap, newPath, fileName);

            return Path.Combine(newPath, fileName + ".png");
        }

        public static void ClearCache()
        {
            FileSystem.ClearFolder(TEMP_Folder, true);
        }

        public static BitmapImage GetCoverUI(TagLib.File newFile)
        {
            Bitmap bitmap = TagLibEXT.GetCoverImage(newFile);

            if (bitmap == null)
                return new BitmapImage(new Uri(@"C:\Users\Megaport\source\repos\ID3 Tag Editor\resources\no_cover.jpg"));

            return new BitmapImage(new Uri(CacheBitMap(bitmap, newFile.Tag.Title)));
        }
    }
}
