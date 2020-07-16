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

namespace User
{
    /// <summary>
    /// 
    /// </summary>
    public static class Paths
    {
        /// <summary>
        /// 
        /// </summary>
        //private static string DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        private static string DefaultPath = @"F:\Μουσική\________";

        /// <summary>
        /// 
        /// </summary>
        public static string Import = DefaultPath;

        /// <summary>
        /// 
        /// </summary>
        public static string Export = DefaultPath;

        /// <summary>
        /// 
        /// </summary>
        public static string ImageFile = null;
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

    public partial class FolderBrowserDialogSample : MainWindow
    {
        public FolderBrowserDialogSample()
        {
            InitializeComponent();
        }

        public static void OpenDialog(TextBox textBox)
        {
            FolderBrowserDialog objDialog = new FolderBrowserDialog();

            objDialog.Description = "lolsfjasfjafjsijaf";

            objDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            DialogResult newResult = objDialog.ShowDialog();

            if (newResult == System.Windows.Forms.DialogResult.OK)
                textBox.Text = objDialog.SelectedPath;

            else
                textBox.Text = "Invalid Input!";
        }
    }
}