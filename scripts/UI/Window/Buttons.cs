using ID3_Tag_Editor.Scripts.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ID3_Tag_Editor.Scripts.UI.Window
{
    public class Buttons
    {
        public void OpenEditor()
        {
            Debug.WriteLine("It does work - sick");
        } 

        public void SelectFile()
        {
            Debug.WriteLine("It does work - sick!!!");

            string path = OpenStuff.Files.GetPathFromDialog("Choose File", Paths.Import);

            //var myTextBlock = MainWindow.FindTextBox("Song_Title");

            //var myTextBlock = Window.UI.

            //myTextBlock.Text = Tags.TagProcessing.GetTagsFromFile(path).Tag.Title;

            Debug.WriteLine(path);
        }
    }
}
