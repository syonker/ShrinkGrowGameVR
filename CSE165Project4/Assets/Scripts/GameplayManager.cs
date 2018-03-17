using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

    //hard set
    public GameObject Player;
    public GameObject InputManager;
    

    //accessed by other methods
    public int SceneNumber;


    //private
    private Vector3 StartPos2 = new Vector3(-26300.0f, 6.0f, 8720.0f);


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}




    public void OpenScene1()
    {

        SceneNumber = 1;

    }


    public void OpenScene2()
    {

        SceneNumber = 2;

        Player.transform.position = StartPos2;
        Player.transform.localScale = new Vector3(6.0f, 6.0f, 6.0f);
        Player.transform.position = new Vector3(StartPos2.x, StartPos2.y * InputManager.GetComponent<InputManager>().PlayerHeightOffset, StartPos2.z);
        Player.transform.localScale = Player.transform.localScale * InputManager.GetComponent<InputManager>().PlayerHeightOffset;

        InputManager.GetComponent<InputManager>().SizeChange = 10.0f;
        InputManager.GetComponent<InputManager>().IncreaseSizeScene2();

    }


}
