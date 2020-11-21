using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IND.Core.GameLevels.Demo
{
    public class DemoLoadLevelController : MonoBehaviour
    {
        [SerializeField] protected GameLevel gameLevelToLoad;
        [SerializeField] protected bool loadLevelDependenciesBeforeLevel = true;
        [SerializeField] protected bool keepCurrentOpenScenesOpenedThatAreNotDependencies = true;

        public void LoadLevel()
        {
            LoadGameLevel.LoadLevel(gameLevelToLoad, GameLevelManager.singleton.loadedGameLevels, loadLevelDependenciesBeforeLevel, keepCurrentOpenScenesOpenedThatAreNotDependencies, false, false);
        }
    }
}