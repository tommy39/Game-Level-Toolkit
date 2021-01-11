using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IND.Core.GameLevels;

namespace IND.Editor.GameLevelsToolkit
{
    public class AssignCategoryToLevel : EditorWindow
    {
        private GameLevelData gameLevelData;

        public static AssignCategoryToLevel window;
        private int selectedValue = 0;
        private GameLevel[] levels;
        private string[] levelOptionsToLoad;

        private int selectedCategoryValue = 0;
        private string[] categories;
        public static void OpenMenu()
        {
            window = GetWindow<AssignCategoryToLevel>("Assign Category To Level");
            window.gameLevelData = GameLevelToolkitWindow.GetGameLevelsData();
            window.selectedValue = 0;
            window.levels = window.gameLevelData.gameLevelsCreatedByUser.ToArray();
            List<string> levelsToString = new List<string>();
            foreach (GameLevel item in window.levels)
            {
                levelsToString.Add(item.gameLevelName);
            }
            window.levelOptionsToLoad = levelsToString.ToArray();
        }

        private void OnGUI()
        {
            List<string> levelsToString = new List<string>();
            if (levels.Length > 0)
            {
                foreach (GameLevel item in levels)
                {
                    levelsToString.Add(item.gameLevelName);
                }
                levelOptionsToLoad = levelsToString.ToArray();
                selectedValue = EditorGUILayout.Popup("Level To Change", selectedValue, levelOptionsToLoad);
            }

            categories = CategoriesManagement.GetAllCategories(false, true);
            selectedCategoryValue = EditorGUILayout.Popup("Target Category", selectedCategoryValue, categories);
            levels = gameLevelData.gameLevelsCreatedByUser.ToArray();

           if(levels[selectedValue].assignedCategory == categories[selectedCategoryValue])
            {
                EditorGUILayout.HelpBox("Category Is Already Assigned to Level", MessageType.Info);
            }
            else
            {
                if (GUILayout.Button("Change Levels Category"))
                {
                    levels[selectedValue].assignedCategory = categories[selectedCategoryValue];
                }
            }

        }
    }
}
