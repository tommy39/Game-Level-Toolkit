﻿using System.Collections;
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
        private bool automaticallyAddSceneToBuild = false;

        #region Unity GUI
        private GameLevelData gameLevelData;
        private int selectedValue = 0;
        private GameLevel[] levels;
        private string[] levelOptions;
        #endregion

        public static void OpenMenu()
        {
            window = (AddNewSceneToGameLevel)GetWindow(typeof(AddNewSceneToGameLevel), false, "Add New Scene To Level");
            WindowRefresh.RefreshLevelsList(out window.levelOptions, out window.selectedValue, out window.levels, out window.gameLevelData);
        }

        private void OnGUI()
        {
            selectedValue = EditorGUILayout.Popup("Level To Create Duplicate Of", selectedValue, levelOptions);
            selectedGameLevel = levels[selectedValue];
            sceneNameToCreate = EditorGUILayout.TextField("New Scene Name", sceneNameToCreate);

            automaticallyAddSceneToBuild = EditorGUILayout.Toggle("Add to build index", automaticallyAddSceneToBuild);

            bool hasExistingSceneName = false;
            for (int i = 0; i < selectedGameLevel.assignedScenes.Count; i++)
            {
                if (selectedGameLevel.assignedScenes[i] == sceneNameToCreate)
                {
                    hasExistingSceneName = true;
                    break;
                }
                else
                {
                    hasExistingSceneName = false;
                }
            }

            if(hasExistingSceneName == true)
            {
                EditorGUILayout.HelpBox("Scene Name Already Exists in Level", MessageType.Error);
            }

            if (string.IsNullOrEmpty(sceneNameToCreate)) 
            {
                EditorGUILayout.HelpBox("Name of new Level cannot be empty", MessageType.Error);
            }         

            if (!string.IsNullOrEmpty(sceneNameToCreate) && hasExistingSceneName == false)
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

            GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();
            string targetSceneFolderPath = GameLevelToolkitWindow.GetProjectPathStringWithSlash();


            //Check If scene Already Exists
            string targetSceneFile = "Assets/" + targetSceneFolderPath + "Scenes/"+ targetSceneFolderPath  + SceneAndResourceFolderName.folderNameValue + "/" + selectedGameLevel.gameLevelName + "_" +sceneNameToCreate + ".unity";
            if (System.IO.File.Exists(targetSceneFile))
            {
                Debug.LogError("Scene Already Exists");
                return;
            }

            CreateScene.CreateEmptyScene(selectedGameLevel.gameLevelName + "_" + sceneNameToCreate, selectedGameLevel.gameLevelName);

            selectedGameLevel.assignedScenes.Add(sceneNameToCreate);
            EditorUtility.SetDirty(selectedGameLevel);

            if(automaticallyAddSceneToBuild == true)
            {
                List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

                //Get The Existing Build List
                foreach (EditorBuildSettingsScene item in EditorBuildSettings.scenes)
                {
                    editorBuildSettingsScenes.Add(item);
                }

                EditorBuildSettingsScene editorScene = new EditorBuildSettingsScene(targetSceneFile, true);

                if (editorBuildSettingsScenes.Count > 0)
                {
                    bool masterSceneNotFoundInBuild = false;
                    for (int i = 0; i < editorBuildSettingsScenes.Count; i++)
                    {
                        if (editorBuildSettingsScenes[i].path == editorScene.path)
                        {
                            masterSceneNotFoundInBuild = true;
                            break;
                        }
                    }
                    if (masterSceneNotFoundInBuild == false)
                    {
                        editorBuildSettingsScenes.Add(editorScene);
                    }
                }

                EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
            }

            OpenGameLevel.OpenLevel(selectedGameLevel, true, false, true);
        }

    }
}