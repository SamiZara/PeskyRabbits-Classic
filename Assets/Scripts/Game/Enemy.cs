using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    public float speed;
    public Rigidbody2D rb;
    private float rebornTimer;
    private bool isDead;
    public SpriteRenderer sprite;
    public Collider2D collider;
    public GameObject effect;
    void Start()
    {
        if (MenuManager.selectedLevel < 20)
        {
            if(MenuManager.selectedLevel != 10)
                speed *= Mathf.Pow((MenuManager.selectedLevel % 10), 0.1f);
            else
                speed *= Mathf.Pow(1, 0.1f);
        }
        else
        {
            speed *= Mathf.Pow((MenuManager.selectedLevel - 19), 0.1f);
        }
        //Debug.Log(speed);
        int randomDirection = Random.Range(0, 3);
        int randomDegree = Random.Range(1,15);
        if (randomDirection == 0)
        {
            int degree = 135 + randomDegree;
            rb.velocity = new Vector2(speed * Mathf.Cos(degree * Mathf.Deg2Rad), speed * Mathf.Sin(degree * Mathf.Deg2Rad));
        }
        else if (randomDirection == 1)
        {
            int degree = 45 + randomDegree;
            rb.velocity = new Vector2(speed * Mathf.Cos(degree * Mathf.Deg2Rad), speed * Mathf.Sin(degree * Mathf.Deg2Rad));
        }
        else if (randomDirection == 2)
        {
            int degree = 315 + randomDegree;
            rb.velocity = new Vector2(speed * Mathf.Cos(degree * Mathf.Deg2Rad), speed * Mathf.Sin(degree * Mathf.Deg2Rad));
        }
        else if (randomDirection == 3)
        {
            int degree = 225 + randomDegree;
            rb.velocity = new Vector2(speed * Mathf.Cos(degree * Mathf.Deg2Rad), speed * Mathf.Sin(degree * Mathf.Deg2Rad));
        }

    }

    void Update()
    {
        if (!isDead)
        {
            if (rb.velocity.magnitude != speed && rb.velocity.magnitude != 0)
            {
                rb.velocity += (speed / rb.velocity.magnitude * rb.velocity) / 10;
                if (rb.velocity.magnitude > speed)//If exceeds speed limit
                    rb.velocity *= speed / rb.velocity.magnitude;
            }
            else if (rb.velocity.magnitude != speed && rb.velocity.magnitude == 0)
            {
                int randomDegree = Random.Range(1, 360);
                rb.velocity = new Vector2(speed * Mathf.Cos(randomDegree * Mathf.Deg2Rad), speed * Mathf.Sin(randomDegree * Mathf.Deg2Rad));
            }
        }
        if (isDead && rebornTimer < Time.time)
        {
            Reborn();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.layer == LayerMask.NameToLayer("VulnurableWall"))
        {
            GameManager.instance.FinishGame(false);
        }
        if(coll.gameObject.tag == "Enemy")
        {
            float degree = MathHelper.degreeBetween2Points(coll.transform.position, transform.position);
            float random = Random.Range(-5, 5);
            degree += random;
            rb.velocity = new Vector2(speed * Mathf.Cos(degree * Mathf.Deg2Rad), speed * Mathf.Sin(degree * Mathf.Deg2Rad));
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            sprite.enabled = false;
            collider.enabled = false;
            rb.velocity = Vector2.zero;
            rebornTimer = Time.time + 5;
            isDead = true;
            Instantiate(effect, transform.position, Quaternion.identity);
            SoundManager.instance.PlayEnemeyEaten();
        }
    }

    public void Reborn()
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;
        foreach (Vector2 point in GameReferenceManager.instance.map.allPoints)
        {
            if (point.x < minX)
                minX = point.x;
            if (point.x > maxX)
                maxX = point.x;
            if (point.y < minY)
                minY = point.y;
            if (point.y > maxY)
                maxY = point.y;
        }
        minX += 0.6f;
        minY += 0.6f;
        maxX -= 0.6f;
        maxY -= 0.6f;
        Vector2 pos = new Vector2(Random.Range(minX,maxX),Random.Range(minY,maxY));
        while(!MathHelper.ContainsPoint(GameReferenceManager.instance.map.allPoints.ToArray(), pos))
        {
            pos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        }
        transform.position = GameReferenceManager.instance.map.MapToWorldPoint(pos);
        isDead = false;
        sprite.enabled = true;
        collider.enabled = true;
        /*Vector2 pos = Vector3.zero;
        int counter = 0;
        foreach(Vector2 point in GameReferenceManager.instance.map.allPoints)
        {
            pos += GameReferenceManager.instance.map.MapToWorldPoint(point);
            counter++;
        }
        transform.position = pos / counter;
        isDead = false;
        sprite.enabled = true;
        collider.enabled = true;*/
    }

}
