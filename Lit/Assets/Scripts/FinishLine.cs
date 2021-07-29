using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private float lineDistance;
    public static bool hasCrossedLine = false;

    public LayerMask whatIsRunner;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private bool hasCrossed()
    {
        return Physics2D.Raycast(transform.position, new Vector2(transform.position.x, transform.position.y + lineDistance), whatIsRunner);
    }
    // Update is called once per frame
    void Update()
    {
        if (hasCrossed())
            hasCrossedLine = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + lineDistance));
    }
}
