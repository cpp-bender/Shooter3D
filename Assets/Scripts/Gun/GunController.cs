using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform weaponHold;
    [SerializeField] private Gun startingGun;
    private Gun currentGun;

    private void Start()
    {
        EquipGun(startingGun);
    }

    public void EquipGun(Gun gun)
    {
        currentGun = Instantiate(gun, weaponHold.position, weaponHold.rotation, weaponHold);
    }

    public void Shoot()
    {
        currentGun.Shoot();
    }

    public float GetGunHeight()
    {
        return weaponHold.transform.position.y;
    }
}