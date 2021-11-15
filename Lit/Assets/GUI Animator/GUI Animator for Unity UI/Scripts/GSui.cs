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
using System.Collections;

using UnityEngine.UI;

using GUIAnimator;

#endregion // Namespaces

namespace GUIAnimator
{

    // ######################################################################
    // GSui class
    // Handles animation speed and auto animation of all GAui elements in the scene.
    // ######################################################################
    
    public class GSui : GUIAnimSystem
    {

        // ########################################
        // MonoBehaviour Functions
        // http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
        // ########################################

        #region MonoBehaviour

        // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        //void Start ()
        public override void GUIAnimSystemStart()
        {
            // ########################################
            // PERFORM YOUR SCRIPTS


            // ########################################
        }

        // Update is called every frame, if the MonoBehaviour is enabled.
        //void Update ()
        public override void GUIAnimSystemUpdate()
        {
            // ########################################
            // PERFORM YOUR SCRIPTS


            // ########################################
        }

        #endregion // MonoBehaviour

    }

}