using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IND.Core.GameLevels;

namespace IND.Editor.GameLevelsToolkit
{
    public class RemoveLevelDepdendencyWindow : EditorWindow
    {
        public static RemoveLevelDepdendencyWindow window;
        private GameLevel targetLevel;
        private GameLevel targetDependencyToRemove;


        #region Unity GUI
        //Game Level
        private GameLevelData gameLevelData;
        private int selectedLevelValue = 0;
        private GameLevel[] targetLevels;
        private string[] targetLevelOptions;

        private int selectedDependencyValue = 0;
        private GameLevel[] targetDependencies;
        private string[] targetDependenciesOptions;
        #endregion

        public static void OpenMenu()
        {
            window = GetWindow<RemoveLevelDepdendencyWindow>("Remove Level Dependency");
            WindowRefresh.RefreshLevelsList(out window.targetLevelOptions, out window.selectedLevelValue, out window.targetLevels, out window.gameLevelData);
            window.targetLevel = window.targetLevels[window.selectedLevelValue];
            window.GetDependenciesList();
        }

        private void OnGUI()
        {
            //Display a list of levels
            selectedLevelValue = EditorGUILayout.Popup("Selected Level", selectedLevelValue, targetLevelOptions);
            targetLevel = targetLevels[selectedLevelValue];

            GetDependenciesList();

            //Display a list of all dependencies assigned to the level
            if(targetDependencies.Length > 0)
            {
                selectedDependencyValue = EditorGUILayout.Popup("Dependency To Remove", selectedDependencyValue, targetDependenciesOptions);
                targetDependencyToRemove = targetDependencies[selectedDependencyValue];
            }
            else
            {
                EditorGUILayout.HelpBox("Game Level Does Not Have Any Dependencies", MessageType.Warning);
            }

            if (targetDependencies.Length > 0)
            {
                if (GUILayout.Button("Remove Target Dependency"))
                {
                    RemoveDependency();
                }
            }
        }

        private void RemoveDependency()
        {
            EditorUtility.SetDirty(targetLevel);
            targetLevel.levelDependencies.Remove(targetDependencyToRemove);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void GetDependenciesList()
        {
            selectedDependencyValue = 0;
            targetDependencies = targetLevel.levelDependencies.ToArray();
            List<string> levelsToString = new List<string>();
            foreach (GameLevel item in targetDependencies)
            {
                levelsToString.Add(item.gameLevelName);
            }
            targetDependenciesOptions = levelsToString.ToArray();

        }
    }
}