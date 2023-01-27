using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterStatus : MonoBehaviour
{
    public float lr_speed = 5.0f;
    protected bool isMoving;
    protected bool isAlive = true;
    protected bool scorePoint;
    protected bool isPaused = false;
    protected Vector3 movement;
    //protected PlayerHealthManager healthManager;

    public bool GetScorePoint
    {
        get { return scorePoint; }
    }

    public bool IsPaused
    {
        get { return isPaused; }
        set { isPaused = value; }
    }
       
    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }

    public bool IsMoving
    {
        get { return isMoving; }
    }

    public Vector3 Movement
    {
        get { return movement; }
    }

    public void Update()
    {
        isMoving = movement.x != 0;     //se non si muove, a destra e sinistra allora isMoving è falso
    }

    public void ScorePoint(InputAction.CallbackContext context)
    {
        if(context.started) scorePoint = true;
        else if(context.canceled) scorePoint = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 v = context.ReadValue<Vector2>();
        movement.x = v.x * lr_speed;
    }

    public void Paused(InputAction.CallbackContext context)
    {
        if(isPaused) isPaused = false;
        else isPaused = true;
    }
}
