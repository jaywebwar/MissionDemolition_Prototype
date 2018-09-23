using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

    static public FollowCam S;//a followcam singleton

    //fields set in inspector
    public float            easing = 0.05f;
    public Vector2          minXY;
    public bool             __________________________________;

    //fields set dynamically
    public GameObject       poi;//point of interest
    public float            camZ;

    private void Awake()
    {
        S = this;
        camZ = this.transform.position.z;
    }
	
	void FixedUpdate () {
        Vector3 destination;
        if (poi == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //Get the position of poi
            destination = poi.transform.position;
            if(poi.tag == "Projectile")
            {
                if (poi.GetComponent<Rigidbody>().IsSleeping() || Input.GetMouseButtonDown(0))
                {
                    poi = null;
                    return;
                }
            }
        }

        //Limit X&Y
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //Interpolate from the current camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        //retain z positioning of camera
        destination.z = camZ;
        //set camera to destination
        transform.position = destination;
        //Set the orthographic size of the camera to keep ground in view
        this.GetComponent<Camera>().orthographicSize = destination.y + 10;
	}
}
