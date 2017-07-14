using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float speed;
    public Vector3 originPoint;
    public GameObject wall, lastWall;
    private List<GameObject> walls = new List<GameObject>();
    public Rigidbody2D rb;
    //public GameObject col;
    void Start()
    {
        speed = GameReferenceManager.instance.map.speed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPoint = GameReferenceManager.instance.map.MapToWorldPoint(GameReferenceManager.instance.map.playerMapPoint);
        if (GameReferenceManager.instance.map.isGoingDown)
        {
            //col.transform.localPosition = new Vector3(0, -0.1f, 0);
            transform.position = new Vector3(targetPoint.x, transform.position.y, transform.position.z);
            rb.velocity = -new Vector2(0, GameReferenceManager.instance.map.StepToWorldPointVerticalAmount()/0.05f);
            //transform.position -= new Vector3(0, GameReferenceManager.instance.map.StepToWorldPointVerticalAmount() * Time.deltaTime / 0.05f, 0);
        }
        else if (GameReferenceManager.instance.map.isGoingLeft)
        {
            //col.transform.localPosition = new Vector3(-0.1f, 0, 0);
            transform.position = new Vector3(transform.position.x, targetPoint.y, transform.position.z);
            rb.velocity = -new Vector2(GameReferenceManager.instance.map.StepToWorldPointHorizontalAmount() / 0.05f, 0);
            //transform.position -= new Vector3(GameReferenceManager.instance.map.StepToWorldPointHorizontalAmount() * Time.deltaTime / 0.05f, 0, 0);
        }
        else if (GameReferenceManager.instance.map.isGoingRight)
        {
            //col.transform.localPosition = new Vector3(0.1f,0, 0);
            transform.position = new Vector3(transform.position.x, targetPoint.y, transform.position.z);
            rb.velocity = new Vector2(GameReferenceManager.instance.map.StepToWorldPointHorizontalAmount() / 0.05f, 0);
            //transform.position += new Vector3(GameReferenceManager.instance.map.StepToWorldPointHorizontalAmount() * Time.deltaTime / 0.05f, 0, 0);
        }
        else if (GameReferenceManager.instance.map.isGoingUp)
        {
            //col.transform.localPosition = new Vector3(0, 0.1f, 0);
            transform.position = new Vector3(targetPoint.x, transform.position.y, transform.position.z);
            rb.velocity = new Vector2(0, GameReferenceManager.instance.map.StepToWorldPointVerticalAmount() / 0.05f);
            //transform.position += new Vector3(0, GameReferenceManager.instance.map.StepToWorldPointVerticalAmount() * Time.deltaTime / 0.05f, 0);
        }
        else
        {
            //col.transform.localPosition = new Vector3(0, 0, 0);
            transform.position = Vector2.Lerp(transform.position, targetPoint, 0.30f);
            if (Vector3.Distance(transform.position, targetPoint) < 0.05f)
                transform.position = targetPoint;
        }
        //transform.position = Vector2.Lerp(transform.position, targetPoint, 0.16f);
        transform.position = targetPoint;
        transform.position = new Vector3(transform.position.x, transform.position.y, -5);

        if (walls.Count != 0)
        {
            //Debug.Log((transform.position + originPoint) / 2);
            lastWall.transform.position = (transform.position + originPoint) / 2;
            if(originPoint.x != transform.position.x)
            {
                lastWall.transform.localScale = new Vector3(originPoint.x - transform.position.x - 0.2f, 0.1f, 1);
            }
            else
            {
                lastWall.transform.localScale = new Vector3(0.1f,(originPoint.y - transform.position.y) - 0.2f, 1);
            }
        }
    }

    public void CreateWall(Vector3 originPoint)
    {
        if (walls.Count != 0)
        {
            //Debug.Log((originPoint + this.originPoint) / 2);
            lastWall.transform.position = (this.originPoint+ originPoint) / 2;
            if (originPoint.x != this.originPoint.x)
            {
                lastWall.transform.localScale = new Vector3(this.originPoint.x - originPoint.x, 0.1f, 1);
            }
            else
            {
                lastWall.transform.localScale = new Vector3(0.1f, this.originPoint.y - originPoint.y, 1);
            }
        }
        this.originPoint = originPoint;
        walls.Add(Instantiate(wall, transform.position, Quaternion.identity));
        lastWall = walls[walls.Count - 1];
        lastWall.transform.localScale = new Vector3(0, 0, 1);
    }

    public void StopDrawing(Vector3 finishPoint)
    {
        lastWall.transform.position = (originPoint + finishPoint) / 2;
        if (originPoint.x != finishPoint.x)
        {
            lastWall.transform.localScale = new Vector3(originPoint.x - finishPoint.x, 0.1f, 1);
        }
        else
        {
            lastWall.transform.localScale = new Vector3(0.1f, originPoint.y - finishPoint.y, 1);
        }
        foreach (GameObject w in walls)
        {
            w.layer = LayerMask.NameToLayer("Wall");
        }
        walls.Clear();
    }
}
