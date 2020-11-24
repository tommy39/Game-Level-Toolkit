using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IND.Core.GameLevels;

namespace IND.Editor.GameLevelsToolkit
{
    public static class BuildSettingsSceneManagement
    {

        public static List<EditorBuildSettingsScene> GetEditorBuildSettingsScenes()
        {
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

            //Get The Existing Build List
            foreach (EditorBuildSettingsScene item in EditorBuildSettings.scenes)
            {
                editorBuildSettingsScenes.Add(item);
            }

            return editorBuildSettingsScenes;
        }

        public static void AddMasterSceneToBuild()
        {
            List<EditorBuildSettingsScene> buildScenes = GetEditorBuildSettingsScenes();

            //Add The Master Scene
            string masterscenePath = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/" + "Game Levels/Master Scene/Master Scene.unity";
            EditorBuildSettingsScene masterScene = new EditorBuildSettingsScene(masterscenePath, true);

            if (buildScenes.Count > 0)
            {
                bool masterSceneNotFoundInBuild = false;
                for (int i = 0; i < buildScenes.Count; i++)
                {
                    if (buildScenes[i].path == masterScene.path)
                    {
                        masterSceneNotFoundInBuild = true;
                        break;
                    }
                }
                if (masterSceneNotFoundInBuild == false)
                {
                    buildScenes.Add(masterScene);
                }
            }
            else
            {
                buildScenes.Add(masterScene);
            }

            EditorBuildSettings.scenes = buildScenes.ToArray();
        }

        public static void AddLevelToBuild(GameLevel level, List<EditorBuildSettingsScene> buildScenes = null, bool updateSceneBuild = true)
        {
            if (buildScenes == null)
            {
                buildScenes = GetEditorBuildSettingsScenes();
            }

            //Add Game Level
            for (int i = 0; i < level.assignedScenes.Count; i++)
            {
                bool sceneExistsInBuild = false;
                string scenePath = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/Game Levels/" + level.gameLevelName + "/" + level.gameLevelName + "_" + level.assignedScenes[i] + ".unity";
                EditorBuildSettingsScene scene = new EditorBuildSettingsScene(scenePath, true);

                for (int g = 0; g < buildScenes.Count; g++)
                {
                    if (buildScenes[g].path == scene.path)
                    {
                        sceneExistsInBuild = true;
                        break;
                    }
                }

                if (sceneExistsInBuild == false)
                {
                    buildScenes.Add(scene);
                }
            }

            //Add Dependencies
            for (int i = 0; i < level.levelDependencies.Count; i++)
            {
                for (int g = 0; g < level.levelDependencies[i].assignedScenes.Count; g++)
                {
                    bool sceneExistsInBuild = false;
                    string gameLevelName = level.levelDependencies[i].gameLevelName;
                    string scenePath = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/Game Levels/" + gameLevelName + "/" + gameLevelName + "_" + level.levelDependencies[i].assignedScenes[g] + ".unity";
                    EditorBuildSettingsScene targetScene = new EditorBuildSettingsScene(scenePath, true);

                    for (int f = 0; f < buildScenes.Count; f++)
                    {
                        if (buildScenes[f].path == targetScene.path)
                        {
                            sceneExistsInBuild = true;
                            break;
                        }
                    }

                    if (sceneExistsInBuild == false)
                    {
                        buildScenes.Add(targetScene);
                    }
                }
            }

            //Add Dependencies of Dependencies
            for (int i = 0; i < level.levelDependencies.Count; i++)
            {
                for (int g = 0; g < level.levelDependencies[i].levelDependencies.Count; g++)
                {
                    GameLevel targetDependency = level.levelDependencies[i].levelDependencies[g];

                    for (int f = 0; f < targetDependency.assignedScenes.Count; f++)
                    {
                        bool sceneExistsInBuild = false;
                        string gameLevelName = targetDependency.gameLevelName;
                        string scenePath = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/Game Levels/" + gameLevelName + "/" + gameLevelName + "_" + targetDependency.assignedScenes[f] + ".unity";
                        EditorBuildSettingsScene targetScene = new EditorBuildSettingsScene(scenePath, true);

                        for (int r = 0; r < buildScenes.Count; r++)
                        {
                            if (buildScenes[r].path == targetScene.path)
                            {
                                sceneExistsInBuild = true;
                                break;
                            }
                        }

                        if (sceneExistsInBuild == false)
                        {
                            buildScenes.Add(targetScene);
                        }
                    }
                }
            }

            if (updateSceneBuild == true)
            {
                EditorBuildSettings.scenes = buildScenes.ToArray();
            }
        }

        public static List<EditorBuildSettingsScene> RemoveSceneFromBuild(EditorBuildSettingsScene scene, List<EditorBuildSettingsScene> buildScenes = null, bool forceRemoval = true, bool updateSceneBuild = false)
        {
            if (buildScenes == null)
            {
                buildScenes = GetEditorBuildSettingsScenes();
            }

            if (forceRemoval == false)
            {
                bool isSceneInBuild = false;

                for (int r = 0; r < buildScenes.Count; r++)
                {
                    if (buildScenes[r].path == scene.path)
                    {
                        isSceneInBuild = true;
                        break;
                    }
                }

                Debug.Log(isSceneInBuild);

                if (isSceneInBuild == false)
                {
                    return buildScenes;
                }
            }

            for (int i = 0; i < buildScenes.Count; i++)
            {
                if (buildScenes[i].path == scene.path)
                {
                    buildScenes.RemoveAt(i);
                }
            }

            if (updateSceneBuild == true)
            {
                EditorBuildSettings.scenes = buildScenes.ToArray();
            }

            return buildScenes;
        }
    }
}