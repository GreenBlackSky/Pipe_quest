using UnityEngine;

public class Collectable : MonoBehaviour, Interactable
{
    public void Interact(GameObject hero)
    {
        Destroy(gameObject);
    }
}
