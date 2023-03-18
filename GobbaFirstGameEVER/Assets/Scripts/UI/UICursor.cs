using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{
    [SerializeField] RectTransform rt;
    [SerializeField] Image cursorImage;
    [SerializeField] Sprite normal;
    [SerializeField] Sprite interact;
    [SerializeField] float interactDistance = 4.5f;

    private void Update()
    {
        Cursor.visible = false;
        rt.position = Input.mousePosition;

        if (PlayerMovement.Instance.Frozen)
        {
            cursorImage.sprite = normal;
            return;
        }

        IInteractable ii = CheckInteract();
        cursorImage.sprite = ii == null ? normal : interact;

        if (ii != null && Input.GetMouseButtonDown(1))
            ii.Interact();
    }

    IInteractable CheckInteract()
    {
        Collider2D[] cda = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        for (int i = 0; i < cda.Length; i++)
        {
            IInteractable ii = cda[i].GetComponent<IInteractable>();
            if (ii != null && Vector2.Distance(cda[i].transform.position, PlayerMovement.Instance.transform.position) < interactDistance) return ii;
        }
        return null;
    }
}
