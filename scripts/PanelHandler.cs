using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ID3_Tag_Editor;

namespace Scripts
{
    public static class PanelHandler
    {
        static MainWindow Form = Application.Current.Windows[0] as MainWindow;

        public static void HideSubMenus()
        {
            foreach (var SubMenu in MainWindow.UI.allSubMenus)
            {
                SubMenu.Visibility = Visibility.Hidden;

                SubMenu.MaxHeight = 0;
            }
        }

        public static void ShowSubMenu(Panel SubMenu)
        {
            SubMenu.Visibility = Visibility.Visible;
            SubMenu.MaxHeight = 80;
        }

        public static void HighlightSubMenu(Panel SubMenu)
        {
            HideSubMenus();
            ShowSubMenu(SubMenu);
        }
    }
}
