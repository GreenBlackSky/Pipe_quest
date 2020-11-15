using UnityEngine;
using UnityEngine.UIElements;


public class CollectingHero : MonoBehaviour, InteractionListener
{
    public bool[] isFull;
    Transform[] slots;

    public GameObject InventoryPanel;

    void Start() {
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
                Transform icon = interactable.transform.Find("Image");
                Instantiate(icon, slots[i], false);
                isFull[i] = true;
                Destroy(interactable);
                // TODO pass info about collected item
                // EventManager.TriggerEvent("Collect");
                break;
            }
        }
    }
}
