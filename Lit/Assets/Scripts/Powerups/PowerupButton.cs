using UnityEngine;
using UnityEngine.UI;

public class PowerupButton : MonoBehaviour
{
    private Button button;
    Racer racer;
    Opponent opponent;
    public PowerupBehaviour powerupBehaviour { get; set; }
    private Image image;
    [SerializeField] private JustRotate justRotate;
    [SerializeField] private JustRotate justRotateSmall;

    private float rotateSpeedTemp;

    bool isSelected;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
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
            justRotateSmall.rotateSpeed = 0f;
        }
        else
        {
            justRotate.rotateSpeed = rotateSpeedTemp;
            justRotateSmall.rotateSpeed = rotateSpeedTemp;
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
    public void UsePowerup(bool calledFromScript)
    {
        if (powerupBehaviour == null) { return; }
        if (racer == null)
            racer = transform.root.gameObject.GetComponent<Racer>();
        if (racer.powerup.isSelectable && isSelected && !calledFromScript)
        {
            racer.powerup.isSelected = false;
            isSelected = false;
        }
        else if (racer.powerup.isSelectable && !isSelected && !calledFromScript)
        {
            racer.powerup.isSelected = true;
            isSelected = true;
        }
        else if (!racer.powerup.isSelectable || calledFromScript)
        {
            powerupBehaviour.ActivatePowerup();
            image.sprite = null;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            racer.powerup.isSelected = false;
            isSelected = false;
            racer.powerup = null;
            powerupBehaviour = null;
        }
    }
}
