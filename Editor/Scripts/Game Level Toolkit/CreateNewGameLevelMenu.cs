using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using IND.Core.GameLevels;
using System.IO;

namespace IND.Editor.GameLevelsToolkit
{
    public class CreateNewGameLevelMenu : EditorWindow
    {
        public static CreateNewGameLevelMenu window;

        public string gameLevelName;
        public List<NewGameLevelScene> scenesToCreateInGameLevel = new List<NewGameLevelScene>();
        public List<GameLevel> gameLevelDependencies = new List<GameLevel>();
        private bool hasEmptySceneName = false;
        private bool hasEmptyDependency = false;
        private bool addLevelToUnityBuildScenes = false;
        private GameLevelData gameLevelData;

        public static void OpenMenu()
        {
            window = (CreateNewGameLevelMenu)GetWindow(typeof(CreateNewGameLevelMenu));
            window.gameLevelData = GameLevelToolkitWindow.GetGameLevelsData();
        }

        private void OnGUI()
        {
            bool doesLevelNameExist = false;
            for (int i = 0; i < gameLevelData.gameLevelsCreatedByUser.Count; i++)
            {
                if (gameLevelData.gameLevelsCreatedByUser[i].gameLevelName == gameLevelName)
                {
                    doesLevelNameExist = true;
                    break;
                }
                else
                {
                    doesLevelNameExist = false;
                }
            }
    
            gameLevelName = EditorGUILayout.TextField("New Level Name", gameLevelName);
            if (gameLevelName == null || gameLevelName == "")
            {
                EditorGUILayout.HelpBox("Game Level Name Cannot Be Empty", MessageType.Error);
            }
            if(doesLevelNameExist == true)
            {
                EditorGUILayout.HelpBox("Game Level Already Exists", MessageType.Error);
            }
            if (scenesToCreateInGameLevel.Count == 0)
            {
                EditorGUILayout.HelpBox("Levels Require at least one scene", MessageType.Error);
            }

            if (scenesToCreateInGameLevel.Count > 0)
            {
                for (int i = 0; i < scenesToCreateInGameLevel.Count; i++)
                {
                    if (scenesToCreateInGameLevel[i].sceneName == "" || scenesToCreateInGameLevel[i].sceneName == null)
                    {
                        hasEmptySceneName = true;
                        break;
                    }
                    else
                    {
                        hasEmptySceneName = false;
                    }
                }
            }
            else
            {
                hasEmptySceneName = false;
            }

            //Dependencies
            ScriptableObject target = this;
            SerializedObject so = new SerializedObject(target);
            SerializedProperty dependenciesProperty = so.FindProperty("gameLevelDependencies");
            SerializedProperty scenesProperty = so.FindProperty("scenesToCreateInGameLevel");
            EditorGUILayout.PropertyField(scenesProperty, true); // True means show children
            EditorGUILayout.PropertyField(dependenciesProperty, true); 
            so.ApplyModifiedProperties(); // Remember to apply modified properties

            if (hasEmptySceneName == true)
            {
                EditorGUILayout.HelpBox("Scene Names Cannot Be Empty", MessageType.Error);
            }

            if (gameLevelDependencies.Count > 0)
            {
                for (int i = 0; i < gameLevelDependencies.Count; i++)
                {
                    if (gameLevelDependencies[i] == null)
                    {
                        hasEmptyDependency = true;
                        break;
                    }
                    else
                    {
                        hasEmptyDependency = false;
                    }
                }
            }
            else
            {
                hasEmptyDependency = false;
            }

            if(hasEmptyDependency == true)
            {
                EditorGUILayout.HelpBox("Dependency value cannot be null", MessageType.Error);
            }

            addLevelToUnityBuildScenes = GUILayout.Toggle(addLevelToUnityBuildScenes, "Add Level To Project Scene Build Settings");

            if (gameLevelName != null && gameLevelName != "" && hasEmptySceneName == false && hasEmptyDependency == false && doesLevelNameExist == false && scenesToCreateInGameLevel.Count > 0)
            {
                if (GUILayout.Button("Create Level"))
                {
                    CreateLevel();
                }
            }
        }

        private void CreateLevel()
        {
            GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();
            string projectPathName = GameLevelToolkitWindow.GetProjectPathStringWithSlash();                      

            //Check If the Location Already Exists
            GameLevel existingLocation = (GameLevel)AssetDatabase.LoadAssetAtPath("Assets/" + projectPathName + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + gameLevelName + "/" + gameLevelName + ".asset", typeof(GameLevel));
            if (existingLocation != null)
            {
                Debug.LogError("Level With Same Name Already Exists");
                return;
            }

            //First We Will need to make sure we have all the required data before proceeding
            if (gameLevelName == null || gameLevelName == "")
            {
                Debug.LogError("Level Name Cannot Be Empty");
                return;
            }

            //Check If Any Scene Names are empty
            for (int i = 0; i < scenesToCreateInGameLevel.Count; i++)
            {
                if (scenesToCreateInGameLevel[i].VerifySceneNameIsNotEmpty() == true)
                {
                    Debug.LogError("Scene Name Cannot Be Empty");
                    return;
                }
            }

            for (int i = 0; i < scenesToCreateInGameLevel.Count; i++)
            {
                string sceneDirectory = gameLevelName;
                CreateScene.CreateEmptyScene(gameLevelName + "_" + scenesToCreateInGameLevel[i].sceneName, sceneDirectory);
            }

            //Create the Location Resources
            string resourceFolder = AssetDatabase.CreateFolder("Assets/" + projectPathName + "Resources/Locations", gameLevelName);
            string newFolderPath = AssetDatabase.GUIDToAssetPath(resourceFolder);

            //Check Does FolderExist To Place Resource File Inside
            if (!Directory.Exists("Assets/" + projectPathName + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/Existing " + SceneAndResourceFolderName.folderNameValue))
            {
                AssetDatabase.CreateFolder("Assets/" + projectPathName + "Resources/" + SceneAndResourceFolderName.folderNameValue, "Existing " + SceneAndResourceFolderName.folderNameValue);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            GameLevel gameLevelResource = GameLevelData.CreateInstance<GameLevel>();
            AssetDatabase.CreateAsset(GameLevel.CreateInstance<GameLevel>(), "Assets/" + projectPathName + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/Existing " + SceneAndResourceFolderName.folderNameValue + "/" + gameLevelName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();


            GameLevel createdGameLevel = (GameLevel)AssetDatabase.LoadAssetAtPath("Assets/" + projectPathName + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/Existing " + SceneAndResourceFolderName.folderNameValue + "/" + gameLevelName + ".asset", typeof(GameLevel));
            createdGameLevel.gameLevelName = gameLevelName;
            createdGameLevel.assignedScenesDirectory = "Assets/" + projectPathName + "Scenes/" + SceneAndResourceFolderName.folderNameValue + "/" + gameLevelName;
            GameLevelToolkitWindow.GetGameLevelsData().gameLevelsCreatedByUser.Add(createdGameLevel);
            EditorUtility.SetDirty(createdGameLevel); 

            for (int i = 0; i < scenesToCreateInGameLevel.Count; i++)
            {
                createdGameLevel.assignedScenes.Add(scenesToCreateInGameLevel[i].sceneName);
            }

            for (int i = 0; i < gameLevelDependencies.Count; i++)
            {
                createdGameLevel.levelDependencies.Add(gameLevelDependencies[i]);
            }
            EditorUtility.SetDirty(toolkit.gameLevelsData);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            OpenGameLevel.OpenLevel(createdGameLevel, true, false, true);

            if(addLevelToUnityBuildScenes == true)
            {
                BuildSettingsSceneManagement.AddLevelToBuild(createdGameLevel);
            }
        }

        private bool CheckIfHasNoLocationName()
        {
            if (gameLevelName == "" || gameLevelName == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckIfHasNoScenes()
        {
            if (scenesToCreateInGameLevel.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    [System.Serializable]
    public class NewGameLevelScene
    {
        //[InfoBox("Scene Name cannot be empty", InfoMessageType.Error, "VerifySceneNameIsNotEmpty")]
        public string sceneName;

        public bool VerifySceneNameIsNotEmpty()
        {
            if (sceneName == "" || sceneName == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}