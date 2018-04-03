using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCollision : MonoBehaviour {

    public GameObject AudioManager;
    public GameObject Building;

    public Material crack0;
    public Material crack1;
    public Material crack2;
    public Material crack3;
    public Material crack4;

    private int crackCount = 0;
    private int breakLimit = 10;
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

            Building.GetComponent<AudioSource>().Play();

            //explode car
            other.gameObject.GetComponent<Explode>().MakeExplode();

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
            if (crackCount == 2)
            {
                this.GetComponent<MeshRenderer>().material = cracks[1];
            }
            else if (crackCount == 4)
            {
                this.GetComponent<MeshRenderer>().material = cracks[2];
            }
            else if (crackCount == 6)
            {
                this.GetComponent<MeshRenderer>().material = cracks[3];
            }
            else if (crackCount == 8)
            {
                this.GetComponent<MeshRenderer>().material = cracks[4];
            }

        }
        
    }

    public void Break()
    {
        AudioManager.GetComponent<AudioManagement>().Play("LaserOn");
        this.gameObject.SetActive(false);
    }


}
