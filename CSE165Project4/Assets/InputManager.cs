using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    //hard set
    public GameObject Player;
    public GameObject World;
    public GameObject Ground;
    public GameObject RightHand;
    public GameObject LeftHand;
    public GameObject Belt;
    public GameObject Headset;

    //accessed by other methods
    public int SizeState = 2;

    //method only
	private LineRenderer Line;
	private bool firstLeftTrigger;
	private bool firstRightTrigger;
    private Vector3 LargeSize = new Vector3(10.0f, 10.0f, 10.0f);
    private Vector3 MediumSize = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 SmallSize = new Vector3(0.1f, 0.1f, 0.1f);

    // Use this for initialization
    void Start () {
		Line = this.GetComponent<LineRenderer> ();

		firstLeftTrigger = true;
		firstRightTrigger = true;
    }



    // Update is called once per frame
    void Update()
    {

        
        //move belt
        Vector3 offset = new Vector3(Headset.transform.position.x, Player.transform.position.y * 0.75f, Headset.transform.position.z);
        Belt.transform.position = offset;

        //Need to make the belt face the way you are facing
        //Belt.transform.right = -Headset.transform.right;

        offset = (Player.transform.localScale.x * 0.2f) * Belt.transform.right;
        Belt.transform.position = Belt.transform.position + offset;
        

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

			} else if (!OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) && !firstRightTrigger) {

				firstRightTrigger = true;
			}


		} else if  (!OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger) && !firstLeftTrigger) {

			ToggleLine (false);
			firstLeftTrigger = true;

		}















        //Handle temporary A Y B button presses

        //A button pressed once
        if (OVRInput.GetDown (OVRInput.Button.One))
        {
            if (SizeState > 1)
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

    public void Teleport()
    {

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(RightHand.transform.position, RightHand.transform.forward, out hit);

        if (hitSomething && hit.collider.gameObject.CompareTag("Floor"))
        {
            float offset = Player.transform.position.y - Ground.transform.position.y;
            Vector3 newPos = new Vector3(hit.point.x, hit.point.y + offset, hit.point.z);
            Player.transform.position = newPos;
        }
    }

    public void IncreaseSize()
    {
        SizeState++;

        if (SizeState == 3)
        {
            Player.transform.localScale = Player.transform.localScale * 10.0f;
            Line.startWidth = Line.startWidth * 10.0f;
            Line.endWidth = Line.endWidth * 10.0f;
        }
        else
        {
            Player.transform.localScale = Player.transform.localScale * 50.0f;
            Line.startWidth = Line.startWidth * 50.0f;
            Line.endWidth = Line.endWidth * 50.0f;
        }

        Vector3 shiftUp = new Vector3(Player.transform.position.x, Player.transform.localScale.y, Player.transform.position.z);
        Player.transform.position = shiftUp;

    }

    public void DecreaseSize()
    {
        SizeState--;

        if (SizeState == 2)
        {
            Player.transform.localScale = Player.transform.localScale / 10.0f;
            Line.startWidth = Line.startWidth / 10.0f;
            Line.endWidth = Line.endWidth / 10.0f;
        }
        else
        {
            Player.transform.localScale = Player.transform.localScale / 50.0f;
            Line.startWidth = Line.startWidth / 50.0f;
            Line.endWidth = Line.endWidth / 50.0f;
        }

        Vector3 shiftDown = new Vector3(Player.transform.position.x, Player.transform.localScale.y, Player.transform.position.z);
        Player.transform.position = shiftDown;


    }




























}
