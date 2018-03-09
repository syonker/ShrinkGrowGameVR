using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    public GameObject InputManager;
    public bool RightHand;


    void OnTriggerEnter(Collider other)
    {
        if (RightHand)
        {
            if (!OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
            {
                InputManager.GetComponent<InputManager>().RightCollisionObject = other.gameObject;
            }
        }
        else
        {
            if (!OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
            {
                InputManager.GetComponent<InputManager>().LeftCollisionObject = other.gameObject;
            }
        }
    }


    void OnTriggerStay(Collider other)
    {
    }


    void OnTriggerExit(Collider other)
    {
        if (RightHand)
        {
            if (!OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
            {
                InputManager.GetComponent<InputManager>().RightCollisionObject = null;
            }
        }
        else
        {
            if (!OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
            {
                InputManager.GetComponent<InputManager>().LeftCollisionObject = null;
            }
        }
    }



}
