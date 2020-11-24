using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IND.Core.GameLevels;

namespace IND.Editor.GameLevelsToolkit
{
    public class AddLevelToBuildWindow : EditorWindow
    {
        public static AddLevelToBuildWindow window;
        private GameLevel targetLevel;

        #region Unity GUI
        //Game Level
        private GameLevelData gameLevelData;
        private int selectedLevelValue = 0;
        private GameLevel[] targetLevels;
        private string[] targetLevelOptions;
        #endregion

        public static void OpenMenu()
        {
            window = GetWindow<AddLevelToBuildWindow>();
            WindowRefresh.RefreshLevelsList(out window.targetLevelOptions, out window.selectedLevelValue, out window.targetLevels, out window.gameLevelData);
            window.FilterPotentialLevels();
            if (window.targetLevels.Length > 0)
            {
                window.targetLevel = window.targetLevels[window.selectedLevelValue];
            }
        }

        private void OnGUI()
        {
            if (targetLevels.Length > 0)
            {
                selectedLevelValue = EditorGUILayout.Popup("Selected Level", selectedLevelValue, targetLevelOptions);
                targetLevel = targetLevels[selectedLevelValue];

                if (GUILayout.Button("Add Selected Level To Build"))
                {
                    AddSelectedLevel();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("All Possible Levels have been added to the build", MessageType.Info);
            }
        }

        private void AddSelectedLevel()
        {
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = BuildSettingsSceneManagement.GetEditorBuildSettingsScenes();

            //Add The Master Scene
            string masterscenePath = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/" + "Game Levels/Master Scene/Master Scene.unity";
            EditorBuildSettingsScene masterScene = new EditorBuildSettingsScene(masterscenePath, true);

            if (editorBuildSettingsScenes.Count > 0)
            {
                bool masterSceneNotFoundInBuild = false;
                for (int i = 0; i < editorBuildSettingsScenes.Count; i++)
                {
                    if (editorBuildSettingsScenes[i].path == masterScene.path)
                    {
                        masterSceneNotFoundInBuild = true;
                        break;
                    }
                }
                if (masterSceneNotFoundInBuild == false)
                {
                    editorBuildSettingsScenes.Add(masterScene);
                }
            }
            else
            {
                editorBuildSettingsScenes.Add(masterScene);
            }

            //Add Game Level
            for (int i = 0; i < targetLevel.assignedScenes.Count; i++)
            {
                bool sceneExistsInBuild = false;
                string scenePath = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/Game Levels/" + targetLevel.gameLevelName + "/" + targetLevel.gameLevelName + "_" + targetLevel.assignedScenes[i] + ".unity";
                EditorBuildSettingsScene scene = new EditorBuildSettingsScene(scenePath, true);

                for (int g = 0; g < editorBuildSettingsScenes.Count; g++)
                {
                    if (editorBuildSettingsScenes[g].path == scene.path)
                    {
                        sceneExistsInBuild = true;
                        break;
                    }
                }

                if (sceneExistsInBuild == false)
                {
                    editorBuildSettingsScenes.Add(scene);
                }
            }

            //Add Dependencies
            for (int i = 0; i < targetLevel.levelDependencies.Count; i++)
            {
                for (int g = 0; g < targetLevel.levelDependencies[i].assignedScenes.Count; g++)
                {
                    bool sceneExistsInBuild = false;
                    string gameLevelName = targetLevel.levelDependencies[i].gameLevelName;
                    string scenePath = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/Game Levels/" + gameLevelName + "/" + gameLevelName + "_" + targetLevel.levelDependencies[i].assignedScenes[g] + ".unity"; 
                    EditorBuildSettingsScene targetScene = new EditorBuildSettingsScene(scenePath, true);

                    for (int f = 0; f < editorBuildSettingsScenes.Count; f++)
                    {
                        if (editorBuildSettingsScenes[f].path == targetScene.path)
                        {
                            sceneExistsInBuild = true;
                            break;
                        }
                    }

                    if (sceneExistsInBuild == false)
                    {
                        editorBuildSettingsScenes.Add(targetScene);
                    }
                }
            }

            //Add Dependencies of Dependencies
            for (int i = 0; i < targetLevel.levelDependencies.Count; i++)
            {
                for (int g = 0; g < targetLevel.levelDependencies[i].levelDependencies.Count; g++)
                {
                    GameLevel targetDependency = targetLevel.levelDependencies[i].levelDependencies[g];

                    for (int f = 0; f < targetDependency.assignedScenes.Count; f++)
                    {
                        bool sceneExistsInBuild = false;
                        string gameLevelName = targetDependency.gameLevelName;
                        string scenePath = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/Game Levels/" + gameLevelName + "/" + gameLevelName + "_" + targetDependency.assignedScenes[f] + ".unity"; 
                        EditorBuildSettingsScene targetScene = new EditorBuildSettingsScene(scenePath, true);

                        for (int r = 0; r < editorBuildSettingsScenes.Count; r++)
                        {
                            if (editorBuildSettingsScenes[r].path == targetScene.path)
                            {
                                sceneExistsInBuild = true;
                                break;
                            }
                        }

                        if (sceneExistsInBuild == false)
                        {
                            editorBuildSettingsScenes.Add(targetScene);
                        }
                    }
                }
            }

            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
            FilterPotentialLevels();
        }

        private void FilterPotentialLevels()
        {
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = BuildSettingsSceneManagement.GetEditorBuildSettingsScenes();

            List<GameLevel> filteredGameLevelsList = new List<GameLevel>();
            foreach (GameLevel item in targetLevels)
            {
                //Get Scene and Scene Path
                bool doesAllScenesExistInBuild = false;
                for (int i = 0; i < item.assignedScenes.Count; i++)
                {
                    bool sceneExistsInBuild = false;
                    string gameLevelName = item.gameLevelName;
                    string scenePath = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/Game Levels/" + gameLevelName + "/" + gameLevelName + "_" + item.assignedScenes[i] + ".unity";
                    EditorBuildSettingsScene targetScene = new EditorBuildSettingsScene(scenePath, true);

                    for (int r = 0; r < editorBuildSettingsScenes.Count; r++)
                    {
                        if (editorBuildSettingsScenes[r].path == targetScene.path)
                        {
                            sceneExistsInBuild = true;                 
                            break;
                        }
                    }

                    if (sceneExistsInBuild == false)
                    {
                        doesAllScenesExistInBuild = false;
                        break;
                    }
                    else
                    {
                        doesAllScenesExistInBuild = true;
                    }
                }

                if(doesAllScenesExistInBuild == false)
                {
                    filteredGameLevelsList.Add(item);
                }
            }

            targetLevels = filteredGameLevelsList.ToArray();

            List<string> levelsToString = new List<string>();
            foreach (GameLevel item in targetLevels)
            {
                levelsToString.Add(item.gameLevelName);
            }
            targetLevelOptions = levelsToString.ToArray();
        }
    }

   
}