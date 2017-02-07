using UnityEngine;

public class ChargeShot : MonoBehaviour
{
    public GameObject chargeShotBulletPrefab;

    private float speed = 0;
    private Vector3 direction;

    private GameObject player;

    private GameObject charge;
    private ParticleSystem chargeParticle;

    private bool doubleClickFlag = false;
    private float doubleClickThreshold = 0.5f;
    private float doubleClickTimer = 0;

    private bool charging = false;
    private float chargePower = 0;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Start()
    {
        // Set the Player object.
        player = GameObject.FindGameObjectWithTag("Player");

        // Set the ChargeParticle object.
        charge = GameObject.Find("ChargeParticle");
        chargeParticle = charge.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Detect double click.
        if (doubleClickFlag)
        {
            doubleClickTimer += Time.deltaTime;
            if (doubleClickTimer > doubleClickThreshold)
            {
                doubleClickTimer = 0;
                doubleClickFlag = false;
            }
            else
            {
                if (Input.GetButtonUp("Fire1"))
                {
                    Fire();
                }
                else if (Input.GetButton("Fire1"))
                {
                    Charging();
                }
            }
        }
        else
        {
            if (Input.GetButtonUp("Fire1"))
            {
                Ready();
            }
        }

        // Update charging particle
        charge.transform.position = player.transform.position;
        if (charging)
        {
            if (chargeParticle.isStopped)
            {
                chargeParticle.Play();
            }
        }
        else
        {
            if (chargeParticle.isPlaying)
            {
                chargeParticle.Stop();
            }
        }
    }

    /// <summary>
    /// Prepare for charging.
    /// </summary>
    private void Ready()
    {
        doubleClickFlag = true;

        //Debug.Log("Ready.");
    }

    /// <summary>
    /// Start charging.
    /// </summary>
    private void Charging()
    {
        doubleClickTimer = 0;

        chargePower += Time.deltaTime;
        charging = true;

        //Debug.Log("Charging...");
    }

    /// <summary>
    /// End charging.
    /// </summary>
    private void Fire()
    {
        doubleClickTimer = 0;
        doubleClickFlag = false;

        if (chargeShotBulletPrefab != null)
        {
            var chargeShotBullet = Instantiate(chargeShotBulletPrefab) as GameObject;
            chargeShotBullet.transform.position = player.transform.position;

            var chargeShotBulletScript = chargeShotBullet.GetComponent<ChargeShotBullet>();
            if (chargeShotBulletScript != null)
            {
                chargeShotBulletScript.SetSpeed(speed);
                chargeShotBulletScript.SetDirection(direction);
                chargeShotBulletScript.SetPower(chargePower);
            }
        }

        chargePower = 0;
        charging = false;

        //Debug.Log("Fire!");
    }

    /// <summary>
    /// Set the speed.
    /// </summary>
    /// <param name="speed">The speed.</param>
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    /// <summary>
    /// Set the direction.
    /// </summary>
    /// <param name="direction">The direction.</param>
    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    /// <summary>
    /// Use this checker to disable jumping, or disable shooting, or decrease movement speed, or anything.
    /// </summary>
    /// <returns>Now charging or not</returns>
    public bool IsCharging()
    {
        return this.charging;
    }
}