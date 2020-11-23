using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IND.Editor.GameLevelsToolkit
{
    public class GameLevelToolkitSettings : ScriptableObject
    {
        public bool useRootProjectDirectoryPath = true;
        public string projectDirectoryPathName = "_ProjectDirectory";

        private void OnEnable()
        {
            EditorUtility.SetDirty(this);
        }
    }
}