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
            BuildSettingsSceneManagement.AddLevelToBuild(targetLevel);
           
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