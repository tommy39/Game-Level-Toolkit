using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace IND.Core.GameLevels
{
    [Serializable]
    public class GameLevelData : ScriptableObject
    {
        public List<GameLevel> gameLevelsCreatedByUser = new List<GameLevel>();
        [HideInInspector] public bool initialDataHasBeenCreated = true;
        [HideInInspector] public string allCategoriesName = "All";
        [HideInInspector] public string unassignedCategoryName = "UnAssigned";
        public List<string> allUserCreatedCategories = new List<string>();

        private void OnEnable()
        {
            EditorUtility.ClearDirty(this);
        }
    }
}