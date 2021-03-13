using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachingHero : MonoBehaviour, InteractionListener {

    public void interact(GameObject interactable) {
        new ReachMarkerTrigger().Trigger(
            interactable.GetComponent<MarkerController>().id.ToString()
        );
    }
}
