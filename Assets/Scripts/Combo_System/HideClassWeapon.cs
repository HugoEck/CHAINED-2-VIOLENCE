using UnityEngine;

public class HideClassWeapon : MonoBehaviour
{
    [SerializeField] private WeaponManager _weaponManager;
    [SerializeField] private GameObject _classWeapon;
    // Start is called before the first frame update
    void Start()
    {
        _weaponManager.OnWeaponEquipped += WeaponManagerOnWeaponEquipped;
        _weaponManager.OnWeaponBroken += WeaponManagerOnWeaponBroken;
    }

    private void WeaponManagerOnWeaponBroken(GameObject @object)
    {
        _classWeapon.SetActive(true);
    }

    private void WeaponManagerOnWeaponEquipped(GameObject @object)
    {
        _classWeapon.SetActive(false);
    }

    
}
