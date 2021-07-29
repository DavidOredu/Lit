using UnityEngine;

[ExecuteInEditMode]
public class Features_Object<T> : MonoBehaviour
{
    public UiFeatures uiFeatures;
    
    
    protected T component;
    public GameSettings gameSettings;

    bool dynamicUpdateEnabled = false;
    // Start is called before the first frame update
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        if (uiFeatures == null)
        {
            try
            {
                uiFeatures = Resources.Load<UiFeatures>("LitUiFeatures");
            }

            catch
            {
                Debug.Log("No UI Manager found. Assign it manually, otherwise it won't work properly.");
            }
        }
        if(gameSettings == null)
        {
            try
            {
                gameSettings = Resources.Load<GameSettings>("LitGameSettings");
            }
            catch
            {
                Debug.Log("No Game Settings found. Assign it manually, otherwise it won't work properly.");
            }
        }
        if (component == null)
            component = this.gameObject.GetComponent<T>();


        if (uiFeatures.enableDynamicUpdate == false)
        {
            this.enabled = true;
            UpdateUI();
        }
    }
    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        if (uiFeatures != null)
        {
            if (uiFeatures.enableDynamicUpdate == true)
                dynamicUpdateEnabled = true;
            else
                dynamicUpdateEnabled = false;

            if (dynamicUpdateEnabled == true)
                UpdateUI();
        }
        
    }

    protected virtual void UpdateUI()
    {

        
    }

    
}
