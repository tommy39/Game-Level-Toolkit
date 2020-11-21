using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IND.Core.GameLevels.Demo
{
    public class DemoLoadLevelController : MonoBehaviour
    {
        [SerializeField] private GameLevel gameLevelToLoad;

        private void OnGUI()
        {
            if(GUILayout.Button("Load Level"))
            {
                LoadLevel();
            }
        }

        private void LoadLevel()
        {
            LoadGameLevel.LoadLevel(gameLevelToLoad, GameLevelManager.singleton.loadedGameLevels, true, false, false);
        }
    }
}