using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage = 1;

    private float speed = 6f;

    private Rigidbody2D rd;

    private void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        

        GameObject.Destroy(gameObject, 5f);
    }

    public void InitDir(Dir dir)
    {
        Vector2 v2;

        switch (dir)
        {
            case Dir.Up:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                v2 = new Vector2(0, speed);
                rd.velocity = v2;
                break;

            case Dir.Forward:
                Transform Player = GameObject.FindGameObjectWithTag("Player").transform;

                if (Player.transform.rotation.y == 0) 
                    {
                        v2 = new Vector2(-speed, 0);
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        rd.velocity = v2;
                        Debug.Log("这是正");
                    }
                else
                    {
                        v2 = new Vector2(speed, 0);
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        rd.velocity = v2;
                        Debug.Log("这是反");
                    }
                
                break;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().Hurt();
            Destroy(gameObject);
        }

        if (other.tag == "Boss")
        { 

        }
    }

}
