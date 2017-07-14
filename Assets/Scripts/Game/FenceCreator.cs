using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceCreator : MonoBehaviour
{

    public GameObject flatFence, verticalFence;
    private int fenceCount = 0;
    private float fenceGap;
    public bool isGoingRight, isGoingLeft, isGoingUp, isGoingDown;
    private Vector3 lastFenceCreationPoint;

    // Use this for initialization
    void Start()
    {
        if (isGoingRight || isGoingLeft)
            fenceGap = flatFence.GetComponent<SpriteRenderer>().sprite.rect.size.x;
        else
            fenceGap = verticalFence.GetComponent<SpriteRenderer>().sprite.rect.size.y;
        lastFenceCreationPoint = transform.position;
    }

    // Update is called once per frame
    public void Update()
    {
        if (transform.localScale.x / 100f > (((fenceCount + 1) * fenceGap)) / 100 && (isGoingRight || isGoingLeft))
        {
            if (fenceCount == 0)
            {
                Vector3 nextCreationPoint;
                if (isGoingRight)
                {
                    nextCreationPoint = transform.position - new Vector3(transform.localScale.x / 100f / 2, 0, 0);
                    nextCreationPoint += new Vector3(fenceGap / 100 / 2, 0);

                }
                else
                {
                    nextCreationPoint = transform.position + new Vector3(transform.localScale.x / 100f / 2, 0, 0);
                    nextCreationPoint -= new Vector3(fenceGap / 100 / 2, 0);
                }
                Instantiate(flatFence, nextCreationPoint, Quaternion.identity);
                fenceCount++;
                lastFenceCreationPoint = nextCreationPoint;
            }
            else
            {
                Vector3 nextCreationPoint = lastFenceCreationPoint;
                if (isGoingRight)
                    nextCreationPoint += new Vector3(fenceGap / 100, 0);
                else
                    nextCreationPoint -= new Vector3(fenceGap / 100, 0);
                Instantiate(flatFence, nextCreationPoint, Quaternion.identity);
                lastFenceCreationPoint = nextCreationPoint;
                fenceCount++;
            }
        }
        else if (transform.localScale.x / 100f > (((fenceCount + 1) * fenceGap)) / 100 && (isGoingDown || isGoingUp))
        {
            if (fenceCount == 0)
            {
                Vector3 nextCreationPoint;
                float degree = transform.rotation.eulerAngles.z;
                if (isGoingDown)
                {   
                    nextCreationPoint = transform.position - new Vector3(0, transform.localScale.x / 100f / 2, 0);
                    nextCreationPoint += new Vector3(Mathf.Cos(degree*Mathf.Deg2Rad) * fenceGap / 100 / 2, Mathf.Sin(degree * Mathf.Deg2Rad) * fenceGap / 100 / 2);
                }
                else
                {
                    nextCreationPoint = transform.position + new Vector3(0, transform.localScale.x / 100f / 2, 0);
                    nextCreationPoint -= new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad) * fenceGap / 100 / 2 , Mathf.Sin(degree * Mathf.Deg2Rad) * fenceGap / 100 / 2);
                }
                if(isGoingDown)
                    Instantiate(verticalFence, nextCreationPoint, Quaternion.Euler(0,0,transform.rotation.eulerAngles.z-90));
                else
                    Instantiate(verticalFence, nextCreationPoint, Quaternion.Euler(0, 0, (transform.rotation.eulerAngles.z - 90)));
                fenceCount++;
                lastFenceCreationPoint = nextCreationPoint;
            }
            else
            {
                Vector3 nextCreationPoint = lastFenceCreationPoint;
                float degree = transform.rotation.eulerAngles.z;
                if (isGoingDown)
                    nextCreationPoint -= new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad) * fenceGap / 100 , Mathf.Sin(degree * Mathf.Deg2Rad) * fenceGap / 100 );
                else
                    nextCreationPoint += new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad) * fenceGap / 100 , Mathf.Sin(degree * Mathf.Deg2Rad) * fenceGap / 100 );
                if (isGoingDown)
                    Instantiate(verticalFence, nextCreationPoint, Quaternion.Euler(0, 0, (transform.rotation.eulerAngles.z - 90)));
                else
                    Instantiate(verticalFence, nextCreationPoint, Quaternion.Euler(0, 0, (transform.rotation.eulerAngles.z - 90)));
                lastFenceCreationPoint = nextCreationPoint;
                fenceCount++;
            }
        }
    }

    public void PutLastFence()
    {
        Vector3 endPoint;
        float degree = transform.rotation.eulerAngles.z;
        if (isGoingRight)
        {
            endPoint = transform.position + new Vector3(transform.localScale.x / 100 / 2, 0);
            lastFenceCreationPoint += new Vector3(fenceGap / 100 / 2, 0);
        }
        else if (isGoingLeft)
        {
            endPoint = transform.position - new Vector3(transform.localScale.x / 100 / 2, 0);
            lastFenceCreationPoint += new Vector3(-fenceGap / 100 / 2, 0);
        }
        else if (isGoingUp)
        {
            endPoint = GameReferenceManager.instance.player.transform.position;
            lastFenceCreationPoint += new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad) * fenceGap / 100 / 2, Mathf.Sin(degree * Mathf.Deg2Rad) * fenceGap / 100 / 2);
        }
        else
        {
            endPoint = GameReferenceManager.instance.player.transform.position;
            lastFenceCreationPoint -= new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad) * fenceGap / 100 / 2, Mathf.Sin(degree * Mathf.Deg2Rad) * fenceGap / 100 / 2 );
        }
        float ratio = Vector2.Distance(lastFenceCreationPoint,endPoint) / fenceGap * 100;
        GameObject temp = null;
        if ((isGoingRight || isGoingLeft))
        {
            if (ratio > 1)
                ratio = 1;
            //Debug.Log(ratio + "," + endPoint + "," + lastFenceCreationPoint);
            temp = Instantiate(flatFence, (endPoint + lastFenceCreationPoint) / 2, Quaternion.identity);
            temp.transform.localScale = new Vector3(ratio,1,1);
        }
        else
        {
            if (ratio > 1)
                ratio = 1;
            temp = Instantiate(verticalFence, (endPoint + lastFenceCreationPoint) / 2, Quaternion.Euler(0, 0, (transform.rotation.eulerAngles.z - 90)));
            temp.transform.localScale = new Vector3(1, ratio, 1);
        }
        Destroy(this);
    }
}
