using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public Transform TargetPos;
    public float OffSet;

    private float MoveX, MoveY;
    public float RotateSpeed;
    public float MoveSpeed;
    private void Update()
    {
        MoveX += -(Input.GetAxis("Mouse Y")) * RotateSpeed * Time.deltaTime;
        MoveY += Input.GetAxis("Mouse X") * RotateSpeed * Time.deltaTime;

        MoveX = Mathf.Clamp(MoveX, 10f, 30f);
        Quaternion rot = Quaternion.Euler(MoveX, MoveY, 0);

        transform.rotation = rot;
    }
    private void LateUpdate()
    {       
        transform.position = new Vector3(TargetPos.position.x,1.5f,TargetPos.position.z) - transform.forward * OffSet;
    }
}
