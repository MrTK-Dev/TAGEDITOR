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

            public static FolderBrowserDialog OpenFoldersDialog(string description, string startingPath)
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

            public static string GetPathFromFolderDialog(string description, string startingPath)
            {
                return GetPathFromFolderDialog(
                    OpenFoldersDialog(description, startingPath)
                    );
            }

            public static string GetPathFromFolderDialog(FolderBrowserDialog objDialog)
            {
                if (objDialog.SelectedPath != " ")
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
            public static OpenFileDialog OpenFilesDialog(string description, string startingPath)
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

            public static string GetPathFromFilesDialog(string description, string startingPath)
            {
                return GetPathFromFilesDialog(
                    OpenFilesDialog(description, startingPath)
                    );
            }

            public static string GetPathFromFilesDialog(OpenFileDialog objDialog)
            {
                if (objDialog.FileName != " ")
                    return objDialog.FileName;

                else
                    return null;
            }
        }
    }
}
