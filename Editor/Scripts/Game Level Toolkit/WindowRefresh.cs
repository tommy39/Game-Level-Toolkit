using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IND.Core.GameLevels;

namespace IND.Editor.GameLevelsToolkit
{
    public static class WindowRefresh
    {
        public static void RefreshLevelsList(out string[] levelOptionsToLoad, out int selectedValue, out GameLevel[] levels, out GameLevelData gameLevelData)
        {
            gameLevelData = GameLevelToolkitWindow.GetGameLevelsData();
            selectedValue = 0;
            levels = gameLevelData.gameLevelsCreatedByUser.ToArray();
            List<string> levelsToString = new List<string>();
            foreach (GameLevel item in levels)
            {
                levelsToString.Add(item.gameLevelName);
            }
            levelOptionsToLoad = levelsToString.ToArray();
        }

        public static void RefreshScenesList(GameLevel level, out string[] sceneOptions, out int selectedValue)
        {
            selectedValue = 0;
            sceneOptions = level.assignedScenes.ToArray();
        }
    }
}