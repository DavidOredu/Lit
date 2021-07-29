using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OutfitManager : MonoBehaviour
{
    public SpriteRenderer bodyPart;
    public BodyPart bodyPartType;
    public List<Accessory> options = new List<Accessory>();
    // Start is called before the first frame update

    public int currentOption = 0;

    private AccessoryData accessoryData;
    private void Start()
    {
        accessoryData = Resources.Load<AccessoryData>("AccessoryData");
        switch (bodyPartType)
        {
            case BodyPart.Head:
                AddAccessory(accessoryData.headAccessories);
                break;
            case BodyPart.Body:
                AddAccessory(accessoryData.bodyCloth);
                break;
            case BodyPart.LeftArm:
                break;
            case BodyPart.RightArm:
                break;
            case BodyPart.LeftHand:
                break;
            case BodyPart.RightHand:
                break;
            case BodyPart.LeftThigh:
                break;
            case BodyPart.RightThigh:
                break;
            case BodyPart.LeftLeg:
                break;
            case BodyPart.RightLeg:
                break;
            case BodyPart.LeftFoot:
                break;
            case BodyPart.RightFoot:
                break;
            default:
                break;
        }
    }
    public void Next()
    {
        currentOption++;
        if (currentOption >= options.Count)
        {
            currentOption = 0;
        }

        bodyPart.sprite = options[currentOption].sprite;
    }

    public void Previous()
    {
        currentOption--;
        if (currentOption <= 0)
        {
            currentOption = options.Count - 1;
        }
        bodyPart.sprite = options[currentOption].sprite;
    }
    void AddAccessory(Accessory[] accessories)
    {
        foreach (var accessory in accessories)
        {
            if (accessory.isUnlocked)
                options.Add(accessory);
        }
    }
    public enum BodyPart
    {
        Head,
        Body,
        LeftArm,
        RightArm,
        LeftHand,
        RightHand,
        LeftThigh,
        RightThigh,
        LeftLeg,
        RightLeg,
        LeftFoot,
        RightFoot,
        // weapon object
    }
}
