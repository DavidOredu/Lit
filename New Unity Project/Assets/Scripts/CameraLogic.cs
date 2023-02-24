using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    float cameraMovementOffset = 0.15f;
    float cameraZOffset = 5f;

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();

        if (Input.GetKey(KeyCode.Space))
        {
            CenterCamera();
        }
    }
    void UpdateCameraPosition()
    {
        var mousePos = Input.mousePosition;

        if(mousePos.x >= Screen.width)
        {
            // move camera to the right
            transform.position = new Vector3(transform.position.x + cameraMovementOffset, transform.position.y, transform.position.z);
        }
        else if(mousePos.x <= 0)
        {
            // move camera to the left
            transform.position = new Vector3(transform.position.x - cameraMovementOffset, transform.position.y, transform.position.z);
        }

        if (mousePos.y >= Screen.height)
        {
            // move camera up
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + cameraMovementOffset);
        }
        else if (mousePos.y <= 0)
        {
            // move camera down
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - cameraMovementOffset);
        }
    }
    void CenterCamera()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z - cameraZOffset);
    }
}
