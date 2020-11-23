using IND.Core.GameLevels;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IND.Editor.GameLevelsToolkit
{
    public static class DoesGameLevelExistStatic
    {
        public static bool ReturnBool(string locationName)
        {
            GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();
            string projectPathName = GameLevelToolkitWindow.GetProjectPathStringWithSlash();

            GameLevel existingGameLevel = (GameLevel)AssetDatabase.LoadAssetAtPath("Assets/" + projectPathName + "/Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + locationName + "/" + locationName + ".asset", typeof(GameLevel));
            if (existingGameLevel != null)
            {
                Debug.LogError("Location With Same Name Already Exists");
                return true;
            }
            return false;
        }

        public static GameLevel ReturnGameLevel(string locationName)
        {
            GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();
            string projectPathName = GameLevelToolkitWindow.GetProjectPathStringWithSlash();

            GameLevel existingGameLevel = (GameLevel)AssetDatabase.LoadAssetAtPath("Assets/" + projectPathName + "/Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + locationName + "/" + locationName + ".asset", typeof(GameLevel));
            if (existingGameLevel != null)
            {
                Debug.LogError("Location With Same Name Already Exists");
                return existingGameLevel;
            }
            return null;
        }

        public static GameLevel ReturnLocationFromDirectory(string directory)
        {
            GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();
            string projectPathName = GameLevelToolkitWindow.GetProjectPathStringWithSlash();

            GameLevel existingGameLevel = (GameLevel)AssetDatabase.LoadAssetAtPath("Assets/" + projectPathName + "/Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + directory + "/" + directory + ".asset", typeof(GameLevel));
            return null;
        }
    }
}