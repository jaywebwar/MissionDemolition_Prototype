using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {

    //fields set in inspector
    public GameObject       prefabProjectile;
    public float            velocityMult = 4f;
    public bool             ____________________________________;//separator for inspector
    //fields set dynamically
    public GameObject       launchPoint;
    public Vector3          launchPos;
    public GameObject       projectile;
    public bool             aimingMode;


    private void Awake()
    {
        Transform launchpointTrans = transform.Find("LaunchPoint");
        launchPoint = launchpointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchpointTrans.position;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Has to be in aiming mode
        if (!aimingMode)
            return;

        //get mouse pos in 2D coordinates
        Vector3 mousePos2D = Input.mousePosition;
        //convert to 3D world coordinates
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        //find delta from mousePos to launchpos
        Vector3 mouseDelta = mousePos3D - launchPos;
        //limit mousedelta to radius of slingshot sphere collider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //move projectile to mouse
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        //fire projectile
        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            projectile.GetComponent<Rigidbody>().isKinematic = false;
            projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;
            projectile = null;
        }
	}

    private void OnMouseEnter()
    {
        launchPoint.SetActive(true);
        //print("Slingshot: OnMouseEnter");
    }

    private void OnMouseExit()
    {
        launchPoint.SetActive(false);
        //print("Slingshot: OnMouseExit");
    }

    private void OnMouseDown()
    {
        //player has pressed mouse while over slingshot
        aimingMode = true;
        //instantiate projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        //start it at launchpoint
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
}
