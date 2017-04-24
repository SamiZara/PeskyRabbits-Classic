using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private Vector3 fingerDownPosition;
    private float leftSlope, rightSlope;
    private Rigidbody2D rb;
    public float speed;
    private bool isGoingRight, isGoingLeft, isGoingUp, isGoingDown, isDrawingWall;
    public LayerMask layerMask;
    public GameObject vulnurableWall;
    private List<GameObject> wallsCreated;
    private List<Vector3> allPoints, currentShapePoints;
    private Vector3 lastPoint;


    // Use this for initialization
    void Start()
    {
        leftSlope = MathHelper.degreeBetween2Points(new Vector3(-5.31f, -8.14f, 0), new Vector3(-4.04f, 5.71f, 0));
        rightSlope = MathHelper.degreeBetween2Points(new Vector3(5.25f, -8.14f, 0), new Vector3(3.95f, 5.71f, 0));
        rb = GetComponent<Rigidbody2D>();
        wallsCreated = new List<GameObject>();
        allPoints = new List<Vector3>();
        currentShapePoints = new List<Vector3>();
        /*List<int> indexes = new List<int>();
        indexes.Add(0);
        indexes.Add(1);
        indexes.Add(2);
        indexes.Add(3);*/
        //StartCoroutine();
        var input = new[] { 1, 2, 3 };
        var output = MathHelper.FindCombinations(input);
        List<List<int>> allSets = MathHelper.GetCombinationSet(output);
        allPoints.Add(GameReferenceManager.instance.leftTopCorner.transform.position);
        allPoints.Add(GameReferenceManager.instance.rightTopCorner.transform.position);
        allPoints.Add(GameReferenceManager.instance.leftBottomCorner.transform.position);
        allPoints.Add(GameReferenceManager.instance.rightButtomCorner.transform.position);
        Debug.Log(MathHelper.degreeBetween2Points(new Vector3(-4.04f,5.71f,0),new Vector3(5.25f,-8.14f,0)));
        Line2D line1 = new Line2D(GameReferenceManager.instance.leftBottomCorner.transform.position, GameReferenceManager.instance.leftTopCorner.transform.position);
        Line2D line2 = new Line2D(GameReferenceManager.instance.leftTopCorner.transform.position, GameReferenceManager.instance.rightTopCorner.transform.position);
        Debug.Log(line1.intersectsLine(line2));
    }

    // Update is called once per frame
    void Update()
    {
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
            if (degree <= 135 && degree > 45)//To up
            {
                if (!isGoingUp)
                {
                    if (isThereWallAtTop())
                        return;
                    if (isGoingDown && isDrawingWall)
                        return;
                    float maxHeightGap = GameReferenceManager.instance.leftTopCorner.transform.position.y - GameReferenceManager.instance.leftBottomCorner.transform.position.y;
                    float minWidthGap = GameReferenceManager.instance.rightTopCorner.transform.position.x - GameReferenceManager.instance.leftTopCorner.transform.position.x;
                    float maxWidthGap = GameReferenceManager.instance.rightButtomCorner.transform.position.x - GameReferenceManager.instance.leftBottomCorner.transform.position.x;
                    float slopeGap = rightSlope - leftSlope;
                    float height = GameReferenceManager.instance.leftTopCorner.transform.position.y - transform.position.y;
                    float currentWidth = (maxWidthGap - minWidthGap) * height / maxHeightGap + minWidthGap;
                    float ratio = 0;
                    ratio = (transform.position.x + currentWidth / 2) / currentWidth;
                    rb.velocity = new Vector2(Mathf.Cos(((ratio * slopeGap) + leftSlope) * Mathf.Deg2Rad) * speed, Mathf.Sin(((ratio * slopeGap) + leftSlope) * Mathf.Deg2Rad) * speed);
                    transform.rotation = Quaternion.Euler(0, 0, (ratio * slopeGap) + leftSlope);
                    lastPoint = transform.position;
                    isGoingUp = true;
                    isGoingDown = false;
                    isGoingLeft = false;
                    isGoingRight = false;
                    isDrawingWall = false;
                    float degree2 = (ratio * slopeGap) + leftSlope;
                }
            }
            else if (degree <= 225 && degree > 135)//To left
            {
                if (!isGoingLeft)
                {
                    if (isThereWallAtLeft())
                        return;
                    if (isGoingRight && isDrawingWall)
                        return;
                    rb.velocity = new Vector2(-speed, 0);
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    lastPoint = transform.position;
                    isGoingLeft = true;
                    isGoingDown = false;
                    isGoingRight = false;
                    isGoingUp = false;
                    isDrawingWall = false;
                }
            }
            else if (degree <= 315 && degree > 225)//To down
            {
                if (!isGoingDown)
                {
                    if (isThereWallAtBottom())
                        return;
                    if (isGoingUp && isDrawingWall)
                        return;
                    float maxHeightGap = GameReferenceManager.instance.leftTopCorner.transform.position.y - GameReferenceManager.instance.leftBottomCorner.transform.position.y;
                    float minWidthGap = GameReferenceManager.instance.rightTopCorner.transform.position.x - GameReferenceManager.instance.leftTopCorner.transform.position.x;
                    float maxWidthGap = GameReferenceManager.instance.rightButtomCorner.transform.position.x - GameReferenceManager.instance.leftBottomCorner.transform.position.x;
                    float slopeGap = rightSlope - leftSlope;
                    float height = GameReferenceManager.instance.leftTopCorner.transform.position.y - transform.position.y;
                    float currentWidth = (maxWidthGap - minWidthGap) * height / maxHeightGap + minWidthGap;
                    float ratio = 0;
                    ratio = (transform.position.x + currentWidth / 2) / currentWidth;
                    rb.velocity = new Vector2(-Mathf.Cos(((ratio * slopeGap) + leftSlope) * Mathf.Deg2Rad) * speed, -Mathf.Sin(((ratio * slopeGap) + leftSlope) * Mathf.Deg2Rad) * speed);
                    transform.rotation = Quaternion.Euler(0, 0, (ratio * slopeGap) + leftSlope);
                    lastPoint = transform.position;
                    isGoingDown = true;
                    isGoingLeft = false;
                    isGoingRight = false;
                    isGoingUp = false;
                    isDrawingWall = false;
                    float degree2 = (ratio * slopeGap) + leftSlope;
                }
            }
            else if (degree <= 45 || degree > 315)//To right
            {
                if (!isGoingRight)
                {
                    if (isThereWallAtRight())
                        return;
                    if (isGoingLeft && isDrawingWall)
                        return;
                    rb.velocity = new Vector2(speed, 0);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    lastPoint = transform.position;
                    isGoingRight = true;
                    isGoingDown = false;
                    isGoingLeft = false;
                    isGoingUp = false;
                    isDrawingWall = false;
                }
            }

        }

        if (rb.velocity.magnitude != 0)
        {
            if (!isDrawingWall && !isThereWallAtBottom() && !isThereWallAtLeft() && !isThereWallAtRight() && !isThereWallAtTop())
            {
                isDrawingWall = true;
                currentShapePoints.Add(lastPoint);
                wallsCreated.Add(Instantiate(vulnurableWall, currentShapePoints[currentShapePoints.Count - 1], transform.rotation));
            }
        }

        if (isDrawingWall)
        {
            wallsCreated[wallsCreated.Count - 1].transform.position = ((currentShapePoints[currentShapePoints.Count - 1] + transform.position) / 2);
            wallsCreated[wallsCreated.Count - 1].transform.localScale = new Vector3(Vector2.Distance(transform.position, currentShapePoints[currentShapePoints.Count - 1]) * 100, 10, 1);
        }

        if (rb.velocity.magnitude == 0)
        {
            if (wallsCreated.Count != 0)
            {
                currentShapePoints.Add(transform.position);
                foreach (GameObject wall in wallsCreated)
                {
                    wall.layer = LayerMask.NameToLayer("Wall-Player");
                    wall.tag = "Wall-Player";
                    Destroy(wall.GetComponent<BoxCollider2D>());
                }
                wallsCreated.Clear();
                isDrawingWall = false;
                List<List<Vector3>> matches = new List<List<Vector3>>();
                int[] input = new int[allPoints.Count];
                for (int i = 0; i < input.Length; i++)
                {
                    input[i] = i;
                }
                var output = MathHelper.FindCombinations(input);
                List<List<int>> allSets = MathHelper.GetCombinationSet(output);
                List<List<Vector3>> suitableSets = new List<List<Vector3>>();
                List<Vector3> tempList = new List<Vector3>(currentShapePoints);
                List<Vector3> orderedList = new List<Vector3>();
                List<Vector3> tempList2;
                if (tempList.Count > 3 && (tempList2 = IsPolygon(tempList, tempList[0],true,0,tempList[0],orderedList)) != null )
                    suitableSets.Add(orderedList);
                foreach (List<int> set in allSets)
                {
                    List<Vector3> vectorList = new List<Vector3>(currentShapePoints);
                    orderedList = new List<Vector3>();
                    foreach (int index in set)
                    {
                        vectorList.Add(allPoints[index]);
                    }
                    if (vectorList.Count > 3 && (tempList2 = IsPolygon(vectorList, vectorList[0], true, 0, vectorList[0], orderedList)) != null)
                    {
                        suitableSets.Add(orderedList);
                    }
                }
                int counter3 = 0;
                float minArea = float.MaxValue;
                int minAreaIndex = 0;
                foreach (List<Vector3> set in suitableSets)
                {
                    float area = Mathf.Abs(MathHelper.CalculatePolygonArea(set));
  
                    //Debug.Log("Area2:"+ area +" edge count:+"+set.Count);
                    if (area < minArea)
                    {
                        minArea = area;
                        minAreaIndex = counter3;
                    }
                    counter3++;
                    /*foreach(Vector3 point in set)
                    {
                        GameObject x = new GameObject("Set:"+ counter3);
                        x.transform.position = point;
                    }*/
                }          
                Debug.Log("Minimum set:"+minAreaIndex+",area:"+minArea);
                Vector2[] pointArray = new Vector2[suitableSets[minAreaIndex].Count];
                int counter = 0;
                
                foreach (Vector3 point in suitableSets[minAreaIndex])
                {
                    GameObject temp = new GameObject("point");
                    temp.transform.position = point;
                    pointArray[counter++] = new Vector2(point.x, point.y);
                    if (!currentShapePoints.Contains(point))
                    {
                        allPoints.Remove(point);
                    }
                }

                foreach (Vector3 point in currentShapePoints)
                {
                    allPoints.Add(point);
                }
                GameObject temp1 = new GameObject("PolygonCollider");
                PolygonCollider2D collider = temp1.AddComponent<PolygonCollider2D>();
                collider.points = pointArray;
                collider.transform.tag = "Wall-Player";
                collider.gameObject.layer = LayerMask.NameToLayer("Wall-Player");
                currentShapePoints.Clear();
            }
        }
    }

    bool isThereWallAtLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-0.2f, 0), Vector2.left, 0.1f, layerMask);
        if (hit.collider == null)
        {
            return false;
        }
        hit = Physics2D.Raycast(transform.position + new Vector3(-0.2f, 0.2f), Vector2.left, 0.1f, layerMask);
        if (hit.collider == null)
        {
            return false;
        }
        hit = Physics2D.Raycast(transform.position + new Vector3(-0.2f, -0.2f), Vector2.left, 0.1f, layerMask);
        if (hit.collider == null)
        {
            return false;
        }
        else
        {
            Debug.Log("There is something(" + hit.transform.name + ") at left can't go");
            return true;
        }
    }

    bool isThereWallAtRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.2f, 0), Vector2.right, 0.1f, layerMask);
        if (hit.collider == null)
        {
            return false;
        }
        hit = Physics2D.Raycast(transform.position + new Vector3(0.2f, 0.2f), Vector2.right, 0.1f, layerMask);
        if (hit.collider == null)
        {
            return false;
        }
        hit = Physics2D.Raycast(transform.position + new Vector3(0.2f, -0.2f), Vector2.right, 0.1f, layerMask);
        if (hit.collider == null)
        {
            return false;
        }
        else
        {
            Debug.Log("There is something(" + hit.transform.name + ") at right can't go");
            return true;
        }
    }

    bool isThereWallAtTop()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.2f), Vector2.up, 0.1f, layerMask);
        if (hit.collider == null)
        {
            return false;
        }
        hit = Physics2D.Raycast(transform.position + new Vector3(0.2f, 0.2f), Vector2.up, 0.1f, layerMask);
        if (hit.collider == null)
        {
            return false;
        }
        hit = Physics2D.Raycast(transform.position + new Vector3(-0.2f, 0.2f), Vector2.up, 0.1f, layerMask);
        if (hit.collider == null)
        {
            return false;
        }
        else
        {
            Debug.Log("There is something(" + hit.transform.name + ") at top can't go");
            return true;
        }

    }

    bool isThereWallAtBottom()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, -0.2f), Vector2.down, 0.1f, layerMask);
        if (hit.collider == null)
        {
            return false;
        }
        hit = Physics2D.Raycast(transform.position + new Vector3(0.2f, -0.2f), Vector2.down, 0.1f, layerMask);
        if (hit.collider == null)
        {
            return false;
        }
        hit = Physics2D.Raycast(transform.position + new Vector3(-0.2f, -0.2f), Vector2.down, 0.1f, layerMask);
        if (hit.collider == null)
        {
            return false;
        }
        else
        {
            Debug.Log("There is something(" + hit.transform.name + ") at bottom can't go");
            return true;
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall" || other.tag == "Wall-Player")
        {
            rb.velocity = new Vector2(0, 0);
            return;
        }
        else if (other.tag == "VulnurableWall")
        {
            Debug.Log("Vulnurablella player çarpıştı");
        }
        else
        {
            Debug.Log("Bune lo " + other.name);
        }
    }

    List<Vector3> IsPolygon(List<Vector3> pointList, Vector3 currentPoint, bool isFirst, float currentDegree, Vector3 firstPoint, List<Vector3> orderedList)
    {
        foreach (Vector3 p1 in pointList)
        {
            if (p1 != currentPoint)
            {
                float degree = 0;
                if (isFirst)
                {
                    degree = MathHelper.degreeBetween2Points(p1, currentPoint);
                    if (degree < 0)
                        degree += 360;
                    if ((degree > 84 && degree < 96) || (degree > 174 && degree < 186) || (degree > 264 && degree < 276) || (degree > 354 || degree < 6))
                    {
                        //Debug.Log(currentDegree+","+degree+"p1:"+p1+" p2:"+currentPoint);
                        isFirst = false;
                        orderedList.Add(currentPoint);
                        pointList.Remove(currentPoint);
                        currentPoint = p1;
                        return IsPolygon(pointList, currentPoint, isFirst, degree, firstPoint, orderedList);
                    }
                }
                else
                {
                    degree = MathHelper.degreeBetween2Points(p1, currentPoint);
                    if (degree <= 0)
                        degree += 360;
                    if ((Mathf.Abs(currentDegree - degree) > 83 && Mathf.Abs(currentDegree - degree) < 97) || (Mathf.Abs(currentDegree - degree - 360) > 83 && Mathf.Abs(currentDegree - degree - 360) < 97) || (Mathf.Abs(currentDegree - degree + 360) > 83 && Mathf.Abs(currentDegree - degree + 360) < 97))
                    {
                        //Debug.Log(currentDegree + "," + degree + "p1:" + p1 + " p2:" + currentPoint);
                        orderedList.Add(currentPoint);
                        pointList.Remove(currentPoint);
                        currentPoint = p1;
                        List<Line2D> lineList = new List<Line2D>();
                        for (int i = 0; i < orderedList.Count; i++)
                        {
                            if (i + 1 != orderedList.Count)
                                lineList.Add(new Line2D(orderedList[i], orderedList[i + 1]));
                            else
                            {
                                lineList.Add(new Line2D(orderedList[i], orderedList[0]));
                            }
                        }
                        foreach (Line2D line in lineList)
                        {
                            foreach (Line2D line2 in lineList)
                            {
                                if (line != line2)
                                {
                                    if (line.intersectsLine(line2))
                                    {
                                        //Debug.Log("Intersection detected l1p1:"+line.P1+" l1p2:"+line.P2+" l2p1"+line2.P1+" l2p2"+line2.P2);
                                        return null;
                                    }
                                }
                            }
                        }
                        return IsPolygon(pointList, currentPoint, isFirst, degree, firstPoint, orderedList);
                    }
                }

            }
            else if (pointList.Count == 1)
            {
                float degree = MathHelper.degreeBetween2Points(firstPoint, pointList[0]);
                if (degree <= 0)
                    degree += 360;
                if ((Mathf.Abs(currentDegree - degree) > 83 && Mathf.Abs(currentDegree - degree) < 97) || (Mathf.Abs(currentDegree - degree - 360) > 83 && Mathf.Abs(currentDegree - degree - 360) < 97) || (Mathf.Abs(currentDegree - degree + 360) > 83 && Mathf.Abs(currentDegree - degree + 360) < 97))
                {
                    orderedList.Add(currentPoint);
                    pointList.Remove(currentPoint);
                    currentPoint = p1;
                    List<Line2D> lineList = new List<Line2D>();
                    for(int i=0;i< orderedList.Count;i++)
                    {
                        if(i+1 != orderedList.Count)
                            lineList.Add( new Line2D(orderedList[i], orderedList[i + 1]));
                        else
                        {
                            lineList.Add(new Line2D(orderedList[i], orderedList[0]));
                        }
                    }
                    foreach(Line2D line in lineList)
                    {
                        foreach (Line2D line2 in lineList)
                        {
                            if(line != line2)
                            {
                                if (line.intersectsLine(line2))
                                {
                                    //Debug.Log("Intersection detected l1p1:"+line.P1+" l1p2:"+line.P2+" l2p1"+line2.P1+" l2p2"+line2.P2);
                                    return null;
                                }
                            }
                        }
                    }
                    return orderedList;
                }
            }
        }
        return null;
    }

    void OnTriggerExit(Collider other)
    {

    }


}
