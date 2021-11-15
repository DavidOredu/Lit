// GUI Animator for Unity UI
// Version: 1.2.0
// Compatilble: Unity 2017.1.0f3 or higher, more info in Readme.txt file.
//
// Developer:				Gold Experience Team (https://assetstore.unity.com/publishers/4162)
// Unity Asset Store:		https://assetstore.unity.com/packages/tools/gui/gui-animator-for-unity-ui-28709
//
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com.

#region Namespaces

using UnityEngine;
using UnityEditor;
using System.Collections;

using GUIAnimator;

#endregion // Namespaces

namespace GUIAnimatorEditor
{

    // ######################################################################
    // GAuiEditor class
    // Custom editor for GAui component
    // ######################################################################

    // http://docs.unity3d.com/ScriptReference/CustomEditor.html
    // http://docs.unity3d.com/ScriptReference/CustomEditor-ctor.html
    // http://unity3d.com/learn/tutorials/modules/intermediate/editor/adding-buttons-to-inspector
    [CustomEditor(typeof(GAui))]
    // http://docs.unity3d.com/ScriptReference/Editor.html
    public class GAuiEditor : GUIAnimEditor
    {
        #region Variables

        #endregion // Variables

        // ########################################
        // Editor Functions
        // http://docs.unity3d.com/ScriptReference/Editor.html
        // ########################################

        #region Editor functions

        // This function is called when the object is loaded
        // http://docs.unity3d.com/ScriptReference/ScriptableObject.OnEnable.html
        //public override void OnEnable()
        //{
        //	base.OnEnable();

        // ########################################
        //*** PERFORM YOUR EDITOR SCRIPTS HERE ***//


        // ########################################
        //}

        // Implement this function to make a custom inspector
        // http://docs.unity3d.com/ScriptReference/Editor.OnInspectorGUI.html
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // ########################################
            //*** PERFORM YOUR EDITOR SCRIPTS HERE ***//


            // ########################################
        }

        #endregion // Editor functions
    }

}