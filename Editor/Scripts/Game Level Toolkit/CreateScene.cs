using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IND.Editor.GameLevelsToolkit
{
    public static class CreateScene
    {
        public static Scene CreateEmptyScene(string sceneName, string parentFolderName)
        {
            GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();
            string projectDirectory = null;
            if (toolkit.settings == null)
            {
                projectDirectory = toolkit.projectDirectoryPathName;
            }
            else
            {
                projectDirectory = GameLevelToolkitWindow.GetProjectPathStringWithSlash();
            }

            if (!Directory.Exists("Assets/" + projectDirectory + "Scenes/" + SceneAndResourceFolderName.folderNameValue + "/" + parentFolderName))
            {
                AssetDatabase.CreateFolder("Assets/" + projectDirectory + "Scenes/" + SceneAndResourceFolderName.folderNameValue, parentFolderName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            string fullSceneDirectory = "Assets/" + projectDirectory + "Scenes/" + SceneAndResourceFolderName.folderNameValue + "/" + parentFolderName + "/";
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            newScene.name = sceneName;
            EditorSceneManager.SaveScene(newScene, fullSceneDirectory + sceneName + ".unity");
            return newScene;
        }
    }
}