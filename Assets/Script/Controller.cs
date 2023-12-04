using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private float CameraSensitivity;
    [SerializeField]
    private float MoveSpeed;
    [SerializeField]
    private float RunSpeed;
    [SerializeField]
    private GameObject PlayerObject;

    Rigidbody Rigid;
    Animator anim;
    
    void Start()
    {
        Rigid = GetComponent<Rigidbody>();
        anim = PlayerObject.GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        Moving();
        CameraMove();
    }

    void Moving()
    {
        Debug.DrawRay(Camera.position, new Vector3(Camera.forward.x, 0f, Camera.forward.z), Color.blue);
        Vector2 MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        bool isCheck = MoveInput.magnitude != 0;
        if(isCheck == true)
        {
            anim.SetFloat("Move", 0.2f);

            Vector3 LookForward = new Vector3(Camera.forward.x, 0f, Camera.forward.z).normalized;
            Vector3 LookRight = new Vector3(Camera.right.x, 0f, Camera.right.z).normalized;
            Vector3 MoveDir = LookForward * MoveInput.y + LookRight * MoveInput.x;

            PlayerObject.transform.forward = MoveDir;
            Rigid.velocity = MoveDir * Time.deltaTime * MoveCheck(Input.GetKey(KeyCode.LeftShift));
        }
        else
        {
            anim.SetFloat("Move", 0f);
        }
    }

    private float MoveCheck(bool Check)
    {
        anim.SetBool("RunCheck", Check);
        return Check ? RunSpeed: MoveSpeed;
    }

    private void CameraMove()
    {
        Vector2 MouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 CamAngle = Camera.rotation.eulerAngles;

        float x = CamAngle.x - MouseMove.y;
        if(x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        Camera.rotation = Quaternion.Euler(x * CameraSensitivity, CamAngle.y - MouseMove.x, CamAngle.z);
    }
}
