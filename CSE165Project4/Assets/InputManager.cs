using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    //hard set
    public GameObject Player;
    public GameObject RightHand;
    public GameObject LeftHand;

    //other methods
    public GameObject RightCollisionObject;
    public GameObject LeftCollisionObject;


    private bool firstGrabLeft = true;
    private bool firstGrabRight = true;
    private Vector3 offsetLeft;
    private Vector3 offsetRight;





    // Use this for initialization
    void Start () {
        offsetLeft = Vector3.zero;
        offsetRight = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {


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


        /*


        //object grabbing

        //left hand
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            if (LeftCollisionObject != null)
            {
                if (firstGrabLeft)
                {
                    //LeftHand.GetComponent<Collider>().isTrigger = false;
                    offsetLeft = LeftHand.transform.position - LeftCollisionObject.transform.position;
                    LeftCollisionObject.GetComponent<Rigidbody>().useGravity = false;
                    firstGrabLeft = false;
                }

                LeftCollisionObject.transform.position = LeftHand.transform.position - offsetLeft;

            }
        }
        else
        {
            //drop object
            if (!firstGrabLeft)
            {
                //LeftHand.GetComponent<Collider>().isTrigger = false;
                LeftCollisionObject.GetComponent<Rigidbody>().useGravity = true;
                firstGrabLeft = true;
            }
        }



        //right hand
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            if (RightCollisionObject != null)
            {
                if (firstGrabRight)
                {
                    //RightHand.GetComponent<Collider>().isTrigger = false;
                    offsetRight = RightHand.transform.position - RightCollisionObject.transform.position;
                    RightCollisionObject.GetComponent<Rigidbody>().useGravity = false;
                    firstGrabRight = false;
                }

                RightCollisionObject.transform.position = RightHand.transform.position - offsetRight;

            }
        }
        else
        {
            //drop object
            if (!firstGrabRight)
            {
                //RightHand.GetComponent<Collider>().isTrigger = true;
                RightCollisionObject.GetComponent<Rigidbody>().useGravity = true;
                firstGrabRight = true;
            }

        }
        */












    }
}
