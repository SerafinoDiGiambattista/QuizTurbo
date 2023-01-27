using KartGame.KartSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MoveKart : MonoBehaviour
{

    [SerializeField] protected float baseLRSpeed = 5.0f;
    protected float computed_speed;
    //[SerializeField] protected Camera camera;
    [SerializeField] protected string LR_SPEED = "HORIZONTAL_SPEED";
    protected float currentLRSpeed = 0;
    //protected CharacterController cc;
    //protected Animation animation;
    protected CharacterStatus status;
    protected PlayerManager pm;


    void Awake()
    {
        //cc = GetComponent<CharacterController>();
        status = GetComponent<CharacterStatus>();
        pm = GetComponent<PlayerManager>();
    }

    void FixedUpdate()
    {
        if(status.IsAlive && !status.IsPaused)
        {
            Vector3 movement = new Vector3(0, 0, 0);
            currentLRSpeed = baseLRSpeed;
        
            computed_speed = currentLRSpeed * ComputateFeature(LR_SPEED);

            if(status.IsMoving)
            {
                movement.x = status.Movement.x * computed_speed * Time.deltaTime;
            }
       
            //AnimationSpeed(movement);
            movement = transform.TransformDirection(movement);
            //cc.Move(movement * Time.deltaTime);
         }
    }

    protected float ComputateFeature(string feature)
    {
        return pm.FeatureValue(feature);
    }
}


