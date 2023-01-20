using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using static Unity.Barracuda.Model;

public class InputEvent : MonoBehaviour
{
    public Animator PlayerAnimator;
    public string SteeringParam = "Steering";
    public float speed = 2f;
    private Vector2 currentMove;
    int m_SteerHash;
    //float steeringSmoother = 0;

    void Awake()
    {
        //Assert.IsNotNull(Kart, "No ArcadeKart found!");
        Assert.IsNotNull(PlayerAnimator, "No PlayerAnimator found!");
        m_SteerHash = Animator.StringToHash(SteeringParam);
        
    }
    private void FixedUpdate()
    {
        Vector3 moveVelocity = speed * (
        currentMove.x * Vector3.right +
        currentMove.y * Vector3.left
        );
        Vector3 moveThisFrame = Time.deltaTime * moveVelocity;
        transform.position += moveThisFrame;
        //
        

    }


    public void OnMove(InputAction.CallbackContext context)
    {
        currentMove = context.ReadValue<Vector2>();
        //steeringSmoother = Mathf.Lerp(steeringSmoother, currentMove.x, Time.deltaTime * 5f);
        PlayerAnimator.SetFloat(m_SteerHash, currentMove.x);
    }


}
