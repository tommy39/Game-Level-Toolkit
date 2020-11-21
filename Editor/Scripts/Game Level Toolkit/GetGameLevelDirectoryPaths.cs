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
            GameLevelToolkit toolkit = GameLevelToolkit.GetRefreshedToolkitWindow();
            string projectDirectory = GameLevelToolkit.GetProjectPathStringWithSlash();

            string targetDirectory = "Assets/" + projectDirectory + "Scenes/" + SceneAndResourceFolderName.folderNameValue + "/" + locationName;
            if (includeSlashEnding == true)
            {
                targetDirectory += "/";
            }
            return targetDirectory;
        }

        public static string GetGameLevelResourceFolder(string locationName, bool includeSlashEnding = false)
        {
            GameLevelToolkit toolkit = GameLevelToolkit.GetRefreshedToolkitWindow();
            string projectDirectory = GameLevelToolkit.GetProjectPathStringWithSlash();

            string targetDirectory = "Assets/" + projectDirectory + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + locationName;
            if (includeSlashEnding == true)
            {
                targetDirectory += "/";
            }
            return targetDirectory;
        }

        public static string GetGameLevelAssetFileDirectory(GameLevel level)
        {
            GameLevelToolkit toolkit = GameLevelToolkit.GetRefreshedToolkitWindow();
            string projectDirectory = GameLevelToolkit.GetProjectPathStringWithSlash();

            string gameLevelDirectory = "Assets/" + projectDirectory + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + "Existing " + SceneAndResourceFolderName.folderNameValue+ "/" + level.name + ".asset";
            return gameLevelDirectory;
        }
    }
}