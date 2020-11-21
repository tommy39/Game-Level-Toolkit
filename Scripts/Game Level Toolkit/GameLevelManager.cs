using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Core.GameLevels
{
    //Script Should be included in the games master scene
    public class GameLevelManager : MonoBehaviour
    {
        public bool loadStartLevelOnStart = true;
        public GameLevel levelToLoadOnGameStart;
        public List<GameLevel> loadedGameLevels = new List<GameLevel>();

        public static GameLevelManager singleton;
        private void Awake()
        {
            singleton = this;
        }

        void Start()
        {
            loadedGameLevels.Clear();

            if (loadStartLevelOnStart == true) //Load Start Level
            {
                if (levelToLoadOnGameStart == null)
                {
                    Debug.LogError("Level Required To Load On Start");
                    return;
                }
            }

           loadedGameLevels = LoadGameLevel.LoadLevel(levelToLoadOnGameStart, loadedGameLevels, true, false, false);
        }
    }
}