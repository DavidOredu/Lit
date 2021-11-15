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
    // Demo08 class
    // - Animates all GAui elements in the scene.
    // - Responds to user mouse click or tap on buttons.
    // This class is attached with "-SceneController-" object in "GA UUI - Demo08 (960x600px)" scene.
    // ######################################################################

    public class Demo08 : MonoBehaviour
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

        // GAui objects of 4 primary buttons
        public GAui m_CenterButtons;

        // GAui objects of buttons
        public GAui m_Button1;
        public GAui m_Button2;
        public GAui m_Button3;
        public GAui m_Button4;

        // GAui objects of top, left, right and bottom bars
        public GAui m_Bar1;
        public GAui m_Bar2;
        public GAui m_Bar3;
        public GAui m_Bar4;

        // Toggle state of top, left, right and bottom bars
        bool m_Bar1_IsOn = false;
        bool m_Bar2_IsOn = false;
        bool m_Bar3_IsOn = false;
        bool m_Bar4_IsOn = false;

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

            // Play In-Animations of m_Title1 and m_Title2
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

        // Play In-Animations of m_Title1 and m_Title2
        IEnumerator MoveInTitleGameObjects()
        {
            yield return new WaitForSeconds(1.0f);

            // Play In-Animations of m_Title1 and m_Title2
            m_Title1.PlayInAnims(eGUIMove.Self);
            m_Title2.PlayInAnims(eGUIMove.Self);

            // Play In-Animations of all primary buttons
            StartCoroutine(MoveInPrimaryButtons());
        }

        // Play In-Animations of all primary buttons
        IEnumerator MoveInPrimaryButtons()
        {
            yield return new WaitForSeconds(1.0f);

            // Play In-Animations of all primary buttons
            m_CenterButtons.PlayInAnims();

            // Enable all scene switch buttons
            StartCoroutine(EnableAllDemoButtons());
        }

        // Play Out-Animations of all primary buttons
        public void HideAllGUIs()
        {
            // Play Out-Animations of all primary buttons
            m_CenterButtons.PlayOutAnims();

            // Play Out-Animations of all side bars
            if (m_Bar1_IsOn == true)
                m_Bar1.PlayOutAnims();
            if (m_Bar2_IsOn == true)
                m_Bar2.PlayOutAnims();
            if (m_Bar3_IsOn == true)
                m_Bar3.PlayOutAnims();
            if (m_Bar4_IsOn == true)
                m_Bar4.PlayOutAnims();

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
        IEnumerator DisableButtonForSeconds(GameObject GO, float DisableTime)
        {
            // Disable all buttons
            GSui.Instance.EnableButton(GO.transform, false);

            yield return new WaitForSeconds(DisableTime);

            // Enable all buttons
            GSui.Instance.EnableButton(GO.transform, true);
        }

        #endregion // Enable/Disable buttons

        // ########################################
        // UI Responder functions
        // ########################################

        #region UI Responder

        public void OnButton_1()
        {
            // Toggle m_Bar1
            ToggleBar1();

            // Toggle other bars
            if (m_Bar2_IsOn == true)
            {
                ToggleBar2();
            }
            if (m_Bar3_IsOn == true)
            {
                ToggleBar3();
            }
            if (m_Bar4_IsOn == true)
            {
                ToggleBar4();
            }

            // Disable m_Button1, m_Button2, m_Button3, m_Button4 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Button1.gameObject, 0.75f));
            StartCoroutine(DisableButtonForSeconds(m_Button2.gameObject, 0.75f));
            StartCoroutine(DisableButtonForSeconds(m_Button3.gameObject, 0.75f));
            StartCoroutine(DisableButtonForSeconds(m_Button4.gameObject, 0.75f));
        }

        public void OnButton_2()
        {
            // Toggle m_Bar2
            ToggleBar2();

            // Toggle other bars
            if (m_Bar1_IsOn == true)
            {
                ToggleBar1();
            }
            if (m_Bar3_IsOn == true)
            {
                ToggleBar3();
            }
            if (m_Bar4_IsOn == true)
            {
                ToggleBar4();
            }

            // Disable m_Button1, m_Button2, m_Button3, m_Button4 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Button1.gameObject, 0.75f));
            StartCoroutine(DisableButtonForSeconds(m_Button2.gameObject, 0.75f));
            StartCoroutine(DisableButtonForSeconds(m_Button3.gameObject, 0.75f));
            StartCoroutine(DisableButtonForSeconds(m_Button4.gameObject, 0.75f));
        }

        public void OnButton_3()
        {
            // Toggle m_Bar3
            ToggleBar3();

            // Toggle other bars
            if (m_Bar1_IsOn == true)
            {
                ToggleBar1();
            }
            if (m_Bar2_IsOn == true)
            {
                ToggleBar2();
            }
            if (m_Bar4_IsOn == true)
            {
                ToggleBar4();
            }

            // Disable m_Button1, m_Button2, m_Button3, m_Button4 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Button1.gameObject, 0.75f));
            StartCoroutine(DisableButtonForSeconds(m_Button2.gameObject, 0.75f));
            StartCoroutine(DisableButtonForSeconds(m_Button3.gameObject, 0.75f));
            StartCoroutine(DisableButtonForSeconds(m_Button4.gameObject, 0.75f));
        }

        public void OnButton_4()
        {
            // Toggle m_Bar4
            ToggleBar4();

            // Toggle other bars
            if (m_Bar1_IsOn == true)
            {
                ToggleBar1();
            }
            if (m_Bar2_IsOn == true)
            {
                ToggleBar2();
            }
            if (m_Bar3_IsOn == true)
            {
                ToggleBar3();
            }

            // Disable m_Button1, m_Button2, m_Button3, m_Button4 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Button1.gameObject, 0.75f));
            StartCoroutine(DisableButtonForSeconds(m_Button2.gameObject, 0.75f));
            StartCoroutine(DisableButtonForSeconds(m_Button3.gameObject, 0.75f));
            StartCoroutine(DisableButtonForSeconds(m_Button4.gameObject, 0.75f));
        }

        #endregion // UI Responder

        // ########################################
        // Toggle button functions
        // ########################################

        #region Toggle Button

        // Toggle m_Bar1
        void ToggleBar1()
        {
            m_Bar1_IsOn = !m_Bar1_IsOn;
            if (m_Bar1_IsOn == true)
            {
                // m_Bar1 moves in
                m_Bar1.PlayInAnims();
            }
            else
            {
                // m_Bar1 moves out
                m_Bar1.PlayOutAnims();
            }
        }

        // Toggle m_Bar2
        void ToggleBar2()
        {
            m_Bar2_IsOn = !m_Bar2_IsOn;
            if (m_Bar2_IsOn == true)
            {
                // m_Bar2 moves in
                m_Bar2.PlayInAnims();
            }
            else
            {
                // m_Bar2 moves out
                m_Bar2.PlayOutAnims();
            }
        }

        // Toggle m_Bar3
        void ToggleBar3()
        {
            m_Bar3_IsOn = !m_Bar3_IsOn;
            if (m_Bar3_IsOn == true)
            {
                // m_Bar3 moves in
                m_Bar3.PlayInAnims();
            }
            else
            {
                // m_Bar3 moves out
                m_Bar3.PlayOutAnims();
            }
        }

        // Toggle m_Bar4
        void ToggleBar4()
        {
            m_Bar4_IsOn = !m_Bar4_IsOn;
            if (m_Bar4_IsOn == true)
            {
                // m_Bar4 moves in
                m_Bar4.PlayInAnims();
            }
            else
            {
                // m_Bar4 moves out
                m_Bar4.PlayOutAnims();
            }
        }

        #endregion // Toggle Button
    }

}