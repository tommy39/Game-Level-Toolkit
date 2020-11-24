using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Core.GameLevels;
using IND.Editor.GameLevelsToolkit;
using UnityEditor;

public class DeleteGameLevel : EditorWindow
{
    public static DeleteGameLevel window;
    private bool ignoreDependencies = false;

    // [InlineEditor] 
    public GameLevel gameLevelToDelete;

    //[InlineEditor] [HideIf("HideDependancies")]
    public List<GameLevel> gameLevelsThatAreDependantOnThisGameLevel = new List<GameLevel>();

    #region Unity GUI
    private GameLevelData gameLevelData;
    private int selectedValue = 0;
    private GameLevel[] levels;
    private string[] levelOptions;
    #endregion

    public static void OpenMenu()
    {
        window = (DeleteGameLevel)GetWindow(typeof(DeleteGameLevel));
        WindowRefresh.RefreshLevelsList(out window.levelOptions, out window.selectedValue, out window.levels, out window.gameLevelData);
    }

    private void OnGUI()
    {
        selectedValue = EditorGUILayout.Popup("Level to Delete", selectedValue, levelOptions);

        if (GUILayout.Button("Delete Level"))
        {
            gameLevelToDelete = levels[selectedValue];
            DeleteLevel();
            WindowRefresh.RefreshLevelsList(out levelOptions, out selectedValue, out levels, out gameLevelData);
        }
    }


    private bool HideDependancies()
    {
        if (gameLevelsThatAreDependantOnThisGameLevel.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void DeleteLevel()
    {
        if (gameLevelToDelete == null || string.IsNullOrEmpty(gameLevelToDelete.gameLevelName))
        {
            Debug.LogError("Requires a Level To Delete");
            return;
        }



        gameLevelsThatAreDependantOnThisGameLevel.Clear();

        //Check If any locations are dependant on  this location
        if (ignoreDependencies == false)
        {
            bool hasDependency = false;
            GameLevelData gameLevelsData = GameLevelToolkitWindow.GetRefreshedToolkitWindow().gameLevelsData;
            for (int i = 0; i < gameLevelsData.gameLevelsCreatedByUser.Count; i++)
            {
                for (int g = 0; g < gameLevelsData.gameLevelsCreatedByUser[i].levelDependencies.Count; g++)
                {
                    if (gameLevelsData.gameLevelsCreatedByUser[i].levelDependencies[g] == gameLevelToDelete)
                    {
                        gameLevelsThatAreDependantOnThisGameLevel.Add(gameLevelsData.gameLevelsCreatedByUser[i].levelDependencies[g]);
                        hasDependency = true;
                    }
                }
            }

            if (hasDependency == true)
            {
                Debug.LogError("Has Existing Dependencies");
                return;
            }
        }
        GameLevelToolkitWindow toolkit = GameLevelToolkitWindow.GetRefreshedToolkitWindow();
        string projectPath = GameLevelToolkitWindow.GetProjectPathStringWithSlash();

        //Remove From The SceneBuild
        for (int i = 0; i < gameLevelToDelete.assignedScenes.Count; i++)
        {
            EditorBuildSettingsScene scene = new EditorBuildSettingsScene("Assets/" + projectPath + "Scenes/" + SceneAndResourceFolderName.folderNameValue + "/" + gameLevelToDelete.gameLevelName + "/" + gameLevelToDelete.gameLevelName + "_" + gameLevelToDelete.assignedScenes[i] + ".unity", true);
            BuildSettingsSceneManagement.RemoveSceneFromBuild(scene, null, false, true);
        }



        //Delete Locations Scene Folder
        string scenesFolderPath = gameLevelToDelete.assignedScenesDirectory + "/";
        FileUtil.DeleteFileOrDirectory(scenesFolderPath);
        FileUtil.DeleteFileOrDirectory(gameLevelToDelete.assignedScenesDirectory + ".meta");

        //Remove Location From LocationData
        GameLevelToolkitWindow.GetGameLevelsData().gameLevelsCreatedByUser.Remove(gameLevelToDelete);



        //Delete Locations Resource Folder
        string resourceDirectory = "Assets/" + projectPath + "Resources/" + SceneAndResourceFolderName.folderNameValue + "/Existing " + SceneAndResourceFolderName.folderNameValue + "/" + gameLevelToDelete.gameLevelName + ".asset";
        FileUtil.DeleteFileOrDirectory(resourceDirectory);
        FileUtil.DeleteFileOrDirectory(resourceDirectory + ".meta");
        EditorUtility.SetDirty(toolkit.gameLevelsData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
