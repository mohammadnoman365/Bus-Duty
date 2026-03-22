using UnityEngine;
using System;
using System.Collections.Generic;

public class BusSounds : MonoBehaviour
{

    public float minSpeed; // The minimum speed threshold where pitch changes begin
    public float maxSpeed; // The maximum speed threshold where pitch reaches its peak
    private float currentSpeed;

    private Rigidbody busRb;
    public AudioSource busAudio;
    public AudioSource hornAudio;


    public float minPitch; // The lowest audio pitch (when stationary or below minSpeed)
    public float maxPitch; // The highest audio pitch (when at or above maxSpeed)
    private float pitchFromBus;

    void Start()
    {
        busRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        EngineSound();
        HornSoundKeyboard();
    }

    void EngineSound()
    {
        currentSpeed = busRb.linearVelocity.magnitude;

        if (currentSpeed < minSpeed)
        {
            busAudio.pitch = minPitch;
        }
        else if (currentSpeed < maxSpeed)
        {
            // Simple percentage calculation
            float speedPercent = (currentSpeed - minSpeed) / (maxSpeed - minSpeed);
            busAudio.pitch = minPitch + (speedPercent * (maxPitch - minPitch));
        }
        else
        {
            busAudio.pitch = maxPitch;
        }
    }

    void HornSoundKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            hornAudio.Play();
        }
    }

    public void PlayHorn()
    {
        hornAudio.Play();
    }

}
