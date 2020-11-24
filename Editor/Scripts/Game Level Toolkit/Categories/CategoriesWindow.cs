using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IND.Core.GameLevels;

namespace IND.Editor.GameLevelsToolkit
{
    public class CategoriesWindow : EditorWindow
    {
        public static CategoriesWindow window;
        public static void OpenMenu()
        {
            window = GetWindow<CategoriesWindow>();
        }

        private void OnGUI()
        {
            if(GUILayout.Button("Create New Category"))
            {
                CreateCategoryWindow.OpenMenu();
            }

            if(GUILayout.Button("Rename Existing Category"))
            {
                RenameCategoryWindow.OpenMenu();
            }
        }
    }
}