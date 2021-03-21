using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachingHero : MonoBehaviour, InteractionListener {

    public void interact(GameObject interactable) {
        EventManager.Trigger((
            "reach_marker",
            interactable.GetComponent<MarkerController>().id.ToString()
        ));
    }
}
