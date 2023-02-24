using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using static Unity.Barracuda.Model;

public class CarAnimation : MonoBehaviour
{

    /*public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    public Transform wheelFLTrans;
    public Transform wheelFRTrans;
    public Transform wheelBLTrans;
    public Transform wheelBRTrans;

    public float maxMotorTorque = 30f;
    public float maxSteeringAngle = 30f;

    void FixedUpdate()
    {
        wheelBR.motorTorque = maxMotorTorque * Input.GetAxis("Vertical");
        wheelBL.motorTorque = maxMotorTorque * Input.GetAxis("Vertical");

        wheelFL.steerAngle = maxSteeringAngle * Input.GetAxis("Horizontal");
        wheelFR.steerAngle = maxSteeringAngle * Input.GetAxis("Horizontal");
    }

    void Update()
    {
        wheelFLTrans.Rotate(0, wheelFL.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelFRTrans.Rotate(0, wheelFR.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelBLTrans.Rotate(0, wheelBL.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelBRTrans.Rotate(0, wheelBR.rpm / 60 * 360 * Time.deltaTime, 0);
    }*/

    [Serializable]
    public class Wheel
    {
        [Tooltip("A reference to the transform of the wheel.")]
        public Transform wheelTransform;
        [Tooltip("A reference to the WheelCollider of the wheel.")]
        public WheelCollider wheelCollider;

        Quaternion m_SteerlessLocalRotation;

        public void Setup() => m_SteerlessLocalRotation = wheelTransform.localRotation;

        public void StoreDefaultRotation() => m_SteerlessLocalRotation = wheelTransform.localRotation;
        public void SetToDefaultRotation() => wheelTransform.localRotation = m_SteerlessLocalRotation;
    }

    [Space]
    [Tooltip("The damping for the appearance of steering compared to the input.  The higher the number the less damping.")]
    public float steeringAnimationDamping = 10f;

    [Space]
    [Tooltip("The maximum angle in degrees that the front wheels can be turned away from their default positions, when the Steering input is either 1 or -1.")]
    public float maxSteeringAngle;
    [Tooltip("Information referring to the front left wheel of the kart.")]
    public Wheel frontLeftWheel;
    [Tooltip("Information referring to the front right wheel of the kart.")]
    public Wheel frontRightWheel;
    [Tooltip("Information referring to the rear left wheel of the kart.")]
    public Wheel rearLeftWheel;
    [Tooltip("Information referring to the rear right wheel of the kart.")]
    public Wheel rearRightWheel;
    float m_SmoothedSteeringInput;

    public Animator PlayerAnimator;
    int m_SteerHash;
    public string SteeringParam = "Steering";

    private void Awake()
    {
        Assert.IsNotNull(PlayerAnimator, "No PlayerAnimator found!");
        m_SteerHash = Animator.StringToHash(SteeringParam);
    }
    void Start()
    {
        frontLeftWheel.Setup();
        frontRightWheel.Setup();
        rearLeftWheel.Setup();
        rearRightWheel.Setup();
    }

    void FixedUpdate()
    {
        m_SmoothedSteeringInput = Mathf.MoveTowards(m_SmoothedSteeringInput, GetComponent<InputCar>().CurrentMove(), steeringAnimationDamping * Time.deltaTime);
        PlayerAnimator.SetFloat(m_SteerHash, GetComponent<InputCar>().CurrentMove());

        // Steer front wheels
        float rotationAngle = m_SmoothedSteeringInput * maxSteeringAngle;

        frontLeftWheel.wheelCollider.steerAngle = rotationAngle;
        frontRightWheel.wheelCollider.steerAngle = rotationAngle;

        // Update position and rotation from WheelCollider
        UpdateWheelFromCollider(frontLeftWheel);
        UpdateWheelFromCollider(frontRightWheel);
        UpdateWheelFromCollider(rearLeftWheel);
        UpdateWheelFromCollider(rearRightWheel);
    }

    void LateUpdate()
    {
        // Update position and rotation from WheelCollider
        UpdateWheelFromCollider(frontLeftWheel);
        UpdateWheelFromCollider(frontRightWheel);
        UpdateWheelFromCollider(rearLeftWheel);
        UpdateWheelFromCollider(rearRightWheel);
    }

    void UpdateWheelFromCollider(Wheel wheel)
    {
        wheel.wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheel.wheelTransform.position = position;
        wheel.wheelTransform.rotation = rotation;
    }
}
