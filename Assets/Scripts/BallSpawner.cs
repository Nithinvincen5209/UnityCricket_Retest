using UnityEngine;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform ballPoint;
    public Transform pitchPointer;

    [Header("UI Slider Settings")]
    public Slider accuracySlider;
    public float sliderSpeed = 2f;

    [Header(" SIDE SWITCHING")]
    public GameObject bowlerPrefab;
    public Transform rightPos;
    public Transform leftPos;
    private bool onrightSide = true;

    private bool isMeterMoving = false;
    private bool isLocked = false; 
    private float selectedSwingDirection = 0;
    private float lockedSwingIntensity = 0f; 

    void Update()
    {
        // Move the slider ONLY if it's moving and NOT yet locked
        if (isMeterMoving && !isLocked)
        {
            accuracySlider.value = Mathf.PingPong(Time.time * sliderSpeed, 1f);
        }

        //  "Enter" key now ONLY locks the slider and calculates intensity
        if (Input.GetKeyDown(KeyCode.Return) && isMeterMoving && !isLocked)
        {
            LockSlider();
        }
    }

    public void SetSwingLeft()
    {
        selectedSwingDirection = -1.2f;
        ResetMeter();
    }

    public void SetSwingRight()
    {
        selectedSwingDirection = 1.2f;
        ResetMeter();
    }

    private void ResetMeter()
    {
        isMeterMoving = true;
        isLocked = false; // Unlock so it can move again
        lockedSwingIntensity = 0f;
        Debug.Log("Meter Started. Press ENTER to Lock accuracy.");
    }

    private void LockSlider()
    {
        isLocked = true; // This stops the PingPong in Update
        float val = accuracySlider.value;

        // Determine Intensity based on zones
        if (val >= 0.8f)
        {
            lockedSwingIntensity = selectedSwingDirection * 4f;
            Debug.Log("LOCKED: High Swing. Now click the Ball Button!");
        }
        else if (val <= 0.2f)
        {
            lockedSwingIntensity = selectedSwingDirection * 2.5f;
            Debug.Log("LOCKED: Low Swing. Now click the Ball Button!");
        }
        else
        {
            lockedSwingIntensity = 0f;
            Debug.Log("LOCKED: Straight. Now click the Ball Button!");
        }
    }

    
    public void SpawnBall()
    {
        // Only spawn if the user has already locked the slider with Enter
        if (isLocked)
        {
            GameObject newBall = Instantiate(ballPrefab, ballPoint.position, ballPoint.rotation);
            BallPhysics physics = newBall.GetComponent<BallPhysics>();

            float seamMovement = -selectedSwingDirection * 3f;
            if (physics != null)
            {
                physics.Launch(pitchPointer.position, lockedSwingIntensity, seamMovement);
            }

            Destroy(newBall, 2f);
            // Reset states for the next delivery
            isMeterMoving = false;
            isLocked = false;
        }
        else
        {
            Debug.Log("Cannot spawn! You must press ENTER to lock accuracy first.");
        }
    }
    public  void SwitchSide()
    {
        onrightSide = !onrightSide;
        if(onrightSide)
        {
            bowlerPrefab.transform.position = rightPos.transform.position;
            bowlerPrefab.transform.rotation = rightPos.transform.rotation;
        }
        else
        {
            bowlerPrefab.transform.position = leftPos.transform.position;
            bowlerPrefab.transform.rotation = leftPos.transform.rotation;
        }
    }
}