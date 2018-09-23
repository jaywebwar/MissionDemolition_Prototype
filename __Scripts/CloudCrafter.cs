using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour {

    //inspector
    public int                  numClouds = 40;
    public GameObject[]         cloudPrefabs;
    public Vector3              cloudPosMin;
    public Vector3              cloudPosMax;
    public float                cloudScaleMin = 1;
    public float                cloudScaleMax = 5;
    public float                cloudSpeedMult = 0.5f;

    public bool             _______________________________________;

    //dynamic fields
    public GameObject[]         cloudInstances;

    private void Awake()
    {
        //make an array to hold cloud instances
        cloudInstances = new GameObject[numClouds];
        //find cloud anchor
        GameObject anchor = GameObject.Find("CloudAnchor");
        //make clouds
        GameObject cloud;
        for(int i=0; i<numClouds; i++)
        {
            //instantiate random cloud
            int prefabNum = Random.Range(0, cloudPrefabs.Length);
            cloud = Instantiate(cloudPrefabs[prefabNum]) as GameObject;
            //position the cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //scale cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //smaller cloud are closer to the ground and further away
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            cPos.z = 100 - 90 * scaleU;
            //apply transforms to cloud
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //make cloud a child of the anchor
            cloud.transform.parent = anchor.transform;
            //add cloud to cloud instances
            cloudInstances[i] = cloud;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach(GameObject cloud in cloudInstances)
        {
            //get scale and pos
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //move larger clouds faster
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //if cloud moves too far out of scene, move it to the other side of the scene
            if(cPos.x <= cloudPosMin.x)
            {
                cPos.x = cloudPosMax.x;
            }
            //apply new position to cloud
            cloud.transform.position = cPos;
        }
	}
}
