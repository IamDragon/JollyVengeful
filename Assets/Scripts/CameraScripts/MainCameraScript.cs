using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    public Transform targetObject;
    public Vector3 cameraOffset;
    public float smoothFactor = 0.5f;

    public bool lookAtTarget = false;

    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    // Start is called before the first frame update
    public static MainCameraScript instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        PlayerController.instance.SetCamera(GetComponent<Camera>());
        targetObject = PlayerController.instance.transform;
        transform.position = new Vector3(PlayerController.instance.transform.position.x, transform.position.y, PlayerController.instance.transform.position.z - 4);
        cameraOffset = new Vector3(3,11,-6);//transform.position - targetObject.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = targetObject.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);


        if (lookAtTarget)
            transform.LookAt(newPosition);

        OnMouseDrag();
    }


    private void OnMouseDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) return;

        Vector3 pos = UnityEngine.Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

        transform.Translate(move, Space.World);
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
