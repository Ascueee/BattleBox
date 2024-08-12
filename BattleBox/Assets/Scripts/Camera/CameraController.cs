using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Vars")]
    [SerializeField] float camSpeed;
    [SerializeField] float sensX;
    [SerializeField] float sensY;

    [Header("Camera Componenets")]
    [SerializeField] Transform cameraObj;
    float horizontalMovement;
    float verticalMovement;
    float mouseHorizontalMovement;
    float mouseVerticalMovement;

    float xRotation;
    float yRotation;

    bool canRotate;

    Vector3 moveDir;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraInput();
    }

    private void FixedUpdate()
    {
        MoveCam();
        if (canRotate == true)
        {
            RotateCam();
        }
    }

    void MoveCam()
    {
        moveDir = cameraObj.forward * verticalMovement + cameraObj.right * horizontalMovement;
        rb.velocity = moveDir * camSpeed;
        //rb.AddForce(moveDir.normalized * camSpeed, ForceMode.Force);
    }

    void RotateCam()
    {
        mouseHorizontalMovement = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseVerticalMovement = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;


        yRotation += mouseHorizontalMovement;

        xRotation -= mouseVerticalMovement;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    void CameraInput()
    {
        //gets keyboard input
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(1))
        {
            canRotate = true;
        }
        
        if(Input.GetMouseButtonUp(1))
        {
            canRotate = false;
        }

    }

}
