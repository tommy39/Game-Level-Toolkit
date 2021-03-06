﻿using UnityEngine;
using UnityEditor;
using System.IO;
using IND.Core.GameLevels;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace IND.Editor.GameLevelsToolkit
{
    public class GameLevelToolkitWindow : EditorWindow
    {
        public static GameLevelToolkitWindow toolkitWindow;

        public bool initialDataHasBeenCreated = false;

        public GameLevelData gameLevelsData;
        private Object levelsDataObj;
        public GameLevelToolkitSettings settings;
        private Object settingsObj;

        public bool useRootProjectDirectoryPath = true;

        public string projectDirectoryPathName = "_ProjectDirectory";


        [MenuItem("Game Level Toolkit/Open Toolkit", false, 3)]
        private static void OpenToolkitWindow()
        {
            toolkitWindow = GetRefreshedToolkitWindow();
            OnEditorQuit();
        }

        public static GameLevelToolkitWindow GetRefreshedToolkitWindow()
        {
            if (toolkitWindow == null)
            {
                toolkitWindow = (GameLevelToolkitWindow)GetWindow(typeof(GameLevelToolkitWindow), false, "Game Level Toolkit");
            }
            toolkitWindow.settings = GetGameLevelToolkitSettings();
            toolkitWindow.gameLevelsData = GetGameLevelsData();

            if (toolkitWindow.gameLevelsData == null || toolkitWindow.settings == null)
            {
                toolkitWindow.initialDataHasBeenCreated = false;

            }
            else
            {
                toolkitWindow.initialDataHasBeenCreated = true;
                toolkitWindow.levelsDataObj = toolkitWindow.gameLevelsData;
                toolkitWindow.settingsObj = toolkitWindow.settings;
            }

            //Check If Initial Required Data Has Been Created
            if (toolkitWindow.initialDataHasBeenCreated == true)
            {
                toolkitWindow.initialDataHasBeenCreated = toolkitWindow.gameLevelsData.initialDataHasBeenCreated;
            }
            return toolkitWindow;
        }

        private void OnGUI()
        {
            if (initialDataHasBeenCreated == false)
            {
                useRootProjectDirectoryPath = EditorGUILayout.Toggle("Use Root Directory For Install", useRootProjectDirectoryPath);
                if (useRootProjectDirectoryPath == false)
                {
                    projectDirectoryPathName = EditorGUILayout.TextField("Target Install Directory", projectDirectoryPathName);
                }

                if (GUILayout.Button("Install Game Level Toolkit"))
                {
                    CreateRequiredInitialData();
                }
            }

            if (initialDataHasBeenCreated == true)
            {
                settingsObj = EditorGUILayout.ObjectField(settingsObj, typeof(ScriptableObject), true);
                levelsDataObj = EditorGUILayout.ObjectField(levelsDataObj, typeof(ScriptableObject), true);

                if (GUILayout.Button("Load Level"))
                {
                    LoadGameLevel();
                }

                if (GUILayout.Button("Load Master Scene"))
                {
                    LoadMasterScene.OpenMenu();
                }

                if (GUILayout.Button("Create New Level"))
                {
                    CreateNewLevel();
                }

                if (GUILayout.Button("Modify Existing Level"))
                {
                    ModifyExistingLevelWindow.OpenMenu();
                }

                if (GUILayout.Button("Modify Categories"))
                {
                    CategoriesWindow.OpenMenu();
                }

                if (GUILayout.Button("Move Install Directory"))
                {
                    MoveInstallDirectory();
                }

                if (GUILayout.Button("Uninstall Toolkit"))
                {
                    UninstallTool();
                }
            }
        }


        public static void OpenLocationsData()
        {
            GameLevelData data = GetGameLevelsData();
            var inspectorType = typeof(EditorGUI).Assembly.GetType("UnityEditor.InspectorWindow");
            var inspectorInstance = ScriptableObject.CreateInstance(inspectorType) as EditorWindow;
            inspectorInstance.Show();
            var prevSelection = Selection.activeGameObject;
            Selection.activeObject = data;
            var isLocked = inspectorType.GetProperty("isLocked", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            isLocked.GetSetMethod().Invoke(inspectorInstance, new object[] { true });
            Selection.activeGameObject = prevSelection;
        }

        public static GameLevelData GetGameLevelsData()
        {
            string[] assetGuid = AssetDatabase.FindAssets(SceneAndResourceFolderName.folderNameValue + " Data");
            string path = "";
            if (assetGuid.Length == 0)
                return null;

            foreach (string item in assetGuid)
            {
                path = AssetDatabase.GUIDToAssetPath(item);
                break;
            }

            GameLevelData data = (GameLevelData)AssetDatabase.LoadAssetAtPath(path, typeof(GameLevelData));
            return data;
        }

        public static GameLevelToolkitSettings GetGameLevelToolkitSettings()
        {
            string[] assetGuid = AssetDatabase.FindAssets(SceneAndResourceFolderName.folderNameValue + " Settings");
            string path = "";

            if (assetGuid.Length == 0)
                return null;

            foreach (string item in assetGuid)
            {
                path = AssetDatabase.GUIDToAssetPath(item);
                break;
            }
            GameLevelToolkitSettings tool = (GameLevelToolkitSettings)AssetDatabase.LoadAssetAtPath(path, typeof(GameLevelToolkitSettings));
            return tool;
        }

        private static string RemoveUneccessaryCharactersFromAssetPath(string originalPath)
        {
            GameLevelToolkitWindow toolkit = toolkitWindow;
            int amountOfCharsToRemoveFromStart = 7;
            if (toolkit.useRootProjectDirectoryPath == false)
            {
                amountOfCharsToRemoveFromStart += toolkit.projectDirectoryPathName.Length + 1;
            }

            amountOfCharsToRemoveFromStart += 10; //Remove the Resources Folder Directory

            string pathAfterStartRemoved = originalPath.Remove(0, amountOfCharsToRemoveFromStart); //Remove the Assets/ + Any Projet Directory Path
            string finalPath = pathAfterStartRemoved.Remove(pathAfterStartRemoved.Length - 6); //Remove the .Asset ending

            return finalPath;
        }

        private void CreateRequiredInitialData()
        {
            string projectPathName = "";
            string projectPathNameWithoutSlash = "";

            GameLevelToolkitWindow toolkit = GetRefreshedToolkitWindow();

            if (toolkit.useRootProjectDirectoryPath == false)
            {
                projectPathName = toolkit.projectDirectoryPathName + "/";
                projectPathNameWithoutSlash = toolkit.projectDirectoryPathName;
            }

            if (!Directory.Exists("Assets/" + projectPathName + "Resources"))
            {
                if (useRootProjectDirectoryPath == true)
                {
                    string resourcesDirectory = AssetDatabase.CreateFolder("Assets", "Resources");
                }
                else
                {
                    string resourcesDirectory = AssetDatabase.CreateFolder("Assets/" + projectPathNameWithoutSlash, "Resources");
                }
            }

            if (!Directory.Exists("Assets/" + projectPathName + "Scenes"))
            {
                if (useRootProjectDirectoryPath == true)
                {
                    string resourcesDirectory = AssetDatabase.CreateFolder("Assets", "Scenes");
                }
                else
                {
                    string resourcesDirectory = AssetDatabase.CreateFolder("Assets/" + projectPathNameWithoutSlash, "Scenes");
                }
            }

            if (!Directory.Exists("Assets/" + projectPathName + "Resources/" + SceneAndResourceFolderName.folderNameValue))
            {
                //Create locationsResourceDirectory Directory
                string gameLevelsResourceDirectoryGuid = AssetDatabase.CreateFolder("Assets/" + projectPathName + "Resources", SceneAndResourceFolderName.folderNameValue);
                string gameLevelsResourceDirectoryPath = AssetDatabase.GUIDToAssetPath(gameLevelsResourceDirectoryGuid);
            }

            if (!Directory.Exists("Assets/" + projectPathName + "Scenes/" + SceneAndResourceFolderName.folderNameValue))
            {
                //Create Locations Directory in Scenes
                string gameLevelsScenesDirectory = AssetDatabase.CreateFolder("Assets/" + projectPathName + "Scenes", SceneAndResourceFolderName.folderNameValue);
            }

            //Create Game Levels Data
            string desiredGameLevelsDataDirectory = "Assets/" + projectPathName + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + SceneAndResourceFolderName.folderNameValue + " Data.asset";
            if (!Directory.Exists(desiredGameLevelsDataDirectory))
            {
                AssetDatabase.CreateAsset(GameLevelData.CreateInstance<GameLevelData>(), desiredGameLevelsDataDirectory);

            }

            //Create Game Levels Settings
            string desiredGameLevelsSettingsDirectory = "Assets/" + projectPathName + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + SceneAndResourceFolderName.folderNameValue + " Settings.asset";
            if (!Directory.Exists(desiredGameLevelsSettingsDirectory))
            {
                AssetDatabase.CreateAsset(CreateInstance<GameLevelToolkitSettings>(), desiredGameLevelsSettingsDirectory);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            toolkit.gameLevelsData = Resources.Load(SceneAndResourceFolderName.folderNameValue + "/" + SceneAndResourceFolderName.folderNameValue + " Data") as GameLevelData;
            toolkit.settings = Resources.Load(SceneAndResourceFolderName.folderNameValue + "/" + SceneAndResourceFolderName.folderNameValue + " Settings") as GameLevelToolkitSettings;
            toolkit.settings.useRootProjectDirectoryPath = toolkit.useRootProjectDirectoryPath;
            toolkit.settings.projectDirectoryPathName = toolkit.projectDirectoryPathName;

            //Create Master Scene
            if (!Directory.Exists("Assets/" + projectPathName + "Scenes/" + SceneAndResourceFolderName.folderNameValue + "/MasterScene"))
            {
                //Create Master Scene Directory
                string dir = AssetDatabase.CreateFolder("Assets/" + projectPathName + "Scenes/" + SceneAndResourceFolderName.folderNameValue, "Master Scene");
                Scene masterScene = CreateScene.CreateEmptyScene("Master Scene", "Master Scene");
                EditorSceneManager.OpenScene("Assets/" + projectPathName + "Scenes/" + SceneAndResourceFolderName.folderNameValue + "/Master Scene/Master Scene.unity");
            }

            EditorUtility.SetDirty(toolkit.settings);
            EditorUtility.SetDirty(toolkit.gameLevelsData);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorSceneManager.MarkAllScenesDirty();

            //Setup Master Scene
            Scene currentScene = SceneManager.GetActiveScene();
            // GameObject geo = new GameObject();
            GameObject go = new GameObject("Game Level Manager");
            go.AddComponent<GameLevelManager>();

            EditorSceneManager.SaveOpenScenes();

            BuildSettingsSceneManagement.AddMasterSceneToBuild();

            initialDataHasBeenCreated = true;
        }

        private void LoadGameLevel()
        {
            OpenGameLevel.OpenMenu();
        }

        private void CreateNewLevel()
        {
            CreateNewGameLevelMenu.OpenMenu();
        }

        private void MoveInstallDirectory()
        {
            MoveGameLevelsInstallDirectory.OpenMenu();
        }

        private void UninstallTool()
        {
            UninstallToolkit.OpenMenu();
        }

        public static string GetProjectPathStringWithSlash()
        {
            GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();

            string pathName = "";
            if (toolkit.settings.useRootProjectDirectoryPath == false)
            {
                pathName = toolkit.settings.projectDirectoryPathName + "/";
            }
            return pathName;
        }
        static void OnEditorQuit()
        {
            EditorApplication.quitting += toolkitWindow.Close;
        }
    }
}