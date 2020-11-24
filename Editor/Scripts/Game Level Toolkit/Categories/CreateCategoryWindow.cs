using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace IND.Editor.GameLevelsToolkit
{
    public class CreateCategoryWindow : EditorWindow
    {
        public string newCategoryName;
        public static CreateCategoryWindow window;

        public static void OpenMenu()
        {
            window = GetWindow<CreateCategoryWindow>();
        }

        private void OnGUI()
        {
            newCategoryName = EditorGUILayout.TextField("New Category Name", newCategoryName);

            if (string.IsNullOrEmpty(newCategoryName))
            {
                EditorGUILayout.HelpBox("Category Name Cannot Be Empty", MessageType.Error);
            }

            //Check Does CategoryNameExist
            string[] allCategories = CategoriesManagement.GetAllCategories(true, true);
            bool doesCategoryAlreadExist = false;
            foreach (string item in allCategories)
            {
                if(item == newCategoryName)
                {
                    doesCategoryAlreadExist = true;
                    break;
                }
            }

            if(doesCategoryAlreadExist == true)
            {
                EditorGUILayout.HelpBox("Category Already Exists", MessageType.Error);
            }

            if(doesCategoryAlreadExist == false && !string.IsNullOrEmpty(newCategoryName))
            {
                if(GUILayout.Button("Create Category"))
                {
                    CategoriesManagement.CreateCategory(newCategoryName);
                }
            }
        }
    }
}
