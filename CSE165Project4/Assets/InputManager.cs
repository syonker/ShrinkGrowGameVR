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
    public GameObject PlayerController;
    public GameObject Shrimp;
    public GameObject Watermelon;

    //accessed by other methods
    public int SizeState = 2;
    public bool ShrimpGrabbed = false;
    public bool WatermelonGrabbed = false;

    //method only
    private LineRenderer Line;
	private bool firstLeftTrigger;
	private bool firstRightTrigger;
    private float SmallToMedium = 50.0f;
    private float MediumToLarge = 8.0f;
    private float PlayerHeightOffset = 0.5f;
    private Vector3 OGShrimpPos;
    private Quaternion OGShrimpRot;
    private Vector3 OGWatermelonPos;
    private Quaternion OGWatermelonRot;

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
    }



    // Update is called once per frame
    void Update()
    {


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

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(RightHand.transform.position, RightHand.transform.forward, out hit);

        if (hitSomething && hit.collider.gameObject.CompareTag("Floor"))
        {
            float offset = Player.transform.position.y - Ground.transform.position.y;
            Vector3 newPos = new Vector3(hit.point.x, hit.point.y + offset, hit.point.z);
            Player.transform.position = newPos;
        }
    }


    public void MoveBelt()
    {
        Vector3 offset = new Vector3(Headset.transform.position.x, Player.transform.localScale.y, Headset.transform.position.z);

        Belt.transform.position = offset;
        offset = (Player.transform.localScale.x * 0.2f) * Belt.transform.right;
        Belt.transform.position = Belt.transform.position + offset;

        /*
        if (!ShrimpGrabbed)
        {

        }
        */
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
                DecreaseSize();
                ResetShrimp();

            } else if (food.CompareTag("Watermelon"))
            {
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

        SizeState++;

        //Medium to Large
        if (SizeState == 3)
        {
            Player.transform.localScale = Player.transform.localScale * MediumToLarge;
            Line.startWidth = Line.startWidth * MediumToLarge;
            Line.endWidth = Line.endWidth * MediumToLarge;
        }
        //Small to Medium
        else
        {
            Player.transform.localScale = Player.transform.localScale * SmallToMedium;
            Line.startWidth = Line.startWidth * SmallToMedium;
            Line.endWidth = Line.endWidth * SmallToMedium;
        }

        Vector3 shiftUp = new Vector3(Player.transform.position.x, Player.transform.localScale.y, Player.transform.position.z);
        Player.transform.position = shiftUp;

        PlayerController.GetComponent<CharacterController>().radius /= Player.transform.localScale.y;

    }

    //Decrease the players size
    public void DecreaseSize()
    {
        if (SizeState == 1)
        {
            return;
        }

        SizeState--;

        //Large to Medium
        if (SizeState == 2)
        {
            Player.transform.localScale = Player.transform.localScale / MediumToLarge;
            Line.startWidth = Line.startWidth / MediumToLarge;
            Line.endWidth = Line.endWidth / MediumToLarge;
        }
        //Medium to Small
        else
        {
            Player.transform.localScale = Player.transform.localScale / SmallToMedium;
            Line.startWidth = Line.startWidth / SmallToMedium;
            Line.endWidth = Line.endWidth / SmallToMedium;
        }

        Vector3 shiftDown = new Vector3(Player.transform.position.x, Player.transform.localScale.y, Player.transform.position.z);
        Player.transform.position = shiftDown;

        PlayerController.GetComponent<CharacterController>().radius *= Player.transform.localScale.y;


    }




























}
