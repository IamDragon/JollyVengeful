using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Weapon activeWeapon;
    private Inventory inventory;

    private delegate void AttackTypedel();
    AttackTypedel currentAttack;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        activeWeapon = inventory.CurrentWeapon;
        SetCurrentAttack();
    }

    public void CheckInputs()
    {
        Attack();
        SwitchToPrimary();
        SwitchToSecondary();
        ThrowWeapon();
        DropWeapon();
        ReloadGun();
    }

    private void Attack()
    {
        if (activeWeapon)
            currentAttack();
    }
    private void Shoot()
    {
        GunController gun = activeWeapon as GunController;
        if (Input.GetMouseButtonUp(0))
            gun.isFiring = false;
        else if (Input.GetMouseButtonDown(0))
            gun.isFiring = true;
    }

    private void SetAttackWithGun()
    {
        currentAttack = Shoot;
    }

    private void SwordSlash()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<Animator>().Play("SwordSlash"); //change to play from PlayerAnimation.cs
            BasicMelee melee = activeWeapon as BasicMelee;
            melee.StartAttack();
        }
    }
    private void SetAttackWithMelee()
    {
        currentAttack = SwordSlash;
    }

    public void SetCurrentAttack()
    {
        if (activeWeapon is GunController)
            SetAttackWithGun();
        else if (activeWeapon is BasicMelee)
            SetAttackWithMelee();
    }
    private void SwitchToPrimary()
    {
        if (Input.GetKey(KeyCode.Alpha1) && inventory.PrimaryWeapon != null)
            inventory.SwitchToPrimary(out activeWeapon);
        SetCurrentAttack();
    }

    private void SwitchToSecondary()
    {
        if (Input.GetKey(KeyCode.Alpha2) && inventory.SecondaryWeapon != null)
            inventory.SwitchToSecondary(out activeWeapon);
        SetCurrentAttack();
    }

    private void ThrowWeapon()
    {
        if (Input.GetKeyDown(KeyCode.T) && activeWeapon != null)
        {
            GetComponent<HandPlacementIK>().ClearTargetAndHint();
            activeWeapon.ThrowWeapon();
            activeWeapon = null;
            inventory.OnWeaponRelease();
        }
    }
    
    private void DropWeapon()
    {
        if (Input.GetKeyDown(KeyCode.V) && activeWeapon != null)
        {
            GetComponent<HandPlacementIK>().ClearTargetAndHint();
            activeWeapon.DropWeapon();
            activeWeapon = null;
            inventory.OnWeaponRelease();
        }
    }

    private void ReloadGun()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (activeWeapon is GunController)
            {
                GunController gun = activeWeapon as GunController;
                gun.StartReloading();
            }
        }
    }

    public void SetActiveWeapon(Weapon newWeapon)
    {
        activeWeapon = newWeapon;
        SetCurrentAttack();
    }
}
