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

            _Debug.Log("Application has started", _Debug.Level.INFO, new _Debug.Tags[] { _Debug.Tags.GUI }, true);

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

        bool saveable = true;

        private void ImportSong_Click(object sender, RoutedEventArgs e)
        {
            //select file
            string currentPath = OpenStuff.Files.GetPathFromDialog(null, Paths.Defaults.Music, OpenStuff.Files.Kinds.Music);

            if (currentPath != null)
            {

                TagLib.File newFile = TagProcessing.GetTagsFromFile(currentPath);

                //Cache Info
                Caching.currentFile = new Caching.CachedFile
                {
                    fullPath = currentPath,
                    Cover = newFile.GetCoverImage(),
                    active = true
                };

                TB_Interpret.Text = newFile.Tag.FirstPerformer;
                TB_Title.Text = newFile.Tag.Title;
                TB_Album.Text = newFile.Tag.Album;
                TB_Track.Text = newFile.Tag.Track.ToString();
                TB_Year.Text = newFile.Tag.Year.ToString();
                TB_Comment.Text = newFile.Tag.Comment;

                CB_Genre.SelectNewItem(newFile.Tag.FirstGenre);

                IMG_Cover.Source = Images.GetCoverForUI(Caching.currentFile.Cover);

                _Debug.Log("Imported: " + currentPath, _Debug.Level.INFO, true);
            }

            else
                _Debug.Log("Import was canceled!", _Debug.Level.INFO, true);
        }

        private void SaveTags_Click(object sender, RoutedEventArgs e)
        {
            if (saveable)
            {
                TagProcessing.SaveTags(
                    new TagProcessing.SongFile
                    {
                        Interpret = TB_Interpret.Text,
                        Title = TB_Title.Text,
                        Album = TB_Album.Text,
                        Comment = TB_Comment.Text,
                        Track = int.Parse(TB_Track.Text),
                        Year = int.Parse(TB_Year.Text),
                        Genre = CB_Genre.GetSelectedContent()
                    });
            }

            else
                //TODO
                return;
        }

        private void B_SelectCover_Click(object sender, RoutedEventArgs e)
        {
            string path = OpenStuff.Files.GetPathFromDialog("Pick your cover.", Paths.Defaults.Pictures, OpenStuff.Files.Kinds.Image);

            if (path != null)
            {
                Caching.currentFile.Cover = new Bitmap(path);

                IMG_Cover.Source = Images.GetCoverForUI(Caching.currentFile.Cover);
            }
        }

        private void B_DeleteCover_Click(object sender, RoutedEventArgs e)
        {
            if (Caching.currentFile.active == true &&
                Caching.currentFile.Cover != null)
            {
                Caching.currentFile.Cover = null;

                IMG_Cover.Source = Images.GetCoverForUI(null);
            }
        }

        private void B_DownloadCover_Click(object sender, RoutedEventArgs e)
        {
            /*
             * Maybe add some kind of indicator or log system for the user.
             * This function does return a string to the file!
             */
            if (Caching.currentFile.active == true &&
                Caching.currentFile.Cover != null)
                Images.SaveBitMapToFile(Caching.currentFile.Cover, OpenStuff.Folders.GetPathFromDialog("Pick the final destination.", Paths.Defaults.Pictures), TB_Title.Text);
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

        #region UI Functions

        public void UpdateLogger(string content)
        {
            TB_Logger.Text = content;
        }

        #endregion
    }
}