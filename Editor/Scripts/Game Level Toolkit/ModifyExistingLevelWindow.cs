using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace IND.Editor.GameLevelsToolkit
{
    public class ModifyExistingLevelWindow : EditorWindow
    {
        public static ModifyExistingLevelWindow window;

        public static void OpenMenu()
        {
            window = (ModifyExistingLevelWindow)GetWindow(typeof(ModifyExistingLevelWindow));
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Delete Level"))
            {
                DeleteExistingLevel();
            }

            if (GUILayout.Button("Duplicate Level"))
            {
                DuplicateAnExistingLevel();
            }

            if (GUILayout.Button("Add New Scene to Level"))
            {
                AddNewSceneToSelectedLevel();
            }

            if (GUILayout.Button("Delete Scene From Level"))
            {
                DeleteSceneFromExistingLevel();
            }

            if (GUILayout.Button("Rename Level"))
            {
                RenameExistingLevel();
            }

            if (GUILayout.Button("Rename Scene"))
            {
                RenameScene();
            }

            if (GUILayout.Button("Add Existing Independent Scene to Level"))
            {
                AddExistingSceneToExistingLevel();
            }

            if (GUILayout.Button("Decouple Scene from Level"))
            {
                DecoupleSceneFromLevel();
            }

            if(GUILayout.Button("Remove a Dependency From Level"))
            {
                RemoveLevelDepdendencyWindow.OpenMenu();
            }
        }

        private void DeleteExistingLevel()
        {
            DeleteGameLevel.OpenMenu();
        }

        private void DuplicateAnExistingLevel()
        {
            DuplicateGameLevel.OpenMenu();
        }

        private void AddNewSceneToSelectedLevel()
        {
            AddNewSceneToGameLevel.OpenMenu();
        }

        private void DeleteSceneFromExistingLevel()
        {
            DeleteSceneFromGameLevel.OpenMenu();
        }

        private void RenameExistingLevel()
        {
            RenameGameLevel.OpenMenu();
        }

        private void AddExistingSceneToExistingLevel()
        {
            AddExistingSceneToGameLevel.OpenMenu();
        }

        private void DecoupleSceneFromLevel()
        {
            DecoupleSceneFromGameLevel.OpenMenu();
        }

        private void RenameScene()
        {
            GameLevelsToolkit.RenameScene.OpenMenu();
        }

    }
}