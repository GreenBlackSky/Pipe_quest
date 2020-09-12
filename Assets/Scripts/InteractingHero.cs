using System.Collections.Generic;
using UnityEngine;

public class InteractingHero : MonoBehaviour
{
    public GameObject interactionButton;

    private List<InteractionListener> _listeners;
    private GameObject _interactable;

    void Start()
    {
        _listeners = new List<InteractionListener>(GetComponents<InteractionListener>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            _interactable = other.gameObject;
            interactionButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null && other.gameObject == _interactable)
        {
            _interactable = null;
            interactionButton.SetActive(false);
        }
    }

    public void interact()
    {
        if (_interactable != null)
        {
            foreach(InteractionListener listener in _listeners)
            {
                listener.interact(_interactable);
            }
            _interactable.gameObject.GetComponent<Interactable>().interact(gameObject);
            _interactable = null;
            interactionButton.SetActive(false);
        }
    }
}