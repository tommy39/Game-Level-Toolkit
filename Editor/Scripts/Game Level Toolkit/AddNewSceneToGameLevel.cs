using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IND.Core.GameLevels;

namespace IND.Editor.GameLevelsToolkit
{
    public class AddNewSceneToGameLevel : EditorWindow
    {
        public static AddNewSceneToGameLevel window;
        public GameLevel selectedGameLevel;
        //[InfoBox("Must Have a Scene Name", InfoMessageType.Error, "HasSceneName")]
        public string sceneNameToCreate;

        #region Unity GUI
        private GameLevelData gameLevelData;
        private int selectedValue = 0;
        private GameLevel[] levels;
        private string[] levelOptions;
        #endregion

        public static void OpenMenu()
        {
            window = (AddNewSceneToGameLevel)GetWindow(typeof(AddNewSceneToGameLevel));
            WindowRefresh.RefreshLevelsList(out window.levelOptions, out window.selectedValue, out window.levels, out window.gameLevelData);
        }

        private void OnGUI()
        {
            selectedValue = EditorGUILayout.Popup("Level To Create Duplicate Of", selectedValue, levelOptions);
            selectedGameLevel = levels[selectedValue];
            sceneNameToCreate = EditorGUILayout.TextField("New Scene Name", sceneNameToCreate);
            if (string.IsNullOrEmpty(sceneNameToCreate)) 
            {
                EditorGUILayout.HelpBox("Name of new Level cannot be empty", MessageType.Error);
            }         

            if (!string.IsNullOrEmpty(sceneNameToCreate))
            {
                if (GUILayout.Button("Add Scene"))
                {
                    AddScene();
                }
            }
        }

        private bool HasSceneName()
        {
            if (sceneNameToCreate == "" || sceneNameToCreate == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void AddScene()
        {
            if (HasSceneName() == true)
            {
                Debug.LogError("Scene Name Is Required");
                return;
            }

            GameLevelToolkit toolkit = GameLevelToolkit.GetRefreshedToolkitWindow();
            string targetSceneFolderPath = GameLevelToolkit.GetProjectPathStringWithSlash();


            //Check If scene Already Exists
            string targetSceneFile = "Assets/" + targetSceneFolderPath + "Scenes/"+ targetSceneFolderPath  + SceneAndResourceFolderName.folderNameValue + "/" + selectedGameLevel.gameLevelName + "_" +sceneNameToCreate + ".unity";
            if (System.IO.File.Exists(targetSceneFile))
            {
                Debug.LogError("Scene Already Exists");
                return;
            }

            CreateScene.CreateEmptyScene(selectedGameLevel.gameLevelName + "_" + sceneNameToCreate, selectedGameLevel.gameLevelName);

            selectedGameLevel.assignedScenes.Add(sceneNameToCreate);

            OpenGameLevel.OpenLevel(selectedGameLevel, true, false, true);
        }

    }
}