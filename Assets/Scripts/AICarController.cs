using UnityEngine;

public class AICarController : MonoBehaviour
{
    public Transform[] waypoints;
    public float waypointReachDistance = 5f;

    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;

    public float maxMotorTorque = 150f;
    public float maxSteeringAngle = 60f;
    public float maxSpeed = 40f;
    public float brakeForce = 5000f;

    public float sensorLength = 10f;
    public float sensorSideOffset = 0.8f;
    public float sensorForwardOffset = 1.5f;
    public float sensorUpwardOffset = 1f;
    public LayerMask obstacleMask;

    private int currentWaypoint;
    private Rigidbody rb;
    private float currentSteer;
    private float currentTorque;
    private bool obstacleAhead;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    private void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        HandleSensors();
        HandleSteering();
        HandleMotor();
        UpdateWheelVisuals();
        CheckWaypoint();
    }

    void HandleSensors()
    {
        obstacleAhead = false;

        Vector3 origin = transform.position + transform.up * sensorUpwardOffset + transform.forward * sensorForwardOffset;

        Vector3[] offsets =
        {
            Vector3.zero,
            transform.right * sensorSideOffset,
            -transform.right * sensorSideOffset,
            transform.right * sensorSideOffset * 0.5f,
            -transform.right * sensorSideOffset * 0.5f
        };

        foreach (Vector3 offset in offsets)
        {
            Vector3 sensorStart = origin + offset;
            if (Physics.Raycast(sensorStart, transform.forward, sensorLength, obstacleMask))
            {
                obstacleAhead = true;
                Debug.DrawRay(sensorStart, transform.forward * sensorLength, Color.red);
                break;
            }

            Debug.DrawRay(sensorStart, transform.forward * sensorLength, Color.green);
            
        }
    }

    //void HandleSteering()
    //{
    //    Vector3 target = waypoints[currentWaypoint].position;
    //    Vector3 localTarget = transform.InverseTransformPoint(target);

    //    float steerPercent = localTarget.x / localTarget.magnitude;
    //    currentSteer = steerPercent * maxSteeringAngle;

    //    frontLeft.steerAngle = currentSteer;
    //    frontRight.steerAngle = currentSteer;
    //}

    void HandleSteering()
    {
        Vector3 target = waypoints[currentWaypoint].position;

        Vector3 direction = (target - transform.position).normalized;

        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

        currentSteer = Mathf.Clamp(angle, -maxSteeringAngle, maxSteeringAngle);

        frontLeft.steerAngle = currentSteer;
        frontRight.steerAngle = currentSteer;
    }

    void HandleMotor()
    {
        float speed = rb.linearVelocity.magnitude * 3.6f;

        if (obstacleAhead || speed > maxSpeed)
        {
            ApplyBrakes();
            currentTorque = 0;
        }
        else
        {
            ReleaseBrakes();

            float steerSlowdown = Mathf.Abs(currentSteer) / maxSteeringAngle;
            float speedLimit = Mathf.Lerp(maxSpeed, maxSpeed * 0.35f, steerSlowdown);

            currentTorque = speed < speedLimit ? maxMotorTorque : 0;
            if (speed > speedLimit)
            {
                currentTorque = 0;
                ApplyBrakes();
            }
        }   

        rearLeft.motorTorque = currentTorque;
        rearRight.motorTorque = currentTorque;
    }

    void ApplyBrakes()
    {
        frontLeft.brakeTorque = brakeForce;
        frontRight.brakeTorque = brakeForce;
        rearLeft.brakeTorque = brakeForce;
        rearRight.brakeTorque = brakeForce;
    }
    void ReleaseBrakes()
    {
        frontLeft.brakeTorque = 0;
        frontRight.brakeTorque = 0;
        rearLeft.brakeTorque = 0;
        rearRight.brakeTorque = 0;
    }

    void CheckWaypoint()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < waypointReachDistance)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
                currentWaypoint = 0;
        }
    }

    void UpdateWheelVisuals()
    {
        UpdateWheel(frontLeft, frontLeftMesh);
        UpdateWheel(frontRight, frontRightMesh);
        UpdateWheel(rearLeft, rearLeftMesh);
        UpdateWheel(rearRight, rearRightMesh);
    }

    void UpdateWheel(WheelCollider collider, Transform mesh)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        mesh.position = position;
        mesh.rotation = rotation;
    }
}
