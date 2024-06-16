using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction movement;
    [SerializeField] InputAction fire;

    [Header("General Setup Settings")]
    [Tooltip("How fast ship moves up and down based upon player input")]
    [SerializeField] float controlSpeed = 10f;
    [Tooltip("How fast player moves horizontally")][SerializeField] float xRange = 10f;
    [Tooltip("How fast player moves vertically")][SerializeField] float yRange = 7f;

    [Header("Screen position based tuning")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionYawFactor = 2f;

    [Header("Player input based tuning")]
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float controlRollFactor = -20f;

    [Header("Laser gun array")]
    [Tooltip("Add all player lasers here")]
    [SerializeField] GameObject[] lasers;



    float xThrow, yThrow;

    //this is called the first thing before awake
    void OnEnable()
    {
        movement.Enable();
        fire.Enable();
    }

    //this is the last thing being called before destroy
    void OnDisable()
    {
        movement.Disable();
        fire.Disable();
    }


    void Start()
    {
        
    }

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    private void ProcessTranslation()
    {
        // new system for movement
        xThrow = movement.ReadValue<Vector2>().x;
        yThrow = movement.ReadValue<Vector2>().y;

        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float newXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(newXPos, -xRange, xRange);


        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float newYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(newYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;

        // x
        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        
        // y
        float yaw = transform.localPosition.x * positionYawFactor;

        // z
        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessFiring()
    {
        if (fire.ReadValue<float>() > 0.5f)
        {
            SetLasersActive(true);
            Debug.Log("Not Fire");
        }
        else
        {
            SetLasersActive(false);
            Debug.Log("Not Fire");
        }
    }

    void SetLasersActive(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }


}
