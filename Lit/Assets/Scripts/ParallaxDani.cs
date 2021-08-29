/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxDani : MonoBehaviour
{
    private float length, startpos;
    private Transform cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxDani : MonoBehaviour
{
    private float lengthX, lengthY, startposX, startposY;
    private Transform cam;
    public Vector2 parallaxEffect;
    public bool infiniteX;
    public bool infiniteY;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        startposX = transform.position.x;
        startposY = transform.position.y;
        lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
        lengthY = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float tempX = (cam.transform.position.x * (1 - parallaxEffect.x));
        float tempY = (cam.transform.position.y * (1 - parallaxEffect.y));
        float distX = (cam.transform.position.x * parallaxEffect.x);
        float distY = (cam.transform.position.y * parallaxEffect.y);
        transform.position = new Vector3(startposX + distX, startposY + distY, transform.position.z);
        if (infiniteX)
        {
            if (tempX > startposX + lengthX) startposX += lengthX;
            else if (tempX < startposX - lengthX) startposX -= lengthX;
        }
        if (infiniteY)
        {
            if (tempY > startposY + lengthY) startposY += lengthY;
            else if (tempY < startposY - lengthY) startposY -= lengthY;
        }
    }
}

