using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class UIManager : SingletonDontDestroy<UIManager>
{
    // Nested class that governs the properties of a UI screen
    [Serializable]
    public class UIScreen
    {
        // Types of screens
        public enum Screen
        {
            None,
            MainMenu,
            SelectPlay,
            Profile,
            GameMode_SinglePlayer,
            GameMode_MultiPlayer,
            Map_1_LevelSelect,
            Map_2_LevelSelect,
            Map_3_LevelSelect,
            Map_4_LevelSelect,
            Map_5_LevelSelect,
            Map_6_LevelSelect,
            Map_7_LevelSelect,
            GameUI,
            Lobby,
            Settings,
        }
        // Current screen type
        public Screen tag;
        // The gameobject associated with this screen
        public GameObject UIPrefab;
        // The screen the current screen goes back to
        public Screen previousUiTag;
        // The int type of this screen
        public int currentUiType;
        // All the buttons within this screen
        public List<Button> buttonsOfUiScreen = new List<Button>();
    }
    [Serializable]
    // Nested class that governs the properties of a UI popup
    public class UIPopUp
    {
        // Types of popups
        public enum PopUp
        {
            PlayerNameInput,
            ExitGame,
            ResetGame,
        }

        // The gameobject associated with this screen
        public GameObject popUpPrefab;
        // The current popup type
        public PopUp tag;
        //The int type of this popup
        public int currentPopUpType;
        // The animator of the popup... mainly responsible for closing and opening popups
        public Animator popUpAnimation;
        // All the buttons within this popup
        public List<Button> buttonsOfUiPopUp = new List<Button>();
    }

    // All the UI Screens or windows contained in the game can be found in this list, except for textboxes and pop-ups, that is
    public List<UIScreen> Uis = new List<UIScreen>();

    public List<UIPopUp> popUps = new List<UIPopUp>();


    // Is the game paused? Is it not?
    public static bool GameIsPaused = false;

    // The current UI screen and UI popup at any point in the game. 
    private UIScreen currentUiScreen;
    private UIPopUp currentUiPopUp;

    //Reference to level loader
    private LevelLoader levelLoader;


    // FUNCTION: Used to pause to game using a button or key. Espace key for PC and back button for android
    public void OnPauseInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    // FUNCTION: Takes you back to the previous UI screen depending on the UI screen currently active. Key or button version of back function
    public void OnBackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            foreach (var uiScreen in Uis)
            {
                if (currentUiScreen.previousUiTag == UIScreen.Screen.None)
                {
                    if (GetUiPopUp(UIPopUp.PopUp.ExitGame).popUpPrefab.transform.Find("Content").gameObject.activeSelf)
                    {
                        ClosePopUp();
                        return;
                    }
                    else
                    {

                        UpdatePopUp(2);
                        return;
                    }
                }
                if (uiScreen.tag == currentUiScreen.previousUiTag)
                {
                    if (uiScreen.tag == UIScreen.Screen.None) { continue; }
                    uiScreen.UIPrefab.SetActive(true);
                    currentUiScreen.UIPrefab.SetActive(false);
                    currentUiScreen = uiScreen;
                    return;
                }
                else
                {
                    continue;
                }
            }
        }
    }
    // FUNCTION: Start function. Unity stuff... ;)
    private void Start()
    {
        // Get the level loader
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();

        // Close any other ui elements and start the "starting" game ui
        CloseAllPopUps();
        SetStartingUI();
    }

    // FUNCTION: This sets the main UI at the start of the game
    private void SetStartingUI()
    {

        foreach (UIScreen uIScreen in Uis)
        {
            if (uIScreen.currentUiType == 1)
            {
                uIScreen.UIPrefab.SetActive(true);
                currentUiScreen = uIScreen;
            }
            else
            {
                uIScreen.UIPrefab.SetActive(false);
            }
        }
    }
    // FUNCTION: Pauses the game
    public void Pause()
    {
        if (!GameIsPaused)
        {
            Time.timeScale = 0;
            GameIsPaused = true;
        }
    }

    // DEBUG FUNCTION: This was used in the previous project. Not used at all here but here just in case of reference :(
    public void MenuPlay(int index)
    {
        levelLoader.LoadLevel(index);
        if (GameIsPaused)
        {
            Time.timeScale = 1f;
            GameIsPaused = false;
        }
    }
    // FUNCTION: Restarts a current level
    public void Restart()
    {
        levelLoader.Reload();
        if (GameIsPaused)
        {
            Time.timeScale = 1f;
            GameIsPaused = false;
        }
    }
    // FUNTION: Resumes the game if its paused
    public void Resume()
    {
        if (GameIsPaused)
        {
            Time.timeScale = 1f;
            GameIsPaused = false;
        }
    }
    // FUNCTION: Simply exits the application
    public void ExitApp()
    {
        Application.Quit();
        Debug.Log("Has quit Application!");
    }
    // FUNCTION: Sets the buttons of a current UI inactive. This is useful when a window is displayed so that tapping a button in the current window won't go somewhere else until the is disabled
    public void SetInactiveButton(int uiType)
    {
        if (uiType == 1)
        {
            foreach (Button button in currentUiScreen.buttonsOfUiScreen)
            {
                button.interactable = false;
            }
        }
        else if (uiType == 2)
        {
            foreach (Button button in currentUiPopUp.buttonsOfUiPopUp)
            {
                button.interactable = false;
            }
        }
        else
        {
            return;
        }
    }

    // FUNCTION: Sets the inactive buttons of the current UI back to active. To be used after using the funtion above
    public void SetActiveButton(int uiType)
    {
        if (uiType == 1)
        {
            foreach (Button button in currentUiScreen.buttonsOfUiScreen)
            {
                button.interactable = true;
            }
        }
        else if (uiType == 2)
        {
            foreach (Button button in currentUiPopUp.buttonsOfUiPopUp)
            {
                button.interactable = true;
            }
        }
        else
        {
            return;
        }


    }

    // FUNCTION: Takes you back to the previous UI screen depending on the UI screen currently active. 
    public void Back()
    {
        foreach (var uiScreen in Uis)
        {
            if (currentUiScreen.previousUiTag == UIScreen.Screen.None)
            {
                if (GetUiPopUp(UIPopUp.PopUp.ExitGame).popUpPrefab.transform.Find("Content").gameObject.activeSelf)
                {
                    ClosePopUp();
                    return;
                }
                else
                {

                    UpdatePopUp(2);
                    return;
                }
            }
            if (uiScreen.tag == currentUiScreen.previousUiTag)
            {
                if (uiScreen.tag == UIScreen.Screen.None) { continue; }

                uiScreen.UIPrefab.SetActive(true);
                currentUiScreen.UIPrefab.SetActive(false);
                currentUiScreen = uiScreen;
                return;
            }
            else
            {
                continue;
            }
        }
    }

    // FUNCTION: Reponsible for switching on or off UI prefab for the page you are currently in. Sets all other active UIs to not display except for the current one
    public void UpdateUI(int uiType)
    {
        if (uiType == 0) { return; }

        foreach (UIScreen ui in Uis)
        {
            if (ui.currentUiType != uiType)
            {
                ui.UIPrefab.SetActive(false);
            }
            else
            {
                ui.UIPrefab.SetActive(true);
                currentUiScreen = ui;
            }
        }
    }
    // Responsible for switching on or off different popups during the game. Set the popups active using the popup's int type
    public void UpdatePopUp(int uiType)
    {
        if (uiType == 0) { return; }

        foreach (UIPopUp popUp in popUps)
        {
            if (popUp.currentPopUpType != uiType)
            {
                popUp.popUpPrefab.SetActive(false);
            }
            else
            {

                popUp.popUpPrefab.SetActive(true);
                popUp.popUpAnimation.SetTrigger("open");
                currentUiPopUp = popUp;
            }
        }
    }
    // Closes the currently active popup
    public void ClosePopUp()
    {
        currentUiPopUp.popUpAnimation.SetTrigger("close");
        currentUiPopUp = null;
    }
    // Closes all popups in the game
    void CloseAllPopUps()
    {
        foreach (var popUp in popUps)
        {
            if (popUp.popUpPrefab.transform.Find("Content"))
                popUp.popUpPrefab.transform.Find("Content").gameObject.SetActive(false);

            popUp.popUpPrefab.SetActive(false);
        }
    }
    // Gets a specific popup using its enum tag
    UIPopUp GetUiPopUp(UIPopUp.PopUp popUpTag)
    {
        foreach (var popUp in popUps)
        {
            if (popUp.tag == popUpTag)
            {
                return popUp;
            }
            else
            {
                continue;
            }
        }
        return null;
    }
}
