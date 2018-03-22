using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

    //hard set
    public GameObject Player;
    public GameObject AudioManager;
    public GameObject InputManager;
    public GameObject DirectionalLight;
    public GameObject StackablesParent;
    public GameObject LaserStartZone;
    public GameObject LaserEndZone;
    public GameObject LaserParent;
    public GameObject DoorOpen;
    public GameObject DoorClose;
    public GameObject HandMap;

    public GameObject key1;
    public GameObject key2;
    public GameObject key3;
    public GameObject key4;
    public GameObject key5;
    public GameObject minikey1;
    public GameObject minikey2;
    public GameObject minikey3;
    public GameObject minikey4;
    public GameObject minikey5;


    //accessed by other methods
    public int SceneNumber;


    //private
    private Vector3 StartPos2 = new Vector3(-26300.0f, 6.0f, 8720.0f);
    //private Vector3 StartPos2 = new Vector3(-24748.9f, 6.0f, 9437.74f);
    //private Vector3 StartPos2 = new Vector3(-24748.9f, 6.0f, 11589.68f);

    private Vector3 LaserStartPos = new Vector3(-24748.9f, 6.0f, 9437.74f);


    private Vector3 StartPos3 = new Vector3(-20580.0f, 6.0f, 27537.0f);
    private bool StairsFrozen = false;
    private Material skybox;
    private int KeyCount = 0;
    private float countDown = 60f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //if we are doing to laser trial
        if (LaserParent.activeSelf)
        {
            countDown -= Time.deltaTime;
            if (countDown <= 0f && LaserParent.activeSelf)
            {
                ResetLasers();
            }
        }
		
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

        //DirectionalLight.SetActive(true);
        //DirectionalLight.GetComponent<Light>().shadows = LightShadows.None;

    }

    public void OpenScene3()
    {
        SceneNumber = 3;

        HandMap.SetActive(false);

        Player.transform.position = StartPos3;
        Player.transform.position = new Vector3(StartPos3.x, StartPos3.y * InputManager.GetComponent<InputManager>().PlayerHeightOffset, StartPos3.z);

        InputManager.GetComponent<InputManager>().DecreaseSizeScene3();

        //DirectionalLight.SetActive(false);
        //DirectionalLight.GetComponent<Light>().shadows = LightShadows.None;

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



    public void LaserStart()
    {
        LaserStartZone.SetActive(false);

        skybox = RenderSettings.skybox;
        RenderSettings.skybox = null;

        LaserParent.SetActive(true);
        AudioManager.GetComponent<AudioManagement>().Play("LaserOn");
        AudioManager.GetComponent<AudioManagement>().Play("Countdown");
    }

    public void LaserEnd()
    {

        Debug.Log("Laser End");
        AudioManager.GetComponent<AudioManagement>().Stop("Countdown");

        Destroy(LaserEndZone);

        RenderSettings.skybox = skybox;

        LaserParent.SetActive(false);
        AudioManager.GetComponent<AudioManagement>().Play("LaserOn");

        DirectionalLight.SetActive(true);
        DirectionalLight.GetComponent<Light>().shadows = LightShadows.None;
    }

    public void ResetLasers()
    {
        AudioManager.GetComponent<AudioManagement>().Stop("Countdown");

        RenderSettings.skybox = skybox;
        LaserParent.SetActive(false);
        countDown = 60f;

        LaserStartZone.SetActive(true);

        InputManager.GetComponent<InputManager>().SizeState = 3;

        Player.transform.position = LaserStartPos;
        Player.transform.localScale = new Vector3(60.0f, 60.0f, 60.0f);
        Player.transform.localScale = Player.transform.localScale * InputManager.GetComponent<InputManager>().PlayerHeightOffset;
        Player.transform.position = new Vector3(LaserStartPos.x, Player.transform.localScale.y, LaserStartPos.z);
        
    }

    public void KeyPickup(GameObject Key)
    {
        if (Key == key1)
        {
            Destroy(minikey1);
        }
        else if (Key == key2)
        {
            Destroy(minikey2);
        }
        else if (Key == key3)
        {
            Destroy(minikey3);
        }
        else if (Key == key4)
        {
            Destroy(minikey4);
        }
        else if (Key == key5)
        {
            Destroy(minikey5);
        }
        Destroy(Key);
        KeyCount++;
        if (KeyCount == 5)
        {
            DoorClose.SetActive(false);
            DoorOpen.SetActive(true);
        }
    }






}
