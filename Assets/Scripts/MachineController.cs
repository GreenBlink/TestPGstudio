using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineController : MonoBehaviour
{
    public static MachineController instance;

    public WeaponController weaponController;
    public Transform machineTransform;
    public float speedMove = 1.0f;
    public float speedAngle = 1.0f;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        StartCoroutine(ControllerProcess());
        weaponController.Init();
    }

    public void RotationMachine(Vector3 rotation)
    {
        machineTransform.eulerAngles += rotation.normalized * speedAngle * Time.deltaTime;
    }

    public void MoveMachine(Vector3 move)
    {
        machineTransform.Translate(move);
    }

    private Vector3 GetMoveInput()
    {
        return transform.forward * Input.GetAxis("Vertical") * speedMove * Time.deltaTime;
    }

    private Vector3 GetAngleInput()
    {
        return transform.up * Input.GetAxis("Horizontal");
    }

    private IEnumerator ControllerProcess()
    {
        while (true)
        {
            MoveMachine(GetMoveInput());
            RotationMachine(GetAngleInput());

            yield return null;
        }
    }
}
