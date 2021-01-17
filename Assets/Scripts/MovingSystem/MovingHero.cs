using System;
using UnityEngine;


public class MovingHero : MonoBehaviour {
    private Animator animator;
    private Rigidbody body;

    public float speed;
    private bool movingX;
    private bool movingZ;

    void Start() {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        movingX = false;
        movingZ = false;
    }

    void Update() {
        float Vx = Input.GetAxis("Horizontal");
        float Vz = Input.GetAxis("Vertical");

        bool movingXnew = (Math.Abs(Vx) > 0.5);
        bool movingZnew = (Math.Abs(Vz) > 0.5);

        // start moving
        if(!movingX && !movingZ) {
            if(movingXnew && !movingZnew) {
                movingX = true;
            } else if (!movingXnew && movingZnew) {
                movingZ = true;
            }
        // stop moving
        } else if(!movingXnew && !movingZnew) {
            movingX = false;
            movingZ = false;
        //change direction
        } else if ((movingX != movingXnew) && (movingZ != movingZnew)) {
            movingX = !movingX;
            movingZ = !movingZ;
        }

        if(movingX) {
            body.velocity = new Vector3(speed*Vx, body.velocity.y, 0);
        } else if(movingZ) {
            body.velocity = new Vector3(0, body.velocity.y, speed*Vz);
        }

        // if (!wasMoving && moving) {
        //     animator.SetTrigger("IdleToRun");
        // } else if (wasMoving && !moving) {
        //     animator.SetTrigger("RunToIdle");
        // }
    }
}
