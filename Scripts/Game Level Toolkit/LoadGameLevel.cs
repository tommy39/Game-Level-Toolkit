using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IND.Core.GameLevels
{
    public static class LoadGameLevel
    {
        // #levelToLoad - Reference to the level we want to load
        // #loadDependenciesBeforeLevel - Any Dependencies that are required for the game level to be loaded. If FALSE dependencies will be loaded after the Level. if TRUE dependencies will be loaded before the level. 
        // #keepCurrentLevelsOpenedThatAreNotDependency - if FALSE Any Game Levels that are already opened will not be unloaded, if TRUE Game Levels will remain loaded
        // #forceReloadAnyDependenciesAlreadyOpened - if FALSE dependencies that are currently opened will remain opened, if TRUE the opened dependencies will be reloaded. 
        // #makeMasterSceneActiveSceneWhileLoading - We want to make sure our active scene is not a scene that will be unloaded, we can make the master scene the active scene as a safety net as a result. 

        public static List<GameLevel> LoadLevel(GameLevel levelToLoad, List<GameLevel> currentlyLoadedLevels, bool loadDependenciesBeforeLevel, bool keepCurrentLevelsOpenedThatAreNotDependency = false, bool forceReloadAnyDependenciesAlreadyOpened = false, bool makeMasterSceneActiveSceneWhileLoading = true)
        {
            //Make The Master Scene the Active Scene While Loading
            if (makeMasterSceneActiveSceneWhileLoading == true)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Master Scene"));
            }

            //Find Any Scenes that we dont want to keep 
            List<GameLevel> gameLevelsToUnload = new List<GameLevel>();
            for (int i = 0; i < currentlyLoadedLevels.Count; i++)
            {
                gameLevelsToUnload.Add(currentlyLoadedLevels[i]);
            }

            if (keepCurrentLevelsOpenedThatAreNotDependency == false)
            {
                for (int i = 0; i < levelToLoad.levelDependencies.Count; i++)
                {
                    gameLevelsToUnload.Remove(levelToLoad.levelDependencies[i]);
                }
            }

            if(keepCurrentLevelsOpenedThatAreNotDependency == true)
            {
                gameLevelsToUnload.Clear();
            }

            //Unload any unwanted Scenes
            if (gameLevelsToUnload.Count > 0)
            {
                List<string> scenesToUnload = new List<string>();
                for (int i = 0; i < gameLevelsToUnload.Count; i++) //Get Unity Scene Files
                {
                    List<string> unityScenes = GetGameLevelSceneNames(gameLevelsToUnload[i]);
                    for (int g = 0; g < unityScenes.Count; g++)
                    {
                        scenesToUnload.Add(unityScenes[g]);
                    }
                    currentlyLoadedLevels.Remove(gameLevelsToUnload[i]);
                }

                for (int i = 0; i < scenesToUnload.Count; i++) //Unload Scenes Through SceneManager
                {
                    SceneManager.UnloadSceneAsync(scenesToUnload[i]);
                }
                
            }

            List<GameLevel> levelDependenciesToLoad = new List<GameLevel>();
            for (int i = 0; i < levelToLoad.levelDependencies.Count; i++)
            {
                levelDependenciesToLoad.Add(levelToLoad.levelDependencies[i]);
            }
            if (forceReloadAnyDependenciesAlreadyOpened == true) //Unload Any Dependencies that are loaded and need a reload. 
            {
                List<GameLevel> dependenciesToUnLoad = new List<GameLevel>();

                //Add Dependencies to reload to the list
                for (int i = 0; i < levelToLoad.levelDependencies.Count; i++)
                {
                    if (currentlyLoadedLevels.Contains(levelToLoad.levelDependencies[i]))
                    {
                        dependenciesToUnLoad.Add(levelToLoad.levelDependencies[i]);
                    }
                }

                for (int i = 0; i < dependenciesToUnLoad.Count; i++)
                {
                    //Unload Scenes
                    List<string> scenesToUnload = GetGameLevelSceneNames(dependenciesToUnLoad[i]);
                    for (int g = 0; g < scenesToUnload.Count; g++)
                    {
                        SceneManager.UnloadSceneAsync(scenesToUnload[i]);
                    }
                    currentlyLoadedLevels.Remove(dependenciesToUnLoad[i]);
                }
            }
            else
            {
                for (int i = 0; i < levelToLoad.levelDependencies.Count; i++)
                {
                    if (currentlyLoadedLevels.Contains(levelToLoad.levelDependencies[i]))
                    {
                        levelDependenciesToLoad.Remove(levelToLoad.levelDependencies[i]);
                    }
                }
            }

            //Load Dependencies before the level
            if (loadDependenciesBeforeLevel == true)
            {
                if (levelDependenciesToLoad.Count > 0)
                {
                    LoadDependencies(levelDependenciesToLoad);

                    for (int i = 0; i < levelDependenciesToLoad.Count; i++)
                    {
                        currentlyLoadedLevels.Add(levelDependenciesToLoad[i]);
                    }
                }
            }

            //Load Game Level Scenes
            List<string> scenesToLoad = GetGameLevelSceneNames(levelToLoad);
            for (int i = 0; i < scenesToLoad.Count; i++)
            {
                SceneManager.LoadSceneAsync(scenesToLoad[i], LoadSceneMode.Additive);
            }
            currentlyLoadedLevels.Add(levelToLoad);

            //Load Dependencies After the level
            if (loadDependenciesBeforeLevel == false)
            {
                if (levelDependenciesToLoad.Count > 0)
                {
                    LoadDependencies(levelDependenciesToLoad);

                    for (int i = 0; i < levelDependenciesToLoad.Count; i++)
                    {

                        currentlyLoadedLevels.Add(levelDependenciesToLoad[i]);
                    }
                }
            }

            return currentlyLoadedLevels; //Return the List Of Loaded Game Levels
        }

        public static void LoadDependencies(List<GameLevel> levelDependencies)
        {

            for (int i = 0; i < levelDependencies.Count; i++)
            {
                List<string> scenesToLoad = GetGameLevelSceneNames(levelDependencies[i]);

                for (int g = 0; g < scenesToLoad.Count; g++)
                {
                    SceneManager.LoadSceneAsync(scenesToLoad[g], LoadSceneMode.Additive);
                }
            }

        }

        public static List<string> GetGameLevelSceneNames(GameLevel targetLevel)
        {
            List<string> scenes = new List<string>();
            for (int g = 0; g < targetLevel.assignedScenes.Count; g++)
            {
                string sceneName = targetLevel.gameLevelName + "_" + targetLevel.assignedScenes[g];
                scenes.Add(sceneName);
            }

            return scenes;
        }
    }
}