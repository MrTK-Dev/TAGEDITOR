using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;
using System.Diagnostics;
using System.Drawing;
using ID3_Tag_Editor;
using Scripts;
using User;
using ID3_Tag_Editor.scripts;

namespace ID3_Tag_Editor
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #region LoadUI Variables

            UI.allSubMenus = new List<StackPanel>()
            {
                panel_SubMenu_Automation,
                panel_SubMenu_Editor
            };

            UI.userPathBoxes.Import = TextBox_Dialog_Input;
            UI.userPathBoxes.Export = TextBox_Dialog_Output;

            #endregion

            #region LoadUI Graphics

            PanelHandler.HideSubMenus();

            #endregion

            Settings.Load();
        }

        public void OutputCount(object sender, RoutedEventArgs e)
        {
            string[] allFiles = FileSystem.GetFilesFromDirectory(Paths.Import, true);

            Console.WriteLine("The folder contains {0} songs.", allFiles.Length);

            for (int i = 0; i < allFiles.Length; i++)
            {
                if (allFiles[i].Contains(".mp3"))
                {
                    TagLib.File Song = TagLib.File.Create(allFiles[i]);

                    if (Song.Tag.FirstPerformer != null && Song.Tag.Title != null)
                    {
                        string newFileName = Song.Tag.FirstPerformer + " - " + Song.Tag.Title.Replace(":", " ").Replace("<", "").Replace(">", "").Replace("/", "_").Replace(@"\", "_");
                        Debug.WriteLine(newFileName);

                        //Album
                        if (string.IsNullOrEmpty(Song.Tag.Album))
                        {
                            if (Song.Tag.Title.ContainsAny(new string[] {"Remix", "Cover", "Flip", "Rework", "Edit", "Redo", "VIP" }))
                            {
                                string newName;

                                //Test
                                if (!Song.Tag.Title.Contains("["))
                                {
                                    if (Song.Tag.Title.Contains("("))
                                    {
                                        string[] Splitted = Song.Tag.Title.Split('(');

                                        newName = Splitted[0].RemoveLastChar();
                                    }

                                    else
                                    {
                                        newName = Song.Tag.Title;
                                    }
                                }

                                else
                                {
                                    string[] Splitted = Song.Tag.Title.Split('[');

                                    newName = Splitted[0].RemoveLastChar();
                                }

                                Song.Tag.Album = newName + " (The Remixes)";
                            }

                            else
                            {
                                Song.Tag.Album = Song.Tag.Title + " - Single";
                            }
                        }

                        else
                        {
                            //string[] Splitted = Song.Tag.Title.Split('[');
                        }

                        Song.Save();

                        Console.WriteLine("Title: " + Song.Tag.Title);
                        Console.WriteLine("Artist: " + Song.Tag.FirstPerformer);
                        Console.WriteLine("Album: " + Song.Tag.Album);

                        if (Modes.text != Modes.Text_Mode.none)
                        {

                            if (Modes.text == Modes.Text_Mode.all || Modes.text == Modes.Text_Mode.Folders)
                            {
                                string newPath = User.Paths.Export + @"\" + Song.Tag.Album.Replace(":", " ").Replace("/", "_");

                                if (!Directory.Exists(newPath))
                                {
                                    Directory.CreateDirectory(newPath);

                                    Debug.WriteLine("Created the directory {0}", Song.Tag.Album);
                                }

                                File.Move(allFiles[i], newPath + @"\" + newFileName + ".mp3");
                            }

                            else if (Modes.text == Modes.Text_Mode.Rename)
                            {
                                File.Move(allFiles[i], User.Paths.Export + @"\" + newFileName + ".mp3");
                            }
                        }

                        if (Modes.image != Modes.Image_Mode.none || Modes.image_2 != Modes.Image_2_Mode.none)
                        {
                            var mStream = new MemoryStream();
                            var firstPicture = Song.Tag.Pictures.FirstOrDefault();
                            if (firstPicture != null)
                            {
                                byte[] pData = firstPicture.Data.Data;
                                mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                                var CoverImage = new Bitmap(mStream, false);
                                mStream.Dispose();

                                if (Modes.image == Modes.Image_Mode.CheckSquare || Modes.image == Modes.Image_Mode.all)
                                {
                                    if (CoverImage.Width != CoverImage.Height)
                                    {
                                        Console.WriteLine("{0} [{1} x {2}]", newFileName, CoverImage.Width, CoverImage.Height);
                                    }
                                }

                                if (Modes.image == Modes.Image_Mode.CheckSize || Modes.image == Modes.Image_Mode.all)
                                {
                                    if (CoverImage.Width > 2000)
                                    {
                                        Console.WriteLine("{0} [{1} x {2}]", newFileName, CoverImage.Width, CoverImage.Height);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Tags are missing!");
                    }
                }
            }

            Settings.Save();
        }

        public void ChangeImage(object sender, RoutedEventArgs e)
        {
            if (Modes.image_2 == Modes.Image_2_Mode.overwrite)
            {
                string[] allFiles = Directory.GetFiles(Paths.ImageFile);

                List<string> SongFiles = new List<string>();
                List<string> Coverfile = new List<string>();

                for (int i = 0; i < allFiles.Length; i++)
                {
                    if (allFiles[i].Contains(".mp3"))
                    {
                        SongFiles.Add(allFiles[i]);
                    }
                    else if (allFiles[i].Contains("folder."))
                    {
                        Coverfile.Add(allFiles[i]);
                    }
                }

                for (int i = 0; i < SongFiles.Count; i++)
                {
                    TagLib.File Song = TagLib.File.Create(SongFiles[i]);
                    Debug.WriteLine(Song.Tag.Track);

                    if (File.Exists(Coverfile[0]))
                    {
                        var pic = new TagLib.IPicture[1];
                        pic[0] = new TagLib.Picture(Coverfile[0]);
                        Song.Tag.Pictures = pic;
                        Song.Save();
                    }
                }
            }
        }

        public static class Modes
        {
            public static Text_Mode text = Text_Mode.Rename;
            public static Image_Mode image = Image_Mode.all;
            public static Image_2_Mode image_2 = Image_2_Mode.none;

            public enum Text_Mode
            {
                Folders,
                Rename,
                all,
                none
            }

            public enum Image_Mode
            {
                CheckSize,
                CheckSquare,
                all,
                none
            }

            public enum Image_2_Mode
            {
                delete,
                overwrite,
                none
            }
        }

        private void Button_OpenSubMenu_Automation(object sender, RoutedEventArgs e)
        {
            Scripts.PanelHandler.HighlightSubMenu(panel_SubMenu_Automation);
        }

        private void Button_OpenSubMenu_Editor(object sender, RoutedEventArgs e)
        {
            Scripts.PanelHandler.HighlightSubMenu(panel_SubMenu_Editor);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public static class UI
        {
            public static List<StackPanel> allSubMenus = new List<StackPanel>();

            public static class userPathBoxes
            {
                public static TextBox Import = new TextBox();

                public static TextBox Export = new TextBox();
            }
        }

        private void BClick_SelectFolder_Input(object sender, RoutedEventArgs e)
        {
            PathMethods.SelectFolder(TextBox_Dialog_Input, User.Paths.PathType.INPUT);
        }

        private void BClick_SelectFolder_Output(object sender, RoutedEventArgs e)
        {
            PathMethods.SelectFolder(TextBox_Dialog_Output, User.Paths.PathType.OUTPUT);
        }
    }
}