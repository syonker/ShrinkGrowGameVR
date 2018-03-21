using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

    public GameObject ExplosionEffect;
    private bool RanAlready = false;

	public void MakeExplode()
    {
        Instantiate(ExplosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);

    }
}
