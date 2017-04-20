using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private Vector3 fingerDownPosition;
    private float leftSlope, rightSlope;
    private Rigidbody2D rb;
    public float speed;
    private bool isGoingRight, isGoingLeft, isGoingUp, isGoingDown, isDrawingWall;
    private int firstMoveDirection, lastMoveDirection;
    public LayerMask layerMask;
    public GameObject vulnurableWall;
    private List<GameObject> wallsCreated;
    private List<Vector3> allPoints, currentShapePoints;
    private Vector3 lastPoint;
    public GameObject originObject, destinationObject;
    public GameObject lastTouchedObject;

    // Use this for initialization
    void Start()
    {
        leftSlope = MathHelper.degreeBetween2Points(new Vector3(-5.31f, -8.14f, 0), new Vector3(-4.04f, 5.71f, 0));
        rightSlope = MathHelper.degreeBetween2Points(new Vector3(5.25f, -8.14f, 0), new Vector3(3.95f, 5.71f, 0));
        rb = GetComponent<Rigidbody2D>();
        wallsCreated = new List<GameObject>();
        allPoints = new List<Vector3>();
        currentShapePoints = new List<Vector3>();
        //Debug.Log(MathHelper.degreeBetween2Points(new Vector3(3.9f,-5.8f,0),new Vector3(3.87f,-5.41f,0)));
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
                if (wallsCreated.Count == 0)
                {
                    if (isGoingDown)
                        firstMoveDirection = 0;
                    else if (isGoingLeft)
                        firstMoveDirection = 1;
                    else if (isGoingRight)
                        firstMoveDirection = 2;
                    else if (isGoingUp)
                        firstMoveDirection = 3;
                }
                allPoints.Add(lastPoint);
                currentShapePoints.Add(lastPoint);
                wallsCreated.Add(Instantiate(vulnurableWall, allPoints[allPoints.Count - 1], transform.rotation));
                originObject = lastTouchedObject;
            }
        }

        if (isDrawingWall)
        {
            wallsCreated[wallsCreated.Count - 1].transform.position = ((allPoints[allPoints.Count - 1] + transform.position) / 2);
            wallsCreated[wallsCreated.Count - 1].transform.localScale = new Vector3(Vector2.Distance(transform.position, allPoints[allPoints.Count - 1]) * 100, 10, 1);
        }

        if (rb.velocity.magnitude == 0)
        {
            if (wallsCreated.Count != 0)
            {
                allPoints.Add(transform.position);
                currentShapePoints.Add(transform.position);
                destinationObject = lastTouchedObject;
                foreach (GameObject wall in wallsCreated)
                {
                    wall.layer = LayerMask.NameToLayer("Wall-Player");
                    wall.tag = "Wall-Player";
                    Destroy(wall.GetComponent<BoxCollider2D>());
                }
                wallsCreated.Clear();
                isDrawingWall = false;
                if (isGoingDown)
                    lastMoveDirection = 0;
                else if (isGoingLeft)
                    lastMoveDirection = 1;
                else if (isGoingRight)
                    lastMoveDirection = 2;
                else if (isGoingUp)
                    lastMoveDirection = 3;
                if (originObject.name != destinationObject.name)//Between created and original wall
                {
                    Vector2[] originWallPossiblePoints = new Vector2[2];//Origin possible points
                    if (originObject.name == "PolygonCollider")
                    {
                        int counter2 = 0;
                        Vector2[] polygonPoints = originObject.GetComponent<PolygonCollider2D>().points;
                        foreach (Vector2 polygonPoint in polygonPoints)
                        {
                            float degree = MathHelper.degreeBetween2Points(currentShapePoints[0], polygonPoint);
                            if (degree < 0)
                                degree += 360;
                            Debug.Log(degree);
                            if ((degree > 84 && degree < 96) || (degree > 174 && degree < 186) || (degree > 264 && degree < 276) || (degree > 354 || degree < 6))
                            {
                                originWallPossiblePoints[counter2++] = polygonPoint;
                            }
                        }
                        foreach (Vector2 p in originWallPossiblePoints)
                            Debug.Log(p);
                    }
                    else
                    {
                        if (firstMoveDirection == 0)
                        {
                            originWallPossiblePoints[0] = GameReferenceManager.instance.leftBottomCorner.transform.position;
                            originWallPossiblePoints[1] = GameReferenceManager.instance.rightButtomCorner.transform.position;
                        }
                        else if (firstMoveDirection == 1)
                        {
                            originWallPossiblePoints[0] = GameReferenceManager.instance.leftBottomCorner.transform.position;
                            originWallPossiblePoints[1] = GameReferenceManager.instance.leftTopCorner.transform.position;
                        }
                        else if (firstMoveDirection == 2)
                        {
                            originWallPossiblePoints[0] = GameReferenceManager.instance.rightTopCorner.transform.position;
                            originWallPossiblePoints[1] = GameReferenceManager.instance.rightButtomCorner.transform.position;
                        }
                        else if (firstMoveDirection == 3)
                        {
                            originWallPossiblePoints[0] = GameReferenceManager.instance.rightTopCorner.transform.position;
                            originWallPossiblePoints[1] = GameReferenceManager.instance.leftTopCorner.transform.position;
                        }
                    }

                    Vector2[] destinationWallPossiblePoints = new Vector2[2];//Origin possible points
                    if (destinationObject.name == "PolygonCollider")
                    {
                        int counter2 = 0;
                        Vector2[] polygonPoints = destinationObject.GetComponent<PolygonCollider2D>().points;
                        foreach (Vector2 polygonPoint in polygonPoints)
                        {
                            float degree = MathHelper.degreeBetween2Points(currentShapePoints[0], polygonPoint);
                            if (degree < 0)
                                degree += 360;
                            Debug.Log(degree);
                            if ((degree > 84 && degree < 96) || (degree > 174 && degree < 186) || (degree > 264 && degree < 276) || (degree > 354 || degree < 6))
                            {
                                destinationWallPossiblePoints[counter2++] = polygonPoint;
                            }
                        }
                        foreach (Vector2 p in destinationWallPossiblePoints)
                            Debug.Log(p);
                    }
                    else
                    {
                        if (firstMoveDirection == 0)
                        {
                            destinationWallPossiblePoints[0] = GameReferenceManager.instance.leftBottomCorner.transform.position;
                            destinationWallPossiblePoints[1] = GameReferenceManager.instance.rightButtomCorner.transform.position;
                        }
                        else if (firstMoveDirection == 1)
                        {
                            destinationWallPossiblePoints[0] = GameReferenceManager.instance.leftBottomCorner.transform.position;
                            destinationWallPossiblePoints[1] = GameReferenceManager.instance.leftTopCorner.transform.position;
                        }
                        else if (firstMoveDirection == 2)
                        {
                            destinationWallPossiblePoints[0] = GameReferenceManager.instance.rightTopCorner.transform.position;
                            destinationWallPossiblePoints[1] = GameReferenceManager.instance.rightButtomCorner.transform.position;
                        }
                        else if (firstMoveDirection == 3)
                        {
                            destinationWallPossiblePoints[0] = GameReferenceManager.instance.rightTopCorner.transform.position;
                            destinationWallPossiblePoints[1] = GameReferenceManager.instance.leftTopCorner.transform.position;
                        }
                    }
                    List<Vector3> possibleMatches = new List<Vector3>();
                    foreach (Vector3 p1 in originWallPossiblePoints)
                    {
                        foreach (Vector3 p2 in destinationWallPossiblePoints)
                        {
                            float degree = MathHelper.degreeBetween2Points(p1, p2);
                            if (degree < 0)
                                degree += 360;
                            if ((degree > 84 && degree < 96) || (degree > 174 && degree < 186) || (degree > 264 && degree < 276) || (degree > 354 || degree < 6))
                            {
                                Debug.Log("First check is suitable for points:" + p1 + "," + p2 + " degree:" + degree+ " second check..");
                                bool flag = false;
                                foreach(Vector3 point in currentShapePoints)
                                {
                                    float degree2 = MathHelper.degreeBetween2Points(point, p1);
                                    float degree3 = MathHelper.degreeBetween2Points(point, p2);
                                    if (degree2 < 0)
                                        degree2 += 360;
                                    Debug.Log(degree+","+degree2);
                                    if ((degree2 > degree - 6 && degree2 < degree + 6) || (degree3 > degree - 6 && degree3 < degree + 6))
                                    {
                                        flag = true;
                                        Debug.Log("p1:"+p1+",p2:"+p2+" couldnt survive second check");                              
                                    }
                                }
                                if (!flag)
                                {
                                    possibleMatches.Add(p1);
                                    possibleMatches.Add(p2);
                                }
                                
                            }
                        }
                    }
                    if (possibleMatches.Count == 0)
                    {
                        Debug.Log("Area find error");
                    }
                    else if (possibleMatches.Count == 2)
                    {
                        Debug.Log("1 possible area");
                        currentShapePoints.Add(possibleMatches[1]);
                        currentShapePoints.Add(possibleMatches[0]);
                    }
                    else
                    {
                        Debug.Log("Comparising areas");
                        float minimumArea = float.MaxValue;
                        int winningIndex = 0;
                        for (int i = 0; i < possibleMatches.Count / 2; i += 2)
                        {
                            if (possibleMatches[i] != possibleMatches[i + 1])
                            {
                                List<Vector3> tempPoints = new List<Vector3>(currentShapePoints);
                                tempPoints.Add(possibleMatches[i]);
                                tempPoints.Add(possibleMatches[i + 1]);
                                float area = MathHelper.CalculatePolygonArea(tempPoints);
                                Debug.Log("p1:"+possibleMatches[i]+",p2:"+possibleMatches[i+1]+" area:"+area);
                                if (area < minimumArea)
                                {
                                    minimumArea = area;
                                    winningIndex = i;
                                }
                            }
                        }
                        currentShapePoints.Add(possibleMatches[winningIndex + 1]);
                        currentShapePoints.Add(possibleMatches[winningIndex]);
                    }
 
                }
                
                Vector2[] pointArray = new Vector2[currentShapePoints.Count];
                int counter = 0;
                foreach (Vector3 point in currentShapePoints)
                {
                    GameObject temp = new GameObject("point");
                    temp.transform.position = point;
                    pointArray[counter++] = new Vector2(point.x, point.y);
                }
                GameObject temp1 = new GameObject("PolygonCollider");
                PolygonCollider2D collider = temp1.AddComponent<PolygonCollider2D>();
                collider.points = pointArray;
                collider.transform.tag = "Wall-Player";
                collider.gameObject.layer = LayerMask.NameToLayer("Wall-Player");
                lastTouchedObject = collider.gameObject;
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
            lastTouchedObject = other.gameObject;
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

    void OnTriggerExit(Collider other)
    {

    }
}
