using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace IND.Editor.GameLevelsToolkit
{
    public class LoadMasterScene : EditorWindow
    {
        public static LoadMasterScene window;
        public bool openAdditively = true;

        public static void OpenMenu()
        {
            window = GetWindow<LoadMasterScene>();
        }

        private void OnGUI()
        {
            openAdditively = EditorGUILayout.Toggle("Open Scene Additively", openAdditively);
            if (GUILayout.Button("Open Master Scene"))
            {
                OpenMasterScene();
            }
        }

        public void OpenMasterScene()
        {
            List<Scene> loadedScenes = new List<Scene>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i));
            }
            Scene[] loadedScenesArray = loadedScenes.ToArray();
            EditorSceneManager.SaveModifiedScenesIfUserWantsTo(loadedScenesArray);

            string scenePath = "Assets/" + GameLevelToolkit.GetProjectPathStringWithSlash() + "Scenes/" + SceneAndResourceFolderName.folderNameValue + "/Master Scene/Master Scene.unity";
            if (openAdditively == true)
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }
            else
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            }
        }
    }
}