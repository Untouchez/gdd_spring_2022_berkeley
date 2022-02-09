using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Vector3 calculatedInput;
    Vector3 rawInput;

    public bool canMove;
    public bool canRotate;
    void Start()
    {
        //FINDS ANIMATOR FROM THE PLAYER GAMEOBJECT
        anim = GetComponent<Animator>();

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

        //ANGLE BETWEEN PLAYER AND CAMERA
        angle = Vector3.Angle(transform.TransformDirection(transform.forward), transform.TransformDirection(Camera.main.transform.forward));

        //IF TURNING THEN PLAY TURNING ANIMATION
        if(angle > 20f)
            anim.SetBool("turn", true);
        else
            anim.SetBool("turn", false);
        HandleInputs();
        HandleSprint();
        HandleAttack();
    }

    void FixedUpdate()
    {
        HandleRotations();
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
        if (isSprinting || !canRotate)
            return;
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
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
        
        if (rawInput.magnitude != 0 || !canMove)
            calculatedInput = new Vector3(Mathf.MoveTowards(calculatedInput.x, rawInput.x, acceleration * Time.deltaTime), 0, Mathf.MoveTowards(calculatedInput.z, rawInput.z, acceleration * Time.deltaTime));        
        else
            calculatedInput = new Vector3(Mathf.MoveTowards(calculatedInput.x, 0, acceleration * Time.deltaTime),0, Mathf.MoveTowards(calculatedInput.z, 0, decceleration * Time.deltaTime));

        calculatedInput = Vector3.ClampMagnitude(calculatedInput, 1);
        anim.SetFloat("Horizontal", calculatedInput.x);
        anim.SetFloat("Vertical", calculatedInput.z);
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
