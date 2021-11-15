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
    // Demo07 class
    // - Animates all GAui elements in the scene.
    // - Responds to user mouse click or tap on buttons.
    // This class is attached with "-SceneController-" object in "GA UUI - Demo07 (960x600px)" scene.
    // ######################################################################

    public class Demo07 : MonoBehaviour
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
        public GAui m_Dialog;
        public GAui m_DialogButtons;

        // GAui objects of buttons
        public GAui m_Button1;
        public GAui m_Button2;
        public GAui m_Button3;
        public GAui m_Button4;

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

            // Play In-Animations of all dialogs and buttons
            StartCoroutine(MoveInPrimaryButtons());
        }

        // Play In-Animations of all dialogs and buttons
        IEnumerator MoveInPrimaryButtons()
        {
            yield return new WaitForSeconds(1.0f);

            // Play In-Animations of all dialogs
            m_Dialog.PlayInAnims();
            m_DialogButtons.PlayInAnims();

            // Play In-Animations of all buttons
            m_Button1.PlayInAnims();
            m_Button2.PlayInAnims();
            m_Button3.PlayInAnims();
            m_Button4.PlayInAnims();

            // Enable all scene switch buttons
            StartCoroutine(EnableAllDemoButtons());
        }

        // Play Out-Animations of all dialogs and buttons
        public void HideAllGUIs()
        {
            // Play Out-Animations of all dialogs
            m_Dialog.PlayOutAnims();
            m_DialogButtons.PlayOutAnims();

            // Play Out-Animations of all buttons
            m_Button1.PlayOutAnims();
            m_Button2.PlayOutAnims();
            m_Button3.PlayOutAnims();
            m_Button4.PlayOutAnims();

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
            // Play Out-Animations of m_Button1
            MoveButtonsOut();

            // Disable m_Button1, m_Button2, m_Button3, m_Button4 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Button1.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Button2.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Button3.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Button4.gameObject, 2.0f));

            // Set next move in of m_Button1 to new position
            StartCoroutine(SetButtonMove(ePosMove.UpperScreenEdge, ePosMove.UpperScreenEdge));
        }

        public void OnButton_2()
        {
            // Play Out-Animations of m_Button2
            MoveButtonsOut();

            // Disable m_Button1, m_Button2, m_Button3, m_Button4 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Button1.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Button2.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Button3.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Button4.gameObject, 2.0f));

            // Set next move in of m_Button2 to new position
            StartCoroutine(SetButtonMove(ePosMove.LeftScreenEdge, ePosMove.LeftScreenEdge));
        }

        public void OnButton_3()
        {
            // Play Out-Animations of m_Button3
            MoveButtonsOut();

            // Disable m_Button1, m_Button2, m_Button3, m_Button4 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Button1.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Button2.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Button3.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Button4.gameObject, 2.0f));

            // Set next move in of m_Button3 to new position
            StartCoroutine(SetButtonMove(ePosMove.RightScreenEdge, ePosMove.RightScreenEdge));
        }

        public void OnButton_4()
        {
            // Play Out-Animations of m_Button4
            MoveButtonsOut();

            // Disable m_Button1, m_Button2, m_Button3, m_Button4 for a short time
            StartCoroutine(DisableButtonForSeconds(m_Button1.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Button2.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Button3.gameObject, 2.0f));
            StartCoroutine(DisableButtonForSeconds(m_Button4.gameObject, 2.0f));

            // Set next move in of m_Button3 to new position
            StartCoroutine(SetButtonMove(ePosMove.BottomScreenEdge, ePosMove.BottomScreenEdge));
        }

        public void OnDialogButton()
        {
            // Play Out-Animations of m_Dialog
            m_Dialog.PlayOutAnims();
            m_DialogButtons.PlayOutAnims();

            // Disable m_DialogButtons for a short time
            StartCoroutine(DisableButtonForSeconds(m_DialogButtons.gameObject, 2.0f));

            // Moves m_Dialog back to screen
            StartCoroutine(DialogPlayInAnimations());
        }

        #endregion // UI Responder

        // ########################################
        // Move Dialog/Button functions
        // ########################################

        #region Move Dialog/Button

        // Play Out-Animations of all buttons
        void MoveButtonsOut()
        {
            m_Button1.PlayOutAnims();
            m_Button2.PlayOutAnims();
            m_Button3.PlayOutAnims();
            m_Button4.PlayOutAnims();
        }

        // Set next move in of all buttons to new position
        IEnumerator SetButtonMove(ePosMove PosMoveIn, ePosMove PosMoveOut)
        {
            yield return new WaitForSeconds(2.0f);

            // Set next MoveIn position of m_Button1 to PosMoveIn
            m_Button1.m_MoveIn.MoveFrom = PosMoveIn;
            // Reset m_Button1
            m_Button1.Reset();
            // Play In-Animations of m_Button1
            m_Button1.PlayInAnims();

            // Set next MoveIn position of m_Button2 to PosMoveIn
            m_Button2.m_MoveIn.MoveFrom = PosMoveIn;
            // Reset m_Button2
            m_Button2.Reset();
            // Play In-Animations of m_Button2
            m_Button2.PlayInAnims();

            // Set next MoveIn position of m_Button3 to PosMoveIn
            m_Button3.m_MoveIn.MoveFrom = PosMoveIn;
            // Reset m_Button3
            m_Button3.Reset();
            // Play In-Animations of m_Button3
            m_Button3.PlayInAnims();

            // Set next MoveIn position of m_Button4 to PosMoveIn
            m_Button4.m_MoveIn.MoveFrom = PosMoveIn;
            // Reset m_Button4
            m_Button4.Reset();
            // Play In-Animations of m_Button4
            m_Button4.PlayInAnims();
        }

        // Moves m_Dialog back to screen
        IEnumerator DialogPlayInAnimations()
        {
            yield return new WaitForSeconds(1.5f);

            m_Dialog.PlayInAnims();
            m_DialogButtons.PlayInAnims();
        }

        #endregion // Move Dialog/Button
    }

}