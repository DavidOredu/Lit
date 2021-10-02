using DapperDino.Mirror.Tutorials.Lobby;
using UnityEngine;
using UnityEngine.UI;

public class PowerupButton : MonoBehaviour
{
    private Button button;
    GamePlayerLobby gamePlayer;
    public PowerupBehaviour powerupBehaviour { get; set; }
    private Image image;
    [SerializeField] private JustRotate justRotate;
    [SerializeField] private JustRotate justRotateSmall;

    private float rotateSpeedTemp;

    public bool isSelected { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        button = transform.Find("Button").GetComponent<Button>();
        image = transform.Find("Button").transform.Find("Image").GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        rotateSpeedTemp = justRotate.rotateSpeed;

        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
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
    public void UsePowerup(bool endPowerup)
    {
        if (powerupBehaviour == null) { return; }
        if (gamePlayer == null)
            gamePlayer = transform.root.gameObject.GetComponent<GamePlayerLobby>();
        if (!gamePlayer.racer.canUsePowerup) { return; }
    //    gamePlayer.racer.InputHandler.UsePowerupInput();
        if (gamePlayer.powerup.isSelectable && isSelected && !endPowerup)
        {
            gamePlayer.powerup.isSelected = false;
            isSelected = false;
            gamePlayer.powerup.SelectedEnd(gamePlayer.racer);
        }
        else if (gamePlayer.powerup.isSelectable && !isSelected && !endPowerup)
        {
            gamePlayer.powerup.isSelected = true;
            isSelected = true;
            gamePlayer.powerup.SelectedStart(gamePlayer.racer);
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
                powerupBehaviour = null;
            }
        }
    }
}
