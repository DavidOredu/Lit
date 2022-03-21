using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New UI Features", menuName = " UI/New UI Features")]
public class UiFeatures : ScriptableObject
{
   // [HideInInspector]
    public bool enableDynamicUpdate = true;
    [Header("BACKGROUND")]
    public Animator bgAnimator;
    public Image bgImage;

    [Header("COLORS")]
    public Color panelsColor = new Color(255, 255, 255, 255);
    public Color lineColor = new Color(255, 255, 255, 255);
    public Color buttonsColor = new Color(255, 255, 255, 255);
    public Color particleColor = new Color(255, 255, 255, 255);
    public Color sliderButtonColor = new Color(255, 255, 255, 255);

    [Header("TEXT")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI gameText;
    public TextMeshProUGUI headerText;

    [Header("PARTICLES")]
    public ParticleSystem glowMoteParticle;

    [Header("SOUNDS")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    [Header("MAIN CAMERA")]
    public bool renderPostProcessing;
    public LayerMask volumeLayerMask;

    [Header("UI POPUP")]
    public Animator popUpAnimator;

    [Header("SCROLL_RECT")]
    public ScrollRect playerLobby;
    public ScrollRect barPanel;
    public ScrollRect levelSelect;

    [Header("GRID LAYOUT GROUP")]
    public int leftPadding;
    public int rightPadding;
    public int topPadding;
    public int bottomPadding;
    public Vector2 cellSize;
    public Vector2 spacing;
    public GridLayoutGroup.Corner startCorner;
    public GridLayoutGroup.Axis startAxis;
    public TextAnchor childAlignment;
    public GridLayoutGroup.Constraint constraint;

    [Header("CONTENT SIZE FITTER")]
    public ContentSizeFitter.FitMode horizontalFit;
    public ContentSizeFitter.FitMode verticalFit;

    [Header("CANVAS SCALER")]
    public CanvasScaler.ScaleMode scaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    public CanvasScaler.ScreenMatchMode screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
    public Vector2 referenceResolution = new Vector2(1920, 1080);
    [Range(0f, 1f)]
    public float matchWidthOrHeight = 1f;




    public enum textType
    {
        titleText,
        gameText,
        headerText
    }

    public enum panelPart
    { 
        PanelBorder,
        LineSegment,
        Button,
        Background
    }
    public enum SoundType
    {
        Music,
        SFX
    }
    public enum ScrollRectType
    {
        PlayerLobby,
        BarPanel,
        LevelSelect
    }
    public enum AnimatorType
    {
        BackgroundAnimator,
        UiPopUpAnimator
    }
}
