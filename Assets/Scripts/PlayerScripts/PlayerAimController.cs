using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [SerializeField] GameObject crosshairGameObject;

    Vector3 targetDir;
    private Camera mainCamera;
    private Inventory inventory;
    private Crosshair crosshair;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        mainCamera = FindObjectOfType<Camera>();
        if (crosshairGameObject != null)
            crosshair = crosshairGameObject.GetComponent<Crosshair>();
    }

    public void Aim(Transform aimTarget)
    {
        TurnTowardsMousePosition(aimTarget);
        if (inventory.CurrentWeapon is GunController)
            AimGunAtTarget(aimTarget);
    }

    void TurnTowardsMousePosition(Transform aimTarget)
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        float rayLength;

        if (aimTarget != null)
            targetDir = aimTarget.position - transform.position;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));

            if (aimTarget != null)
            {
                aimTarget.position = new Vector3(pointToLook.x, aimTarget.position.y, pointToLook.z);
                crosshair.SetLocation(aimTarget.position);
            }
        }
    }

    private void AimGunAtTarget(Transform aimTarget)
    {
        GunController gun;
        gun = inventory.CurrentWeapon as GunController;
        Ray cameraRay = mainCamera.ScreenPointToRay(crosshair.GetLocation());

        Debug.DrawRay(cameraRay.origin, cameraRay.direction * 50000000, Color.red);
        RaycastHit raycastHit = new RaycastHit();

        if (Physics.Raycast(cameraRay, out raycastHit))
        {
            if (raycastHit.collider.CompareTag("Enemy"))
            {
                Vector3 enemyPosition = raycastHit.collider.transform.position;
                float enemyHeight = 1.25f;
                Vector3 aimLocation = new Vector3(enemyPosition.x, enemyPosition.y + enemyHeight, enemyPosition.z);
                if (gun != null)
                {
                    aimTarget.position = new Vector3(aimTarget.position.x, aimLocation.y, aimTarget.position.z);
                    Debug.DrawLine(gun.firePoint.position, aimLocation, Color.red);
                }
            }

            if (gun != null)
                gun.transform.LookAt(aimTarget);

            crosshair.IsOnTarget(raycastHit.collider.CompareTag("Enemy"));
        }
    }
}
