using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace IND.Editor.GameLevelsToolkit
{
    public static class BuildSettingsSceneManagement
    {

        public static List<EditorBuildSettingsScene> GetEditorBuildSettingsScenes()
        {
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

            //Get The Existing Build List
            foreach (EditorBuildSettingsScene item in EditorBuildSettings.scenes)
            {
                editorBuildSettingsScenes.Add(item);
            }

            return editorBuildSettingsScenes;
        }

        public static List<EditorBuildSettingsScene> RemoveSceneFromBuild(EditorBuildSettingsScene scene, List<EditorBuildSettingsScene> buildScenes = null, bool forceRemoval = true, bool updateSceneBuild = false)
        {
            if(buildScenes == null)
            {
                buildScenes = GetEditorBuildSettingsScenes();
            }

            if (forceRemoval == false)
            {
                bool isSceneInBuild = false;

                for (int r = 0; r < buildScenes.Count; r++)
                {
                    if (buildScenes[r].path == scene.path)
                    {
                        isSceneInBuild = true;
                        break;
                    }
                }

                Debug.Log(isSceneInBuild);

                if (isSceneInBuild == false)
                {
                    return buildScenes;
                }
            }

            for (int i = 0; i < buildScenes.Count; i++)
            {
                if(buildScenes[i].path == scene.path)
                {
                    buildScenes.RemoveAt(i);
                }
            }

            if(updateSceneBuild == true)
            {
                EditorBuildSettings.scenes = buildScenes.ToArray();
            }

            return buildScenes;
        }
    }
}