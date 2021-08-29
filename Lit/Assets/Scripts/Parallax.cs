using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private bool infiniteX;
    [SerializeField] private bool infiniteY;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitsSizeX;
    private float textureUnitsSizeY;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitsSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitsSizeY = texture.height / sprite.pixelsPerUnit;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;

        if (infiniteX)
        {
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitsSizeX)
            {
                float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitsSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }
        if (infiniteY)
        {
            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitsSizeY)
            {
                float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitsSizeY;
                transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPositionY);
            }
        }
    }
}
