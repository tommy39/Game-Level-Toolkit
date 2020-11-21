using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Editor.GameLevelsToolkit
{
    public class GameLevelToolkitSettings : ScriptableObject
    {
        public bool useRootProjectDirectoryPath = true;
        public string projectDirectoryPathName = "_ProjectDirectory";
    }
}