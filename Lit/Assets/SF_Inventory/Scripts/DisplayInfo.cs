using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInfo : MonoBehaviour {

    [Header("Not mandatory")]
    public TextMeshProUGUI highScore;
    [Header("Mandatory values")]
    public TextMeshProUGUI coins;
    public TextMeshProUGUI darkMatter;
    
    // Use this for initialization

    public static DisplayInfo Instance;

    private void Start () {
        Instance = this;
        UpdateInfo();
    }
    /// <summary>
    ///  updates the info in the main menu
    /// </summary>
    public void UpdateInfo()
    {
        if (highScore != null)
        {
            highScore.text = "" + PlayerPrefs.GetInt(PlayerPrefsReferences.highscore, 0); 
        }
        coins.text = "" + PlayerPrefs.GetInt(PlayerPrefsReferences.coins, 0); 
        darkMatter.text = "" + PlayerPrefs.GetInt(PlayerPrefsReferences.premiumCurrency, 0);
    }
	
}
