using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using static Unity.Barracuda.Model;

public class InputCar : MonoBehaviour
{
    [SerializeField] protected GameObject kartCapsule;
    public Animator PlayerAnimator;
    public string SteeringParam = "Steering";
    protected float horizontalSpeed = 0f;
    private Vector2 currentMove;
    int m_SteerHash;
    protected RoadManager roadManager;

    void Awake()
    {
        //Assert.IsNotNull(Kart, "No ArcadeKart found!");
        Assert.IsNotNull(PlayerAnimator, "No PlayerAnimator found!");
        m_SteerHash = Animator.StringToHash(SteeringParam);
        roadManager = kartCapsule.GetComponent<RoadManager>();
    }

    private void FixedUpdate()
    {
        if (!roadManager.GetMove) return;

        horizontalSpeed = roadManager.HorizontalSpeed;
        Vector3 moveVelocity = horizontalSpeed * (
        currentMove.x * Vector3.right +
        currentMove.y * Vector3.left
        );
        Vector3 moveThisFrame = Time.deltaTime * moveVelocity;
        transform.position += moveThisFrame;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (!roadManager.GetMove) return;
        currentMove = context.ReadValue<Vector2>();
        //steeringSmoother = Mathf.Lerp(steeringSmoother, currentMove.x, Time.deltaTime * 5f);
        PlayerAnimator.SetFloat(m_SteerHash, currentMove.x);
    }


}
