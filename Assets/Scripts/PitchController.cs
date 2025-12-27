using UnityEngine;

public class PitchController : MonoBehaviour
{
    [Header("PitchSettings")]
    public float speed = 5f;
    public float minX = -1.5f;
    public float maxX = 1.5f;
    public float minZ = -10f;
    public float maxZ = 10f;
    
    void Update()
    {
        // Pitchcontroller  Movement.
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 movement = transform.position + new Vector3(moveX, 0, moveZ);
        //set clamp range.
        movement.x = Mathf.Clamp(movement.x, minX, maxX);
        movement.z = Mathf.Clamp(movement.z, minZ, maxZ);
        transform.position = movement;
    }
}
