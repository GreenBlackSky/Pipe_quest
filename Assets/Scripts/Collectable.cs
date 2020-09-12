using UnityEngine;

public class Collectable : MonoBehaviour, Interactable
{
    public GameObject icon;

    public void interact(GameObject hero)
    {
        Destroy(gameObject);
    }
}
