using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;


public class MovingHero : MonoBehaviour
{
    private readonly float speed = 10;

    void Update()
    {
        float Vx = Input.GetAxis("Horizontal") * speed;
        float Vz = Input.GetAxis("Vertical") * speed;
        GetComponent<Rigidbody>().velocity = new Vector3(Vx, GetComponent<Rigidbody>().velocity.y, Vz);
    }
}
