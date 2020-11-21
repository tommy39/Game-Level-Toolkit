using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace IND.Core.GameLevels.Demo
{
    [CustomEditor(typeof(DemoLoadLevelController))]
    public class DemoLoadLevelControllerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DemoLoadLevelController controller = (DemoLoadLevelController)target;

            if(GUILayout.Button("Load Level"))
            {
                controller.LoadLevel();
            }
        }

    
    }
}