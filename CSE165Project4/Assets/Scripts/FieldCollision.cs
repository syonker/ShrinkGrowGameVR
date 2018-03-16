using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCollision : MonoBehaviour {

    public Material crack0;
    public Material crack1;
    public Material crack2;
    public Material crack3;
    public Material crack4;


    private int crackCount = 0;
    private int breakLimit = 2;
    private Material[] cracks = new Material[5];


    private void Start()
    {
        cracks[0] = crack0;
        cracks[1] = crack1;
        cracks[2] = crack2;
        cracks[3] = crack3;
        cracks[4] = crack4;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Large"))
        {
            Crack();

            //explode car
            Destroy(other.gameObject);

        }

    }


    public void Crack()
    {
        crackCount++;

        if (crackCount == breakLimit)
        {
            Break();
        }
        else if (crackCount < breakLimit)
        {
            this.GetComponent<MeshRenderer>().material = cracks[crackCount];
        }
        
    }

    public void Break()
    {
        this.gameObject.SetActive(false);
    }


    void OnTriggerStay(Collider other)
    {
    }


    void OnTriggerExit(Collider other)
    {

    }
}
