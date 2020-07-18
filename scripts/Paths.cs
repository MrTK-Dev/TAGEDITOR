using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using ID3_Tag_Editor;
using System.Windows.Forms;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using TextBox = System.Windows.Controls.TextBox;
using ID3_Tag_Editor.scripts;
using static User.Paths;

namespace User
{
    /// <summary>
    /// 
    /// </summary>
    public static class Paths
    {
        public static class Defaults
        {
            public static string Music = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
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
                string newPath = OpenStuff.Folders.OpenDialog("Hallo", Defaults.Music);

                if (pathType == PathType.INPUT)
                    Import = newPath;

                else if (pathType == PathType.OUTPUT)
                    Export = newPath;

                textBox.Text = newPath;
            }

            else
            {
                //Error
            }
        }

        public static void SelectFile()
        {

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