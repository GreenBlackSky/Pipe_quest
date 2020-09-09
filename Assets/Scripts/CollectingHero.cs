using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectingHero : MonoBehaviour, InteractionListener
{
    List<string> _inventory;

    public GameObject InventoryPanel;

    public void interact(GameObject interactable)
    {
        _inventory.Add(interactable.name);
    }

    void Start()
    {
        _inventory = new List<string>();
    }

    void Update()
    {
        
    }
}
