using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnvilMenu : MonoBehaviour
{
    //JA JAG VET ATT JAG KUNDE GJORT DETTA BÄTTRE, HÅLL KÄFTEN
    //KOLLA INTE OM DU INTE GILLAR DET
    //TITTA INTE!!!!!

    public static AnvilMenu Instance { get; private set; }

    [SerializeField] Image ing1;
    [SerializeField] Image ing2;
    [SerializeField] ProductSlot productSlot;

    public int ingredient1;
    public int ingredient2;

    private void Awake()
    {
        Instance = this;

        Close();
    }

    // Update is called once per frame
    void Update()
    {
        productSlot.item = CheckCombine();

        ing1.enabled = ingredient1 >= 0;
        if (ing1.enabled)
        {
            ing1.sprite = PlayerInventory.Instance.Items[ingredient1].coverImage;
        }

        ing2.enabled = ingredient2 >= 0;
        if (ing2.enabled)
        {
            ing2.sprite = PlayerInventory.Instance.Items[ingredient2].coverImage;
        }
    }

    public void OpenMenu()
    {
        ingredient1 = -1;
        ingredient2 = -1;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public ItemBase CheckCombine()
    {
        if (ingredient1 < 0 || ingredient2 < 0)
            return null;

        ItemBase it1 = PlayerInventory.Instance.Items[ingredient1];
        ItemBase it2 = PlayerInventory.Instance.Items[ingredient2];

        for (int i = 0; i < GameManager.Instance.recipes.Length; i++)
        {
            CombineRecipe r = GameManager.Instance.recipes[i];

            if ((r.item1 == it1 && r.item2 == it2) || (r.item2 == it1 && r.item1 == it2))
            {
                return r.product;
            }
        }

        return null;
    }

    public void SelectItem(int index)
    {
        if (ingredient2 == index || ingredient1 >= 0)
        {
            if (ingredient1 == index || ingredient2 >= 0)
                return;

            ingredient2 = index;
            return;
        }

        ingredient1 = index;
    }

    public void Drop1()
    {
        ingredient1 = -1;
    }
    public void Drop2()
    {
        ingredient2 = -1;
    }

    public void TakeItem()
    {
        ItemBase i = CheckCombine();
        if (!i)
            return;

        if (ingredient1 > ingredient2)
        {
            PlayerInventory.Instance.RemoveItem(ingredient1);
            PlayerInventory.Instance.RemoveItem(ingredient2);
        }
        else
        {
            PlayerInventory.Instance.RemoveItem(ingredient2);
            PlayerInventory.Instance.RemoveItem(ingredient1);
        }

        OpenMenu();

        GameManager.Instance.SpawnItem(PlayerMovement.Instance.transform.position, i, PlayerMovement.Instance.CurrentRoom);
    }
}
