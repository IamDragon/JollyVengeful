using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] Image targetCrosshair;
    private Camera cam;
    private bool onTarget;
    private Image thisImage;
    void Start()
    {
        cam = MainCameraScript.instance.GetComponent<Camera>();
        targetCrosshair.gameObject.SetActive(false);
        thisImage = GetComponent<Image>();
    }
    
    void Update()
    {
        SetCursorVisability();
    }

    private void SetCursorVisability()
    {
        if (!Cursor.visible)
        {
            if (onTarget && !targetCrosshair.gameObject.activeInHierarchy)
                targetCrosshair.gameObject.SetActive(true);

            else if (!onTarget && targetCrosshair.gameObject.activeInHierarchy)
                targetCrosshair.gameObject.SetActive(false);

            if (!thisImage.enabled)
                thisImage.enabled = true;
        }

        else
        {
            if (thisImage.enabled)
                thisImage.enabled = false;
        }
    }

    public void SetLocation(Vector3 pos)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(pos);
        transform.parent.transform.position = new Vector3(screenPos.x, screenPos.y, transform.parent.transform.position.z);
    }
    public Vector3 GetLocation()
    {
        return transform.position;
    }

    public void IsOnTarget(bool onTarget)
    {
        if (onTarget != this.onTarget)
            this.onTarget = onTarget;
    }
}
