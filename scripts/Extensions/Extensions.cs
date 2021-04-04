using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ID3_Tag_Editor.Scripts.Extensions
{
    public static class Extensions
    {
        #region String

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Chars"></param>
        /// <returns></returns>
        public static bool ContainsAny(this string Item, string[] Chars)
        {
            for (int i = 0; i < Chars.Length; i++)
            {
                if (Item.Contains(Chars[i]))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string RemoveLastChar(this string Input)
        {
            return Input.Substring(0, Input.Length - 1);
        }

        public static bool EqualsAny(this string Input, string[] Chars)
        {
            return Input.EqualsAny(Chars, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Chars"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool EqualsAny(this string Input, string[] Chars, bool ignoreCase)
        {
            if (ignoreCase)
                for (int j = 0; j < Chars.Length; j++)
                    if (Input.Equals(Chars[j], StringComparison.OrdinalIgnoreCase))
                        return true;

            else
                for (int i = 0; i < Chars.Length; i++)
                    if (Input == Chars[i])
                        return true;

            return false;
        }

        #endregion

        #region GUI

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// Selects a ComboBoxItem with the given Content. If the item does not exist it will be added to list.
        /// </summary>
        /// <param name="comboBox">The ComboBoxItem which is the parent of the items.</param>
        /// <param name="newContent">The content of the ComboBoxItem that will be selected.</param>
        public static void SelectNewItem(this ComboBox comboBox, string newContent)
        {
            if (newContent != null)
            {
                bool itemexists = false;

                foreach (ComboBoxItem item in comboBox.Items)
                    if (item.Content.Equals(newContent))
                    {
                        itemexists = true;

                        comboBox.SelectedItem = item;

                        break;
                    }

                if (!itemexists)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem()
                    {
                        Content = newContent
                    };

                    comboBox.Items.Add(comboBoxItem);

                    comboBox.SelectedItem = comboBoxItem;
                }
            }
        }

        public static string GetSelectedContent(this ComboBox comboBox)
        {
            foreach (ComboBoxItem item in comboBox.Items)
                if (item == comboBox.SelectedItem)
                    return item.Content.ToString();

            return null;
        }

        #endregion
    }
}
