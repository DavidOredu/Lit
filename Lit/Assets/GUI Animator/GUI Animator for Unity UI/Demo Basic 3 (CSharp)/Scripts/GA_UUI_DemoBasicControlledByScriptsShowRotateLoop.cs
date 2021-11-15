// GUI Animator for Unity UI
// Version: 1.2.0
// Compatilble: Unity 2017.1.0f3 or higher, more info in Readme.txt file.
//
// Developer:				Gold Experience Team (https://assetstore.unity.com/publishers/4162)
// Unity Asset Store:		https://assetstore.unity.com/packages/tools/gui/gui-animator-for-unity-ui-28709
//
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using GUIAnimator;

#endregion // Namespaces

namespace GUIAnimatorDemo
{

    // ######################################################################
    // GA_UUI_DemoBasicControlledByScriptsShowRotateLoop class
    // - Shows buttons
    // - Plays sample Rotate animations when the button is pressed.
    // This script is attached with Canvas in the scene
    // ######################################################################

    public class GA_UUI_DemoBasicControlledByScriptsShowRotateLoop : MonoBehaviour
    {

        // ########################################
        // MonoBehaviour functions
        // ########################################

        #region MonoBehaviour Functions

        private float m_WaitTime = 4.0f;
        private float m_WaitTimeCount = 0;
        private bool m_ShowAnimationButton = true;

        // Use this for initialization
        // https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
        void Awake()
        {
            // Disable auto-animation and let this script controls all GAui elements in the scene.
            if (enabled)
            {
                // Set Animation Speed
                GSui.Instance.m_GUISpeed = 1.0f;

                // Disable AutoAnimation
                GSui.Instance.m_AutoAnimation = false;
            }
        }

        // Use this for initialization
        // https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
        void Start()
        {
        }

        // Update is called once per frame
        // https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
        void Update()
        {

            // Count down timer for MoveIn/MoveOut buttons
            if (m_WaitTimeCount > 0 && m_WaitTimeCount <= m_WaitTime)
            {
                m_WaitTimeCount -= Time.deltaTime;
                if (m_WaitTimeCount <= 0)
                {
                    m_WaitTimeCount = 0;

                    // Switch status of m_ShowMoveInButton
                    m_ShowAnimationButton = !m_ShowAnimationButton;
                }
            }
        }

        // OnGUI is called for rendering and handling GUI events.
        // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnGUI.html
        void OnGUI()
        {
            // Show GUI button when ready
            if (m_WaitTimeCount <= 0)
            {
                Rect rect = new Rect((Screen.width - 250) / 2, (Screen.height - 50) / 2, 250, 50);

                // Show MoveIn button
                if (m_ShowAnimationButton == true)
                {
                    if (GUI.Button(rect, "Play Animations"))
                    {
                        // Play MoveIn animations
                        GSui.Instance.PlayInAnims(this.transform, true);
                        m_WaitTimeCount = m_WaitTime;
                    }
                }
            }
        }

        #endregion // MonoBehaviour Functions
    }

}