using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class CollectingHero : MonoBehaviour, InteractionListener
{
    public bool[] isFull;
    Transform[] slots;
    public Dictionary<string, int> items;

    public GameObject InventoryPanel;

    void Start() {
        items = new Dictionary<string, int>();
        int shildCount = InventoryPanel.transform.childCount;
        // TODO generate slots in stead of searching for them
        slots = new Transform[shildCount];
        for(int i = 0; i < shildCount; i++) {
            slots[i] = InventoryPanel.transform.Find("Slot" + i.ToString());
        }
    }

    public void interact(GameObject interactable)
    {
        Collectable collectable = interactable.GetComponent<Collectable>();
        if(collectable is null) {
            return;
        }
        for(int i = 0; i < slots.Length; i++) {
            if(!isFull[i]) {
                if(!items.ContainsKey(collectable.item_uid)) {
                    items[collectable.item_uid] = 0;
                }
                items[collectable.item_uid] += 1;


                Transform icon = interactable.transform.Find("Image");
                Instantiate(icon, slots[i], false);
                isFull[i] = true;

                Destroy(interactable);
                // TODO trigger event
                break;
            }
        }
    }
}
