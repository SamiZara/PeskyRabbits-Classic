using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public Vector3 startPoint, endPoint;

    void Update()
    {
        transform.position = (startPoint + endPoint) / 2;
        transform.localScale = new Vector3(Vector2.Distance(endPoint, startPoint) * 100, 10, 1);
    }

    public void FinalResize()
    {
        transform.position = (startPoint + endPoint) / 2;
        transform.localScale = new Vector3(Vector2.Distance(endPoint, startPoint) * 100, 10, 1);
    }
}
