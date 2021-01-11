using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HeroBody3D : MonoBehaviour {
    private Animator animator;
    private bool moving;
 
    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        moving = false;
    }

    // Update is called once per frame
    void Update() {
        float Vx = Input.GetAxis("Horizontal");
        float Vz = Input.GetAxis("Vertical");

        bool wasMoving = moving;
        moving = (Vx != 0 || Vz != 0);

        if(moving) {
            float angle = Mathf.Rad2Deg * Mathf.Atan2(Vx, Vz);
            // TODO remove get component from update
            GetComponent<Transform>().rotation = Quaternion.Euler(0, angle, 0);
        }

        if (!wasMoving && moving) {
            animator.SetTrigger("IdleToRun");
        } else if (wasMoving && !moving) {
            animator.SetTrigger("RunToIdle");
        }
    }
}
