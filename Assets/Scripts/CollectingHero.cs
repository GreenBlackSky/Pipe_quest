using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectingHero : MonoBehaviour, InteractionListener
{
    public bool[] isFull;
    public GameObject[] slots;

    public GameObject InventoryPanel;

    public void interact(GameObject interactable)
    {
        Collectable collectable = interactable.GetComponent<Collectable>();
        if(collectable is null)
        {
            return;
        }
        for(int i = 0; i < slots.Length; i++)
        {
            if(!isFull[i])
            {
                Instantiate(collectable.icon, slots[i].transform, false);
                isFull[i] = true;
                Destroy(interactable);
                break;
            }
        }
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
