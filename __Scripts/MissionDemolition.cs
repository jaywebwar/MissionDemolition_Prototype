using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour {

    static public MissionDemolition S;

    //inspector
    public GameObject[] castles;
    public Text levelText;
    public Text scoreText;
    public Vector3 castlePos;

    public bool ____________________________________________;

    //dynamic fields
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Slingshot";

	// Use this for initialization
	void Start () {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
		
	}

    void StartLevel()
    {
        //destroy existing castle
        if(castle != null)
        {
            Destroy(castle);
        }

        //destroy projectiles
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //instantiate new castle
        castle = Instantiate(castles[level]) as GameObject;
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //reset camera
        SwitchView("Both");
        ProjectileLine.S.Clear();

        //reset goal
        Goal.goalMet = false;

        ShowGT();

        mode = GameMode.playing;
    }

    void ShowGT()
    {
        //show data in text fields
        levelText.text = "Level: " + (level + 1).ToString() + "/" + levelMax.ToString();
        scoreText.text = "Shots Taken: " + shotsTaken.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        ShowGT();//update texts
        
        //check for level end
        if(mode == GameMode.playing && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            //zoom out
            SwitchView("Both");
            //start next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
	}

    void NextLevel()
    {
        level++;
        if(level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    private void OnGUI()
    {
        //Draw GUI button for view switching
        Rect buttonRect = new Rect((Screen.width / 2) - 50, 10, 100, 24);

        switch (showing)
        {
            case "Slingshot":
                if(GUI.Button(buttonRect, "Show Castle"))
                {
                    SwitchView("Castle");
                }
                break;
            case "Castle":
                if(GUI.Button(buttonRect, "Show Both"))
                {
                    SwitchView("Both");
                }
                break;
            case "Both":
                if(GUI.Button(buttonRect, "Show Slingshot"))
                {
                    SwitchView("Slingshot");
                }
                break;
        }

        //draw GUI button for skipping level
        Rect skipButton = new Rect((Screen.width) - 150, 10, 100, 24);

        if(GUI.Button(skipButton, "Skip Level"))
        {
            Goal.goalMet = true;
        }
    }

    //Static allows these methods to be called from the class itself in any other script (e.g. MissionDemolition.SwitchView("Both");)
    //This is a reason why the singleton is helpful
    static public void SwitchView(string view)
    {
        S.showing = view;
        switch (S.showing)
        {
            case "Slingshot":
                FollowCam.S.poi = null;
                break;
            case "Castle":
                FollowCam.S.poi = S.castle;
                break;
            case "Both":
                FollowCam.S.poi = GameObject.Find("ViewBoth");
                break;
        }
    }

    static public void ShotFired()
    {
        S.shotsTaken++;
    }
}
