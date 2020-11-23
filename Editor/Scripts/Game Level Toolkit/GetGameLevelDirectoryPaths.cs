using IND.Core.GameLevels;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IND.Editor.GameLevelsToolkit
{
    public static class GetGameLevelDirectoryPaths
    {
        public static string GetGameLevelScenesFolder(string locationName, bool includeSlashEnding = false)
        {
            GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();
            string projectDirectory = GameLevelToolkitWindow.GetProjectPathStringWithSlash();

            string targetDirectory = "Assets/" + projectDirectory + "Scenes/" + SceneAndResourceFolderName.folderNameValue + "/" + locationName;
            if (includeSlashEnding == true)
            {
                targetDirectory += "/";
            }
            return targetDirectory;
        }

        public static string GetGameLevelResourceFolder(string locationName, bool includeSlashEnding = false)
        {
            GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();
            string projectDirectory = GameLevelToolkitWindow.GetProjectPathStringWithSlash();

            string targetDirectory = "Assets/" + projectDirectory + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + locationName;
            if (includeSlashEnding == true)
            {
                targetDirectory += "/";
            }
            return targetDirectory;
        }

        public static string GetGameLevelAssetFileDirectory(GameLevel level)
        {
            GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();
            string projectDirectory = GameLevelToolkitWindow.GetProjectPathStringWithSlash();

            string gameLevelDirectory = "Assets/" + projectDirectory + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + "Existing " + SceneAndResourceFolderName.folderNameValue+ "/" + level.name + ".asset";
            return gameLevelDirectory;
        }
    }
}