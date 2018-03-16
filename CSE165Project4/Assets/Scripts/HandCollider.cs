using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour {

    public GameObject InputManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("EnterWall");
            InputManager.GetComponent<InputManager>().HandInWall = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("ExitWall");
            InputManager.GetComponent<InputManager>().HandInWall = false;
        }

    }

}
