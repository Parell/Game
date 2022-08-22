using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCamera;
    public bool useStaticBillboard;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (!useStaticBillboard)
        {
            transform.LookAt(mainCamera.transform);
        }
        else
        {
            transform.rotation = mainCamera.transform.rotation;
        }

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
