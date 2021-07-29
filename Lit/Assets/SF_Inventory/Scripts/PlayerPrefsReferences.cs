using UnityEngine;


/**
 *  NEVER CHANGE THESE VALUES !!!!
 *  IF CHANGED THEN THE PROGRESS FROM PLAYERS WILL BE LOST!!!!!
 */
public class PlayerPrefsReferences : MonoBehaviour
{
    private static PlayerPrefsReferences references;
    private void Awake()
    {
        if (references == null)
        {
            DontDestroyOnLoad(gameObject);
            references = this;
        }
        else if (references != this)
        {
            Destroy(gameObject);
        }
    }
    
    public const string premiumCurrency = "darkmatter";
    public const string coins = "coins";
    public const string highscore = "highscore";

}