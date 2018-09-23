using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    static public bool goalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Projectile")
        {
            Goal.goalMet = true;
            //set color to green
            Color c = GetComponent<MeshRenderer>().material.color;
            c.r = 0;
            c.g = 1;
            GetComponent<MeshRenderer>().material.color = c;
        }
    }
}
