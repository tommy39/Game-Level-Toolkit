using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Core.GameLevels;
using UnityEditor;

namespace IND.Editor.GameLevelsToolkit
{
    public class DuplicateGameLevel : EditorWindow
    {
        public static DuplicateGameLevel window;
        //[Required] [InlineEditor] 
        public GameLevel gameLevelToCreateDuplicateOf;
        //[InfoBox("Must Have a Location Name", InfoMessageType.Error, "HasNewLocationNameAssigned")]
        public string newGameLevelName;

        #region Unity GUI
        private GameLevelData gameLevelData;
        private int selectedValue = 0;
        private GameLevel[] levels;
        private string[] levelOptions;
        #endregion

        public static void OpenMenu()
        {
            window = (DuplicateGameLevel)GetWindow(typeof(DuplicateGameLevel));
            WindowRefresh.RefreshLevelsList(out window.levelOptions, out window.selectedValue, out window.levels, out window.gameLevelData);
        }

        private void OnGUI()
        {         
            newGameLevelName = EditorGUILayout.TextField("New Level Name", newGameLevelName);
            if (newGameLevelName == null || newGameLevelName == "")
            {
                EditorGUILayout.HelpBox("Name of new Level cannot be empty", MessageType.Error);
            }
            selectedValue = EditorGUILayout.Popup("Level To Create Duplicate Of", selectedValue, levelOptions);
            gameLevelToCreateDuplicateOf = levels[selectedValue];

            if (newGameLevelName != null && newGameLevelName != "")
            {
                if (GUILayout.Button("Duplicate Level"))
                {
                    DuplicateLevel();
                    WindowRefresh.RefreshLevelsList(out levelOptions, out selectedValue, out levels, out gameLevelData);
                    Debug.Log("Level Duplicated");
                }
            }
        }

        private bool HasNewLocationNameAssigned()
        {
            if (newGameLevelName == "" || newGameLevelName == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DuplicateLevel()
        {
            if (HasNewLocationNameAssigned() == true)
            {
                Debug.LogError("Has No New Location Name Assigned");
                return;
            }

            if (gameLevelToCreateDuplicateOf == null)
            {
                Debug.LogError("Location Required to Duplicate");
                return;
            }

            GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();
            string projectPathName = GameLevelToolkitWindow.GetProjectPathStringWithSlash();

            //Check if there is a location already existing with that name.
            GameLevel existingGameLevel = (GameLevel)AssetDatabase.LoadAssetAtPath("Assets/" + projectPathName + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + newGameLevelName + "/" + newGameLevelName + ".asset", typeof(GameLevel));
            if (existingGameLevel != null)
            {
                Debug.LogError("Location With Same Name Already Exists");
                return;
            }

            //Create the Location Resource 
            GameLevel gameLevelResource = GameLevelData.CreateInstance<GameLevel>();
            AssetDatabase.CreateAsset(GameLevel.CreateInstance<GameLevel>(), "Assets/" + projectPathName + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + "Existing " + SceneAndResourceFolderName.folderNameValue + "/" + newGameLevelName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            GameLevel createdGameLevel = (GameLevel)AssetDatabase.LoadAssetAtPath("Assets/" + projectPathName + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/" + "Existing " + SceneAndResourceFolderName.folderNameValue + "/" + newGameLevelName + ".asset", typeof(GameLevel));
            createdGameLevel.gameLevelName = newGameLevelName;
            createdGameLevel.assignedScenesDirectory = "Assets/" + projectPathName + "Scenes/" + SceneAndResourceFolderName.folderNameValue + "/" + newGameLevelName;
            createdGameLevel.levelDependencies = gameLevelToCreateDuplicateOf.levelDependencies;
            createdGameLevel.assignedScenes = gameLevelToCreateDuplicateOf.assignedScenes;
            GameLevelToolkitWindow.GetGameLevelsData().gameLevelsCreatedByUser.Add(createdGameLevel);

            //Create the Scene Directory
            string scenesFolder = AssetDatabase.CreateFolder("Assets/" + projectPathName + "Scenes/" + SceneAndResourceFolderName.folderNameValue, newGameLevelName);

            //Copy the Existing Scenes
            for (int i = 0; i < gameLevelToCreateDuplicateOf.assignedScenes.Count; i++)
            {
                string sceneToCopy = gameLevelToCreateDuplicateOf.assignedScenesDirectory + "/" + gameLevelToCreateDuplicateOf.gameLevelName + "_" + gameLevelToCreateDuplicateOf.assignedScenes[i] + ".unity";
                string destination = createdGameLevel.assignedScenesDirectory + "/" + newGameLevelName + "_" + gameLevelToCreateDuplicateOf.assignedScenes[i] + ".unity";
                FileUtil.CopyFileOrDirectory(sceneToCopy, destination);
            }

            EditorUtility.SetDirty(createdGameLevel);
            EditorUtility.SetDirty(toolkit.gameLevelsData);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}