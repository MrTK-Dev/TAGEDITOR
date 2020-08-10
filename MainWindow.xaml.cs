﻿using System;
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
using ID3_Tag_Editor.Scripts.IO;
using ID3_Tag_Editor.Scripts.UI;
using ID3_Tag_Editor.Scripts.User;
using ID3_Tag_Editor.Scripts.Extensions;
using ID3_Tag_Editor.Scripts.Tags;

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

            UI.UserPathBoxes.Import = TextBox_Dialog_Input;
            UI.UserPathBoxes.Export = TextBox_Dialog_Output;

            #endregion

            #region LoadUI Graphics

            PanelHandler.HideSubMenus();

            #endregion

            Preferences.LoadSettings();

            //test
            TagProcessing.ProcessAllSongs();
        }

        public void OutputCount(object sender, RoutedEventArgs e)
        {
            /*
            Console.WriteLine("Title: " + Song.Tag.Title);
            Console.WriteLine("Artist: " + Song.Tag.FirstPerformer);
            Console.WriteLine("Album: " + Song.Tag.Album);

            if (Modes.text != Modes.Text_Mode.none)
            {

                if (Modes.text == Modes.Text_Mode.all || Modes.text == Modes.Text_Mode.Folders)
                {
                    string newPath = Paths.Export + @"\" + Song.Tag.Album.Replace(":", " ").Replace("/", "_");

                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);

                        Debug.WriteLine("Created the directory {0}", Song.Tag.Album);
                    }

                    File.Move(allFiles[i], newPath + @"\" + newFileName + ".mp3");
                }

                else if (Modes.text == Modes.Text_Mode.Rename)
                {
                    File.Move(allFiles[i], Paths.Export + @"\" + newFileName + ".mp3");
                }
            }
        */
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
            PanelHandler.HighlightSubMenu(panel_SubMenu_Automation);
        }

        private void Button_OpenSubMenu_Editor(object sender, RoutedEventArgs e)
        {
            PanelHandler.HighlightSubMenu(panel_SubMenu_Editor);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public static class UI
        {
            public static List<StackPanel> allSubMenus = new List<StackPanel>();

            public static class UserPathBoxes
            {
                public static TextBox Import = new TextBox();

                public static TextBox Export = new TextBox();
            }
        }

        #region Button Clicks

        private void BClick_SelectFolder_Input(object sender, RoutedEventArgs e)
        {
            PathMethods.SelectFolder(TextBox_Dialog_Input, Paths.PathType.INPUT);
        }

        private void BClick_SelectFolder_Output(object sender, RoutedEventArgs e)
        {
            PathMethods.SelectFolder(TextBox_Dialog_Output, Paths.PathType.OUTPUT);
        }

        private void BC_ProcessSongs(object sender, RoutedEventArgs e)
        {
            TagProcessing.ProcessAllSongs();

            Preferences.SaveSettings();
        }

        #endregion
    }
}