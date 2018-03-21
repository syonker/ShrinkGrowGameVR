﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

    //hard set
    public GameObject AudioManager;
    public GameObject Player;
    public GameObject Ground;
    public GameObject RightHand;
    public GameObject LeftHand;
    public GameObject Belt;
    public GameObject Headset;
    public GameObject PlayerController;
    public GameObject Shrimp;
    public GameObject Watermelon;
    public GameObject Door;
    public GameObject ForceField;
    public GameObject GameplayManager;
    public GameObject PasswordSymbol1;
    public Canvas PasswordSymbol2;
    public Canvas PasswordSymbol3;
    public Canvas PasswordSymbol4;
    public GameObject delete_key;
    public GameObject Hole1;
    public GameObject Hole2;
    public GameObject Hole3;
    public GameObject Hole4;
    public GameObject Hole5;

    //accessed by other methods
    public int SizeState = 2;
    public bool ShrimpGrabbed = false;
    public bool WatermelonGrabbed = false;
    public bool HandInWall = false;
    public float SizeChange = 6.0f;
    public float PlayerHeightOffset = 0.5f;
    private GameObject LastHit = null;



    //method only
    private LineRenderer Line;
	private bool firstLeftTrigger;
	private bool firstRightTrigger;
    private Vector3 OGShrimpPos;
    private Quaternion OGShrimpRot;
    private Vector3 OGWatermelonPos;
    private Quaternion OGWatermelonRot;
    private bool FirstDoor = true;
    private Vector3 Hole1Pos;
    private Vector3 Hole2Pos;
    private Vector3 Hole2Dir;
    private Vector3 Hole2Forward;
    private int PasswordIndex = 1;
    private int[] Password;
    private GameObject lastFloor;
    


    // Use this for initialization
    void Start () {

        Player.transform.localScale *= PlayerHeightOffset;
        Vector3 pos = new Vector3(Player.transform.position.x, Player.transform.localScale.y, Player.transform.position.z);
        Player.transform.position = pos;

        PlayerController.GetComponent<CharacterController>().radius /= Player.transform.localScale.y;

        Line = this.GetComponent<LineRenderer> ();

		firstLeftTrigger = true;
		firstRightTrigger = true;

        OGShrimpPos = Shrimp.transform.localPosition;
        OGShrimpRot = Shrimp.transform.localRotation;
        OGWatermelonPos = Watermelon.transform.localPosition;
        OGWatermelonRot = Watermelon.transform.localRotation;

        MoveBelt();

        Hole1Pos = new Vector3(-20682.0f, 0.0f, 27502.0f);
        Hole2Pos = new Vector3(-20468.0f, 78.0f, 27701.0f);
        Hole2Dir = new Vector3(-20469.0f, 78.0f, 27688.0f);
        Hole2Forward = -Hole2Pos + Hole2Dir;

        //initialize password array
        Password = new int[4];
        for (int i = 0; i < 4; i++)
        {
            Password[i] = 0;
        }

        lastFloor = Ground;


    }


    // Update is called once per frame
    void Update()
    {

        //Teleport between holes Scene 3
        /*if(GameplayManager.GetComponent<GameplayManager>().SceneNumber == 3)
        {
            float dist = (Player.transform.position - Hole1Pos).magnitude;

            if(dist < 8.0f)
            {
                //move to hole 2
                Player.transform.position = new Vector3(Hole2Pos.x, Hole2Pos.y + Player.transform.localScale.y, Hole2Pos.z);
                Player.transform.forward = Hole2Forward;
            }
        }*/




    

        if (!ForceField.activeSelf)
        {
            DoorOpen();
        }


        if (!ShrimpGrabbed && !WatermelonGrabbed)
        {
            MoveBelt();
        }


        if (ShrimpGrabbed)
        {
            Eat(Shrimp);
        }

        if (WatermelonGrabbed)
        {
            Eat(Watermelon);
        }


        /*
        //movement with left stick
        Vector2 leftStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        float vertical = leftStick.y;
        float horizontal = leftStick.x;
        Player.transform.position = Player.transform.position + Player.transform.forward * (vertical / 50) + Player.transform.right * (horizontal / 50);

        //move view with right stick
        Vector2 rightStick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        vertical = rightStick.y;
        horizontal = rightStick.x;
        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            Player.transform.RotateAround(Player.transform.position, Player.transform.up, horizontal);
        }
		*/


        //if left trigger is just pressed
        if (OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger) && firstLeftTrigger) {

			ToggleLine (true);
			firstLeftTrigger = false;

		//if left trigger is held
		} else if (OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger)) {

			UpdateLine ();

			//pressed right trigger first time
			if (OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) && firstRightTrigger) {

				Teleport ();
				firstRightTrigger = false;

            //right trigger is held
			} else if (!OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) && !firstRightTrigger) {

				firstRightTrigger = true;
			}

        //left trigger released
		} else if  (!OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger) && !firstLeftTrigger) {

			ToggleLine (false);
			firstLeftTrigger = true;

		}















        //Handle temporary A Y B button presses

        //A button pressed once
        if (OVRInput.GetDown (OVRInput.Button.One))
        {
            if (SizeState > 2)
            {
                DecreaseSize();
            }
        }

        //B button pressed once
        else if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            if (SizeState < 3)
            {
                IncreaseSize();
            }
        }

        //X button pressed once
        else if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (SizeState == 2)
            {
                GameplayManager.GetComponent<GameplayManager>().OpenScene2();
            }
        }

        //Y button pressed once
        else if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            
                GameplayManager.GetComponent<GameplayManager>().OpenScene3();
            
        }












    }

    //Turn on and off the Teleport Line
	public void ToggleLine(bool LineOn) {

		if (LineOn) {

			Line.enabled = true;
			Line.material.color = Color.blue;
			UpdateLine ();

		} else {
			Line.enabled = false;
		}
			
	}

    //Update the Teleport line
	public void UpdateLine() {

		Line.SetPosition(0, RightHand.transform.position);
		Line.SetPosition(1, RightHand.transform.position + RightHand.transform.forward * 100);

	}

    //Teleport
    public void Teleport()
    {

        if (HandInWall)
        {
            return;
        }

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(RightHand.transform.position, RightHand.transform.forward, out hit);

        //reset position if necessary
        if (LastHit && GameplayManager.GetComponent<GameplayManager>().SceneNumber == 3)
        {
            if (LastHit.CompareTag("Stackable") && hit.collider.gameObject.CompareTag("Floor"))
            {
                Player.transform.position = new Vector3(Hole2.transform.position.x, Hole2.transform.position.y + Player.transform.localScale.y + 3, Hole2.transform.position.z);
                Player.transform.forward = Hole2.transform.right;
                LastHit = hit.collider.gameObject;
                return;
            }
        }

        //Debug.Log("HIt: " + hit.collider.gameObject.name);
        if (hitSomething && hit.collider.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("Hit Floor");

            float offset = Player.transform.localScale.y;
            Vector3 newPos = new Vector3(hit.point.x, hit.point.y + offset, hit.point.z);
            Player.transform.position = newPos;
            lastFloor = hit.collider.gameObject;
        }
        else if (hitSomething && hit.collider.gameObject.CompareTag("Scene1Exit") && SizeState == 2)
        {
            GameplayManager.GetComponent<GameplayManager>().OpenScene2();
        }
        else if (hitSomething && hit.collider.gameObject.CompareTag("Stackable") && SizeState == 2 )
        {
            float offset = Player.transform.localScale.y;
            Vector3 newPos = new Vector3(hit.point.x, hit.point.y + offset, hit.point.z);
            Player.transform.position = newPos;
            lastFloor = hit.collider.gameObject;
        }
        else if (hitSomething && hit.collider.gameObject.CompareTag("LaserStart"))
        {
            float offset = Player.transform.localScale.y;
            Vector3 newPos = new Vector3(-24749.0f, offset, 9567.4f);
            Player.transform.position = newPos;
            lastFloor = hit.collider.gameObject;

            GameplayManager.GetComponent<GameplayManager>().LaserStart();
        }
        else if (hitSomething && hit.collider.gameObject.CompareTag("LaserEnd"))
        {
            float offset = Player.transform.localScale.y;
            Vector3 newPos = new Vector3(-24757.5f, offset, 11286.9f);
            Player.transform.position = newPos;
            lastFloor = hit.collider.gameObject;

            GameplayManager.GetComponent<GameplayManager>().LaserEnd();
        }
        else if (hitSomething && hit.collider.gameObject.CompareTag("Scene2Exit"))
        {
            GameplayManager.GetComponent<GameplayManager>().OpenScene3();
        }

        //scene 3 only 
        //else if (GameplayManager.GetComponent<GameplayManager>().SceneNumber == 3) { 

        else if (hitSomething && hit.collider.gameObject.CompareTag("Hole1"))
        {

            Debug.Log("Hole1");
            //move to hole 2
            Player.transform.position = new Vector3(Hole2.transform.position.x, Hole2.transform.position.y + Player.transform.localScale.y, Hole2.transform.position.z - 20.0f);
            Player.transform.forward = Hole2.transform.right;
        }
        else if (hitSomething && hit.collider.gameObject.CompareTag("Hole2"))
        {
            //move to hole 1
            Player.transform.position = new Vector3(Hole1.transform.position.x + 20.0f, Hole1.transform.position.y + Player.transform.localScale.y, Hole1.transform.position.z);
            Player.transform.forward = Hole1.transform.right;
        }
        else if (hitSomething && hit.collider.gameObject.CompareTag("Hole3"))
        {
            //move to hole 5
            Player.transform.position = new Vector3(Hole5.transform.position.x, Hole5.transform.position.y + Player.transform.localScale.y, Hole5.transform.position.z + 20.0f);
            Player.transform.forward = Hole5.transform.forward;
        }
        else if (hitSomething && hit.collider.gameObject.CompareTag("Hole4"))
        {
            //move to hole 5
            Player.transform.position = new Vector3(Hole5.transform.position.x, Hole5.transform.position.y + Player.transform.localScale.y, Hole5.transform.position.z + 20.0f);
            Player.transform.forward = -Hole5.transform.right;
        }
        else if (hitSomething && hit.collider.gameObject.CompareTag("Hole5"))
        {
            //move to hole 3
            Player.transform.position = new Vector3(Hole3.transform.position.x - 20.0f, Hole3.transform.position.y + Player.transform.localScale.y, Hole3.transform.position.z);
            Player.transform.forward = -Hole3.transform.right;
        }
        //check for symbolic input
        else if (hitSomething)
        {
            CheckForSymbolicInput(hit.collider.gameObject);
        }
        // }



        LastHit = hit.collider.gameObject;
    }


    public void MoveBelt()
    {

        if (Headset.transform.localPosition.y < -0.25f) {
            Belt.SetActive(false);
            return;
        }

        if (!Belt.activeSelf)
        {
            Belt.SetActive(true);
        }

        Vector3 offset = new Vector3(Headset.transform.position.x, Player.transform.localScale.y, Headset.transform.position.z);

        Belt.transform.position = offset;
        offset = (Player.transform.localScale.x * 0.2f) * Belt.transform.right;
        Belt.transform.position = Belt.transform.position + offset;

    }

    public void ResetShrimp()
    {
        Shrimp.transform.localPosition = OGShrimpPos;
        Shrimp.transform.localRotation = OGShrimpRot;
    }

    public void ResetWatermelon()
    {
        Watermelon.transform.localPosition = OGWatermelonPos;
        Watermelon.transform.localRotation = OGWatermelonRot;
    }

    public void Eat(GameObject food)
    {
        float dist = (food.transform.position - Headset.transform.position).magnitude;
        if (dist < (Player.transform.localScale.y / 5.0f))
        {
            if (food.CompareTag("Shrimp"))
            {
                AudioManager.GetComponent<AudioManagement>().Play("Eat");
                DecreaseSize();
                ResetShrimp();

            } else if (food.CompareTag("Watermelon"))
            {
                AudioManager.GetComponent<AudioManagement>().Play("Eat");
                IncreaseSize();
                ResetWatermelon();
            } else
            {
                return;
            }

            //force drop the items held
            RightHand.GetComponent<OVRGrabber>().ForceRelease(RightHand.GetComponent<OVRGrabber>().grabbedObject);
            LeftHand.GetComponent<OVRGrabber>().ForceRelease(LeftHand.GetComponent<OVRGrabber>().grabbedObject);

        }


    }

    //Increase the players size
    public void IncreaseSize()
    {
        if (SizeState == 3)
        {
            return;
        }

        if (lastFloor.CompareTag("Stackable"))
        {
            return;
        }

        AudioManager.GetComponent<AudioManagement>().Play("Grow");

        SizeState++;

        Player.transform.localScale = Player.transform.localScale * SizeChange;
        Line.startWidth = Line.startWidth * SizeChange;
        Line.endWidth = Line.endWidth * SizeChange;

        Vector3 shiftUp = new Vector3(Player.transform.position.x, Player.transform.localScale.y, Player.transform.position.z);
        Player.transform.position = shiftUp;
        PlayerController.GetComponent<CharacterController>().radius /= Player.transform.localScale.y;

    }

    //Decrease the players size
    public void DecreaseSize()
    {
        if (SizeState == 2)
        {
            return;
        }

        SizeState--;

        AudioManager.GetComponent<AudioManagement>().Play("Shrink");

        Player.transform.localScale = Player.transform.localScale / SizeChange;
        Line.startWidth = Line.startWidth / SizeChange;
        Line.endWidth = Line.endWidth / SizeChange;

        Vector3 shiftDown = new Vector3(Player.transform.position.x, Player.transform.localScale.y, Player.transform.position.z);
        Player.transform.position = shiftDown;
        PlayerController.GetComponent<CharacterController>().radius *= Player.transform.localScale.y;

    }


    public void IncreaseSizeScene2()
    {
        if (SizeState == 3)
        {
            return;
        }

        SizeState++;

        Player.transform.localScale = Player.transform.localScale * SizeChange;

        Line.startWidth = Line.startWidth / (6.0f*PlayerHeightOffset);
        Line.endWidth = Line.endWidth / (6.0f* PlayerHeightOffset);
        Line.startWidth = Line.startWidth * SizeChange;
        Line.endWidth = Line.endWidth * SizeChange;

        Vector3 shiftUp = new Vector3(Player.transform.position.x, Player.transform.localScale.y, Player.transform.position.z);
        Player.transform.position = shiftUp;
        PlayerController.GetComponent<CharacterController>().radius /= Player.transform.localScale.y;

    }


    public void DoorOpen()
    {

        if (SizeState == 2 && FirstDoor)
        {
            float dist = (Player.transform.position - Door.transform.position).magnitude;
            if (dist < 40.0f)
            {
                Door.GetComponent<Animation>().Play();
                Door.GetComponent<AudioSource>().Play();
                FirstDoor = false;
            }
        }

    }


    public void EnterKey(int key)
    {
        //delete key
        if(key == 10)
        {
            if(PasswordIndex != 1)
            {
                //delete the symbol in the previous text
                if (PasswordIndex == 2)
                {
                    Password[0] = 0;
                    PasswordSymbol1.GetComponentInChildren<Text>().text = "";
                    PasswordIndex--;
                }
                else if (PasswordIndex == 3)
                {
                    Password[1] = 0;
                    PasswordSymbol2.GetComponentInChildren<Text>().text = "";
                    PasswordIndex--;
                }
                else if (PasswordIndex == 4)
                {
                    Password[2] = 0;
                    PasswordSymbol3.GetComponentInChildren<Text>().text = "";
                    PasswordIndex--;
                }
                else if (PasswordIndex == 5)
                {
                    Password[3] = 0;
                    PasswordSymbol4.GetComponentInChildren<Text>().text = "";
                    PasswordIndex--;
                }


            }
            return;

        }
        //enter key
        else if (key == 11)
        {
            if (PasswordIndex == 5)
            {
                //check if password is correct:
                if (Password[0] == 1 && Password[1] == 2 && Password[2] == 3 && Password[3] == 4)
                {
                    Debug.Log("You Win");
                }
                else
                {
                    //clear all texts
                    for (int i = 0; i < 4; i++)
                    {
                        Password[i] = 0;
                    }

                    PasswordIndex = 1;

                    PasswordSymbol1.GetComponentInChildren<Text>().text = "";
                    PasswordSymbol2.GetComponentInChildren<Text>().text = "";
                    PasswordSymbol3.GetComponentInChildren<Text>().text = "";
                    PasswordSymbol4.GetComponentInChildren<Text>().text = "";
                }
            }
            else
            {
                //clear all texts
                for (int i = 0; i < 4; i++)
                {
                    Password[i] = 0;
                }

                PasswordIndex = 1;

                PasswordSymbol1.GetComponentInChildren<Text>().text = "";
                PasswordSymbol2.GetComponentInChildren<Text>().text = "";
                PasswordSymbol3.GetComponentInChildren<Text>().text = "";
                PasswordSymbol4.GetComponentInChildren<Text>().text = "";

            }
            return;
        }
        if (PasswordIndex < 5)
        { 
            //set the current text to be the one entered 
            if (PasswordIndex == 1)
            {
                Password[0] = key;
                PasswordSymbol1.GetComponentInChildren<Text>().text = key.ToString();
                PasswordIndex++;
            }
            else if (PasswordIndex == 2)
            {
                Password[1] = key;
                PasswordSymbol2.GetComponentInChildren<Text>().text = key.ToString();
                PasswordIndex++;
            }
            else if (PasswordIndex == 3)
            {
                Password[2] = key;
                PasswordSymbol3.GetComponentInChildren<Text>().text = key.ToString();
                PasswordIndex++;
            }
            else if (PasswordIndex == 4)
            {
                Password[3] = key;
                PasswordSymbol4.GetComponentInChildren<Text>().text = key.ToString();
                PasswordIndex++;
            }
        }
    }

    public void CheckForSymbolicInput(GameObject obj)
    {
        if (obj.CompareTag("1"))
        {
            EnterKey(1);
        }
        else if (obj.CompareTag("2"))
        {
            EnterKey(2);
        }
        else if (obj.CompareTag("3"))
        {
            EnterKey(3);
        }
        else if (obj.CompareTag("4"))
        {
            EnterKey(4);
        }
        else if (obj.CompareTag("5"))
        {
            EnterKey(5);
        }
        else if (obj.CompareTag("6"))
        {
            EnterKey(6);
        }
        else if (obj.CompareTag("7"))
        {
            EnterKey(7);
        }
        else if (obj.CompareTag("8"))
        {
            EnterKey(8);
        }
        else if (obj.CompareTag("9"))
        {
            EnterKey(9);
        }
        else if (obj.CompareTag("0"))
        {
            EnterKey(0);
        }
        else if (obj.CompareTag("delete_key"))
        {
            EnterKey(10);
        }
        else if (obj.CompareTag("enter_key"))
        {
            EnterKey(11);
        }
    }


   //computer screens

    //Home Screen
    public void OpenScreen2()
    {
        //close screen1

        //change background material
        //set buttons and text off

        //open screen2

        //enable buttons and text
    }

    //Netlfix Error Screen
    public void OpenScreen3()
    {
        //close screen 2

        //disable buttons and text

        //open screen 3

        //change background
    }

    //choose an application
    public void OpenScreen4()
    {
        //close screen 2

        //disable buttons and text

        //open screen 4

        //enable buttons and text
    }

    //schulze bio
    public void OpenScreen5()
    {
        //close screen 4

        //disable buttons and text

        //open screen 5

        //enable buttons and text
    }

    //sorry cant edit applications
    public void OpenScreen6()
    {
        //close screen 4

        //disable buttons and text

        //open screen 6

        //change background
    }

    //game over - you failed
    public void OpenScreen7()
    {
        //close screen 5

        //disable buttons and text

        //open screen 7

        //enable text 
    }

    //congrats you win
    public void OpenScreen8()
    {
        //close screen 5

        //dissable buttons and text

        //open screen 8

        //enable text
    }
















}
