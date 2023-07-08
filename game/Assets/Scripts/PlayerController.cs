using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [Tooltip("The controls")]
    public InputActionAsset actions;

    public Vector2 Velocity;
    
    [Tooltip("Standard speed of the ghost")]
    public float speed = 10f;
    public float dashSpeed = 10f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    
    
    private Vector2 _currentVelocity;
    
    public CharacterController2D Controller;
    
    #region Input Actions & Maps

    private InputActionMap _map;

    private InputAction _moveAction;
    private InputAction _upAction;
    private InputAction _dashAction;
    private InputAction _escapeAction;
    private InputAction _pointerLocation;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _map = actions.FindActionMap("Player");
        _moveAction = _map.FindAction("Move");
        _pointerLocation = _map.FindAction("PointerLocation");
        _dashAction = _map.FindAction("Dash");
        _upAction = _map.FindAction("Up");
        _escapeAction = _map.FindAction("Escape");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log(_moveAction.ReadValue<float>());
        // Controller.Move(_moveAction.ReadValue<float>() * Time.fixedDeltaTime, _dashAction.IsPressed(), _upAction.IsPressed());
        
        Controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
        //animator.SetFloat(Speed, Mathf.Abs(direction * moveSpeed));

        //transform.position += Vector3.right * (direction * moveSpeed * Time.deltaTime);
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        crouch = false;
        // if (Input.GetButtonDown("Crouch"))
        // {
        //     crouch = true;
        // } else if (Input.GetButtonUp("Crouch"))
        // {
        //     crouch = false;
        // }
    }
}