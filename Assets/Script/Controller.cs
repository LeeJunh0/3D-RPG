using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    enum PlayerState { IDLE, MOVE, ATTACK };
    enum AttackState { NONE ,ONE, TWO, THREE };

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
    private AttackState CurAttState = AttackState.NONE;
    [SerializeField]
    private float ComboDeley;   
    [SerializeField]
    private int AttNum;

    Rigidbody Rigid;
    Animator anim;
    Vector2 MoveInput;
    float CurDeley;
    bool UnderAttack = false;

    //public bool MouseInput;
    void Start()
    {
        Rigid = GetComponent<Rigidbody>();
        anim = PlayerObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if(CurDeley > ComboDeley)
        {
            AttackInit();
        }

        if(UnderAttack == true)        
            CurDeley += Time.deltaTime;
        
        if(Input.GetMouseButtonDown(0))
        {
            AttNum++;
            CurDeley = 0f;
            switch(AttNum)
            {
                case 1:                   
                    UnderAttack = true;
                    Attack1Play();
                    break;
                case 2:
                    Attack2Play();
                    break;
                case 3:
                    anim.SetBool("Attack3", true);
                    if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
                    {
                        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime == 0.9f)
                            AttackInit();
                    }
                    
                    break;
            }
            AttNum = Mathf.Clamp(AttNum, 0, 3);
        }
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(Camera.position, new Vector3(Camera.forward.x, 0f, Camera.forward.z), Color.blue);

        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isCheck = MoveInput.magnitude != 0;
        switch (CurState)
        {
            case PlayerState.IDLE:
                anim.SetFloat("Move", 0f);

                if (isCheck == true)                
                    CurState = PlayerState.MOVE;
                
                if (Input.GetMouseButtonDown(0))                
                    CurState = PlayerState.ATTACK;               
                break;
            case PlayerState.MOVE:
                Moving();

                if(isCheck == false)                
                    CurState = PlayerState.IDLE;
                
                if (Input.GetMouseButtonDown(0))                
                    CurState = PlayerState.ATTACK;               
                break;
            case PlayerState.ATTACK:
                if(UnderAttack == false)
                {
                    if (isCheck == true)                    
                        CurState = PlayerState.MOVE;
                    
                    else                    
                        CurState = PlayerState.IDLE;                    
                }               
                break;
        }     
        
        CameraMove();
    }

    private void Moving()
    {
        anim.SetFloat("Move", 0.2f);

        Vector3 LookForward = new Vector3(Camera.forward.x, 0f, Camera.forward.z).normalized;
        Vector3 LookRight = new Vector3(Camera.right.x, 0f, Camera.right.z).normalized;
        Vector3 MoveDir = LookForward * MoveInput.y + LookRight * MoveInput.x;

        PlayerObject.transform.forward = MoveDir;
        Rigid.velocity = MoveDir * Time.deltaTime * MoveCheck(Input.GetKey(KeyCode.LeftShift));
    }

    void AttackInit()
    {
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
        AttNum = 0;
        UnderAttack = false;
    }
    public void Attack1Play()
    {
        anim.SetBool("Attack1", true);
    }

    public void Attack2Play()
    {
        anim.SetBool("Attack2",true);      
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
