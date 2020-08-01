using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ID3_Tag_Editor.Scripts.UI
{
    /// <summary>
    /// Handles everything related to Panels including the DropDownMenu.
    /// </summary>
    public static class PanelHandler
    {
        /// <summary>
        /// 
        /// </summary>
        private static Panel ActivePanel;

        /// <summary>
        /// Reference to the MainWindow class.
        /// </summary>
        static MainWindow Main = Application.Current.Windows[0] as MainWindow;

        /// <summary>
        /// Hides all SubMenus declared in MainWindow.UI.
        /// </summary>
        public static void HideSubMenus()
        {
            foreach (var SubMenu in MainWindow.UI.allSubMenus)
            {
                SubMenu.Visibility = Visibility.Hidden;

                SubMenu.MaxHeight = 0;
            }
        }

        /// <summary>
        /// Enables the given SubMenu.
        /// </summary>
        /// <param name="SubMenu">The Panel that gets enabled.</param>
        public static void ShowSubMenu(Panel SubMenu)
        {
            if (SubMenu != ActivePanel)
            {
                SubMenu.Visibility = Visibility.Visible;
                SubMenu.MaxHeight = 80;

                ActivePanel = SubMenu;
            }

            else
                ActivePanel = null;
        }

        /// <summary>
        /// Hides all SubMenu except the given one.
        /// </summary>
        /// <param name="SubMenu">Highlighted SubMenu</param>
        public static void HighlightSubMenu(Panel SubMenu)
        {
            HideSubMenus();
            ShowSubMenu(SubMenu);
        }
    }
}
