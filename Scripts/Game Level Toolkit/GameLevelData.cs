using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Core.GameLevels
{
    public class GameLevelData : ScriptableObject
    {
        public List<GameLevel> gameLevelsCreatedByUser = new List<GameLevel>();
        [HideInInspector] public bool initialDataHasBeenCreated = true;        
        public void CheckForNullLocations()
        {
            for (int i = 0; i < gameLevelsCreatedByUser.Count; i++)
            {
                if(gameLevelsCreatedByUser[i] ==null)
                {
                    gameLevelsCreatedByUser.Remove(gameLevelsCreatedByUser[i]);
                }
            }
        }
    }
}