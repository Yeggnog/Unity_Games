using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3P : MonoBehaviour
{
    // variables
    public CharacterController control;
    public Transform camera;

    public float moveSpeed = 6f;
    public float maxMoveSpeed = 20f;

    public float turnSmoothTime = 0.1f;
    float smoothVelocity;

    // Update is called once per frame
    void Update()
    {
        // get axes
        float axisH = Input.GetAxisRaw("Horizontal");
        float axisV = Input.GetAxisRaw("Vertical");
        Vector3 dir = (new Vector3(axisH, 0f, axisV)).normalized;

        if(dir.magnitude >= 0.1f){
            float targAngle = ((Mathf.Atan2(dir.x, dir.z))*Mathf.Rad2Deg) + camera.eulerAngles.y;
            float ang = Mathf.SmoothDampAngle(transform.eulerAngles.y, targAngle, ref smoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, ang, 0f);

            Vector3 moveDir = (Quaternion.Euler(0f, targAngle, 0f)) * Vector3.forward;
            control.Move((moveDir.normalized) * moveSpeed * Time.deltaTime);
        }
    }
}
