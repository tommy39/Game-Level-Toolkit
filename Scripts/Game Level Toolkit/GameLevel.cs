using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Core.GameLevels
{
    public class GameLevel : ScriptableObject
    {
        public string gameLevelName;
        public string assignedScenesDirectory;
        public List<string> assignedScenes = new List<string>();
        public List<GameLevel> levelDependencies = new List<GameLevel>();
    }
}
