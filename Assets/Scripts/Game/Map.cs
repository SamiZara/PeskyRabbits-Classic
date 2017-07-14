using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public MapPoint[,] map;
    public int width, height;
    public bool isGoingUp, isGoingRight, isGoingDown, isGoingLeft, isDrawing;
    public Vector2 playerMapPoint;
    public List<Vector2> allPoints, currentShapePoints,lastWallPoints;
    public int speed;
    private Vector3 fingerDownPosition;
    public int scaleAmount;
    public Material material;
    public GameObject enemy;
    public GameObject topLeftCorner, topRightCorner, bottomRightCorner, bottomLeftCorner;
    List<Enemy> enemies = new List<Enemy>();
    public ParticleSystem effect;

    void Start()
    {
        //Debug.Log(Screen.width+","+Screen.height);
        width = ((int)(Vector2.Distance(topRightCorner.transform.position, topLeftCorner.transform.position) * 100) / scaleAmount);
        height = ((int)(Vector2.Distance(topRightCorner.transform.position, bottomRightCorner.transform.position) * 100) / scaleAmount);
        map = new MapPoint[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int h = 0; h < height; h++)
            {
                map[i, h] = new MapPoint();
                map[i, h].currentType = Type.E;
            }
        }
        for (int i = 0; i < width; i++)
        {
            map[i, 0].currentType = Type.W;
            map[i, height - 1].currentType = Type.W;
        }
        for (int i = 0; i < height; i++)
        {
            map[0, i].currentType = Type.W;
            map[width - 1, i].currentType = Type.W;
        }
        allPoints = new List<Vector2>();
        currentShapePoints = new List<Vector2>();
        allPoints.Add(new Vector2(0, 0));
        allPoints.Add(new Vector2(width - 1, 0));
        allPoints.Add(new Vector2(width - 1, height - 1));
        allPoints.Add(new Vector2(0, height - 1));
        lastWallPoints.AddRange(allPoints);
        GameManager.instance.totalArea = MathHelper.CalculatePolygonArea(allPoints);
        int random = Random.Range(0, 3);
        if (random == 0)
        {
            int randomX = Random.Range(0, width - 1);
            playerMapPoint = new Vector2(randomX, 0);
            map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType = map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType;
        }
        else if (random == 1)
        {
            int randomY = Random.Range(0, height - 1);
            playerMapPoint = new Vector2(width - 1, randomY);
            //Debug.Log(playerMapPoint);
            map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType = map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType;
        }
        else if (random == 2)
        {
            int randomX = Random.Range(0, width - 1);
            playerMapPoint = new Vector2(randomX, height - 1);

            map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType = map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType;
        }
        else if (random == 3)
        {
            int randomY = Random.Range(0, height - 1);
            playerMapPoint = new Vector2(0, randomY);
            map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType = map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType;
        }
        int enemiesCount = 0;
        if (MenuManager.selectedLevel < 10)
            enemiesCount = 1;
        if (MenuManager.selectedLevel > 9 && MenuManager.selectedLevel < 20)
            enemiesCount = 2;
        if (MenuManager.selectedLevel >= 20)
            enemiesCount = 3;
        for (int i = 0; i < enemiesCount; i++)
        {
            enemies.Add(Instantiate(enemy, new Vector3(0, 0, -1), Quaternion.identity).GetComponent<Enemy>());
            enemies[enemies.Count - 1].speed = 3;
        }
        //MapToString();
        GameReferenceManager.instance.player.transform.position = MapToWorldPoint(playerMapPoint);
        StartCoroutine(Step());
    }

    void Update()
    {
        //Debug.Log(isDrawing);
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
            float distance = Vector2.Distance(fingerDownPosition, fingerUpPosition);
            if (distance > 0.1f)
            {
                float degree = MathHelper.degreeBetween2Points(fingerDownPosition, fingerUpPosition);
                if (degree < 0)
                    degree += 360;
                if (degree <= 135 && degree > 45)//To up
                {
                    //Debug.Log("Trying to move up");
                    if (!isGoingUp)
                    {
                        if (isGoingDown && isDrawing)
                            return;
                        if (!CanMoveUp())
                            return;
                        isDrawing = false;
                        MoveUp();
                        isGoingUp = true;
                        isGoingDown = false;
                        isGoingLeft = false;
                        isGoingRight = false;
                    }
                }
                else if (degree <= 225 && degree > 135)//To left
                {
                    //Debug.Log("Trying to move left");
                    if (!isGoingLeft)
                    {
                        if (isGoingRight && isDrawing)
                            return;
                        if (!CanMoveLeft())
                            return;
                        isDrawing = false;
                        MoveLeft();
                        isGoingUp = false;
                        isGoingDown = false;
                        isGoingLeft = true;
                        isGoingRight = false;
                    }
                }
                else if (degree <= 315 && degree > 225)//To down
                {
                    if (!isGoingDown)
                    {
                        if (isGoingUp && isDrawing)
                            return;
                        if (!CanMoveDown())
                            return;
                        isDrawing = false;
                        MoveDown();
                        isGoingUp = false;
                        isGoingDown = true;
                        isGoingLeft = false;
                        isGoingRight = false;
                    }
                }
                else if (degree <= 45 || degree > 315)//To right
                {
                    //Debug.Log("Trying to move right");
                    if (!isGoingRight)
                    {
                        if (isGoingLeft && isDrawing)
                            return;
                        if (!CanMoveRight())
                            return;
                        isDrawing = false;
                        MoveRight();
                        isGoingUp = false;
                        isGoingDown = false;
                        isGoingLeft = false;
                        isGoingRight = true;

                    }
                }
            }
        }
        if (isDrawing)
        {
            int index = 0;
            GameReferenceManager.instance.lr.positionCount = currentShapePoints.Count + 1;
            foreach (Vector2 p in currentShapePoints)
            {
                GameReferenceManager.instance.lr.SetPosition(index++, new Vector3(MapToWorldPoint(p).x, MapToWorldPoint(p).y, -5));
            }
            GameReferenceManager.instance.lr.SetPosition(index, GameReferenceManager.instance.player.transform.position);
            if(!effect.isPlaying)
                effect.Play();
        }
        else
        {
            GameReferenceManager.instance.lr.positionCount = 0;
            if(!effect.isStopped)
                effect.Stop();
        }
    }

    IEnumerator Step()
    {
        while (true)
        {
            if (isGoingDown)
            {
                MoveDown();
            }
            else if (isGoingLeft)
            {
                MoveLeft();
            }
            else if (isGoingRight)
            {
                MoveRight();
            }
            else if (isGoingUp)
            {
                MoveUp();
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public enum Type
    {
        E,
        W,
        Enemy,
        C,
        V
    }

    public Vector2 MapToWorldPoint(Vector2 point)
    {
        //point += new Vector2(1, 1);
        return new Vector3((((point.x / (width-1)) - 0.5f) * 8.64f), -((point.y / (height-1)) - 0.5f) * 15.36f);
    }

    public float StepToWorldPointHorizontalAmount()
    {
        //Debug.Log(8.64f / width * speed);
        return 8.64f / width * speed;
    }

    public float StepToWorldPointVerticalAmount()
    {
        //Debug.Log(15.36f / height * speed);
        return 15.36f / height * speed;
    }

    public void MoveUp()
    {
        for (int i = 0; i < speed; i++)
        {
            if (!CanMoveUp())
            {
                return;
            }
            if (!isDrawing && map[(int)playerMapPoint.x, (int)playerMapPoint.y - 1].currentType == Type.E)
            {
                isDrawing = true;
                GameReferenceManager.instance.movement.CreateWall(MapToWorldPoint(playerMapPoint));
                currentShapePoints.Add(new Vector2(playerMapPoint.x, playerMapPoint.y));
            }
            if (!isDrawing)
                map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType = map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType;
            else
            {
                map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType = map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType;
                map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType = Type.V;
            }
            map[(int)playerMapPoint.x, (int)playerMapPoint.y - 1].oldType = map[(int)playerMapPoint.x, (int)playerMapPoint.y - 1].currentType;
            playerMapPoint -= new Vector2(0, 1);
            if (map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType == Type.W && isDrawing)
            {
                isGoingUp = false;
                currentShapePoints.Add(playerMapPoint);
                StopDrawing();
                //MapToString();
            }
            if(map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType == Type.V && isDrawing)
            {
                GameManager.instance.FinishGame(false);
            }
        }
    }

    public void MoveRight()
    {
        for (int i = 0; i < speed; i++)
        {
            if (!CanMoveRight())
            {
                return;
            }
            if (!isDrawing && map[(int)playerMapPoint.x + 1, (int)playerMapPoint.y].currentType == Type.E)
            {
                //Debug.Log("Drawing");
                isDrawing = true;
                GameReferenceManager.instance.movement.CreateWall(MapToWorldPoint(playerMapPoint));
                currentShapePoints.Add(new Vector2(playerMapPoint.x, playerMapPoint.y));
            }
            if (!isDrawing)
                map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType = map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType;
            else
            {
                map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType = map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType;
                map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType = Type.V;
            }
            map[(int)playerMapPoint.x + 1, (int)playerMapPoint.y].oldType = map[(int)playerMapPoint.x + 1, (int)playerMapPoint.y].currentType;
            playerMapPoint += new Vector2(1, 0);
            //Debug.Log(map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType + "," + isDrawing);
            if (map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType == Type.W && isDrawing)
            {
                isGoingRight = false;
                currentShapePoints.Add(playerMapPoint);
                StopDrawing();
                //MapToString();
            }
            if (map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType == Type.V && isDrawing)
            {
                GameManager.instance.FinishGame(false);
            }

        }
    }

    public void MoveLeft()
    {
        for (int i = 0; i < speed; i++)
        {
            if (!CanMoveLeft())
            {
                return;
            }
            if (!isDrawing && map[(int)playerMapPoint.x - 1, (int)playerMapPoint.y].currentType == Type.E)
            {
                isDrawing = true;
                GameReferenceManager.instance.movement.CreateWall(MapToWorldPoint(playerMapPoint));
                currentShapePoints.Add(new Vector2(playerMapPoint.x, playerMapPoint.y));
            }
            if (!isDrawing)
                map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType = map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType;
            else
            {
                map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType = map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType;
                map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType = Type.V;
            }
            map[(int)playerMapPoint.x - 1, (int)playerMapPoint.y].oldType = map[(int)playerMapPoint.x - 1, (int)playerMapPoint.y].currentType;
            playerMapPoint -= new Vector2(1, 0);
            if (map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType == Type.W && isDrawing)
            {
                isGoingLeft = false;
                currentShapePoints.Add(playerMapPoint);
                StopDrawing();
                //MapToString();
            }
            if (map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType == Type.V && isDrawing)
            {
                GameManager.instance.FinishGame(false);
            }

        }
    }

    public void MoveDown()
    {
        for (int i = 0; i < speed; i++)
        {
            if (!CanMoveDown())
            {
                //Debug.Log("Durdu");
                return;
            }
            //Debug.Log(playerMapPoint);
            //Debug.Log(map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType + "," + isDrawing);
            if (!isDrawing && map[(int)playerMapPoint.x, (int)playerMapPoint.y + 1].currentType == Type.E)
            {
                isDrawing = true;
                GameReferenceManager.instance.movement.CreateWall(MapToWorldPoint(playerMapPoint));
                currentShapePoints.Add(new Vector2(playerMapPoint.x, playerMapPoint.y));
            }
            if (!isDrawing)
                map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType = map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType;
            else
            {
                map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType = map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType;
                map[(int)playerMapPoint.x, (int)playerMapPoint.y].currentType = Type.V;
            }
            map[(int)playerMapPoint.x, (int)playerMapPoint.y + 1].oldType = map[(int)playerMapPoint.x, (int)playerMapPoint.y + 1].currentType;
            playerMapPoint += new Vector2(0, 1);
            //Debug.Log(map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType + "," + isDrawing);
            if (map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType == Type.W && isDrawing)
            {
                isGoingDown = false;
                currentShapePoints.Add(playerMapPoint);
                StopDrawing();
                //MapToString();
            }
            if (map[(int)playerMapPoint.x, (int)playerMapPoint.y].oldType == Type.V && isDrawing)
            {
                GameManager.instance.FinishGame(false);
            }
        }
    }

    public void MapToString()
    {
        string m = "";
        for (int i = 0; i < height; i++)
        {
            for (int h = 0; h < width; h++)
            {
                m += map[h, i].currentType;
            }
            m += "\n";
        }
        Debug.Log(m);
    }

    public bool CanMoveUp()//Player's coordinates
    {
        if ((int)playerMapPoint.y == 0)
            return false;
        if (map[(int)playerMapPoint.x, (int)playerMapPoint.y - 1].currentType == Type.C)
            return false;
        return true;
    }

    public bool CanMoveRight()
    {
        if ((int)playerMapPoint.x == width - 1)
        {
            return false;
        }
        //Debug.Log(playerMapPoint+","+ map[(int)playerMapPoint.x , (int)playerMapPoint.y].currentType);
        if (map[(int)playerMapPoint.x + 1, (int)playerMapPoint.y].currentType == Type.C)
        {
            return false;
        }
        return true;
    }

    public bool CanMoveDown()
    {
        if ((int)playerMapPoint.y == height - 1)
            return false;
        if (map[(int)playerMapPoint.x, (int)playerMapPoint.y + 1].currentType == Type.C)
            return false;
        return true;
    }

    public bool CanMoveLeft()
    {
        if ((int)playerMapPoint.x == 0)
            return false;
        if (map[(int)playerMapPoint.x - 1, (int)playerMapPoint.y].currentType == Type.C)
            return false;
        return true;
    }

    public void DrawPolygon(Vector2[] pointList)
    {
        Vector3[] meshVertices = new Vector3[pointList.Length];
        int index = 0;
        //Debug.Log("Drawing polygon");
        int minX, minY, maxX, maxY;
        minX = int.MaxValue;
        minY = int.MaxValue;
        maxX = int.MinValue;
        maxY = int.MinValue;

        foreach (Vector2 point in pointList)
        {
            //Debug.Log(point);
            meshVertices[index++] = MapToWorldPoint(point);
            if (point.x < minX)
                minX = (int)point.x;
            if (point.x > maxX)
                maxX = (int)point.x;
            if (point.y < minY)
                minY = (int)point.y;
            if (point.y > maxY)
                maxY = (int)point.y;
        }
        //Debug.Log(minX + "," + maxX + "," + minY + "," + maxY);
        for (int x = minX ; x < maxX ; x++)
        {
            for (int y = minY ; y < maxY ; y++)
            {
                //Debug.Log(x + "," + y);
                if (MathHelper.ContainsPoint(pointList, new Vector2(x, y)))
                {
                    //Debug.Log(x + "," + y);
                    map[x, y].oldType = map[x, y].currentType;
                    map[x, y].currentType = Type.C;
                }
            }
        }
        //MapToString();
        GameObject m = new GameObject("Mesh");
        MeshFilter meshFilter = m.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = m.AddComponent<MeshRenderer>();
        Mesh mesh = CreateMesh(0, meshVertices);
        meshFilter.mesh = mesh;
        meshRenderer.material = material;
    }

    void StopDrawing()
    {
        GameReferenceManager.instance.movement.StopDrawing(MapToWorldPoint(currentShapePoints[currentShapePoints.Count - 1]));
        List<Vector2> points = new List<Vector2>();
        points.Add(currentShapePoints[currentShapePoints.Count - 1]);
        Vector2 commonPoint = new Vector2(9999, 9999);
        foreach (Vector2 p in currentShapePoints)
        {
            //Debug.Log(MapToWorldPoint(p));
            if (allPoints.Contains(p))
            {
                allPoints.Remove(p);
                commonPoint = p;
            }
        }
        foreach (Vector2 p in allPoints)
        {
            if (!points.Contains(p))
            {
                points.Add(p);
                //Debug.Log(MapToWorldPoint(p));
            }

        }
        List<List<Vector2>> polygonList = new List<List<Vector2>>();
        List<Vector2> orderedList2 = new List<Vector2>(currentShapePoints);
        float degree = MathHelper.degreeBetween2Points(currentShapePoints[currentShapePoints.Count - 2], currentShapePoints[currentShapePoints.Count - 1]);
        if (degree <= 0)
            degree += 360;
        //foreach (Vector2 p in points)
        //Debug.Log(p);
        polygonList = MathHelper.FindPolygons(points, points[0], degree, polygonList, GameReferenceManager.instance.map.currentShapePoints[0], orderedList2, true);
        float minArea = float.MaxValue;
        int minAreaIndex = 0;
        int counter3 = 0;
        if (polygonList.Count > 0)
        {
            foreach (List<Vector2> set in polygonList)
            {
                float area = Mathf.Abs(MathHelper.CalculatePolygonArea(set));

                //Debug.Log("Area2:"+ area +" edge count:+"+set.Count);
                if (area < minArea)
                {
                    minArea = area;
                    minAreaIndex = counter3;
                }
                counter3++;
                /*foreach (Vector3 point in set)
                {
                    GameObject x = new GameObject("Set:" + counter3);
                    x.transform.position = MapToWorldPoint(point);
                }*/
            }
            GameManager.instance.CoverArea(minArea);
            //FlowerSpawner flowerSpawnerScript = Instantiate(flowerSpawner, Vector3.zero, Quaternion.identity).GetComponent<FlowerSpawner>();
            //flowerSpawnerScript.area = minArea;
            foreach (Vector3 point in polygonList[minAreaIndex])
            {
                //GameObject temp = new GameObject("point");
                //temp.transform.position = MapToWorldPoint(point);
                //flowerSpawnerScript.pointList.Add(new Vector2(point.x, point.y));
                if (!currentShapePoints.Contains(point))
                {
                    allPoints.Remove(point);
                }
            }
            foreach (Vector3 point in currentShapePoints)
            {
                if (!allPoints.Contains(point))
                    allPoints.Add(point);
            }
            bool flag = false;
            if (commonPoint.x != 9999)
            {
                foreach (Vector2 p1 in currentShapePoints)
                {
                    if (flag)
                        break;
                    foreach (Vector2 p2 in allPoints)
                    {
                        if (p1 != p2)
                        {
                            if (MathHelper.IsPointInsideLine(commonPoint, p1, p2))
                            {
                                allPoints.Remove(commonPoint);
                                flag = true;
                            }
                        }
                        if (flag)
                            break;
                    }

                }
            }
            DrawPolygon(polygonList[minAreaIndex].ToArray());
            orderAllPoints();
            ChangeVulnurableWalls(allPoints);
            //MapToString();
            currentShapePoints.Clear();
            CheckEnemyInsidePolygon(polygonList[minAreaIndex].ToArray());
            isDrawing = false;

            /*foreach (Vector3 point in allPoints)
            {
                GameObject x = new GameObject("Set:");
                x.transform.position = MapToWorldPoint(point);
            }*/
        }
    }

    void ChangeVulnurableWalls(List<Vector2> polygonPoints)
    {
        if (lastWallPoints.Count > 0)
        {
            Vector2 previousPoint1 = lastWallPoints[0];
            for (int i = 1; i < lastWallPoints.Count+1; i++)
            {
                //Debug.Log("From:" + previousPoint1 + ",to:" + lastWallPoints[i % lastWallPoints.Count]);
                if (previousPoint1.x != lastWallPoints[i % lastWallPoints.Count].x)
                {
                    if (previousPoint1.x > lastWallPoints[i % lastWallPoints.Count].x)
                    {
                        for (float h = lastWallPoints[i % lastWallPoints.Count].x; h < previousPoint1.x + 1; h++)
                        {
                            //Debug.Log(h+","+previousPoint.y);
                            map[(int)h, (int)previousPoint1.y].oldType = map[(int)h, (int)previousPoint1.y].currentType;
                            map[(int)h, (int)previousPoint1.y].currentType = Type.C;
                        }
                    }
                    else
                    {
                        for (float h = previousPoint1.x; h < lastWallPoints[i % lastWallPoints.Count].x + 1; h++)
                        {
                            //Debug.Log(h + "," + previousPoint.y);
                            map[(int)h, (int)previousPoint1.y].oldType = map[(int)h, (int)previousPoint1.y].currentType;
                            map[(int)h, (int)previousPoint1.y].currentType = Type.C;
                        }
                    }
                }
                else
                {
                    if (previousPoint1.y > lastWallPoints[i % lastWallPoints.Count].y)
                    {
                        for (float h = lastWallPoints[i % lastWallPoints.Count].y; h < previousPoint1.y + 1; h++)
                        {
                            //Debug.Log(previousPoint.x + "," + h);
                            map[(int)previousPoint1.x, (int)h].oldType = map[(int)previousPoint1.x, (int)h].currentType;
                            map[(int)previousPoint1.x, (int)h].currentType = Type.C;
                        }
                    }
                    else
                    {
                        for (float h = previousPoint1.y; h < lastWallPoints[i % lastWallPoints.Count].y + 1; h++)
                        {
                            //Debug.Log(previousPoint.x + "," + h);
                            map[(int)previousPoint1.x, (int)h].oldType = map[(int)previousPoint1.x, (int)h].currentType;
                            map[(int)previousPoint1.x, (int)h].currentType = Type.C;
                        }
                    }
                }
                previousPoint1 = lastWallPoints[i % lastWallPoints.Count];
            }
        }
        /*foreach(Vector2 p in polygonPoints)
        {
            GameObject x = new GameObject("p");
            x.transform.position = MapToWorldPoint(p);
        }*/
        Vector2 previousPoint = polygonPoints[0];
        for (int i = 1; i < polygonPoints.Count+1; i++)
        {
            //Debug.Log("From:"+previousPoint+",to:"+polygonPoints[i % polygonPoints.Count]);
            if (previousPoint.x != polygonPoints[i%polygonPoints.Count].x)
            {
                if (previousPoint.x > polygonPoints[i % polygonPoints.Count].x)
                {
                    for (float h = polygonPoints[i % polygonPoints.Count].x; h < previousPoint.x + 1; h++)
                    {
                            map[(int)h, (int)previousPoint.y].currentType = Type.W;
                    }
                }
                else
                {
                    for (float h = previousPoint.x; h < polygonPoints[i % polygonPoints.Count].x + 1; h++)
                    {
                            map[(int)h, (int)previousPoint.y].currentType = Type.W;
                    }
                }
            }
            else
            {
                if (previousPoint.y > polygonPoints[i % polygonPoints.Count].y)
                {
                    for (float h = polygonPoints[i % polygonPoints.Count].y; h < previousPoint.y + 1; h++)
                    {
                            map[(int)previousPoint.x, (int)h].currentType = Type.W;
                    }
                }
                else
                {
                    for (float h = previousPoint.y; h < polygonPoints[i % polygonPoints.Count].y + 1; h++)
                    {
                            map[(int)previousPoint.x, (int)h].currentType = Type.W;
                    }
                }
            }
            previousPoint = polygonPoints[i % polygonPoints.Count];
        }
        lastWallPoints = new List<Vector2>(polygonPoints);

    }

    Mesh CreateMesh(int num, Vector3[] vertex)
    {
        Mesh mesh = new Mesh();
        List<Vector2> v2s = new List<Vector2>();
        foreach (Vector2 v2 in vertex)
        {
            v2s.Add(v2);
        }

        Triangulator tr = new Triangulator(v2s.ToArray());
        int[] indices = tr.Triangulate();
        Vector3[] vertices = new Vector3[v2s.Count];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(v2s[i].x, v2s[i].y, 0);
        }

        //Assign data to mesh
        mesh.vertices = vertices;
        //mesh.uv = uvs;
        mesh.triangles = indices;

        //Recalculations
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        //mesh.MarkDynamic();

        //Name the mesh
        mesh.name = "MyMesh";

        //Return the mesh
        return mesh;
    }

    void CheckEnemyInsidePolygon(Vector2[] points)
    {
        Vector2[] tempPoints = new Vector2[points.Length];
        int counter = 0;
        foreach (Vector2 p in points)
        {
            tempPoints[counter++] = MapToWorldPoint(p);
        }
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
                if (MathHelper.ContainsPoint(tempPoints, enemy.transform.position))
                {
                    enemy.Die();
                }
        }
    }

    void orderAllPoints()
    {
        Vector2 previousPoint = allPoints[0];
        for(int i = 1; i < allPoints.Count + 1; i++)
        {
            float degree = 1;
            int counter = 1;
            do
            {
                degree = MathHelper.degreeBetween2Points(allPoints[i % allPoints.Count], previousPoint);
                if (degree % 90 != 0)
                {
                    Vector2 temp = allPoints[i % allPoints.Count];
                    allPoints[i % allPoints.Count] = allPoints[(i + counter) % allPoints.Count];
                    allPoints[(i + counter++) % allPoints.Count] = temp;
                }
            } while (degree % 90 != 0);
            previousPoint = allPoints[i% allPoints.Count];
        }
    }
}
