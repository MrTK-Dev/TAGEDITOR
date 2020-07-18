using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using TextBox = System.Windows.Controls.TextBox;

namespace ID3_Tag_Editor.scripts
{
    class OpenStuff
    {
        public partial class Folders : MainWindow
        {
            public Folders()
            {
                InitializeComponent();
            }

            public static string OpenDialog(string description, string startingPath)
            {
                FolderBrowserDialog objDialog = new FolderBrowserDialog();

                objDialog.Description = description;

                objDialog.SelectedPath = startingPath;

                DialogResult newResult = objDialog.ShowDialog();

                if (newResult == System.Windows.Forms.DialogResult.OK)
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
}
