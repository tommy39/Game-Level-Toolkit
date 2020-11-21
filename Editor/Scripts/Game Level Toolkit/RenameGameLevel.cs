using IND.Core.GameLevels;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IND.Editor.GameLevelsToolkit
{
    public class RenameGameLevel : EditorWindow
    {

        public static RenameGameLevel window;
        //[InlineEditor] [Required]
        public GameLevel selectedGameLevel;
        //[InfoBox("Game Level Must Have a Name", InfoMessageType.Error, "HasNewName")]
        public string newGameLevelName;

        #region Unity GUI
        //Game Level
        private GameLevelData gameLevelData;
        private int selectedValue = 0;
        private GameLevel[] levels;
        private string[] levelOptions;
        #endregion

        public static void OpenMenu()
        {
            window = (RenameGameLevel)GetWindow(typeof(RenameGameLevel));
            WindowRefresh.RefreshLevelsList(out window.levelOptions, out window.selectedValue, out window.levels, out window.gameLevelData);
            window.selectedGameLevel = window.levels[window.selectedValue];
        }

        private void OnGUI()
        {
            selectedValue = EditorGUILayout.Popup("Selected Level", selectedValue, levelOptions);
            selectedGameLevel = levels[selectedValue];
            newGameLevelName = EditorGUILayout.TextField("New Level Name", newGameLevelName);

            if (!string.IsNullOrEmpty(newGameLevelName))
            {
                if (GUILayout.Button("Rename Selected Level"))
                {
                    RenameSelectedGameLevel();
                    WindowRefresh.RefreshLevelsList(out levelOptions, out selectedValue, out levels, out gameLevelData);
                }
            }
        }


        private bool HasNewName()
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

        private void RenameSelectedGameLevel()
        {
            if (HasNewName() == true)
            {
                Debug.LogError("Requires Name");
                return;
            }

            if (DoesGameLevelExistStatic.ReturnBool(newGameLevelName) == true)
            {
                Debug.LogError("A Game Level With This Name Already Exists");
                return;
            }

            string originalGameLevelName = selectedGameLevel.gameLevelName;
            // string originalGameLevelResourceFolder = GetGameLevelDirectoryPaths.GetGameLevelResourceFolder(originalGameLevelName, false) + ".asset";
            string originalGameLevelScenesFolder = GetGameLevelDirectoryPaths.GetGameLevelScenesFolder(originalGameLevelName, false);

            //string newGameLevelResourceFolder = GetGameLevelDirectoryPaths.GetGameLevelResourceFolder(newGameLevelName, false);
            string newGameLevelScenesFolder = GetGameLevelDirectoryPaths.GetGameLevelScenesFolder(newGameLevelName, false);

            //Create Resources Folder & Meta Data
            // FileUtil.MoveFileOrDirectory(originalGameLevelResourceFolder, newGameLevelResourceFolder);
            // FileUtil.DeleteFileOrDirectory(originalGameLevelResourceFolder + ".meta");

            //Create Scenes Folder & Meta Data
            FileUtil.MoveFileOrDirectory(originalGameLevelScenesFolder, newGameLevelScenesFolder);
            FileUtil.DeleteFileOrDirectory(originalGameLevelScenesFolder + ".meta");

            //Update Scriptable Object Data
            selectedGameLevel.gameLevelName = newGameLevelName;
            selectedGameLevel.assignedScenesDirectory = newGameLevelScenesFolder;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();


            for (int i = 0; i < selectedGameLevel.assignedScenes.Count; i++)
            {
                string newName = selectedGameLevel.assignedScenes[i];
                Debug.Log(selectedGameLevel.gameLevelName);
                RenameScene.RenameSceneWithLocationChange(selectedGameLevel, originalGameLevelName, selectedGameLevel.assignedScenes[i], newName);
            }
            //Rename Scriptable object Location
            AssetDatabase.RenameAsset(GetGameLevelDirectoryPaths.GetGameLevelAssetFileDirectory(selectedGameLevel), newGameLevelName + ".asset");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}