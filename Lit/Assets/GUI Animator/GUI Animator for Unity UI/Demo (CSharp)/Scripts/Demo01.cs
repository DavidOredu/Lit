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
    // Demo01 class
    // - Animates all GAui elements in the scene.
    // - Responds to user mouse click or tap on buttons.
    // This class is attached with "-SceneController-" object in "GA UUI - Demo01 (960x600px)" scene.
    // Note this script is simply use of GUI Animator which lets you figure how it works, you will find using of coroutine. For real
    // ######################################################################

    public class Demo01 : MonoBehaviour
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

        // GAui objects of TopLeft buttons
        public GAui m_TopLeft_A;
        public GAui m_TopLeft_B;

        // GAui objects of BottomLeft buttons
        public GAui m_BottomLeft_A;
        public GAui m_BottomLeft_B;

        // GAui objects of RightBar buttons
        public GAui m_RightBar_A;
        public GAui m_RightBar_B;
        public GAui m_RightBar_C;

        // Toggle state of TopLeft, BottomLeft and BottomLeft buttons
        bool m_TopLeft_IsOn = false;
        bool m_BottomLeft_IsOn = false;
        bool m_RightBar_IsOn = false;

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
            m_TopLeft_A.PlayInAnims(eGUIMove.Self);
            m_BottomLeft_A.PlayInAnims(eGUIMove.Self);
            m_RightBar_A.PlayInAnims(eGUIMove.Self);

            // Enable all scene switch buttons
            StartCoroutine(EnableAllDemoButtons());
        }

        // Play Out-Animations of all primary buttons
        public void HideAllGUIs()
        {
            m_TopLeft_A.PlayOutAnims();
            m_BottomLeft_A.PlayOutAnims();
            m_RightBar_A.PlayOutAnims(eGUIMove.Self);

            if (m_TopLeft_IsOn == true)
                m_TopLeft_B.PlayOutAnims();
            if (m_BottomLeft_IsOn == true)
                m_BottomLeft_B.PlayOutAnims();
            if (m_RightBar_IsOn == true)
                m_RightBar_B.PlayOutAnims();

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

        public void OnButton_TopLeft()
        {
            // Disable m_TopLeft_A, m_RightBar_A, m_RightBar_C, m_BottomLeft_A for a short time
            StartCoroutine(DisableButtonForSeconds(m_TopLeft_A.gameObject, 0.3f));
            StartCoroutine(DisableButtonForSeconds(m_RightBar_A.gameObject, 0.6f));
            StartCoroutine(DisableButtonForSeconds(m_RightBar_C.gameObject, 0.6f));
            StartCoroutine(DisableButtonForSeconds(m_BottomLeft_A.gameObject, 0.3f));

            // Toggle m_TopLeft
            ToggleTopLeft();

            // Toggle other buttons
            if (m_BottomLeft_IsOn == true)
            {
                ToggleBottomLeft();
            }
            if (m_RightBar_IsOn == true)
            {
                ToggleRightBar();
            }
        }

        public void OnButton_BottomLeft()
        {
            // Disable m_TopLeft_A, m_RightBar_A, m_RightBar_C, m_BottomLeft_A for a short time
            StartCoroutine(DisableButtonForSeconds(m_TopLeft_A.gameObject, 0.3f));
            StartCoroutine(DisableButtonForSeconds(m_RightBar_A.gameObject, 0.6f));
            StartCoroutine(DisableButtonForSeconds(m_RightBar_C.gameObject, 0.6f));
            StartCoroutine(DisableButtonForSeconds(m_BottomLeft_A.gameObject, 0.3f));

            // Toggle m_BottomLeft
            ToggleBottomLeft();

            // Toggle other buttons
            if (m_TopLeft_IsOn == true)
            {
                ToggleTopLeft();
            }
            if (m_RightBar_IsOn == true)
            {
                ToggleRightBar();
            }

        }

        public void OnButton_RightBar()
        {
            // Disable m_TopLeft_A, m_RightBar_A, m_RightBar_C, m_BottomLeft_A for a short time
            StartCoroutine(DisableButtonForSeconds(m_TopLeft_A.gameObject, 0.3f));
            StartCoroutine(DisableButtonForSeconds(m_RightBar_A.gameObject, 0.6f));
            StartCoroutine(DisableButtonForSeconds(m_RightBar_C.gameObject, 0.6f));
            StartCoroutine(DisableButtonForSeconds(m_BottomLeft_A.gameObject, 0.3f));

            // Toggle m_RightBar
            ToggleRightBar();

            // Toggle other buttons
            if (m_TopLeft_IsOn == true)
            {
                ToggleTopLeft();
            }
            if (m_BottomLeft_IsOn == true)
            {
                ToggleBottomLeft();
            }

        }

        #endregion // UI Responder

        // ########################################
        // Toggle button functions
        // ########################################

        #region Toggle Button

        // Toggle TopLeft buttons
        void ToggleTopLeft()
        {
            m_TopLeft_IsOn = !m_TopLeft_IsOn;
            if (m_TopLeft_IsOn == true)
            {
                // m_TopLeft_B moves in
                m_TopLeft_B.PlayInAnims();
            }
            else
            {
                // m_TopLeft_B moves out
                m_TopLeft_B.PlayOutAnims();
            }
        }

        // Toggle BottomLeft buttons
        void ToggleBottomLeft()
        {
            m_BottomLeft_IsOn = !m_BottomLeft_IsOn;
            if (m_BottomLeft_IsOn == true)
            {
                // m_BottomLeft_B moves in
                m_BottomLeft_B.PlayInAnims();
            }
            else
            {
                // m_BottomLeft_B moves out
                m_BottomLeft_B.PlayOutAnims();
            }
        }

        // Toggle RightBar buttons
        void ToggleRightBar()
        {
            m_RightBar_IsOn = !m_RightBar_IsOn;
            if (m_RightBar_IsOn == true)
            {
                // m_RightBar_A moves out
                m_RightBar_A.PlayOutAnims();
                // m_RightBar_B moves in
                m_RightBar_B.PlayInAnims();
            }
            else
            {
                // m_RightBar_A moves in
                m_RightBar_A.PlayInAnims();
                // m_RightBar_B moves out
                m_RightBar_B.PlayOutAnims();
            }
        }

        #endregion // Toggle Button
    }
}