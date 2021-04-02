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
using ID3_Tag_Editor.Scripts.IO;
using ID3_Tag_Editor.Scripts.UI;
using ID3_Tag_Editor.Scripts.User;
using ID3_Tag_Editor.Scripts.Extensions;
using ID3_Tag_Editor.Scripts.Tags;
using Brushes = System.Windows.Media.Brushes;

namespace ID3_Tag_Editor
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Initialize

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            //TODO
            //Add a better place for this
            Images.ClearCache();

            #region LoadUI Variables
            /*
            UI.allSubMenus = new List<StackPanel>()
            {
                panel_SubMenu_Automation,
                panel_SubMenu_Editor
            };

            UI.UserPathBoxes.Import = TextBox_Dialog_Input;
            UI.UserPathBoxes.Export = TextBox_Dialog_Output;

            //Tags_TEMP.find

            #endregion

            #region LoadUI Graphics

            PanelHandler.HideSubMenus();
            */
            #endregion


            Preferences.LoadSettings();

            //test
            //TagProcessing.ProcessAllSongs();
        }
      
        /*
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
        }*/

        /*private void Button_OpenSubMenu_Automation(object sender, RoutedEventArgs e)
        {
            PanelHandler.HighlightSubMenu(panel_SubMenu_Automation);
        }

        private void Button_OpenSubMenu_Editor(object sender, RoutedEventArgs e)
        {
            PanelHandler.HighlightSubMenu(panel_SubMenu_Editor);
        }*/

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
        /*
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

        private void BC_SelectFile(object sender, RoutedEventArgs e)
        {
            Buttons.SelectFile();
        }

        private void BC_OpenEditor(object sender, RoutedEventArgs e) => Buttons.OpenEditor();

        private void BC_WriteTestLyrics(object sender, RoutedEventArgs e)
        {
            Buttons.OpenEditor();
        }
        */
        #endregion

        #region newButtons

        string currentPath;

        bool saveable = true;

        private void ImportSong_Click(object sender, RoutedEventArgs e)
        {
            //select file
            currentPath = OpenStuff.Files.GetPathFromDialog(null, Paths.Defaults.Music, OpenStuff.Files.Kinds.Music);

            //Cache Info
            Caching.currentFile = new Caching.CachedFile
            {
                fullPath = currentPath,
                File = TagProcessing.GetTagsFromFile(currentPath),
                active = true
            };

            Caching.currentFile.Cover = Caching.currentFile.File.GetCoverImage();

            TB_Interpret.Text = Caching.currentFile.File.Tag.FirstPerformer;
            TB_Title.Text = Caching.currentFile.File.Tag.Title;
            TB_Album.Text = Caching.currentFile.File.Tag.Album;
            TB_Track.Text = Caching.currentFile.File.Tag.Track.ToString();
            TB_Year.Text = Caching.currentFile.File.Tag.Year.ToString();
            TB_Comment.Text = Caching.currentFile.File.Tag.Comment;

            //TODO
            //Check for null
            CB_Genre.SelectNewItem(Caching.currentFile.File.Tag.FirstGenre);

            IMG_Cover.Source = Images.GetCoverForUI(Caching.currentFile.Cover);
        }

        private void SaveTags_Click(object sender, RoutedEventArgs e)
        {
            if (saveable)
            {
                string cover = null;

                if (IMG_Cover.Source != new BitmapImage(new Uri(Images.Ressources.Placeholder)) && IMG_Cover.Source != null)
                {
                    Console.WriteLine((IMG_Cover.Source as BitmapImage).UriSource.LocalPath);

                    cover = (IMG_Cover.Source as BitmapImage).UriSource.LocalPath;

                    /*Console.WriteLine((IMG_Cover.Source as BitmapImage).UriSource);

                    Console.WriteLine((IMG_Cover.Source as BitmapImage).UriSource.AbsoluteUri);

                    Console.WriteLine((IMG_Cover.Source as BitmapImage).UriSource.AbsolutePath);*/

                    //TB_Comment.Text = cover;
                }

                TagProcessing.SaveTags(
                    new TagProcessing.SongFile
                    {
                        Interpret = TB_Interpret.Text,
                        Title = TB_Title.Text,
                        Album = TB_Album.Text,
                        //TODO
                        //add checks to the UI to prevent Exceptions
                        Track = int.Parse(TB_Track.Text),
                        Year = int.Parse(TB_Year.Text),

                        Cover = cover,

                        Genre = CB_Genre.GetSelectedContent()
                    }, currentPath);
            }

            else
                //TODO
                return;
        }

        private void B_SelectCover_Click(object sender, RoutedEventArgs e)
        {
            string path = OpenStuff.Files.GetPathFromDialog("Pick your cover.", Paths.Defaults.Pictures, OpenStuff.Files.Kinds.Image);

            Console.Write(path);
        }

        private void B_DeleteCover_Click(object sender, RoutedEventArgs e)
        {
            if (currentPath != null)
                if (IMG_Cover.Source != new BitmapImage(new Uri(Images.Ressources.Placeholder)) && IMG_Cover.Source != null)
                {
                    TagLib.File newfile = TagProcessing.GetTagsFromFile(currentPath);

                    newfile.SetImage((TagLib.Picture)null);

                    IMG_Cover.Source = Images.GetCoverUI(newfile);
                }
        }

        private void B_DownloadCover_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Changed Events

        private void TB_INT_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            bool parseable = Int32.TryParse(textBox.Text, out _);

            if (parseable)
                textBox.Foreground = Brushes.Black;

            else
                textBox.Foreground = Brushes.Red;

            saveable = parseable;
        }

        #endregion
    }
}