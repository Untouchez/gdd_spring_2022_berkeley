using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Player : MonoBehaviour
{

    public float turnSpeed = 15;
    public float aimDuration = 0.3f;
    public float angle;

    public float acceleration;
    public float decceleration;

    public bool isSprinting;
    Animator anim;
    Camera mainCamera;
    Rigidbody rb;

    Vector3 calculatedInput;
    Vector3 rawInput;

    public bool canMove;
    public bool canRotate;
    public bool lockedOn;

    public bool closeToWall;
    public bool isClimbing;

    public float gravityForce;

    public bool up;
    public bool down;
    public bool left;
    public bool right;
    void Start()
    {
        //FINDS ANIMATOR FROM THE PLAYER GAMEOBJECT
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        //GETS THE CAMERA FROM THE SCENE
        mainCamera = Camera.main;

        //DISABLES CURSOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //THE WAY THE PLAYER IS FACING
        Debug.DrawRay(transform.position, transform.forward,Color.green);

        //THE WAY THE PLAYER WANTS TO MOVE
        Debug.DrawRay(transform.position, transform.TransformDirection(rawInput),Color.blue);

        HandleInputs();
        HandleClimb();
        HandleMovement();
        HandleSprint();
        HandleAttack();
        HandleGravity();
    }

    void FixedUpdate()
    {
        HandleRotations();
    }

    void HandleMovement()
    {
        if (!canMove)
            return;
        anim.SetFloat("Horizontal", calculatedInput.x);
        anim.SetFloat("Vertical", calculatedInput.z);
    }

    void HandleClimb()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 1f, 0), transform.forward * 4f);
        Debug.DrawRay(transform.position + new Vector3(0, 1f, 0), transform.forward * 4f, Color.green);
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 0.7f)) //MIDDLE CHECK
            {
                transform.forward = -hit.normal;
                isClimbing = !isClimbing;
                anim.applyRootMotion = !isClimbing;
                closeToWall = true;
                anim.SetBool("climbing", isClimbing);
                canRotate = !isClimbing;
                canMove = !isClimbing;
            } else {
                anim.applyRootMotion = false;
                isClimbing = false;
                canMove = true;
                canRotate = true;
                anim.SetBool("climbing", false);
            }
        }

        if (isClimbing) {
            if (Physics.Raycast(ray, out RaycastHit hit, 0.7f)) {
                transform.forward = -hit.normal;
            } else {
                anim.applyRootMotion = true;
                isClimbing = false;
                canMove = true;
                canRotate = true;
                anim.SetBool("climbing", false);
            }
        }
        anim.SetFloat("Horizontal", calculatedInput.x);
        anim.SetFloat("Vertical", calculatedInput.z);
    }
    void HandleGravity()
    {
        if(!isGrounded() && !isClimbing)
        {
            rb.AddForce(Vector3.down * gravityForce, ForceMode.Acceleration);
        }
    }
    bool isGrounded()
    {
        if(Physics.Raycast(transform.position,-Vector3.down*4f,0.05f))       
            return true;        
        return false;
    }
    void HandleSprint()
    {
        if(Input.GetKey(KeyCode.LeftShift)) {
            isSprinting = true;
            anim.SetBool("isSprinting", true);
        } else {
            isSprinting = false;
            anim.SetBool("isSprinting", false);
        }
    }

    //ROTATES THE PLAYER TOWARDS THE CAMERA
    void HandleRotations()
    {
        if (!canRotate)
            return;
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.TransformDirection(calculatedInput)), turnSpeed * Time.fixedDeltaTime);
        
        if (calculatedInput.magnitude != 0)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }

    //CALCULATES AND SETS RAW INPUT AND CALCULATED INPUT
    //CHANGES ANIMATOR TO PLAY CORRECT ANIMATION
    void HandleInputs()
    {
        //GETS WASD INPUT
        // W = 1 Y
        // S = -1 Y
        // A = -1 X
        // D = 1 X
        rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
        if (rawInput.magnitude != 0)
            calculatedInput = new Vector3(Mathf.MoveTowards(calculatedInput.x, rawInput.x, acceleration * Time.deltaTime), 0, Mathf.MoveTowards(calculatedInput.z, rawInput.z, acceleration * Time.deltaTime));        
        else
            calculatedInput = new Vector3(Mathf.MoveTowards(calculatedInput.x, 0, acceleration * Time.deltaTime),0, Mathf.MoveTowards(calculatedInput.z, 0, decceleration * Time.deltaTime));

        calculatedInput = Vector3.ClampMagnitude(calculatedInput, 1);
    }

    void HandleAttack()
    {

    }
    #region animEvents
    public void FootR()
    {

    }

    public void FootL()
    {

    }
    #endregion
}
