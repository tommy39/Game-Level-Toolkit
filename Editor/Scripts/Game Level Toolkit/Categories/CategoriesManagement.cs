using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IND.Core.GameLevels;

namespace IND.Editor.GameLevelsToolkit
{
    public static class CategoriesManagement
    {
        public static GameLevel[] GetGameLevelsBasedOnCategory(string categoryName)
        {
            GameLevelData data = GameLevelToolkitWindow.GetGameLevelsData();
            List<GameLevel> categories = new List<GameLevel>();
            if (categoryName == data.allCategoriesName) // Get All
            {
                for (int i = 0; i < data.gameLevelsCreatedByUser.Count; i++)
                {
                    categories.Add(data.gameLevelsCreatedByUser[i]);
                }
            }
            else // Get By Name
            {
                for (int i = 0; i < data.gameLevelsCreatedByUser.Count; i++)
                {
                    if(data.gameLevelsCreatedByUser[i].assignedCategory == categoryName)
                    {
                        categories.Add(data.gameLevelsCreatedByUser[i]);
                    }
                }
            }

            return categories.ToArray();
        }

        public static string[] GetAllCategories(bool includeAllType, bool includeUnassigned = true)
        {
            GameLevelData data = GameLevelToolkitWindow.GetGameLevelsData();
            List<string> categories = new List<string>();
            if (includeAllType == true)
            {
                categories.Add(data.allCategoriesName);
            }
            if (includeUnassigned == true)
            {
                categories.Add(data.unassignedCategoryName);
            }
            if (data.allUserCreatedCategories.Count > 0)
            {
                for (int i = 0; i < data.allUserCreatedCategories.Count; i++)
                {
                    categories.Add(data.allUserCreatedCategories[i]);
                }
            }
            return categories.ToArray();
        }

        public static void CreateCategory(string targetName)
        {
            GameLevelData data = GameLevelToolkitWindow.GetGameLevelsData();
            data.allUserCreatedCategories.Add(targetName);
            EditorUtility.SetDirty(data);

        }

        public static void RenameCategory(string categoryOriginalName, string targetCategoryName)
        {
            //Get All Levels that Have this category and rename it there too
            GameLevel[] levels = GetGameLevelsBasedOnCategory(categoryOriginalName);
            RenameGameLevelsCategory(levels, targetCategoryName);

            GameLevelData data = GameLevelToolkitWindow.GetGameLevelsData();
            for (int i = 0; i < data.allUserCreatedCategories.Count; i++)
            {
                if(data.allUserCreatedCategories[i] == categoryOriginalName)
                {
                    data.allUserCreatedCategories[i] = targetCategoryName;
                    break;
                }
            }
            EditorUtility.SetDirty(data);

        }

        public static void DeleteExistingCategory(string targetCategory)
        {
            GameLevelData data = GameLevelToolkitWindow.GetGameLevelsData();

            //Get All Levels that have this category and assign them to unassigned
            GameLevel[] levels = GetGameLevelsBasedOnCategory(targetCategory);
            RenameGameLevelsCategory(levels, data.unassignedCategoryName);

            //Remove The Item From The data list
            for (int i = 0; i < data.allUserCreatedCategories.Count; i++)
            {
                if (data.allUserCreatedCategories[i] == targetCategory)
                {
                    data.allUserCreatedCategories.RemoveAt(i);
                    break;
                }
            }

            EditorUtility.SetDirty(data);
        }

        private static void RenameGameLevelsCategory(GameLevel[] levels, string targetCategoryName)
        {
            foreach (GameLevel item in levels)
            {
                item.assignedCategory = targetCategoryName;
                EditorUtility.SetDirty(item);
            }
        }
    }
}