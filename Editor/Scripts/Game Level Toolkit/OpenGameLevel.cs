using IND.Core.GameLevels;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IND.Editor.GameLevelsToolkit
{
    public class OpenGameLevel : EditorWindow
    {
        public static OpenGameLevel window;
        // [InlineEditor] [Required] 
        public GameLevel selectedGameLevel;
        private GameLevelData gameLevelData;


        public bool includeMasterScene = true;
        public bool includeDependencies = true;
        public bool keepCurrentScenesThatAreOpenOpeneded = false;

        #region Unity GUI Variables
        private int selectedValue = 0;
        private GameLevel[] levels;
        private string[] levelOptionsToLoad;

        private int selectedCategoryValue = 0;
        private string[] categories;
        #endregion


        public static void OpenMenu()
        {
            window = (OpenGameLevel)GetWindow(typeof(OpenGameLevel), false, "Load Game Level");
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
            categories = CategoriesManagement.GetAllCategories(true, true);
            selectedCategoryValue = EditorGUILayout.Popup("Selected Category", selectedCategoryValue, categories);
            levels = CategoriesManagement.GetGameLevelsBasedOnCategory(categories[selectedCategoryValue]);
            List<string> levelsToString = new List<string>();
            if (levels.Length > 0)
            {
                foreach (GameLevel item in levels)
                {
                    levelsToString.Add(item.gameLevelName);
                }
                levelOptionsToLoad = levelsToString.ToArray();
            }

            if (levels.Length > 0)
            {
                selectedValue = EditorGUILayout.Popup("Level To Load", selectedValue, levelOptionsToLoad);
                includeMasterScene = EditorGUILayout.Toggle("Load Master Scene", includeMasterScene);
                includeDependencies = EditorGUILayout.Toggle("Load Dependencies", includeDependencies);
                keepCurrentScenesThatAreOpenOpeneded = EditorGUILayout.Toggle("Keep Current Scenes", keepCurrentScenesThatAreOpenOpeneded);

                if (GUILayout.Button("Load Seleected Level"))
                {
                    // Get Target Level
                    selectedGameLevel = levels[selectedValue];

                    OpenLevel(selectedGameLevel, includeMasterScene, keepCurrentScenesThatAreOpenOpeneded, includeDependencies);
                    Close();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Selected Category has no assigned levels", MessageType.Info);
            }
        }

        private List<string> GetAllSceneNames()
        {
            return selectedGameLevel.assignedScenes;
        }

        public static void OpenLevel(GameLevel targetGameLevel, bool includeMasterScene, bool keepCurrentScenesThatAreOpenOpeneded, bool includeDependencies)
        {

            List<Scene> loadedScenes = new List<Scene>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i));
            }
            Scene[] loadedScenesArray = loadedScenes.ToArray();
            EditorSceneManager.SaveModifiedScenesIfUserWantsTo(loadedScenesArray);

            //Load The Master Scene
            if (includeMasterScene == true)
            {
                string masterScenePath = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/" + SceneAndResourceFolderName.folderNameValue + "/Master Scene/Master Scene.unity";
                Scene masterScene;

                if (keepCurrentScenesThatAreOpenOpeneded == true)
                {
                    masterScene = EditorSceneManager.OpenScene(masterScenePath, OpenSceneMode.Additive);
                }
                else
                {
                    masterScene = EditorSceneManager.OpenScene(masterScenePath, OpenSceneMode.Single);
                }

                EditorSceneManager.SetActiveScene(masterScene);
            }

            //Unload Any Existing Scenes If Required
            if (includeMasterScene == false && keepCurrentScenesThatAreOpenOpeneded == false)
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);
                    if (!IsActiveScene(scene)) CloseScene(scene);
                    if (scene.name == "Master Scene")
                    {
                        if (!IsActiveScene(scene))
                        {
                            EditorSceneManager.SetActiveScene(scene);
                        }
                    }
                }
            }

            //Load The Locations Scenes
            LoadLocationsScenesAdditively(targetGameLevel);

            //Load Dependencies If Required
            if (includeDependencies == true)
            {
                for (int i = 0; i < targetGameLevel.levelDependencies.Count; i++)
                {
                    LoadLocationsScenesAdditively(targetGameLevel.levelDependencies[i]);
                }
            }
        }
        private static void LoadLocationsScenesAdditively(GameLevel level)
        {
            string locationsScenesPath = level.assignedScenesDirectory + "/" + level.gameLevelName + "_";
            for (int i = 0; i < level.assignedScenes.Count; i++)
            {
                string targetScenePath = locationsScenesPath + level.assignedScenes[i] + ".unity";
                EditorSceneManager.OpenScene(targetScenePath, OpenSceneMode.Additive);
            }
        }

        private static void CloseScene(Scene scene)
        {
            EditorSceneManager.CloseScene(scene, false);
        }

        private static Scene activeScene
        {
            get { return SceneManager.GetActiveScene(); }
        }
        private static bool IsActiveScene(Scene scene)
        {
            return scene == activeScene;
        }
    }
}