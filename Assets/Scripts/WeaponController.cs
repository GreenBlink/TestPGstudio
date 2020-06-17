using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Vector3 screenSizeMax;
    private Vector3 screenSizeMin;
    private Vector3 lastPositionMouse;
    private bool isAllowedShoot;

    public Transform weaponTransform;
    public RenderTrackBullet renderTrackBullet;
    public Bullet bulletPrefab;
    public float timeReloading = 1f;
    public float camSens = 0.25f;

    [Header("Offset")]
    public Vector2 maxOffset = new Vector2(80, 20);
    public Vector2 minOffset = new Vector2(0, -20);

    public void Init()
    {
        screenSizeMax = new Vector3(Screen.width - Screen.width * 0.2f, Screen.height - Screen.height * 0.2f, 0);
        screenSizeMin = new Vector3(Screen.width * 0.2f, Screen.height * 0.2f, 0);
        lastPositionMouse = Input.mousePosition;

        StartCoroutine(ControllerWeaponProcess());
    }

    public void SetStatusShoot(bool isAllowed)
    {
        isAllowedShoot = isAllowed;
        renderTrackBullet.ShowStatusWeapon(isAllowed);
    }

    public Vector3 RotationWeapon(Vector3 rotation)
    {
        float tempOffsetY = 0;
        weaponTransform.localEulerAngles = weaponTransform.localEulerAngles + rotation;

        if (weaponTransform.localEulerAngles.x > maxOffset.x && weaponTransform.localEulerAngles.x < 180)
        {
            weaponTransform.localEulerAngles = new Vector3(maxOffset.x, weaponTransform.localEulerAngles.y, weaponTransform.localEulerAngles.z);
        }
        else if (weaponTransform.localEulerAngles.x < 360 + minOffset.x && weaponTransform.localEulerAngles.x > 180)
        {
            weaponTransform.localEulerAngles = new Vector3(minOffset.x, weaponTransform.localEulerAngles.y, weaponTransform.localEulerAngles.z);
        }

        if (weaponTransform.localEulerAngles.y > 180 + maxOffset.y)
        {
            weaponTransform.localEulerAngles = new Vector3(weaponTransform.localEulerAngles.x, 180 + maxOffset.y, weaponTransform.localEulerAngles.z);
            tempOffsetY = 1;
        }
        else if (weaponTransform.localEulerAngles.y < 180 + minOffset.y)
        {
            weaponTransform.localEulerAngles = new Vector3(weaponTransform.localEulerAngles.x, 180 + minOffset.y, weaponTransform.localEulerAngles.z);
            tempOffsetY = -1;
        }

        return new Vector3(0, tempOffsetY, 0);
    }

    private void Shoot()
    {
        if (!isAllowedShoot)
            return;

        Bullet temp = Instantiate(bulletPrefab);
        temp.bulletTransform.position = weaponTransform.position;
        temp.bulletTransform.rotation = weaponTransform.rotation;
    }

    private Vector3 GetAngleInput()
    {
        Vector3 offset = (lastPositionMouse - Input.mousePosition).normalized;

        if (Input.mousePosition.x > screenSizeMax.x)
            offset = new Vector3(-1, offset.y, offset.z);
        else if (Input.mousePosition.x < screenSizeMin.x)
            offset = new Vector3(1, offset.y, offset.z);

        if (Input.mousePosition.y > screenSizeMax.y)
            offset = new Vector3(offset.x, -1, offset.z);
        else if(Input.mousePosition.y < screenSizeMin.y)
            offset = new Vector3(offset.x, 1, offset.z);

        offset = new Vector3(-offset.y * camSens, -offset.x * camSens, 0);
        lastPositionMouse = Input.mousePosition;

        return offset;
    }

    private IEnumerator ControllerWeaponProcess()
    {
        float timeReload = 0;

        while (true)
        {
            MachineController.instance.RotationMachine(RotationWeapon(GetAngleInput()));

            if (Input.GetMouseButtonDown(1) && timeReload <= 0)
            {
                timeReload = timeReloading;
                Shoot();
            }

            renderTrackBullet.ShowTrack(weaponTransform.position, weaponTransform.forward * bulletPrefab.speed);
            SetStatusShoot(renderTrackBullet.CheakShootWeapon());

            if (timeReload > 0)
                timeReload -= Time.deltaTime;

            yield return null;
        }
    }
}
