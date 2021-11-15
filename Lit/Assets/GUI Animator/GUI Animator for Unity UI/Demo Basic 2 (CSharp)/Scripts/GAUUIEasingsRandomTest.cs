// GUI Animator for Unity UI
// Version: 1.2.0
// Compatilble: Unity 2017.1.0f3 or higher, more info in Readme.txt file.
//
// Developer:				Gold Experience Team (https://assetstore.unity.com/publishers/4162)
// Unity Asset Store:		https://assetstore.unity.com/packages/tools/gui/gui-animator-for-unity-ui-28709
//
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com.

#region Namespaces

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GUIAnimator;

#endregion // Namespaces

namespace GUIAnimatorDemo
{

    // ######################################################################
    // GAUUIEasingsRandomTest class
    // - Play animations by random easings.
    // - Random Time and delay.
    // This script is attached with "StarButton (Button)" game object.
    // ######################################################################

    public class GAUUIEasingsRandomTest : MonoBehaviour
    {

        // ########################################
        // Variables
        // ########################################

        #region Variables

        // GAui component
        GAui m_GAui = null;

        // Unity UI Text
        public Text m_TextTitle = null;
        public Text m_TextDetails = null;

        // If this is enable, Translation, Rotation, Scale and Fade will use the same easing.
        public bool m_UseSameEasing = true;

        // Translation, this always enable by default.
        [HideInInspector]
        public bool m_Translation = true;

        // Rotation, Scale and Fade
        public bool m_Rotation = true;
        public bool m_Scale = false;
        public bool m_Fade = false;
        public bool m_ShowTimeAndDelay = false;

        [Range(1.5f, 4)]
        public float m_MinRandomTime = 1.5f;
        [Range(0.25f, 4)]
        public float m_MinRandomDelay = 0.25f;

        // Strings for Text UI
        string m_Title = "";
        string m_Details = "";

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

                // Get GAui component
                m_GAui = this.gameObject.GetComponent<GAui>();
                if (m_GAui == null)
                {
                    // Set up new GAui component
                    m_GAui = this.gameObject.AddComponent<GAui>();

                    // Add PlayInAnimsStarted Eventhandler
                    m_GAui.PlayInAnimsStarted.AddListener(PlayInAnimsStarted);

                    // Add PlayInAnimsFinished Eventhandler
                    m_GAui.PlayInAnimsFinished.AddListener(PlayInAnimsFinished);
                }

            }
        }

        // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        // http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
        void Start()
        {
            // Play animation
            Play();
        }

        // Update is called once per frame
        void Update()
        {
            // Always enable m_Translation
            if (m_Translation == false && m_Rotation == false && m_Scale == false && m_Fade == false)
            {
                m_Translation = true;
            }
        }

        // Play Animations
        void Play()
        {
            // Random Time and Delay
            float Time = UnityEngine.Random.Range(m_MinRandomTime, m_MinRandomTime + 2);
            float Delay = UnityEngine.Random.Range(m_MinRandomDelay, m_MinRandomDelay + 0.5f);

            // Reset m_Title and m_Details strings
            m_Title = "";
            m_Details = "";

            // Random Ease type
            eEaseType EaseType = (eEaseType)UnityEngine.Random.Range(0, 22);

            // Translation checkbox is enable
            if (m_Translation == true)
            {
                // Get parent Canvas space
                float LeftEdge = 0, TopEdge = 0, RightEdge = 0, BottomEdge = 0;
                m_GAui.GetParentCanvasEdges(out LeftEdge, out TopEdge, out RightEdge, out BottomEdge);

                // Random Easing for Translation if m_UseSameEasing is disabled
                if (m_UseSameEasing == false)
                {
                    EaseType = (eEaseType)UnityEngine.Random.Range(0, 22);
                }

                // Update Text UI
                m_Title += "Translation:\r\n";
                m_Details += EaseType.ToString() + "\r\n";

                // Setup begin and end position
                RectTransform RT = (RectTransform)m_GAui.transform;
                Vector3 Begin = new Vector3(RT.localPosition.x, RT.localPosition.y, 0);
                float NewEndX = 0;
                Bounds TotalBounds = m_GAui.GetTotalBounds();
                if (RT.localPosition.x > 0)
                {
                    NewEndX = LeftEdge + (TotalBounds.size.x * 2);
                }
                else
                {
                    NewEndX = RightEdge - (TotalBounds.size.x * 2);
                }
                Vector3 End = new Vector3(
                    NewEndX,
                    UnityEngine.Random.Range(BottomEdge + TotalBounds.size.y * 2, TopEdge - TotalBounds.size.y * 2),
                    0);

                // Set MoveIn, we will call PlayInAnims() later
                m_GAui.SetInAnimsMove(true, ePosMove.RectTransformPosition, Begin, End, Time, Delay, EaseType);
            }
            else
            {
                // Update Text UI
                m_Title += "Translation:\r\n";
                m_Details += "-\r\n";

                // Disable MoveIn
                m_GAui.SetInAnimsMove(false);
            }

            // Rotation checkbox is enable
            if (m_Rotation == true)
            {
                // Random Easing for Translation if m_UseSameEasing is disabled
                if (m_UseSameEasing == false)
                {
                    EaseType = (eEaseType)UnityEngine.Random.Range(0, 22);
                }

                // Update m_Title and m_Details strings
                m_Title += "Rotation:\r\n";
                m_Details += EaseType.ToString() + "\r\n";

                // Setup begin and end rotation
                Vector3 Begin = new Vector3(0, 0, 0);
                Vector3 End = new Vector3(0, 0, 360);

                // Set RotateIn, we will call PlayInAnims() later
                m_GAui.SetInAnimsRotate(true, Begin, End, Time, Delay, EaseType);
            }
            else
            {
                // Update m_Title and m_Details strings
                m_Title += "Rotation:\r\n";
                m_Details += "-\r\n";

                // Disable RotateIn
                m_GAui.SetInAnimsRotate(false);
            }

            // Scale checkbox is enable
            if (m_Scale == true)
            {
                // Random Easing for Translation if m_UseSameEasing is disabled
                if (m_UseSameEasing == false)
                {
                    EaseType = (eEaseType)UnityEngine.Random.Range(0, 22);
                }

                // Update m_Title and m_Details strings
                m_Title += "Scale:\r\n";
                m_Details += EaseType.ToString() + "\r\n";

                // Setup begin and end scale
                Vector3 Begin = new Vector3(0, 0, 0);

                // Set RotateIn, we will call PlayInAnims() later
                m_GAui.SetInAnimsScale(true, Begin, Time, Delay, EaseType);
            }
            else
            {
                // Update m_Title and m_Details strings
                m_Title += "Scale:\r\n";
                m_Details += "-\r\n";

                // Disable ScaleIn
                m_GAui.SetInAnimsScale(false);
            }

            // Fade checkbox is enable
            if (m_Fade == true)
            {
                // Random Easing for Translation if m_UseSameEasing is disabled
                if (m_UseSameEasing == false)
                {
                    EaseType = (eEaseType)UnityEngine.Random.Range(0, 22);
                }

                // Update m_Title and m_Details strings
                m_Title += "Fade:\r\n";
                m_Details += EaseType.ToString() + "\r\n";

                // Set RotateIn, we will call PlayInAnims() later
                m_GAui.SetInAnimsFade(true, Time, Delay, EaseType);
            }
            else
            {
                // Update m_Title and m_Details strings
                m_Title += "Fade:\r\n";
                m_Details += "-\r\n";

                // Disable RotateIn
                m_GAui.SetInAnimsFade(false);
            }

            // ShowTimeAndDelay checkbox is enable
            if (m_ShowTimeAndDelay == true)
            {
                // Update m_Title and m_Details strings
                m_Title += "Time:\r\nDelay:\r\n";
                m_Details += string.Format("{0:0.0}", Time) + " secs" + "\r\n" + string.Format("{0:0.0}", Delay) + " secs" + "\r\n";
            }


            if (m_TextTitle != null && m_TextDetails != null)
            {
                // Update Text UI
                m_TextTitle.text = m_Title;
                m_TextDetails.text = m_Details;
            }

            // Play In-animations
            m_GAui.PlayInAnims(eGUIMove.Self);
        }

        // PlayInAnimsStarted event handler
        void PlayInAnimsStarted(GameObject sender)
        {
            // Show log on console tab
            Debug.Log(sender.name + ": PlayInAnims is started");
        }

        // PlayInAnimsFinished event handler
        void PlayInAnimsFinished(GameObject sender)
        {
            // Show log on console tab
            Debug.Log(sender.name + ": PlayInAnims is finished");

            // Play next random easings
            Play();
        }

        #endregion // MonoBehaviour
    }

}