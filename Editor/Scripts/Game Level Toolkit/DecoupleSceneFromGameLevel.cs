using IND.Core.GameLevels;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace IND.Editor.GameLevelsToolkit
{
    public class DecoupleSceneFromGameLevel : EditorWindow
    {
        public static DecoupleSceneFromGameLevel window;
        public GameLevel selectedGameLevel;
        public string targetSceneToDecouple;
        public string targetFolderToDumpSceneIn = "Scenes/";

        #region Unity GUI
        //Game Level
        private GameLevelData gameLevelData;
        private int selectedValue = 0;
        private GameLevel[] levels;
        private string[] levelOptions;

        //Scene Variables
        private int selectedSceneValue = 0;
        private string[] sceneOptions;
        #endregion

        public static void OpenMenu()
        {
            window = (DecoupleSceneFromGameLevel)GetWindow(typeof(DecoupleSceneFromGameLevel), false, "Decouple Scene From Level");
            WindowRefresh.RefreshLevelsList(out window.levelOptions, out window.selectedValue, out window.levels, out window.gameLevelData);
            window.selectedGameLevel = window.levels[window.selectedValue];
            WindowRefresh.RefreshScenesList(window.selectedGameLevel, out window.sceneOptions, out window.selectedSceneValue);
        }

        private void OnGUI()
        {
            selectedValue = EditorGUILayout.Popup("Selected Level", selectedValue, levelOptions);
            selectedSceneValue = EditorGUILayout.Popup("Scene To Decouple", selectedSceneValue, sceneOptions);
            selectedGameLevel = levels[selectedValue];
            targetSceneToDecouple = sceneOptions[selectedSceneValue];

            EditorGUILayout.HelpBox("Use The Directory After 'Assets/', e.g. 'Scenes/MyScene01'", MessageType.Info);
            targetFolderToDumpSceneIn = EditorGUILayout.TextField("Target Folder To Dump Scene In", targetFolderToDumpSceneIn);

            if (Directory.Exists("Assets/" + targetFolderToDumpSceneIn))
            {
                if (GUILayout.Button("Decouple Scene From Game Level"))
                {
                    DecoupleScene();
                    WindowRefresh.RefreshScenesList(selectedGameLevel, out sceneOptions, out selectedSceneValue);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Target Folder Does Not Exist, Please Create Manually", MessageType.Error);
            }
        }

        private void DecoupleScene()
        {
            if (selectedGameLevel == null)
            {
                Debug.LogError("Selected Location Required");
                return;
            }

            if (targetSceneToDecouple == null)
            {
                Debug.LogError("No Scene Selected");
                return;
            }

            if (selectedGameLevel.assignedScenes.Count == 1)
            {
                Debug.LogError("Locations must have at least one scene assigned too it, Otherwise you should delete the scene");
                return;
            }

            if (!selectedGameLevel.assignedScenes.Contains(targetSceneToDecouple))
            {
                Debug.LogError("Scene Does Not Exist, Perhaps wrong one is chosen that has been removed");
                return;
            }

            //Lets Rename The File So It Doesn't have any location  prefix
            string fileToPathToRename = selectedGameLevel.assignedScenesDirectory + "/" + selectedGameLevel.gameLevelName + "_" + targetSceneToDecouple + ".unity";
            AssetDatabase.RenameAsset(fileToPathToRename, targetSceneToDecouple);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //Get New Path Of Renamed File
            string fileToPathToMove = selectedGameLevel.assignedScenesDirectory + "/" + targetSceneToDecouple + ".unity";
            FileUtil.MoveFileOrDirectory(fileToPathToMove, "Assets/" + targetFolderToDumpSceneIn + targetSceneToDecouple + ".unity");
            FileUtil.MoveFileOrDirectory(fileToPathToMove + ".meta", "Assets/" + targetFolderToDumpSceneIn + targetSceneToDecouple + ".unity" + ".meta");

            selectedGameLevel.assignedScenes.Remove(targetSceneToDecouple);
            EditorUtility.SetDirty(selectedGameLevel);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}