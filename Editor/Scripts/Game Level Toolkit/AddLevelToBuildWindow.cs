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
            window.targetLevel = window.targetLevels[window.selectedLevelValue];
        }

        private void OnGUI()
        {
            selectedLevelValue = EditorGUILayout.Popup("Selected Level", selectedLevelValue, targetLevelOptions);
            targetLevel = targetLevels[selectedLevelValue];

            if (GUILayout.Button("Add Selected Level To Build"))
            {
                AddSelectedLevel();
            }
        }

        private void AddSelectedLevel()
        {
            List<SceneAsset> sceneAssets = new List<SceneAsset>();
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

            //Get The Existing Build List
            foreach (EditorBuildSettingsScene item in EditorBuildSettings.scenes)
            {
                editorBuildSettingsScenes.Add(item);
            }

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
                    string scenePath = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/Game Levels/" + gameLevelName + "/" + gameLevelName + "_" + targetLevel.levelDependencies[i].assignedScenes[g] + ".unity"; EditorBuildSettingsScene scene = new EditorBuildSettingsScene(scenePath, true);
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
                        editorBuildSettingsScenes.Add(scene);
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
                        string scenePath = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/Game Levels/" + gameLevelName + "/" + gameLevelName + "_" + targetDependency.assignedScenes[f] + ".unity"; EditorBuildSettingsScene scene = new EditorBuildSettingsScene(scenePath, true);
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
                            editorBuildSettingsScenes.Add(scene);
                        }
                    }
                }
            }

            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }
    }
}