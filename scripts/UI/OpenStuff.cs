using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using TextBox = System.Windows.Controls.TextBox;

namespace ID3_Tag_Editor.Scripts.UI
{
    class OpenStuff
    {
        public partial class Folders : MainWindow
        {
            public Folders()
            {
                InitializeComponent();
            }

            public static FolderBrowserDialog OpenDialog(string description, string startingPath)
            {
                FolderBrowserDialog objDialog = new FolderBrowserDialog
                {
                    Description = description,

                    SelectedPath = startingPath
                };

                if (objDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    return objDialog;

                return null;
            }

            public static string GetPathFromDialog(string description, string startingPath)
            {
                return GetPathFromDialog(
                    OpenDialog(description, startingPath)
                    );
            }

            public static string GetPathFromDialog(FolderBrowserDialog objDialog)
            {
                if (objDialog != null)
                    return objDialog.SelectedPath;

                else
                    return null;
            }
        }

        public partial class Files : MainWindow
        {
            public Files()
            {
                InitializeComponent();
            }

            //TODO add ability to add multiple files
            //https://www.wpf-tutorial.com/dialogs/the-openfiledialog/
            public static OpenFileDialog OpenDialog(string description, string startingPath)
            {
                OpenFileDialog objDialog = new OpenFileDialog
                {
                    Filter = "Music Files (*.mp3)|*.mp3",

                    Title = "File Selection",


                    InitialDirectory = startingPath
                };

                if (objDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    return objDialog;

                return null;
            }

            public static string GetPathFromDialog(string description, string startingPath)
            {
                return GetPathFromDialog(
                    OpenDialog(description, startingPath)
                    );
            }

            public static string GetPathFromDialog(OpenFileDialog objDialog)
            {
                if (objDialog != null)
                    return objDialog.FileName;

                else
                    return null;
            }
        }
    }
}
