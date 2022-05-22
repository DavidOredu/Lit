using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Trajectory))]
public class BombScript : MonoBehaviour, IDamageable
{
    Camera cam;
    public Racer ownerRacer;

    public Trajectory trajectory;
    public Rigidbody2D rb;
    public CircleCollider2D col;

    public bool isDragging = false;
    public bool hasFired = false;
    public bool isDiscarded = false;
    public bool canExplode = true;
    public bool canControl = true;

    public int damageType;
    public float throwForce;
    public Vector2 startPoint;
    public Vector2 endPoint;
    public Vector2 direction;
    private Vector2 force;
    public float distance;

    public float damagePercentage;
    public float damageRate;
    public float explosiveForce;
    public float explosiveRadius;
    public float upwardsModifier;
    public ForceMode2D forceMode;

    public Timer grenadeTimer;
    public float grenadeTime = 3f;
    public Vector3 startScreenCoordinates;

    public Vector3 Pos { get { return transform.position; } }


    // Start is called before the first frame update
    private void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        trajectory = GetComponent<Trajectory>();
    }
    private void Start()
    {
        grenadeTimer = new Timer(grenadeTime);
        grenadeTimer.SetTimer();

        DeactivateRb();
    }
    private void FixedUpdate()
    {
        if (hasFired)
        {
            if (!grenadeTimer.isTimeUp)
            {
                grenadeTimer.UpdateTimer();
            }
            else
            {
                Explode(true);
            }
        }
    }
    public void FollowPlayer(Transform transform)
    {
        if(!hasFired)
        this.transform.position = transform.position;
    }
    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canExplode)
        {
            if (other.CompareTag("Player") || other.CompareTag("Opponent"))
            {
                var racer = other.gameObject.GetComponent<Racer>();
                if (racer == ownerRacer)
                {

                }
                else
                {
                    Explode(true);
                }
            }
            else
            {
                if (other.GetComponent<IDamageable>() == null) { return; }

                Explode(false);
            }
        }
    }
    public void ActivateRb()
    {
        rb.isKinematic = false;
    }
    public void DeactivateRb()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
    }
    public void OnBombDragStart(Vector2 mousePosition)
    {
      //  if (canControl)
        {
            DeactivateRb();
            isDragging = true;
            //   startPoint = cam.ScreenToWorldPoint(mousePosition);
            startPoint = mousePosition;
            trajectory.ShowTrejectory();
        }
    }
    public void OnDrag(Vector2 mousePosition)
    {
      //  if (!hasFired || canControl)
        {
            //  endPoint = cam.ScreenToWorldPoint(mousePosition);
            Vector3 worldCoordinates = cam.ScreenToWorldPoint(startScreenCoordinates);
            worldCoordinates.z = 0f;
            startPoint = worldCoordinates;
            endPoint = mousePosition;
            distance = Vector2.Distance(startPoint, endPoint);
            direction = (startPoint - endPoint).normalized;
            force = direction * throwForce * distance;

            Debug.DrawLine(startPoint, endPoint, Color.green);
            trajectory.UpdateDots(Pos, force);
        }
    }
    public void OnBombDragEnd()
    {
        ActivateRb();
        isDragging = false;
        hasFired = true;
        Push(force);
        trajectory.HideTrejectory();
    }
    [ContextMenu("Explode")]
    public void Explode(bool explodeWithDamage)
    {
        if (!canExplode) { return; }
        var explosion = Resources.Load<GameObject>($"{damageType}/Explosion");
        var explosionInGame = Instantiate(explosion, transform.position, Quaternion.identity);
        var explosionComp = explosionInGame.GetComponent<ElementExplosionScript>();

        Utils.SetBombToExplosionVariables(explosionComp, this);

        explosionComp.runnerDamages.InitDamages();
        
       
        explosionComp.Explode(explodeWithDamage);
        canExplode = false;
    }
    public void EndBomb()
    {
        if (canControl)
        {
            if (ownerRacer.GamePlayer.enemyPowerup == null)
            {
                ownerRacer.GamePlayer.powerupButton.TurnSelectableState(false);
                ownerRacer.GamePlayer.powerupButton.UsePowerup(true);
            }
            else if (ownerRacer.GamePlayer.powerupButton == null)
            {
                ownerRacer.GamePlayer.enemyPowerup.UsePowerup();
            }
        }
    }
    public void Damage()
    {
        Explode(false);
    }
}
