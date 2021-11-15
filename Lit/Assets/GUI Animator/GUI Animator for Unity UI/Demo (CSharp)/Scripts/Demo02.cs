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
    // Demo02 class
    // - Animates all GAui elements in the scene.
    // - Responds to user mouse click or tap on buttons.
    // This class is attached with "-SceneController-" object in "GA UUI - Demo02 (960x600px)" scene.
    // ######################################################################

    public class Demo02 : MonoBehaviour
    {

        // ########################################
        // Variables
        // ########################################

        #region Variables

        // Canvas
        public Canvas m_Canvas;

        // GAui objects of Title text
        public GAui m_Title1;
        public GAui m_Title2;

        // GAui objects of Top and bottom
        public GAui m_TopBar;
        public GAui m_BottomBar;

        // GAui object of Dialog
        public GAui m_Dialog;

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

            // Play In-Animations of m_Dialog
            StartCoroutine(ShowDialog());
        }

        // Play In-Animations of m_Dialog
        IEnumerator ShowDialog()
        {
            yield return new WaitForSeconds(1.0f);

            // Play In-Animations of m_Dialog
            m_Dialog.PlayInAnims();

            // Enable all scene switch buttons
            StartCoroutine(EnableAllDemoButtons());
        }

        // Play Out-Animations of m_Dialog
        public void HideAllGUIs()
        {
            // Play Out-Animations of m_Dialog
            m_Dialog.PlayOutAnims();

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

        public void OnButton_UpperEdge()
        {
            // Play Out-Animations of m_Dialog
            m_Dialog.PlayOutAnims();

            // Play In-Animations of m_Dialog from top
            StartCoroutine(DialogPlayInAnimations(ePosMove.UpperScreenEdge));
        }

        public void OnButton_LeftEdge()
        {
            // Play Out-Animations of m_Dialog
            m_Dialog.PlayOutAnims();

            // Play In-Animations of m_Dialog from left
            StartCoroutine(DialogPlayInAnimations(ePosMove.LeftScreenEdge));
        }

        public void OnButton_RightEdge()
        {
            // Play Out-Animations of m_Dialog
            m_Dialog.PlayOutAnims();

            // Disable all buttons for a short time
            StartCoroutine(DisableAllButtonsForSeconds(2.0f));

            // Play In-Animations of m_Dialog from right
            StartCoroutine(DialogPlayInAnimations(ePosMove.RightScreenEdge));
        }

        public void OnButton_BottomEdge()
        {
            // Play Out-Animations of m_Dialog
            m_Dialog.PlayOutAnims();

            // Disable all buttons for a short time
            StartCoroutine(DisableAllButtonsForSeconds(2.0f));

            // Play In-Animations of m_Dialog from bottom
            StartCoroutine(DialogPlayInAnimations(ePosMove.BottomScreenEdge));
        }

        public void OnButton_UpperLeft()
        {
            // Play Out-Animations of m_Dialog
            m_Dialog.PlayOutAnims();

            // Disable all buttons for a short time
            StartCoroutine(DisableAllButtonsForSeconds(2.0f));

            // Play In-Animations of m_Dialog from upper left
            StartCoroutine(DialogPlayInAnimations(ePosMove.UpperLeft));
        }

        public void OnButton_UpperRight()
        {
            // Play Out-Animations of m_Dialog
            m_Dialog.PlayOutAnims();

            // Disable all buttons for a short time
            StartCoroutine(DisableAllButtonsForSeconds(2.0f));

            // Play In-Animations of m_Dialog from upper right
            StartCoroutine(DialogPlayInAnimations(ePosMove.UpperRight));
        }

        public void OnButton_BottomLeft()
        {
            // Play Out-Animations of m_Dialog
            m_Dialog.PlayOutAnims();

            // Disable all buttons for a short time
            StartCoroutine(DisableAllButtonsForSeconds(2.0f));

            // Play In-Animations of m_Dialog from bottom left
            StartCoroutine(DialogPlayInAnimations(ePosMove.BottomLeft));
        }

        public void OnButton_BottomRight()
        {
            // Play Out-Animations of m_Dialog
            m_Dialog.PlayOutAnims();

            // Disable all buttons for a short time
            StartCoroutine(DisableAllButtonsForSeconds(2.0f));

            // Play In-Animations of m_Dialog from bottom right
            StartCoroutine(DialogPlayInAnimations(ePosMove.BottomRight));
        }

        public void OnButton_Center()
        {
            // Play Out-Animations of m_Dialog
            m_Dialog.PlayOutAnims();

            // Disable all buttons for a short time
            StartCoroutine(DisableAllButtonsForSeconds(2.0f));

            // Play In-Animations of m_Dialog from center of screen
            StartCoroutine(DialogPlayInAnimations(ePosMove.MiddleCenter));
        }

        #endregion // UI Responder

        // ########################################
        // Move dialog functions
        // ########################################

        #region Move Dialog

        // Play In-Animations of m_Dialog by position
        IEnumerator DialogPlayInAnimations(ePosMove PosMoveIn)
        {
            yield return new WaitForSeconds(1.5f);

            //Debug.Log("PosMoveIn="+PosMoveIn);
            switch (PosMoveIn)
            {
                // Set m_Dialog to move in from upper
                case ePosMove.UpperScreenEdge:
                    m_Dialog.m_MoveIn.MoveFrom = ePosMove.UpperScreenEdge;
                    m_Dialog.m_MoveOut.MoveTo = ePosMove.MiddleCenter;
                    break;
                // Set m_Dialog to move in from left
                case ePosMove.LeftScreenEdge:
                    m_Dialog.m_MoveIn.MoveFrom = ePosMove.LeftScreenEdge;
                    m_Dialog.m_MoveOut.MoveTo = ePosMove.MiddleCenter;
                    break;
                // Set m_Dialog to move in from right
                case ePosMove.RightScreenEdge:
                    m_Dialog.m_MoveIn.MoveFrom = ePosMove.RightScreenEdge;
                    m_Dialog.m_MoveOut.MoveTo = ePosMove.MiddleCenter;
                    break;
                // Set m_Dialog to move in from bottom
                case ePosMove.BottomScreenEdge:
                    m_Dialog.m_MoveIn.MoveFrom = ePosMove.BottomScreenEdge;
                    m_Dialog.m_MoveOut.MoveTo = ePosMove.MiddleCenter;
                    break;
                // Set m_Dialog to move in from upper left
                case ePosMove.UpperLeft:
                    m_Dialog.m_MoveIn.MoveFrom = ePosMove.UpperLeft;
                    m_Dialog.m_MoveOut.MoveTo = ePosMove.MiddleCenter;
                    break;
                // Set m_Dialog to move in from upper right
                case ePosMove.UpperRight:
                    m_Dialog.m_MoveIn.MoveFrom = ePosMove.UpperRight;
                    m_Dialog.m_MoveOut.MoveTo = ePosMove.MiddleCenter;
                    break;
                // Set m_Dialog to move in from bottom left
                case ePosMove.BottomLeft:
                    m_Dialog.m_MoveIn.MoveFrom = ePosMove.BottomLeft;
                    m_Dialog.m_MoveOut.MoveTo = ePosMove.MiddleCenter;
                    break;
                // Set m_Dialog to move in from bottom right
                case ePosMove.BottomRight:
                    m_Dialog.m_MoveIn.MoveFrom = ePosMove.BottomRight;
                    m_Dialog.m_MoveOut.MoveTo = ePosMove.MiddleCenter;
                    break;
                // Set m_Dialog to move in from center
                case ePosMove.MiddleCenter:
                default:
                    m_Dialog.m_MoveIn.MoveFrom = ePosMove.MiddleCenter;
                    m_Dialog.m_MoveOut.MoveTo = ePosMove.MiddleCenter;
                    break;
            }

            // Reset m_Dialog
            m_Dialog.Reset();

            // Play In-Animations of m_Dialog by position
            m_Dialog.PlayInAnims();
        }

        #endregion //  Move Dialog
    }
}