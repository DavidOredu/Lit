using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBox : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerData playerData;

    [SerializeField] private List<ColorButtonNetwork> colorButtons = new List<ColorButtonNetwork>();

    private bool isEnlarged = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckButtons();
    }
    public void HandleBoxSize()
    {
        if (!isEnlarged)
        {
            anim.SetTrigger("enlarge");
            isEnlarged = true;
        }
        else
        {
            anim.SetTrigger("shrink");
            isEnlarged = false;
        }
    }
    public void ChangeColor(int color)
    {
        playerData.colorCode = color;
    }
    private void CheckButtons()
    {
        for (int i = 0; i < colorButtons.Count; i++)
        {
            if (colorButtons[i].colorCode == playerData.colorCode)
            {
                colorButtons[i].IsPicked = true;
            }
            else
            {
                colorButtons[i].IsPicked = false;
            }
        }
    }
}
