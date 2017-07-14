using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

    public float delay,explosionTime;
	void Start () {
        explosionTime = Time.time + delay;
    }
	
	// Update is called once per frame
	void Update () {
        if (explosionTime < Time.time)
            Destroy(gameObject);
	}
}
