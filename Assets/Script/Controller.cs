using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    enum PlayerState { IDLE, MOVE, ATTACK };

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
    [SerializeField]
    private PlayerState CurState = PlayerState.IDLE;
    [SerializeField]
    private float ComboDeley;
    Rigidbody Rigid;
    Animator anim;
    Vector2 MoveInput;
    int AttNum;
    bool UnderAttack = false;
    void Start()
    {
        Rigid = GetComponent<Rigidbody>();
        anim = PlayerObject.GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isCheck = MoveInput.magnitude != 0;
        switch (CurState)
        {
            case PlayerState.IDLE:
                anim.SetFloat("Move", 0f);

                if (isCheck == true)
                {
                    CurState = PlayerState.MOVE;
                }
                
                if (Input.GetMouseButtonDown(0))
                {
                    CurState = PlayerState.ATTACK;
                }
                break;
            case PlayerState.MOVE:
                Moving();

                if(isCheck == false)
                {
                    CurState = PlayerState.IDLE;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    CurState = PlayerState.ATTACK;
                }
                break;
            case PlayerState.ATTACK:
                if(UnderAttack != true)
                {
                    Attack(AttNum);
                }

                if(isCheck == true)
                {
                    CurState = PlayerState.MOVE;
                }
                else
                {
                    CurState = PlayerState.IDLE;
                }
                break;
        }     
        
        CameraMove();
    }

    private void Moving()
    {
        Debug.DrawRay(Camera.position, new Vector3(Camera.forward.x, 0f, Camera.forward.z), Color.blue);

        anim.SetFloat("Move", 0.2f);

        Vector3 LookForward = new Vector3(Camera.forward.x, 0f, Camera.forward.z).normalized;
        Vector3 LookRight = new Vector3(Camera.right.x, 0f, Camera.right.z).normalized;
        Vector3 MoveDir = LookForward * MoveInput.y + LookRight * MoveInput.x;

        PlayerObject.transform.forward = MoveDir;
        Rigid.velocity = MoveDir * Time.deltaTime * MoveCheck(Input.GetKey(KeyCode.LeftShift));
    }

    private void Attack(int num)
    {
        anim.SetFloat("Combo", num);
        anim.SetTrigger("Attack");
        StartCoroutine(Combo());
    }

    IEnumerator Combo()
    {
        float curtime = 0f;
        yield return null;

        while(curtime >= ComboDeley || AttNum < 3)
        {
            curtime += Time.deltaTime;
            if(Input.GetMouseButtonDown(0))
            {
                Attack(AttNum++);
            }
        }
        AttNum = 0;
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
