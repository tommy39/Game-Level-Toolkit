using IND.Core.GameLevels;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IND.Editor.GameLevelsToolkit
{
    public class RenameScene : EditorWindow
    {
        public static RenameScene window;
        //[InlineEditor] [Required] 
        public GameLevel selectedGameLevel;
        //[ShowIf("selectedGameLevel")] [ValueDropdown("GetAllSceneNames")] 
        public string sceneToRename;
        public string newName;

        #region Unity GUI
        //Game Level
        private GameLevelData gameLevelData;
        private int selectedValue = 0;
        private GameLevel[] levels;
        private string[] levelOptions;
        private int previousSelectedValue = 0;

        //Scene Variables
        private int selectedSceneValue = 0;
        private string[] sceneOptions;
        #endregion

        public static void OpenMenu()
        {
            window = (RenameScene)GetWindow(typeof(RenameScene), false, "Rename Scenes" );
            WindowRefresh.RefreshLevelsList(out window.levelOptions, out window.selectedValue, out window.levels, out window.gameLevelData);
            window.selectedGameLevel = window.levels[window.selectedValue];
            WindowRefresh.RefreshScenesList(window.selectedGameLevel, out window.sceneOptions, out window.selectedSceneValue);
        }

        private void OnGUI()
        {
            selectedValue = EditorGUILayout.Popup("Selected Level", selectedValue, levelOptions);
            selectedSceneValue = EditorGUILayout.Popup("Scene To Delete", selectedSceneValue, sceneOptions);


            selectedGameLevel = levels[selectedValue];

            if (previousSelectedValue != selectedValue)
            {
                WindowRefresh.RefreshScenesList(selectedGameLevel, out sceneOptions, out selectedSceneValue);
                previousSelectedValue = selectedValue;
            }

            sceneToRename = sceneOptions[selectedSceneValue];

            newName = EditorGUILayout.TextField("New Scene Name", newName);

            if (!string.IsNullOrEmpty(newName))
            {
                if (GUILayout.Button("Rename Selected Scene"))
                {
                    RenameSelectedScene();
                    WindowRefresh.RefreshScenesList(selectedGameLevel, out sceneOptions, out selectedSceneValue);
                }
            }
        }

        private void RenameSelectedScene()
        {
            if (selectedGameLevel == null)
            {
                Debug.LogError("Must Have a Game Level Assigned");
                return;
            }

            if (sceneToRename == null)
            {
                Debug.LogError("Must Have a scene Assigned");
                return;
            }

            if (newName == null || newName == "")
            {
                Debug.LogError("Requires a new name to rename scene");
                return;
            }

            RenameSceneFunc(selectedGameLevel, sceneToRename, newName);
        }

        public static void RenameSceneFunc(GameLevel gameLevelData, string sceneToRename, string newName)
        {
            int indexOfSceneName = gameLevelData.assignedScenes.IndexOf(sceneToRename);

            //Get Path To Target Scene
            string originalPathToScene = gameLevelData.assignedScenesDirectory + "/" + gameLevelData.gameLevelName + "_" + sceneToRename + ".unity";
            string newNameWithPrefix = gameLevelData.gameLevelName + "_" + newName + ".unity";

            string adjustedName = AssetDatabase.RenameAsset(originalPathToScene, newNameWithPrefix);
            EditorUtility.SetDirty(gameLevelData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            gameLevelData.assignedScenes[indexOfSceneName] = newName;
        }

        public static void RenameSceneWithLocationChange(GameLevel newGameLevelData, string originalLocationName, string sceneToRename, string newName)
        {
            int indexOfSceneName = newGameLevelData.assignedScenes.IndexOf(sceneToRename);

            //Get Path To Target Scene
            string originalPathToScene = newGameLevelData.assignedScenesDirectory + "/" + originalLocationName + "_" + sceneToRename + ".unity";
            string newNameWithPrefix = newGameLevelData.gameLevelName + "_" + newName + ".unity";

            Debug.Log(originalPathToScene);
            Debug.Log(newNameWithPrefix);

            string adjustedName = AssetDatabase.RenameAsset(originalPathToScene, newNameWithPrefix);
            Debug.Log(adjustedName);
            EditorUtility.SetDirty(newGameLevelData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log(newName);
            newGameLevelData.assignedScenes[indexOfSceneName] = newName;
        }
    }
}