using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IND.Core.GameLevels;

namespace IND.Editor.GameLevelsToolkit
{
    public class AddDependencyToLevelWindow : EditorWindow
    {
        public static AddDependencyToLevelWindow window;

        private GameLevel targetLevel;
        private GameLevel targetDependency;

        #region Unity GUI
        //Game Level
        private GameLevelData gameLevelData;
        private int selectedLevelValue = 0;
        private GameLevel[] targetLevels;
        private string[] targetLevelOptions;

        private List<GameLevel> potentialLevelsToBeDependency = new List<GameLevel>();
        private GameLevel[] targetDependencies;
        private string[] targetDependencyOptions;
        private int selectedDependencyValue = 0;
        #endregion

        public static void OpenMenu()
        {
            window = GetWindow<AddDependencyToLevelWindow>("Add Dependency To Level");
            WindowRefresh.RefreshLevelsList(out window.targetLevelOptions, out window.selectedLevelValue, out window.targetLevels, out window.gameLevelData);
            window.targetLevel = window.targetLevels[window.selectedLevelValue];
            window.selectedDependencyValue = 0;
            window.GetPossibleDependencies();
        }

        private void OnGUI()
        {
            selectedLevelValue = EditorGUILayout.Popup("Selected Level", selectedLevelValue, targetLevelOptions);
            targetLevel = targetLevels[selectedLevelValue];
            GetPossibleDependencies();

            if (targetDependencies.Length > 0)
            {
                selectedDependencyValue = EditorGUILayout.Popup("Level To Add As Dependency", selectedDependencyValue, targetDependencyOptions);
                targetDependency = targetDependencies[selectedDependencyValue];
            }

            if(GUILayout.Button("Add Level As Dependency"))
            {
                AddTargetDependencyToLevel();
            }
        }

        private void GetPossibleDependencies()
        {
            potentialLevelsToBeDependency.Clear();
            for (int i = 0; i < gameLevelData.gameLevelsCreatedByUser.Count; i++)
            {
                bool canBeUsed = true;
                if (gameLevelData.gameLevelsCreatedByUser[i] == targetLevel)
                {
                    canBeUsed = false;
                    continue;
                }

                if (targetLevel.levelDependencies.Count > 0)
                {
                    for (int g = 0; g < targetLevel.levelDependencies.Count; g++)
                    {
                        if (targetLevel.levelDependencies[g] == gameLevelData.gameLevelsCreatedByUser[i])
                        {
                            canBeUsed = false;
                            continue;
                        }
                    }
                }
                if (canBeUsed == true)
                {
                    potentialLevelsToBeDependency.Add(gameLevelData.gameLevelsCreatedByUser[i]);
                }
            }

            targetDependencies = potentialLevelsToBeDependency.ToArray();

           
            List<string> levelsToString = new List<string>();
            foreach (GameLevel item in targetDependencies)
            {
                levelsToString.Add(item.gameLevelName);
            }
            targetDependencyOptions = levelsToString.ToArray();
        }

        private void AddTargetDependencyToLevel()
        {
            targetLevel.levelDependencies.Add(targetDependency);
            EditorUtility.SetDirty(targetLevel);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            selectedDependencyValue = 0;
            GetPossibleDependencies();
        }
    }
}