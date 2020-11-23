using IND.Core.GameLevels;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace IND.Editor.GameLevelsToolkit
{
    public class MoveGameLevelsInstallDirectory : EditorWindow
    {
        public static MoveGameLevelsInstallDirectory window;
        //[InfoBox("Type 'ROOT' if you wish to move the directory to the root directory of the unity project (Assets/)")]
        public string targetDirectoryToMoveToo;

        public static void OpenMenu()
        {
            window = (MoveGameLevelsInstallDirectory)GetWindow(typeof(MoveGameLevelsInstallDirectory));
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox("Type 'ROOT' if you wish to move the directory to the root directory of the unity project '(Assets/)'", MessageType.Info);
            targetDirectoryToMoveToo = EditorGUILayout.TextField("Target Directory To Move To", targetDirectoryToMoveToo);

            if (!string.IsNullOrEmpty(targetDirectoryToMoveToo))
            {
                if (GUILayout.Button("Move Directory"))
                {
                    MoveDirectory();
                }
            }
        }

        private void MoveDirectory()
        {
            if (targetDirectoryToMoveToo == null || targetDirectoryToMoveToo == "")
            {
                Debug.LogError("Directory cannot be empty");
                return;
            }

            GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();

            string targetDirectory = "";
            bool useRootDirectory = false;
            if (targetDirectoryToMoveToo == "ROOT" || targetDirectory == "root")
            {
                targetDirectory = "";
                useRootDirectory = true;
            }
            else
            {
                targetDirectory = targetDirectoryToMoveToo;
            }

            // Create Project Directory if Does Not Exist
            if (useRootDirectory == false)
            {
                if (!Directory.Exists("Assets/" + targetDirectory))
                {
                    //Create
                    AssetDatabase.CreateFolder("Assets", targetDirectory);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            string finalPath = "Assets/" + targetDirectory;

            //Create Scenes Directory Inside of project if does not exist
            if (!Directory.Exists(finalPath + "/" + "Scenes"))
            {
                AssetDatabase.CreateFolder(finalPath, "Scenes");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            string scenesFolderPath = finalPath + "/Scenes";


            //Create Resources Directory  Inside of project if does not exist.
            if (!Directory.Exists(finalPath + "/" + "Resources"))
            {
                AssetDatabase.CreateFolder(finalPath, "Resources");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            string resourcesFolderPath = finalPath + "/Resources";

            //Move Resources Data
            string originalResourcesDirectory = "";
            if (toolkit.settings.useRootProjectDirectoryPath == false)
            {
                originalResourcesDirectory = "Assets/" + toolkit.settings.projectDirectoryPathName + "/Resources/" + SceneAndResourceFolderName.folderNameValue;
            }
            else
            {
                originalResourcesDirectory = "Assets/" + "Resources/" + SceneAndResourceFolderName.folderNameValue;
            }

            FileUtil.MoveFileOrDirectory(originalResourcesDirectory, resourcesFolderPath + "/" + SceneAndResourceFolderName.folderNameValue);
            FileUtil.MoveFileOrDirectory(originalResourcesDirectory + ".meta", resourcesFolderPath + "/" + SceneAndResourceFolderName.folderNameValue + ".meta");

            //Move Game Levels Scenes Directory 
            string originalScenesDirectory = "";
            if (toolkit.settings.useRootProjectDirectoryPath == false)
            {
                originalScenesDirectory = "Assets/" + toolkit.settings.projectDirectoryPathName + "/Scenes/" + SceneAndResourceFolderName.folderNameValue;
            }
            else
            {
                originalScenesDirectory = "Assets/" + "Scenes/" + SceneAndResourceFolderName.folderNameValue;
            }

            FileUtil.MoveFileOrDirectory(originalScenesDirectory, scenesFolderPath + "/" + SceneAndResourceFolderName.folderNameValue);
            FileUtil.MoveFileOrDirectory(originalScenesDirectory + ".meta", scenesFolderPath + "/" + SceneAndResourceFolderName.folderNameValue + ".meta");

            //Update All Scenes Directory Paths in data


            //Update the settings file with the new directory

            if (useRootDirectory == true)
            {
                toolkit.settings.useRootProjectDirectoryPath = true;
                toolkit.settings.projectDirectoryPathName = "";
            }
            else
            {
                toolkit.settings.useRootProjectDirectoryPath = false;
                toolkit.settings.projectDirectoryPathName = targetDirectoryToMoveToo;
            }

            EditorUtility.SetDirty(toolkit.settings);
            EditorUtility.SetDirty(toolkit.gameLevelsData);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Move Operation Completed Succesfully");
        }
    }
}