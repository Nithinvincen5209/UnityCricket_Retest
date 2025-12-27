using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [Range(0, 1)]
    public float bounceRetention = 0.25f;
    private Rigidbody rb;
    public float swingPower; 
    private bool hasBounced = false;
    private TrailRenderer trail;
    private float postpitchDirection;

   
    public void Launch(Vector3 target, float swingAmount,float afterpitchMove)
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        if(trail != null)
        {
            trail.Clear();
        }
        swingPower = swingAmount;
        postpitchDirection = afterpitchMove;

        float travelTime = 0.4f; // Time in seconds to reach the pitch

        // Calculate Horizontal Distance
        Vector3 distance = target - transform.position;
        Vector3 distanceXZ = new Vector3(distance.x, 0, distance.z);

        // Calculate Horizontal Velocity (Speed to reach the point)
        float vx = distanceXZ.magnitude / travelTime;

        // Calculate Vertical Velocity (To fight gravity)
        float vy = (distance.y / travelTime) + (0.5f * Mathf.Abs(Physics.gravity.y) * travelTime);

        // Combine and Apply Velocity
        Vector3 velocityXZ = distanceXZ.normalized * vx;

        // Use rb.velocity.
        rb.linearVelocity = new Vector3(velocityXZ.x, vy, velocityXZ.z);
    }

    void FixedUpdate()
    {
        // Apply swing force sideways while ball is in the air
        if (!hasBounced && Mathf.Abs(swingPower) > 0.01f)
        {
           // This pushes the ball left or right based on your swingAmount
            rb.AddForce(Vector3.right * swingPower , ForceMode.Acceleration);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Pitch"))
        {
            hasBounced = true;
            Vector3 currentvel = rb.linearVelocity;
            currentvel.x *= bounceRetention;
            rb.linearVelocity = currentvel;
            Debug.Log("Ball  Pitches Ball stop");
        }
    }
}