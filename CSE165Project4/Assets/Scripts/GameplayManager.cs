using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

    //hard set
    public GameObject Player;
    public GameObject InputManager;
    public GameObject DirectionalLight;
    public GameObject StackablesParent;
   
    

    //accessed by other methods
    public int SceneNumber;


    //private
    private Vector3 StartPos2 = new Vector3(-26300.0f, 6.0f, 8720.0f);
    private Vector3 StartPos3 = new Vector3(-20580.0f, 6.0f, 27537.0f);
    private bool StairsFrozen = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (SceneNumber == 2)
        {
            if (InputManager.GetComponent<InputManager>().SizeState == 2)
            {
                if (!StairsFrozen)
                {
                    FreezeStairs(true);
                }
            }
            else
            {
                if (StairsFrozen)
                {
                    FreezeStairs(false);
                }
            }
        }

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

        DirectionalLight.SetActive(false);

    }

    public void OpenScene3()
    {
        SceneNumber = 3;

        Player.transform.position = StartPos3;
        Player.transform.position = new Vector3(StartPos3.x, StartPos3.y * InputManager.GetComponent<InputManager>().PlayerHeightOffset, StartPos3.z);

        InputManager.GetComponent<InputManager>().DecreaseSize();

        DirectionalLight.SetActive(true);

        //Testing symbolic input
         /*Player.transform.position = new Vector3(InputManager.GetComponent<InputManager>().delete_key.transform.position.x, 
             InputManager.GetComponent<InputManager>().delete_key.transform.position.y + Player.transform.localScale.y, 
             InputManager.GetComponent<InputManager>().delete_key.transform.position.z);*/
    }


    public void FreezeStairs(bool freeze)
    {
        if (freeze)
        {
            StairsFrozen = true;
            Rigidbody[] children = StackablesParent.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody comp in children)
            {
                comp.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        else
        {
            StairsFrozen = false;
            Rigidbody[] children = StackablesParent.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody comp in children)
            {
                comp.constraints = RigidbodyConstraints.None;
            }

        }
    }


}
