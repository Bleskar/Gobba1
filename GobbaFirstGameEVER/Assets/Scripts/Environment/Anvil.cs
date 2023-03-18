using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        AnvilMenu.Instance.OpenMenu();
    }
}
