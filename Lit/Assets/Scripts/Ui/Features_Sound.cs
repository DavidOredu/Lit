using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Features_Sound : Features_Object<AudioSource>, IPointerEnterHandler, IPointerDownHandler
{
    private Button button;
    private AudioManager audioManager;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    protected override void UpdateUI()
    {
        if (audioManager == null)
            audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        if (button == null)
            button = gameObject.GetComponent<Button>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(button.interactable)
        audioManager.PlayOneShotSound("Hover");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(button.interactable)
        audioManager.PlayOneShotSound("Click");
    }



    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
