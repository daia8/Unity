using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public float speed = 10f;
    public float travelDistance = 30f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * speed;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, startPos) >= travelDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Lander lander = other.GetComponent<Lander>();
        if (lander != null && !lander.IsImmuneToBullets)
        {
            lander.TriggerOnLanded(new Lander.OnLandedEventArgs
            {
                landingType = Lander.LandingType.TurretHit,
                dotVector = 0,
                landingSpeed = 999,
                scoreMultiplier = 0,
                score = 0
            });

            lander.SendMessage("SetState", Lander.State.GameOver);

            Destroy(gameObject);
        }
    }
}
