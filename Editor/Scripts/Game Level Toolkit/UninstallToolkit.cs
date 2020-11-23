using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IND.Editor.GameLevelsToolkit
{
    public class UninstallToolkit : EditorWindow
    {
        public static UninstallToolkit window;
        public static void OpenMenu()
        {
            window = (UninstallToolkit)GetWindow(typeof(UninstallToolkit));
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox("Uninstall Will Delete Everything Created through the toolkit, including scenes created through the toolkit", MessageType.Warning);

            if (GUILayout.Button("Uninstall"))
            {
                Uninstall();
                Close();
            }
        }

        private void Uninstall()
        {
            string resourcesFolderDir = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Resources/" + SceneAndResourceFolderName.folderNameValue;
            string scenesFolderDir = "Assets/" + GameLevelToolkitWindow.GetProjectPathStringWithSlash() + "Scenes/" + SceneAndResourceFolderName.folderNameValue;
            //Destroy Resources
            FileUtil.DeleteFileOrDirectory(resourcesFolderDir);
            FileUtil.DeleteFileOrDirectory(resourcesFolderDir + ".meta");

            //Destroy Created Scenes Folder
            FileUtil.DeleteFileOrDirectory(scenesFolderDir);
            FileUtil.DeleteFileOrDirectory(scenesFolderDir + ".meta");

            if (GameLevelToolkitWindow.toolkitWindow != null)
            {
                GameLevelToolkitWindow.toolkitWindow.initialDataHasBeenCreated = false;
            }

            Debug.Log("Successful Uninstall");
        }
    }
}