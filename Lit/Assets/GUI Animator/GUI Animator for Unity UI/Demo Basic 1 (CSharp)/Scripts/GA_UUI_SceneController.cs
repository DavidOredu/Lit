// GUI Animator for Unity UI
// Version: 1.2.0
// Compatilble: Unity 2017.1.0f3 or higher, more info in Readme.txt file.
//
// Developer:				Gold Experience Team (https://assetstore.unity.com/publishers/4162)
// Unity Asset Store:		https://assetstore.unity.com/packages/tools/gui/gui-animator-for-unity-ui-28709
//
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using GUIAnimator;

#endregion // Namespaces

namespace GUIAnimatorDemo
{

    // ######################################################################
    // GA_UUI_SceneController class
    // - Play animations by scripts for workflows demostration
    // This script is attached with SceneController in the scene
    // ######################################################################

    public class GA_UUI_SceneController : MonoBehaviour
    {

        public GAui m_Title = null;
        public GAui m_Image = null;
        public GAui m_Button = null;

        // Awake is called when the script instance is being loaded.
        void Awake()
        {
            // Disable auto-animation and let this script controls all GAui elements in the scene.
            if (enabled)
            {
                // Disable AutoAnimation.
                GSui.Instance.m_AutoAnimation = false;

                // Set Animation Speed to default speed.
                GSui.Instance.m_GUISpeed = 1.0f;
            }
        }

        // Use this for initialization
        void Start()
        {
            // Play In animations
            GSui.Instance.PlayInAnims(m_Title.transform, true);
            GSui.Instance.PlayInAnims(m_Image.transform, true);
            GSui.Instance.PlayInAnims(m_Button.transform, true);

            //// Play Out-Animations
            //GSui.Instance.PlayOutAnims(m_Title.transform, true);
            //GSui.Instance.PlayOutAnims(m_Image.transform, true);
            //GSui.Instance.PlayOutAnims(m_Button.transform, true);
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Event handlers
        public void Title_PlayInAnims_Started()
        {
            Debug.Log("Title_PlayInAnims_Started");
        }

        // Event handlers
        public void Title_PlayInAnims_Finished()
        {
            Debug.Log("Title_PlayInAnims_Finished");
        }
    }

}