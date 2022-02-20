using DapperDino.Mirror.Tutorials.Lobby;
using UnityEngine;
using UnityEngine.UI;

public class PowerupButton : MonoBehaviour
{
    private Button button;
    GamePlayerLobby gamePlayer;
    public PowerupBehaviour powerupBehaviour { get; set; }
    public Image image;
    [SerializeField] private JustRotate justRotate;
    [SerializeField] private JustRotate justRotateSmall;

    private float rotateSpeedTemp;

    public bool isSelected { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        var buttonGO = transform.Find("PowerupButton");
        button = buttonGO.GetComponent<Button>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        rotateSpeedTemp = justRotate.rotateSpeed;

        isSelected = false;
        //   button.onClick.AddListener(() => UsePowerup(false));
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePlayer == null)
            gamePlayer = transform.root.gameObject.GetComponent<GamePlayerLobby>();
        button.interactable = !(powerupBehaviour == null);
        if (button.interactable)
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        if (!button.interactable)
        {
            justRotate.rotateSpeed = 0f;
        }
        else
        {
            justRotate.rotateSpeed = rotateSpeedTemp;
        }
        if (isSelected)
        {
            // TODO: display feedback to show that its selected
        }
        else
        {
            // remove display
        }
    }
    [ContextMenu("UsePowerup")]
    public void UsePowerup(bool endPowerup)
    {
        Debug.Log("UsePowerupCalled");
        if (powerupBehaviour == null) { return; }
        
        if (!gamePlayer.racer.canUsePowerup) { return; }

        gamePlayer.racer.InputHandler.UsePowerupInput();
        if (gamePlayer.powerup.isSelectable&&!endPowerup)
        {
            TurnSelectableState(!isSelected);
        }
        else if (!gamePlayer.powerup.isSelectable || endPowerup)
        {
            powerupBehaviour.ActivatePowerup();
            powerupBehaviour.powerupAmmo--;

            if (powerupBehaviour.powerupAmmo <= 0)
            {
                image.sprite = null;
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                gamePlayer.powerup.isSelected = false;
                isSelected = false;
                gamePlayer.powerup = null;
                Destroy(powerupBehaviour.gameObject);
                powerupBehaviour = null;
            }
        }

    }
    public void TurnSelectableState(bool selected)
    {
        if(gamePlayer.powerup == null) { return; }

        if (gamePlayer.powerup.isSelectable && isSelected)
        {
            gamePlayer.powerup.isSelected = selected;
            isSelected = selected;
            gamePlayer.powerup.SelectedEnd(gamePlayer.racer);
        }
        else if (gamePlayer.powerup.isSelectable && !isSelected)
        {
            gamePlayer.powerup.isSelected = selected;
            isSelected = selected;
            gamePlayer.powerup.SelectedStart(gamePlayer.racer);
        }
    }
}
