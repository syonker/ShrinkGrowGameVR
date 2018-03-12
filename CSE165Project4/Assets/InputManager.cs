using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    //hard set
    public GameObject Player;
    public GameObject RightHand;
    public GameObject LeftHand;


	private LineRenderer Line;
	private bool firstLeftTrigger;
	private bool firstRightTrigger;

    // Use this for initialization
    void Start () {
		Line = this.GetComponent<LineRenderer> ();

		firstLeftTrigger = true;
		firstRightTrigger = true;
    }

	void FixedUpdate() {

		OVRInput.FixedUpdate ();
	}

    // Update is called once per frame
    void Update()
    {

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

		//Debug.Log ("");

		OVRInput.Update ();



		if (OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger)) {

			if (firstLeftTrigger) {

				firstLeftTrigger = false;
				Debug.Log ("First left trigger");

			} else {

				if (OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger)) {

					if (firstRightTrigger) {
						firstRightTrigger = false;
						Debug.Log ("teleport");

					}



				} else {

					if (!firstRightTrigger) {

						firstRightTrigger = true;
						Debug.Log ("release right");

					}


				}


			}



		} else {

			if (!firstLeftTrigger) {

				firstLeftTrigger = true;
				Debug.Log ("release left");

			}

		}





		/*

		//if left trigger is just pressed
		if (OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger) && firstLeftTrigger) {

			Debug.Log ("First Left");
			ToggleLine (true);
			firstLeftTrigger = false;

			//if left trigger is held
		} else if (OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger)) {

			UpdateLine ();



			//pressed right trigger first time
			if (OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) && firstRightTrigger) {

				Teleport ();
				firstRightTrigger = false;

			} else if (!OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) && !firstRightTrigger) {

				firstRightTrigger = true;
			}
			else if (OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger)) {

				Debug.Log ("Stuck with both on");

			}


		} else if  (!OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger) && !firstLeftTrigger) {

			ToggleLine (false);
			firstLeftTrigger = true;

		}
			

		*/










    }


	public void ToggleLine(bool LineOn) {

		if (LineOn) {

			Line.enabled = true;
			Line.material.color = Color.blue;
			UpdateLine ();

		} else {
			Line.enabled = false;
		}
			
	}

	public void UpdateLine() {

		Line.SetPosition(0, RightHand.transform.position);
		Line.SetPosition(1, RightHand.transform.position + RightHand.transform.forward * 100);

	}

	public void Teleport() {

		Debug.Log ("Teleported");
	}





























}
