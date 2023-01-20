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

      


        }

        public Rigidbody Rigidbody { get; private set; }
        public float AirPercent { get; private set; }



        public MoveKart.Stats stat = new MoveKart.Stats
        {
            Speed = 10f,

         
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

      
      


        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            //mettere anche l'input
        }

        void FixedUpdate()
        {
            Rigidbody.centerOfMass = transform.InverseTransformPoint(CenterOfMass.position);



            MoveVehicle();


      
        }

        private void MoveVehicle()
        {
       Vector3 constantVelocity = new Vector3(0f, 0f , stat.Speed);
        //Rigidbody.velocity = stat.Speed * (Rigidbody.velocity.normalized); 
        transform.position += constantVelocity * Time.deltaTime;

    }

    

    }


