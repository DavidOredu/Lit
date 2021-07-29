using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class LevelWindow : MonoBehaviour
{
    private LevelData levelData;
    private PlayerData playerData;

    //temporary variable for debugging purposes
    [SerializeField] private int amount;

    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI rankNameText;
    [SerializeField] private Slider topSpeedBar;
    [SerializeField] private Slider accelerationBar;
    [SerializeField] private Slider strengthBar;
    [SerializeField] private Slider litAbilityBar;

    [SerializeField] private List<Button> attributeButtons = new List<Button>();
    [SerializeField] private Button levelUpButton;
    [SerializeField] private Button addTopSpeedXPButton;
    [SerializeField] private Button addAccelerationXPButton;
    [SerializeField] private Button addStrengthXPButton;
    [SerializeField] private Button addLitAbilityXPButton;

    [SerializeField] private TextMeshProUGUI topSpeedXPToAddText;
    [SerializeField] private TextMeshProUGUI accelerationXPToAddText;
    [SerializeField] private TextMeshProUGUI strengthXPToAddText;
    [SerializeField] private TextMeshProUGUI litAbilityXPToAddText;

    [SerializeField] private TextMeshProUGUI topSpeedValue;
    [SerializeField] private TextMeshProUGUI accelerationValue;
    [SerializeField] private TextMeshProUGUI strengthValue;
    [SerializeField] private TextMeshProUGUI litAbilityValue;

    private LevelSystem levelSystem;
    private LevelSystemAnimated levelSystemAnimated;

    private void Awake()
    {
        levelData = Resources.Load<LevelData>("LitLevelData");
        playerData = Resources.Load<PlayerData>("PlayerData");
    }
    private void Start()
    {
        SetButtonValues();
        SetPlayerStatsText();
        experienceText.text = levelData.XP.ToString();
    }
    private void SetExperienceBarSize(float topSpeedNormalized, float accelerationNormalized, float strengthNormalized, float litAbilityNormalized)
    {
        topSpeedBar.value = topSpeedNormalized;
        accelerationBar.value = accelerationNormalized;
        strengthBar.value = strengthNormalized;
        litAbilityBar.value = litAbilityNormalized;
    }
    private void FixedUpdate()
    {
        CheckAttributes();
    }
    private void SetLevelNumber(int levelNumber)
    {
        levelText.text = (levelNumber).ToString();
        rankNameText.text = RankNames.Rank[levelNumber - 1];
        amount = levelSystemAnimated.amountOfBuyingXP;
    }
    public void AddExperienceInLevelData(int amount)
    {
        levelSystem.AddExperienceInLevelData(amount);
    }
    public void AddXP(int i, int amount)
    {
        if (levelData.XP >= amount)
        {
            levelSystem.AddExperience(levelSystem.playerAttributes[i], amount);
            levelData.XP -= amount;
        }
    }
    public void LevelUp()
    {
        levelSystem.LevelUp();
        levelSystemAnimated.LevelUp();
    }
    public void SetLevelSystem(LevelSystem levelSystem)
    {
        //set the levelsystemAnimated object
        this.levelSystem = levelSystem;
    }
    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated)
    {
        //set the levelsystemAnimated object
        this.levelSystemAnimated = levelSystemAnimated;

        //update the starting values
        SetLevelNumber(levelSystemAnimated.GetLevelNumber());
        SetExperienceBarSize(levelSystemAnimated.GetAttributeNormalized(levelSystemAnimated.playerAttributesExperiences[0]), levelSystemAnimated.GetAttributeNormalized(levelSystemAnimated.playerAttributesExperiences[1]), levelSystemAnimated.GetAttributeNormalized(levelSystemAnimated.playerAttributesExperiences[2]), levelSystemAnimated.GetAttributeNormalized(levelSystemAnimated.playerAttributesExperiences[3]));
       

        //subscribe to the changed events
        levelSystemAnimated.OnExperienceChanged += LevelSystemAnimated_OnExperienceChanged;
        levelSystemAnimated.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
    }
    private void SetPlayerStatsText()
    {
        topSpeedValue.text = playerData.topSpeed.ToString();
        accelerationValue.text = (playerData.topSpeed / playerData.timeZeroToMax).ToString();
        //TODO: Set strength value text 
        //TODO: Set LitAbility value text
    }
    private void CheckAttributes()
    {
        // if an attribute is full, set the activity of its corresponding button to false and display the "max" text.
        for (int i = 0; i < levelSystem.playerAttributes.Count; i++)
        {
            if (levelSystem.playerAttributes[i].isFull)
            {
                attributeButtons[i].gameObject.SetActive(false);
                // TODO: display the max text
            }
            else
            {
                attributeButtons[i].gameObject.SetActive(true);
            }
        }
        if (levelSystem.playerAttributes[0].isFull && levelSystem.playerAttributes[1].isFull && levelSystem.playerAttributes[2].isFull && levelSystem.playerAttributes[3].isFull && !levelSystem.IsMaxLevel() && !levelSystemAnimated.isAnimating)
        {
            levelUpButton.gameObject.SetActive(true);
        }
        else
        {
            levelUpButton.gameObject.SetActive(false);
        }

    }
    private void SetButtonValues()
    {
        levelUpButton.onClick.AddListener(() => LevelUp());
        addTopSpeedXPButton.onClick.AddListener(() => AddXP(0, amount /* This value differs from time to time depending on the current level and experience of the player. TODO: Create a function or variable in "level system" to pass in the amount of XP to be used for upgrade.*/));
        addAccelerationXPButton.onClick.AddListener(() => AddXP(1, amount /* This value differs from time to time depending on the current level and experience of the player. TODO: Create a function or variable in "level system" to pass in the amount of XP to be used for upgrade.*/));
        addStrengthXPButton.onClick.AddListener(() => AddXP(2, amount /* This value differs from time to time depending on the current level and experience of the player. TODO: Create a function or variable in "level system" to pass in the amount of XP to be used for upgrade.*/));
        addLitAbilityXPButton.onClick.AddListener(() => AddXP(3, amount /* This value differs from time to time depending on the current level and experience of the player. TODO: Create a function or variable in "level system" to pass in the amount of XP to be used for upgrade.*/));

       
        
    }

    private void LevelSystemAnimated_OnLevelChanged(object sender, System.EventArgs e)
    {
        //level changed update text
        SetLevelNumber(levelSystemAnimated.GetLevelNumber());
        SetExperienceBarSize(levelSystemAnimated.GetAttributeNormalized(levelSystemAnimated.playerAttributesExperiences[0]), levelSystemAnimated.GetAttributeNormalized(levelSystemAnimated.playerAttributesExperiences[1]), levelSystemAnimated.GetAttributeNormalized(levelSystemAnimated.playerAttributesExperiences[2]), levelSystemAnimated.GetAttributeNormalized(levelSystemAnimated.playerAttributesExperiences[3]));
        SetPlayerStatsText();
    }

    private void LevelSystemAnimated_OnExperienceChanged(object sender, System.EventArgs e)
    {
        //experience changed update text
        SetExperienceBarSize(levelSystemAnimated.GetAttributeNormalized(levelSystemAnimated.playerAttributesExperiences[0]), levelSystemAnimated.GetAttributeNormalized(levelSystemAnimated.playerAttributesExperiences[1]), levelSystemAnimated.GetAttributeNormalized(levelSystemAnimated.playerAttributesExperiences[2]), levelSystemAnimated.GetAttributeNormalized(levelSystemAnimated.playerAttributesExperiences[3]));
        
        experienceText.text = levelData.XP.ToString();
        SetPlayerStatsText();
    }
}
