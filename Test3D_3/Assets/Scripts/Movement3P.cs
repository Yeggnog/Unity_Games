using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3P : MonoBehaviour
{
    // variables
    public CharacterController control;
    public Transform camera;

    public float moveSpeed = 7f;
    public float maxMoveSpeed = 20f;
    public float jumpSpeed = 7f;
    public float jumpTime = 0f; // max 3

    public float turnSmoothTime = 0.1f;
    float smoothVelocity;

    // Update is called once per frame
    void Update()
    {
        // get movement axes
        float axisH = Input.GetAxisRaw("Horizontal");
        float axisV = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = (new Vector3(axisH, 0f, axisV)).normalized;

        if(moveDir.magnitude >= 0.6f){
            // run
            float targAngle = ((Mathf.Atan2(moveDir.x, moveDir.z))*Mathf.Rad2Deg) + camera.eulerAngles.y;
            float ang = Mathf.SmoothDampAngle(transform.eulerAngles.y, targAngle, ref smoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, ang, 0f);

            Vector3 move = (Quaternion.Euler(0f, targAngle, 0f)) * Vector3.forward;
            control.Move((move.normalized) * moveSpeed * Time.deltaTime);

            // update moveSpeed
            if(moveSpeed < 20f){
                moveSpeed += 0.2f;
            }
        }else if(moveDir.magnitude >= 0.1f){
            // walk
            float targAngle = ((Mathf.Atan2(moveDir.x, moveDir.z))*Mathf.Rad2Deg) + camera.eulerAngles.y;
            float ang = Mathf.SmoothDampAngle(transform.eulerAngles.y, targAngle, ref smoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, ang, 0f);

            Vector3 move = (Quaternion.Euler(0f, targAngle, 0f)) * Vector3.forward;
            control.Move((move.normalized) * moveSpeed * Time.deltaTime);
        }else{
            // undo momentum
            if(moveSpeed > 7f){
                moveSpeed -= 2f;
            }else{
                moveSpeed = 7f;
            }
	}
    }
}