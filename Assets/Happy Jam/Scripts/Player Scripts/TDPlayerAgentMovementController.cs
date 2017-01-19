using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class TDPlayerAgentMovementController : MonoBehaviour, AI.IAgent
{
    [Header("Input Axis Properties")]
    public string HorizontalAxisName = "Horizontal";
    public string VerticalAxisName = "Vertical";

    [Header("Movement Properties")]
    public float MovementVelocity = 5.0f;

    [Header("Read-Only")]
    [SerializeField]
    private Vector2 _movementDirection;

    private Rigidbody2D _rigidbody2D;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        _movementDirection = new Vector2(Input.GetAxisRaw(HorizontalAxisName), Input.GetAxisRaw(VerticalAxisName));
        _rigidbody2D.velocity = _movementDirection * MovementVelocity;

    }

    public Vector3 GetVelocity()
    {
        return _rigidbody2D.velocity;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    #region Unnecessary

    public void SetSteering(Steering steering, int priority)
    {
        throw new NotImplementedException();
    }

    public void SetSteering(Steering steering, float weight)
    {
        throw new NotImplementedException();
    }

    public Steering GetPrioritySteering()
    {
        throw new NotImplementedException();
    }

    public Transform GetTransform()
    {
        throw new NotImplementedException();
    }

    public float GetMaxSpeed()
    {
        throw new NotImplementedException();
    }

    public float GetMaxAccel()
    {
        throw new NotImplementedException();
    }

    public float GetOrientation()
    {
        throw new NotImplementedException();
    }

    public float GetRotation()
    {
        throw new NotImplementedException();
    }

    public float GetMaxAngularAccel()
    {
        throw new NotImplementedException();
    }

    public float GetMaxRotation()
    {
        throw new NotImplementedException();
    }

    public bool IsUsingPriority()
    {
        throw new NotImplementedException();
    }
    #endregion
}
