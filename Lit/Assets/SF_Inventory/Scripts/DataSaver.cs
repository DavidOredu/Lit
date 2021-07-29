using UnityEngine;
//players cant modify it itself

/// <summary>
///  used for saving scores/currency and the used PlayerShip
///  handles pay requests for currencies
/// </summary>
public class DataSaver : MonoBehaviour {

    public static DataSaver saver;
	private void Awake () {

        if (saver == null)
        {
            DontDestroyOnLoad(gameObject);
            saver = this;
            
        }
        else if(saver != this) 
        {
            Destroy(gameObject);
        }
	}
    

    public void setHighScore(int newScore)
    {
        int score = PlayerPrefs.GetInt(PlayerPrefsReferences.highscore, 0);
        if (newScore> score)
        {
            PlayerPrefs.SetInt(PlayerPrefsReferences.highscore, newScore);
        }
    }

    #region Coin-Methods
    public int getCoins()
    {
        return PlayerPrefs.GetInt(PlayerPrefsReferences.coins, 0);
    }
    public void addCoins(int win)
    {
        int coins = PlayerPrefs.GetInt(PlayerPrefsReferences.coins, 0);
        coins += win;
        PlayerPrefs.SetInt(PlayerPrefsReferences.coins, coins);
    }
    public bool payGold(int price)
    {
        int coins = PlayerPrefs.GetInt(PlayerPrefsReferences.coins, 0);
        if (coins < price)
        {
            Debug.Log("You dont have enougy gold for that item!");
            return false;
        }
        coins -= price;
        PlayerPrefs.SetInt(PlayerPrefsReferences.coins, coins);
        return true;
    }
    /// <summary>
    ///  used for getting currency value from saved cloud progress
    /// </summary>
    public void setCoins(int cloudValue)
    {
        PlayerPrefs.SetInt(PlayerPrefsReferences.coins, cloudValue);
    }
    #endregion Coin-Methods

    #region Premium
    public int getPremiumCurrency()
    {
        return PlayerPrefs.GetInt(PlayerPrefsReferences.premiumCurrency, 0);
    }
    public void addPremiumCurrency(int addValue)
    {
        int darkMatter = PlayerPrefs.GetInt(PlayerPrefsReferences.premiumCurrency, 0);
        darkMatter += addValue;
        PlayerPrefs.SetInt(PlayerPrefsReferences.premiumCurrency, darkMatter);
    }

    /// <summary>
    ///  used for premium currency pay requests
    /// </summary>
    public bool payPremiumCurrency(int price)
    {
        int darkMatter = PlayerPrefs.GetInt(PlayerPrefsReferences.premiumCurrency, 0);
        if (darkMatter < price)
        {
            Debug.Log("You dont have enough dark Matter for that item!");
            return false;
        }
        darkMatter -= price;
        PlayerPrefs.SetInt(PlayerPrefsReferences.premiumCurrency, darkMatter);
        return true;
    }
    #endregion Premium 
}
