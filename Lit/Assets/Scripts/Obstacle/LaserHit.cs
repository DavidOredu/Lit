using System;
using UnityEngine;
using System.Collections.Generic;

public class LaserHit : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform firePoint;
    public Vector3 extensionSpeed;
    public GameObject startVFX;
    public Vector3 startVFXOffset;
    public GameObject endVFX;
    public LayerMask whatToHit;
    public BeamProjectileScript laserBeam;
    public Transform laserLimit;

    bool hasHitRacer = false;

    private List<ParticleSystem> particles = new List<ParticleSystem>();
    // Start is called before the first frame update
    void Start()
    {
        FillLists();
        endVFX.SetActive(false);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, lineRenderer.GetPosition(0));
    }

    private void FillLists()
    {
        for (int i = 0; i < startVFX.transform.childCount; i++)
        {
            var ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
                particles.Add(ps);
        }
        for (int i = 0; i < endVFX.transform.childCount; i++)
        {
            var ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
                particles.Add(ps);
        }
    }

    // Update is called once per frame
    void Update()
    {
       // if (Input.GetButtonDown("Fire1"))
       // {
       //     EnableLaser();
       //}
     //   if (Input.GetButton("Fire1"))
        {
            UpdateLaser();
        }
        //if (Input.GetButtonUp("Fire1"))
        //{
        //    DisableLaser();
        //}
    }
    void EnableLaser()
    {
        lineRenderer.enabled = true;

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Play();
        }
    }
    void UpdateLaser()
    {
        lineRenderer.SetPosition(0, firePoint.position);
        startVFX.transform.position = firePoint.position - startVFXOffset;
       
        Vector2 direction = Vector2.down * -(lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, direction.magnitude, whatToHit);
        
        Debug.DrawRay(transform.position, direction, Color.green);
        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
            endVFX.transform.position = lineRenderer.GetPosition(1);
            endVFX.SetActive(true);

            if(hit.collider.CompareTag("Player") || hit.collider.CompareTag("Opponent"))
            {
                if (!hit.collider.GetComponent<Racer>().isAffectedByLaser)
                {
                  //  laserBeam.OnCollisionEnter2D(hit);
                }
            }
        }
        else
        {
            endVFX.SetActive(false);
            lineRenderer.SetPosition(1, lineRenderer.GetPosition(1) + extensionSpeed);
        }
    }
    void DisableLaser()
    {
        lineRenderer.enabled = false;

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y));
    }
}
