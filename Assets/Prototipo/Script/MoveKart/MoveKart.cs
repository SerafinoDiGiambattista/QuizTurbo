using KartGame.KartSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class MoveKart : MonoBehaviour
    {

        [System.Serializable]
        public struct Stats
        {
            [Header("Velocità messa alla macchina")]
            [Min(0.001f), Tooltip("Top speed attainable when moving forward.")]
            public float Speed;

            [Tooltip("How tightly the kart can turn left or right.")]
            public float Steer;

            [Tooltip("Additional gravity for when the kart is in the air.")]
            public float AddedGravity;


        }

        public Rigidbody Rigidbody { get; private set; }
        public float AirPercent { get; private set; }

        public float GroundPercent { get; private set; }

        public MoveKart.Stats stat = new MoveKart.Stats
        {
            Speed = 10f,

            Steer = 5f,

            AddedGravity = 1f,
        };


        [Header("Vehicle Visual")]
        public List<GameObject> m_VisualWheels;

        [Header("Vehicle Physics")]
        [Tooltip("The transform that determines the position of the kart's mass.")]
        public Transform CenterOfMass;

        [Range(0.0f, 20.0f), Tooltip("Coefficient used to reorient the kart in the air. The higher the number, the faster the kart will readjust itself along the horizontal plane.")]
        public float AirborneReorientationCoefficient = 3.0f;


        [Header("Physical Wheels")]
        [Tooltip("The physical representations of the Kart's wheels.")]
        public WheelCollider FrontLeftWheel;
        public WheelCollider FrontRightWheel;
        public WheelCollider RearLeftWheel;
        public WheelCollider RearRightWheel;

        [Tooltip("Which layers the wheels will detect.")]
        public LayerMask GroundLayers = Physics.DefaultRaycastLayers;

        readonly List<(GameObject trailRoot, WheelCollider wheel, TrailRenderer trail)> m_DriftTrailInstances = new List<(GameObject, WheelCollider, TrailRenderer)>();
        readonly List<(WheelCollider wheel, float horizontalOffset, float rotation, ParticleSystem sparks)> m_DriftSparkInstances = new List<(WheelCollider, float, float, ParticleSystem)>();

        float m_PreviousGroundPercent = 1.0f;
        MoveKart.Stats MoveStat;

        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            //mettere anche l'input
        }

        void FixedUpdate()
        {
            Rigidbody.centerOfMass = transform.InverseTransformPoint(CenterOfMass.position);

            int groundedCount = 0;
            if (FrontLeftWheel.isGrounded && FrontLeftWheel.GetGroundHit(out WheelHit hit))
                groundedCount++;
            if (FrontRightWheel.isGrounded && FrontRightWheel.GetGroundHit(out hit))
                groundedCount++;
            if (RearLeftWheel.isGrounded && RearLeftWheel.GetGroundHit(out hit))
                groundedCount++;
            if (RearRightWheel.isGrounded && RearRightWheel.GetGroundHit(out hit))
                groundedCount++;

            // calculate how grounded and airborne we are
            GroundPercent = (float)groundedCount / 4.0f;
            AirPercent = 1 - GroundPercent;


            MoveVehicle();

            GroundAirbourne();

            m_PreviousGroundPercent = GroundPercent;
        }

        private void MoveVehicle()
        {

            Rigidbody.velocity = stat.Speed * (Rigidbody.velocity.normalized); 




        }

        void GroundAirbourne()
        {
            // while in the air, fall faster
            if (AirPercent >= 1)
            {
                Rigidbody.velocity += Physics.gravity * Time.fixedDeltaTime * MoveStat.AddedGravity;
            }
        }

    }


