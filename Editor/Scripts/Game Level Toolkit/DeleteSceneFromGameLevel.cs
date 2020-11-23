using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Core.GameLevels;
using UnityEditor;

namespace IND.Editor.GameLevelsToolkit
{
    public class DeleteSceneFromGameLevel : EditorWindow
    {
        public static DeleteSceneFromGameLevel window;
        //[InlineEditor] 
        public GameLevel selectedGameLevel;
        //[InfoBox("Must Have a Scene Name", InfoMessageType.Error, "HasSceneName")]
        //[ValueDropdown("GetAllSceneNames")]
        public string selectedSceneToDestroy;

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
            window = (DeleteSceneFromGameLevel)GetWindow(typeof(DeleteSceneFromGameLevel));
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

            selectedSceneToDestroy = sceneOptions[selectedSceneValue];

            if(GUILayout.Button("Delete Scene From Selected Level"))
            {
                RemoveScene();
                WindowRefresh.RefreshScenesList(selectedGameLevel, out sceneOptions, out selectedSceneValue);
            }
        }

        private bool HasSceneName()
        {
            if (selectedSceneToDestroy == "" || selectedSceneToDestroy == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RemoveScene()
        {
            if (HasSceneName() == true)
            {
                Debug.LogError("Scene Name Is Required");
                return;
            }

            //Check If scene Already Exists
            string targetSceneFile = selectedGameLevel.assignedScenesDirectory + "/" + selectedGameLevel.gameLevelName + "_" + selectedSceneToDestroy + ".unity";
            if (!System.IO.File.Exists(targetSceneFile))
            {
                //  Debug.Log(targetSceneFile);
                Debug.LogError("Scene Does Not Exist");
                return;
            }

            if (selectedGameLevel.assignedScenes.Count == 1)
            {
                Debug.LogError("Cannot Delete Last Scene in a location, a location required at least one scene, delete the location if you want otherwise");
                return;
            }

            selectedGameLevel.assignedScenes.Remove(selectedSceneToDestroy);
            EditorUtility.SetDirty(selectedGameLevel);
            FileUtil.DeleteFileOrDirectory(targetSceneFile);
            FileUtil.DeleteFileOrDirectory(targetSceneFile + ".meta");
            selectedSceneToDestroy = selectedGameLevel.assignedScenes[0];
        }
    }
}