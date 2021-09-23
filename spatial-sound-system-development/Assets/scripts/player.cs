using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private bool jumpkeypressed;
    private float horizontalInput;
    private float verticalInput;
    private bool isGrounded;
    float cameraPitch = 0.0f;
    [SerializeField] private Transform groundCheckTransform = null;
    [SerializeField] Transform playerCamera = null;
    [SerializeField] bool lockCursor = true;
    CharacterController controller = null;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // check if space key is pressed down
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            jumpkeypressed = true;
        }

        //horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
        updateMouseLook();
    }

    private void FixedUpdate()
    {
        //GetComponent<Rigidbody>().velocity = new Vector3(horizontalInput, GetComponent<Rigidbody>().velocity.y, verticalInput);
        updateMovement();

        if (Physics.OverlapSphere(groundCheckTransform.position,0.1f).Length==1)
        {
            return;
        }

        if (jumpkeypressed)
        {
            //Debug.Log("Space key was pressed down");
            GetComponent<Rigidbody>().AddForce(Vector3.up*7, ForceMode.VelocityChange);
            jumpkeypressed = false;
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void updateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraPitch -= mouseDelta.y*3.5f;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * mouseDelta.x*3.5f);
    }

    void updateMovement()
    {
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir.Normalize();
        Vector3 velociy = (transform.forward * inputDir.y + transform.right * inputDir.x)*6.0f;
        controller.Move(velociy * Time.deltaTime);
    }
}
