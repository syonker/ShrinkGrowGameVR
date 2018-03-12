using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

	private Vector3 oldPos;
    


	void LateUpdate() {

		oldPos = transform.position;

	}


    void OnTriggerEnter(Collider other)
    {
		
		
		if (other.gameObject.CompareTag ("Wall")) {
			Debug.Log ("Hit Wall");
			transform.position = oldPos;
		}
    }


    void OnTriggerStay(Collider other)
    {
    }


    void OnTriggerExit(Collider other)
    {
        
    }



}
