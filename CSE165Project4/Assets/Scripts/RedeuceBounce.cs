using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedeuceBounce : MonoBehaviour {

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector3 temp = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f , GetComponent<Rigidbody>().velocity.z);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
