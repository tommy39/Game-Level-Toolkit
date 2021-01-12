using IND.Core.GameLevels;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace IND.Editor.GameLevelsToolkit
{
    public class AddExistingSceneToGameLevel : EditorWindow
    {
        public static AddExistingSceneToGameLevel window;
        public GameLevel targetGameLevel;
        public string targetSceneFolderPath;
        public string targetSceneName;
        private bool useRootDirectory = true;

        #region Unity GUI
        //Game Level
        private GameLevelData gameLevelData;
        private int selectedValue = 0;
        private GameLevel[] levels;
        private string[] levelOptions;
        #endregion

        public static void OpenMenu()
        {
            window = (AddExistingSceneToGameLevel)GetWindow(typeof(AddExistingSceneToGameLevel), false, "Add Existing Scene To Level");
            WindowRefresh.RefreshLevelsList(out window.levelOptions, out window.selectedValue, out window.levels, out window.gameLevelData);
            window.targetGameLevel = window.levels[window.selectedValue];
        }

        private void OnGUI()
        {
            selectedValue = EditorGUILayout.Popup("Selected Level", selectedValue, levelOptions);
            targetGameLevel = levels[selectedValue];
            EditorGUILayout.HelpBox("Use The Directory After 'Assets/', e.g. folder ['Scenes'] and scene ['sceneName']", MessageType.Info);

            targetSceneFolderPath = EditorGUILayout.TextField("Target Scene Folder Path", targetSceneFolderPath);
            targetSceneName = EditorGUILayout.TextField("Target Scene Name", targetSceneName);


            if (File.Exists("Assets/" + targetSceneFolderPath + "/" + targetSceneName + ".unity"))
            {
                if (GUILayout.Button("Add Scene To Selected Level"))
                {
                    AddSceneToLocation();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Target Scene Can Not Be Found", MessageType.Error);
            }

        }


        private void AddSceneToLocation()
        {
            string projectPathName = GameLevelToolkitWindow.GetProjectPathStringWithSlash();

            if (targetGameLevel == null)
            {
                Debug.LogError("Target Location Is Required");
                return;
            }

            string originalScenePath = null;
            if (useRootDirectory == true)
            {
                originalScenePath = "Assets/" + targetSceneFolderPath + ".unity";
            }
            else
            {
                originalScenePath = "Assets/" + projectPathName + "Scenes/" + targetSceneFolderPath + "/" + targetSceneName + ".unity";
            }

            //Check to See If the scene belongs to an existing location
            bool sceneExistsInAGameLevel = false;
            string sceneNameWithoutGameLevelPrefix = null;
            GameLevelData gameLevelData = GameLevelToolkitWindow.GetGameLevelsData();
            for (int i = 0; i < gameLevelData.gameLevelsCreatedByUser.Count; i++)
            {
                for (int g = 0; g < gameLevelData.gameLevelsCreatedByUser[i].assignedScenes.Count; g++)
                {

                    string assignedSceneName = gameLevelData.gameLevelsCreatedByUser[i].gameLevelName + "_" + gameLevelData.gameLevelsCreatedByUser[i].assignedScenes[g];

                    if (assignedSceneName == targetSceneName)
                    {
                        sceneNameWithoutGameLevelPrefix = gameLevelData.gameLevelsCreatedByUser[i].assignedScenes[g];
                        //Remove this scene from its location that it is apart of
                        gameLevelData.gameLevelsCreatedByUser[i].assignedScenes.Remove(targetSceneName);
                        sceneExistsInAGameLevel = true;
                    }
                }
            }

            //Rename the scene based on the location naming convention
            string targetGameLevelPrefix = targetGameLevel.gameLevelName;
            string finalSceneName = null;
            if (sceneExistsInAGameLevel == true)
            {
                finalSceneName = targetGameLevelPrefix + "_" + sceneNameWithoutGameLevelPrefix + ".unity";
                targetGameLevel.assignedScenes.Add(finalSceneName);
            }
            else
            {
                finalSceneName = targetGameLevelPrefix + "_" + targetSceneName + ".unity";
                targetGameLevel.assignedScenes.Add(targetSceneName);
            }

            AssetDatabase.RenameAsset(originalScenePath, finalSceneName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //Move the scene to the correct Directory
            string adjustedDirectoryToMoveSceneFrom = "Assets/" + projectPathName  + targetSceneFolderPath + "/" + targetSceneName + ".unity";
            string directoryToMoveSceneTo = targetGameLevel.assignedScenesDirectory + "/" + finalSceneName;
            FileUtil.MoveFileOrDirectory(adjustedDirectoryToMoveSceneFrom, directoryToMoveSceneTo);
            EditorUtility.SetDirty(targetGameLevel);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}