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
    // Demo04 class
    // - Animates all GAui elements in the scene.
    // - Responds to user mouse click or tap on buttons.
    // This class is attached with "-SceneController-" object in "GA UUI - Demo04 (960x600px)" scene.
    // ######################################################################

    public class Demo04 : MonoBehaviour
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

        // GAui object of dialogs
        public GAui m_Dialog1;
        public GAui m_Dialog2;
        public GAui m_Dialog3;
        public GAui m_Dialog4;

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
        }

        // Play In-Animations of dialogs
        IEnumerator MoveInPrimaryButtons()
        {
            yield return new WaitForSeconds(1.0f);

            // Play In-Animations of dialogs
            m_Dialog1.PlayInAnims();
            m_Dialog2.PlayInAnims();
            m_Dialog3.PlayInAnims();
            m_Dialog4.PlayInAnims();

            // Enable all scene switch buttons
            StartCoroutine(EnableAllDemoButtons());
        }

        public void HideAllGUIs()
        {
            // Play Out-Animations of dialogs
            m_Dialog1.PlayOutAnims();
            m_Dialog2.PlayOutAnims();
            m_Dialog3.PlayOutAnims();
            m_Dialog4.PlayOutAnims();

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

        // Disable a button for a short time
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

        public void OnButton_Dialog1()
        {
            // Play Out-Animations of m_Dialog1
            m_Dialog1.PlayOutAnims();

            // Disable m_Dialog1 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Dialog1.gameObject, 2.0f));

            // Moves m_Dialog1 back to screen
            StartCoroutine(Dialog1_PlayInAnimations());
        }

        public void OnButton_Dialog2()
        {
            // Play Out-Animations of m_Dialog2
            m_Dialog2.PlayOutAnims();

            // Disable m_Dialog2 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Dialog2.gameObject, 2.0f));

            // Moves m_Dialog2 back to screen
            StartCoroutine(Dialog2_PlayInAnimations());
        }

        public void OnButton_Dialog3()
        {
            // Play Out-Animations of m_Dialog3
            m_Dialog3.PlayOutAnims();

            // Disable m_Dialog3 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Dialog3.gameObject, 2.0f));

            // Moves m_Dialog3 back to screen
            StartCoroutine(Dialog3_PlayInAnimations());
        }

        public void OnButton_Dialog4()
        {
            // Play Out-Animations of m_Dialog4
            m_Dialog4.PlayOutAnims();

            // Disable m_Dialog4 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Dialog4.gameObject, 2.0f));

            // Moves m_Dialog4 back to screen
            StartCoroutine(Dialog4_PlayInAnimations());
        }

        public void OnButton_PlayOutAnimations_AllDialogs()
        {

            // Disable m_Dialog1, m_Dialog2, m_Dialog3, m_Dialog4 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Dialog1.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Dialog2.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Dialog3.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Dialog4.gameObject, 2.0f));

            if (m_Dialog1.m_MoveOut.Began == false && m_Dialog1.m_MoveOut.Done == false)
            {
                // Move m_Dialog1 out
                m_Dialog1.PlayOutAnims();
                // Move m_Dialog1 back to screen with Coroutines
                StartCoroutine(Dialog1_PlayInAnimations());
            }
            if (m_Dialog2.m_MoveOut.Began == false && m_Dialog2.m_MoveOut.Done == false)
            {
                // Move m_Dialog2 out
                m_Dialog2.PlayOutAnims();
                // Move m_Dialog2 back to screen with Coroutines
                StartCoroutine(Dialog2_PlayInAnimations());
            }
            if (m_Dialog3.m_MoveOut.Began == false && m_Dialog3.m_MoveOut.Done == false)
            {
                // Move m_Dialog3 out
                m_Dialog3.PlayOutAnims();
                // Move m_Dialog3 back to screen with Coroutines
                StartCoroutine(Dialog3_PlayInAnimations());
            }
            if (m_Dialog4.m_MoveOut.Began == false && m_Dialog4.m_MoveOut.Done == false)
            {
                // Move m_Dialog4 out
                m_Dialog4.PlayOutAnims();
                // Move m_Dialog4 back to screen with Coroutines
                StartCoroutine(Dialog4_PlayInAnimations());
            }

        }

        #endregion // UI Responder

        // ########################################
        // Move dialog functions
        // ########################################

        #region Move Dialog

        // Play In-Animations of m_Dialog1
        IEnumerator Dialog1_PlayInAnimations()
        {
            yield return new WaitForSeconds(1.5f);

            // Reset children of m_Dialog1
            m_Dialog1.ResetAllChildren();

            // Moves m_Dialog1 back to screen
            m_Dialog1.PlayInAnims();
        }

        // Play In-Animations of m_Dialog2
        IEnumerator Dialog2_PlayInAnimations()
        {
            yield return new WaitForSeconds(1.5f);

            // Reset children of m_Dialog2
            m_Dialog2.ResetAllChildren();

            // Moves m_Dialog1 back to screen
            m_Dialog2.PlayInAnims();
        }

        // Play In-Animations of m_Dialog3
        IEnumerator Dialog3_PlayInAnimations()
        {
            yield return new WaitForSeconds(1.5f);

            // Reset children of m_Dialog3
            m_Dialog3.ResetAllChildren();

            // Moves m_Dialog1 back to screen
            m_Dialog3.PlayInAnims();
        }

        // Play In-Animations of m_Dialog4
        IEnumerator Dialog4_PlayInAnimations()
        {
            yield return new WaitForSeconds(1.5f);

            // Reset children of m_Dialog4
            m_Dialog4.ResetAllChildren();

            // Moves m_Dialog1 back to screen to screen
            m_Dialog4.PlayInAnims();
        }

        #endregion // Move Dialog
    }

}