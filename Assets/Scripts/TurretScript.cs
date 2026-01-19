using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 1.5f;        // time before starting a burst
    public float rotationSpeed = 360f;   // degrees/sec
    public float fireOffset = 0.5f;

    public int burstCount = 3;           // bullets per burst
    public float burstInterval = 0.3f;   // time between bullets in a burst
    public float burstCooldown = 2f;     // cooldown after burst

    private float fireTimer = 0f;
    private float burstTimer = 0f;
    private int bulletsFiredInBurst = 0;
    private bool isBursting = false;

    private void Update()
    {
        AimAtLander();
        HandleShooting();
    }

    private void AimAtLander()
    {
        if (Lander.Instance == null) return;

        Vector3 dir = Lander.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float currentAngle = transform.eulerAngles.z;

        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }

    private void HandleShooting()
    {
        if (Lander.Instance == null) return;
        if (Lander.Instance.CurrentState != Lander.State.Normal) return;

        if (isBursting)
        {
            // Fire bullets in the burst
            burstTimer += Time.deltaTime;
            if (burstTimer >= burstInterval)
            {
                burstTimer = 0f;
                Shoot();
                bulletsFiredInBurst++;

                if (bulletsFiredInBurst >= burstCount)
                {
                    isBursting = false;
                    bulletsFiredInBurst = 0;
                    fireTimer = -burstCooldown; // cooldown before next burst
                }
            }
        }
        else
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                isBursting = true;
                burstTimer = 0f;
                fireTimer = 0f;
            }
        }
    }

    private void Shoot()
    {
        Vector3 spawnPos = transform.position + transform.right * fireOffset;
        Instantiate(bulletPrefab, spawnPos, transform.rotation);
    }
}
