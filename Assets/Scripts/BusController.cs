using UnityEngine;
using System.Collections.Generic;
using System;

public class BusController : MonoBehaviour
{
    public enum ControlMode
    {
        Keyboard,
        Buttons
    };

    public enum Axel
    {
        Front,
        Rear
    }


    [Serializable]
    public struct wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public ControlMode control;

    public float maxAcceleration = 15.0f;
    public float brakeAcceleration = 20.0f;

    public float turnSensitivity = 1f;
    public float maxTurnAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<wheel> wheels;

    public BusLights busLights;
    public AudioSource reverseBeepSound;

    float moveInput;
    float turnInput;

    private bool isBraking = false;
    private int gearDirection = 1; // 1 = forward, -1 = reverse

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = _centerOfMass;
    }

    void Update()
    {
        GetInputs();
        AnimationWheels();
    }

    void LateUpdate()
    {
        Move();
        Turn();
        Brake();
        HandleLightsAndReverseSound();
    }
    public void MoveInput(float input)
    {
        moveInput = input * gearDirection;
    }

    public void TurnInput(float input)
    {
        turnInput = input;
    }

    public void SetBrake(bool state)
    {
        isBraking = state;
    }

    public void ToggleGear()
    {
        gearDirection *= -1;
    }

    void GetInputs()
    {
        if (control == ControlMode.Keyboard)
        {
            moveInput = Input.GetAxis("Vertical");
            turnInput = Input.GetAxis("Horizontal");
        }
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 20 * maxAcceleration * Time.deltaTime;
        }
    }

    void Turn()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _turnAngle = turnInput * maxTurnAngle * turnSensitivity;

                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _turnAngle, Time.deltaTime * turnSensitivity);
            }
        }
    }

    void Brake()
    {
        if (control == ControlMode.Keyboard)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 20f * brakeAcceleration * Time.deltaTime;
                }
            }
            else
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 0;
                }
            }
        }
        else if (control == ControlMode.Buttons)
        {
            if (isBraking)
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 20f * brakeAcceleration * Time.deltaTime;
                }
            }
            else
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 0;
                }
            }
        }
    }

    void AnimationWheels()
    {
        foreach (var wheel in wheels)
        {
            Vector3 position;
            Quaternion rotation;
            wheel.wheelCollider.GetWorldPose(out position, out rotation);
            wheel.wheelModel.transform.position = position;
            wheel.wheelModel.transform.rotation = rotation;
        }
    }

    void HandleLightsAndReverseSound()
    {
        if (control == ControlMode.Keyboard)
        {
            bool isBraking = Input.GetKey(KeyCode.Space);
            bool isReversing = moveInput < -0.1f;

            busLights.SetBrakeLights(isBraking);
            busLights.SetReverseLights(isReversing);

            if (isReversing)
            {
                if (!reverseBeepSound.isPlaying)
                    reverseBeepSound.Play();
            }
            else
            {
                if (reverseBeepSound.isPlaying)
                    reverseBeepSound.Stop();
            }
        }
        else if (control == ControlMode.Buttons)
        {
            bool braking = isBraking;
            bool reversing = moveInput < -0.1f;

            busLights.SetBrakeLights(braking);
            busLights.SetReverseLights(reversing);

            if (reversing)
            {
                if (!reverseBeepSound.isPlaying)
                    reverseBeepSound.Play();
            }
            else
            {
                if (reverseBeepSound.isPlaying)
                    reverseBeepSound.Stop();
            }
        }

    }

}

