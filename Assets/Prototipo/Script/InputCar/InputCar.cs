using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using static Unity.Barracuda.Model;

public class InputCar : MonoBehaviour
{
    [SerializeField] protected GameObject kartCapsule;
    public float rightboundary = 4.0f;
    public float leftboundary = -4.0f;
    protected float horizontalSpeed = 0f;
    private Vector2 currentMove;

    protected RoadManager roadManager;

    void Awake()
    {        
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

        //Debug.Log(transform.position);
        if (transform.position.x > rightboundary) transform.position = new Vector3(rightboundary, 0, 0);
        if (transform.position.x < leftboundary) transform.position = new Vector3(leftboundary, 0, 0);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (!roadManager.GetMove) return;
        currentMove = context.ReadValue<Vector2>();
    }

    public float CurrentMove()
    {
        return currentMove.x;
    }

}
