using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IND.Core.GameLevels;

namespace IND.Editor.GameLevelsToolkit
{
    public class RenameCategoryWindow : EditorWindow
    {
        public static RenameCategoryWindow window;
        private string newName;

        #region unityGUI
        private int selectedCategoryValue = 0;
        private string[] renamableCategories;
        #endregion

        public static void OpenMenu()
        {
            window = GetWindow<RenameCategoryWindow>();
            window.selectedCategoryValue = 0;
        }

        private void OnGUI()
        {
            renamableCategories = CategoriesManagement.GetAllCategories(false, false);
            if (renamableCategories.Length > 0)
            {
                selectedCategoryValue = EditorGUILayout.Popup("Category To Rename", selectedCategoryValue, renamableCategories);
                newName = EditorGUILayout.TextField("New Category Name", newName);
                if (string.IsNullOrEmpty(newName))
                {
                    EditorGUILayout.HelpBox("New Name cannot be empty", MessageType.Error);
                }

                if (newName == renamableCategories[selectedCategoryValue])
                {
                    EditorGUILayout.HelpBox("Category Name Already Exists", MessageType.Error);
                }
            }

            if (string.IsNullOrEmpty(newName) == false && renamableCategories.Length > 0 && newName != renamableCategories[selectedCategoryValue])
            {
                if(GUILayout.Button("Rename Category"))
                {
                    CategoriesManagement.RenameCategory(renamableCategories[selectedCategoryValue], newName);
                    selectedCategoryValue = 0;
                }
            }
        }
    }
}