using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICursor : MonoBehaviour
{
    [SerializeField] Texture2D cursorTexture;
    [SerializeField] CursorMode cursorMode;
    [SerializeField] Vector2 hotSpot;

    void Awake()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void Update()
    {
        IsMouseOverUI();
    }

    void IsMouseOverUI()
    {
        if (IsMouseOverIgnores())
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else if (!IsMouseOverIgnores())
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    bool IsMouseOverIgnores()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);

        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if (raycastResultList[i].gameObject.GetComponent<UIClickthrough>() != null)
            {
                raycastResultList.RemoveAt(i);
                i--;
            }
        }
        return raycastResultList.Count > 0;
    }
}
