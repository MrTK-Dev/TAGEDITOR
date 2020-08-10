using System.Diagnostics;
using ID3_Tag_Editor.Scripts.IO;

namespace ID3_Tag_Editor.Scripts.User
{
    /// <summary>
    /// This class handles everything related to saving and loading <see cref="Settings"/> made by the user. This heavily relies on the <see cref="FileSystem"/> by saving data on files.
    /// </summary>
    class Preferences
    {
        #region Variables

        /// <summary>
        /// The default path to the settings of the user.
        /// </summary>
        static readonly string settingsPath = @"User/Settings";

        /// <summary>
        /// Name of the settings file.
        /// </summary>
        static readonly string settingsFile = "Settings";

        /// <summary>
        /// This works as a switch if it is the first run of the application to make some preparations.
        /// </summary>
        static bool firstRun = true;

        #endregion

        #region Classes

        #region Main

        /// <summary>
        /// This class saves all preferences choosen by the user. This works as a cache before saving it to a json file.
        /// </summary>
        [System.Serializable]
        public class Settings
        {
            /// <summary>
            /// This class saves all preferences regarding paths on the user's PC.
            /// </summary>
            public UserPaths UserPaths { get; set; }

            /// <summary>
            /// This class saves all preferences regarding the tagsystem.
            /// </summary>
            public UserTags UserTags { get; set; }
        }

        #endregion

        #region Paths

        /// <summary>
        /// Stores paths to folders that the user choose.
        /// </summary>
        [System.Serializable]
        public class UserPaths
        {
            /// <summary>
            /// The folder containing the files PRE-Processing
            /// </summary>
            public string Import { get; set; }

            /// <summary>
            /// The folder containing the files POST-Processing
            /// </summary>
            public string Export { get; set; }
        }

        #endregion

        #region Tags

        /// <summary>
        /// Stores preferences regarding the tag system.
        /// </summary>
        [System.Serializable]
        public class UserTags
        {
            /// <summary>
            /// How the processed songs should get saved.
            /// </summary>
            public Tags.Modes.ExportTarget ExportTarget { get; set; }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Saves all cached preferences to a json file.
        /// </summary>
        public static void SaveSettings()
        {
            Settings UserSettings = new Settings
            {
                UserPaths = new UserPaths
                {
                    Import = Paths.Import,
                    Export = Paths.Export
                },
                UserTags = new UserTags
                {
                    ExportTarget = Tags.Modes.exportTarget
                }
            };

            FileSystem.SaveToJSON(settingsPath, settingsFile, UserSettings);
        }

        /// <summary>
        /// Loads every setting from files in the cache and applies it to the application.
        /// <para>If it is the first run, the function checks for the folder under <see cref="settingsPath"/> and creates it, if it is not there yet.
        /// It also checks, if the savefiles already exist. If not, they will be created with the default settings.</para>
        /// <para>The function loads following modules:</para>
        /// <list type="bullet">
        /// <item>
        /// <term><see cref="UserPaths"/></term>
        /// <description>Paths for the files.</description>
        /// </item>
        /// <item>
        /// <term><see cref="UserTags"/></term>
        /// <description>Settings for the tag system.</description>
        /// </item>
        /// </list>
        /// </summary>
        public static void LoadSettings()
        {
            if (firstRun)
            {
                if (!FileSystem.IsDirectory(settingsPath))
                    FileSystem.CreateDirectory(settingsPath);

                if (!FileSystem.IsFile(settingsPath, settingsFile + ".json"))
                    SaveSettings();

                firstRun = false;
            }

            //cache
            Settings newUserSettings = JSON.ReadSettings(settingsPath, settingsFile);

            Paths.Import = newUserSettings.UserPaths.Import;
            Paths.Export = newUserSettings.UserPaths.Export;

            //TODO Create Method in different class
            MainWindow.UI.UserPathBoxes.Import.Text = Paths.Import;
            MainWindow.UI.UserPathBoxes.Export.Text = Paths.Export;

            Tags.Modes.exportTarget = newUserSettings.UserTags.ExportTarget;
        }

        #endregion
    }
}
