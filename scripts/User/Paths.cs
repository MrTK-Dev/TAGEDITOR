using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using TextBox = System.Windows.Controls.TextBox;
using System.Reflection;
using System.Diagnostics;
using static ID3_Tag_Editor.Scripts.User.Paths;
using ID3_Tag_Editor.Scripts.UI;

namespace ID3_Tag_Editor.Scripts.User
{
    /// <summary>
    /// 
    /// </summary>
    public static class Paths
    {
        public static class Defaults
        {
            public static string Music = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            public static string Pictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }

        public static string GetApplicationPath()
        {
            //ONLY WHILE DEBUG
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);

            return Path.GetFullPath(Path.Combine(new FileInfo(location.AbsolutePath).Directory.FullName + @"\", @"..\..\")).Replace("%", " ").Replace("20", "");
        }

        public static string GetFullPath(string Path)
        {
            return GetFullPath(Path, null);
        }

        public static string GetFullPath(string Path, string fileName)
        {
            if (fileName == null)
                return GetApplicationPath() + Path;

            return GetApplicationPath() + Path + @"/" + fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string Import = Defaults.Music;

        /// <summary>
        /// 
        /// </summary>
        public static string Export = Import;

        /// <summary>
        /// 
        /// </summary>
        public static string ImageFile = null;

        public enum PathType
        {
            INPUT,
            OUTPUT,
            NONE
        }
    }

    public partial class PathMethods : MainWindow
    {
        public static void SelectFolder(TextBox textBox, PathType pathType)
        {
            if (pathType != PathType.NONE)
            {
                if (pathType == PathType.INPUT)
                {
                    Import = OpenStuff.Folders.GetPathFromDialog("Choose the folder that contains the unprocessed files.\nThis setting will be saved.", Import);

                    textBox.Text = Import;
                }

                else if (pathType == PathType.OUTPUT)
                {
                    Export = OpenStuff.Folders.GetPathFromDialog("Choose the folder that should contain the processed files.\nThis setting will be saved.", Export);

                    textBox.Text = Export;
                }

                Preferences.SaveSettings();
            }

            else
            {
                //Error
            }
        }
    }

    public partial class OpenFileDialogSample : MainWindow
    {
        public OpenFileDialogSample()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textBox"></param>
        public static void OpenDialog(TextBox textBox)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Open Import Path",

                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)

                //Filter
            };

            //if (openFileDialog.ShowDialog() == true)
                textBox.Text = Path.GetFileName(openFileDialog.FileName);
        }
    }
}