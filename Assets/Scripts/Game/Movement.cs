using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private Vector3 fingerDownPosition;
    private float leftSlope,rightSlope;
    private Rigidbody2D rb;
    public float speed;
	// Use this for initialization
	void Start () {
        leftSlope = MathHelper.degreeBetween2Points(new Vector3(-5.31f, -8.14f, 0), new Vector3(-4.04f, 5.71f, 0));
        rightSlope = MathHelper.degreeBetween2Points(new Vector3(5.25f, -8.14f, 0), new Vector3(3.95f, 5.71f, 0));
        rb = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            var v3 = Input.mousePosition;
            v3.z = 10.0f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            fingerDownPosition = v3;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            var v3 = Input.mousePosition;
            v3.z = 10.0f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            Vector3 fingerUpPosition = v3;
            float degree = MathHelper.degreeBetween2Points(fingerDownPosition, fingerUpPosition);
            if (degree < 0)
                degree += 360;
            Debug.Log(degree);
            if(degree <= 135 && degree > 45)//To up
            {
                float maxHeightGap = GameReferenceManager.instance.leftTopCorner.transform.position.y - GameReferenceManager.instance.leftBottomCorner.transform.position.y; 
                float minWidthGap = GameReferenceManager.instance.rightTopCorner.transform.position.x - GameReferenceManager.instance.leftTopCorner.transform.position.x;
                float maxWidthGap = GameReferenceManager.instance.rightButtomCorner.transform.position.x - GameReferenceManager.instance.leftBottomCorner.transform.position.x;
                float slopeGap = rightSlope - leftSlope;
                float height = GameReferenceManager.instance.leftTopCorner.transform.position.y - transform.position.y;
                float currentWidth = (maxWidthGap - minWidthGap) * height / maxHeightGap + minWidthGap;
                float ratio = 0;
                ratio = (transform.position.x + currentWidth / 2) / currentWidth;
                rb.velocity = new Vector2(Mathf.Cos(((ratio * slopeGap) + leftSlope) * Mathf.Deg2Rad) * speed, Mathf.Sin(((ratio * slopeGap) + leftSlope) * Mathf.Deg2Rad) * speed);
                float degree2 = (ratio * slopeGap) + leftSlope;
                Debug.Log("ratio:"+ratio+",degree:"+ degree2);
            }
            else if(degree <= 225 && degree > 135)//To left
            {

            }
            else if(degree <= 315 && degree > 225)//To down
            {

            }
            else if(degree <= 45 && degree > 315)//To right
            {

            }

        }
    }
}
