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

using GUIAnimator;

#endregion // Namespaces

namespace GUIAnimatorDemo
{

    // ######################################################################
    // Demo06 class
    // - Animates all GAui elements in the scene.
    // - Responds to user mouse click or tap on buttons.
    // This class is attached with "-SceneController-" object in "GA UUI - Demo06 (960x600px)" scene.
    // ######################################################################

    public class Demo06 : MonoBehaviour
    {

        // ########################################
        // Variables
        // ########################################

        #region Variables

        // Canvas
        public Canvas m_Canvas;

        // GAui objects of title text
        public GAui m_Title1;
        public GAui m_Title2;

        // GAui objects of top and bottom bars
        public GAui m_TopBar;
        public GAui m_BottomBar;

        // GAui objects of primary buttons
        public GAui m_PrimaryButton1;
        public GAui m_PrimaryButton2;
        public GAui m_PrimaryButton3;
        public GAui m_PrimaryButton4;
        public GAui m_PrimaryButton5;

        // GAui objects of secondary buttons
        public GAui m_SecondaryButton1;
        public GAui m_SecondaryButton2;
        public GAui m_SecondaryButton3;
        public GAui m_SecondaryButton4;
        public GAui m_SecondaryButton5;

        // Toggle state of buttons
        bool m_Button1_IsOn = false;
        bool m_Button2_IsOn = false;
        bool m_Button3_IsOn = false;
        bool m_Button4_IsOn = false;
        bool m_Button5_IsOn = false;

        #endregion // Variables

        // ########################################
        // MonoBehaviour Functions
        // http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
        // ########################################

        #region MonoBehaviour

        // Awake is called when the script instance is being loaded.
        // http://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
        void Awake()
        {
            if (enabled)
            {
                // Disable auto-animation and let this script controls all GAui elements in the scene.
                GSui.Instance.m_AutoAnimation = false;
            }
        }

        // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        // http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
        void Start()
        {
            // Play In-Animations of m_TopBar and m_BottomBar
            m_TopBar.PlayInAnims();
            m_BottomBar.PlayInAnims();

            // Play In-Animations of m_Title1 m_Title2
            StartCoroutine(MoveInTitleGameObjects());

            // Disable all scene switch buttons
            // http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
            GSui.Instance.EnableGraphicRaycaster(m_Canvas, false);
        }

        // Update is called every frame, if the MonoBehaviour is enabled.
        // http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
        void Update()
        {

        }

        #endregion // MonoBehaviour

        // ########################################
        // MoveIn/MoveOut functions
        // ########################################

        #region MoveIn/MoveOut

        // Move In m_Title1 and m_Title2
        IEnumerator MoveInTitleGameObjects()
        {
            yield return new WaitForSeconds(1.0f);

            // Move In m_Title1 and m_Title2
            m_Title1.PlayInAnims(eGUIMove.Self);
            m_Title2.PlayInAnims(eGUIMove.Self);

            // Play In-Animations of dialogs
            StartCoroutine(MoveInPrimaryButtons());

            // Enable all scene switch buttons
            // http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
            GSui.Instance.EnableGraphicRaycaster(m_Canvas, true);
        }

        // Play In-Animations of all primary buttons
        IEnumerator MoveInPrimaryButtons()
        {
            yield return new WaitForSeconds(1.0f);

            // Play In-Animations of all primary buttons
            m_PrimaryButton1.PlayInAnims(eGUIMove.Self);
            m_PrimaryButton2.PlayInAnims(eGUIMove.Self);
            m_PrimaryButton3.PlayInAnims(eGUIMove.Self);
            m_PrimaryButton4.PlayInAnims(eGUIMove.Self);

            m_PrimaryButton5.PlayInAnims(eGUIMove.Self);

            // Enable all scene switch buttons
            StartCoroutine(EnableAllDemoButtons());
        }

        // Play Out-Animations of all primary buttons
        public void HideAllGUIs()
        {
            // Play Out-Animations of all primary buttons
            m_PrimaryButton1.PlayOutAnims();
            m_PrimaryButton2.PlayOutAnims();
            m_PrimaryButton3.PlayOutAnims();
            m_PrimaryButton4.PlayOutAnims();
            m_PrimaryButton5.PlayOutAnims();

            // Play Out-Animations of all secondary buttons
            if (m_Button1_IsOn == true)
                m_SecondaryButton1.PlayOutAnims();
            if (m_Button2_IsOn == true)
                m_SecondaryButton2.PlayOutAnims();
            if (m_Button3_IsOn == true)
                m_SecondaryButton3.PlayOutAnims();
            if (m_Button4_IsOn == true)
                m_SecondaryButton4.PlayOutAnims();
            if (m_Button5_IsOn == true)
                m_SecondaryButton5.PlayOutAnims();

            // Play Out-Animations of m_Title1 and m_Title2
            StartCoroutine(HideTitleTextMeshes());
        }

        // Play Out-Animations of m_Title1 and m_Title2
        IEnumerator HideTitleTextMeshes()
        {
            yield return new WaitForSeconds(1.0f);

            // Play Out-Animations of m_Title1 and m_Title2
            m_Title1.PlayOutAnims(eGUIMove.Self);
            m_Title2.PlayOutAnims(eGUIMove.Self);

            // Play Out-Animations of m_TopBar and m_BottomBar
            m_TopBar.PlayOutAnims();
            m_BottomBar.PlayOutAnims();
        }

        #endregion // MoveIn/MoveOut

        // ########################################
        // Enable/Disable button functions
        // ########################################

        #region Enable/Disable buttons

        // Enable/Disable all scene switch Coroutine
        IEnumerator EnableAllDemoButtons()
        {
            yield return new WaitForSeconds(1.0f);

            // Enable all scene switch buttons
            // http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
            GSui.Instance.EnableGraphicRaycaster(m_Canvas, true);
        }

        // Disable all buttons for a short time
        IEnumerator DisableAllButtonsForSeconds(float DisableTime)
        {
            // Disable all buttons
            GSui.Instance.EnableAllButtons(false);

            yield return new WaitForSeconds(DisableTime);

            // Enable all buttons
            GSui.Instance.EnableAllButtons(true);
        }

        #endregion // Enable/Disable buttons

        // ########################################
        // UI Responder functions
        // ########################################

        #region UI Responder

        public void OnButton_1()
        {
            // Disable all buttons for a short time
            StartCoroutine(DisableAllButtonsForSeconds(0.6f));

            // Toggle m_Button1
            ToggleButton_1();

            // Toggle other buttons
            if (m_Button2_IsOn == true)
            {
                ToggleButton_2();
            }
            if (m_Button3_IsOn == true)
            {
                ToggleButton_3();
            }
            if (m_Button4_IsOn == true)
            {
                ToggleButton_4();
            }
            if (m_Button5_IsOn == true)
            {
                ToggleButton_5();
            }
        }

        public void OnButton_2()
        {
            // Disable all buttons for a short time
            StartCoroutine(DisableAllButtonsForSeconds(0.6f));

            // Toggle m_Button2
            ToggleButton_2();

            // Toggle other buttons
            if (m_Button1_IsOn == true)
            {
                ToggleButton_1();
            }
            if (m_Button3_IsOn == true)
            {
                ToggleButton_3();
            }
            if (m_Button4_IsOn == true)
            {
                ToggleButton_4();
            }
            if (m_Button5_IsOn == true)
            {
                ToggleButton_5();
            }
        }

        public void OnButton_3()
        {
            // Disable all buttons for a short time
            StartCoroutine(DisableAllButtonsForSeconds(0.6f));

            // Toggle m_Button3
            ToggleButton_3();

            // Toggle other buttons
            if (m_Button1_IsOn == true)
            {
                ToggleButton_1();
            }
            if (m_Button2_IsOn == true)
            {
                ToggleButton_2();
            }
            if (m_Button4_IsOn == true)
            {
                ToggleButton_4();
            }
            if (m_Button5_IsOn == true)
            {
                ToggleButton_5();
            }
        }

        public void OnButton_4()
        {
            // Disable all buttons for a short time
            StartCoroutine(DisableAllButtonsForSeconds(0.6f));

            // Toggle m_Button4
            ToggleButton_4();

            // Toggle other buttons
            if (m_Button1_IsOn == true)
            {
                ToggleButton_1();
            }
            if (m_Button2_IsOn == true)
            {
                ToggleButton_2();
            }
            if (m_Button3_IsOn == true)
            {
                ToggleButton_3();
            }
            if (m_Button5_IsOn == true)
            {
                ToggleButton_5();
            }
        }

        public void OnButton_5()
        {
            // Disable all buttons for a short time
            StartCoroutine(DisableAllButtonsForSeconds(0.6f));

            // Toggle m_Button5
            ToggleButton_5();

            // Toggle other buttons
            if (m_Button1_IsOn == true)
            {
                ToggleButton_1();
            }
            if (m_Button2_IsOn == true)
            {
                ToggleButton_2();
            }
            if (m_Button3_IsOn == true)
            {
                ToggleButton_3();
            }
            if (m_Button4_IsOn == true)
            {
                ToggleButton_4();
            }
        }

        #endregion // UI Responder

        // ########################################
        // Toggle button functions
        // ########################################

        #region Toggle Button

        // Toggle m_Button1
        void ToggleButton_1()
        {
            m_Button1_IsOn = !m_Button1_IsOn;
            if (m_Button1_IsOn == true)
            {
                // Play In-Animations of m_SecondaryButton1
                m_SecondaryButton1.PlayInAnims();
            }
            else
            {
                // Play Out-Animations of m_SecondaryButton1
                m_SecondaryButton1.PlayOutAnims();
            }
        }

        // Toggle m_Button2
        void ToggleButton_2()
        {
            m_Button2_IsOn = !m_Button2_IsOn;
            if (m_Button2_IsOn == true)
            {
                // Play In-Animations of m_SecondaryButton2
                m_SecondaryButton2.PlayInAnims();
            }
            else
            {
                // Play Out-Animations of m_SecondaryButton2
                m_SecondaryButton2.PlayOutAnims();
            }
        }

        // Toggle m_Button3
        void ToggleButton_3()
        {
            m_Button3_IsOn = !m_Button3_IsOn;
            if (m_Button3_IsOn == true)
            {
                // Play In-Animations of m_SecondaryButton3
                m_SecondaryButton3.PlayInAnims();
            }
            else
            {
                // Play Out-Animations of m_SecondaryButton3
                m_SecondaryButton3.PlayOutAnims();
            }
        }

        // Toggle m_Button4
        void ToggleButton_4()
        {
            m_Button4_IsOn = !m_Button4_IsOn;
            if (m_Button4_IsOn == true)
            {
                // Play In-Animations of m_SecondaryButton4
                m_SecondaryButton4.PlayInAnims();
            }
            else
            {
                // Play Out-Animations of m_SecondaryButton4
                m_SecondaryButton4.PlayOutAnims();
            }
        }

        // Toggle m_Button5
        void ToggleButton_5()
        {
            m_Button5_IsOn = !m_Button5_IsOn;
            if (m_Button5_IsOn == true)
            {
                // Play In-Animations of m_SecondaryButton5
                m_SecondaryButton5.PlayInAnims();
            }
            else
            {
                // Play Out-Animations of m_SecondaryButton5
                m_SecondaryButton5.PlayOutAnims();
            }
        }

        #endregion // Toggle Button
    }

}