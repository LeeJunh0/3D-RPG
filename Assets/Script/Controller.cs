using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject Camera;
    public float MoveSpeed;
    public float RunSpeed;
    Rigidbody Rigid;
    Animator anim;
    
    void Start()
    {
        Rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        Moving();
    }

    void Moving()
    {
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");

        if(X == 0 && Z == 0)
        {
            anim.SetFloat("Move", 0);
            return;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("RunCheck", true);
            anim.SetFloat("Move", 0.2f);
        }
        else
        {
            anim.SetBool("RunCheck", false);
            anim.SetFloat("Move", 0.2f);
        }

        //transform.forward = new Vector3(Camera.transform.forward.x, 0f, Camera.transform.forward.z);
        transform.forward = new Vector3(Camera.transform.forward.x, 0f, Camera.transform.forward.z);
        Rigid.velocity = new Vector3(transform.forward.x * X, 0, transform.right.z * Z) * MoveSpeed * Time.deltaTime;
    }

    private float MoveCheck(bool Check)
    {
        return Check ? RunSpeed: MoveSpeed;
    }
}
