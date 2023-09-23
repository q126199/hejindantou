using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int maxHealth = 1;
    private int curHealth;

    private Animator anim;
    private EnemyAnim enemyAnim;

    public float idleDis = 6f;
    public float walkDis = 1.5f;

    public bool canAttack = true;
    private bool isDied = false;

    private float timer = 0;
    private float attackTime = 2f;
    private void Start()
    {
        enemyAnim = GetComponent<EnemyAnim>();
        curHealth = maxHealth;
    }

    private void Update()
    {
        if (Patrpl() == true)
        {
            enemyAnim.PlayWalkleAnim();
        }
        else if (Attack() == true && canAttack == true)
        {
            enemyAnim.PlayKillAnim();
        }
        else 
        {
            enemyAnim.PlayIdleAnim();
        }
    }
    public void Hurt()
    {
        canAttack = false;
        curHealth -= 1;
        if (curHealth <= 0)
        {
            isDied = true;
            SoundMgr.Instance.PlayMusicByName("enemyDie");

            enemyAnim.PlayDieAnim();
        }
    }

    public void Die()
    {
        GameObject.Destroy(gameObject);
    }

    public bool Patrpl()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null)
        {
            if (go.GetComponent<Player>().GetCurState() == false)
            {
                return false;
            }
            Transform player = go.transform;
            Vector2 r = new Vector2(0, 0);
            if ((player.transform.position.x - transform.position.x) > 0)
            {
                r.y = 180;
            }
            transform.rotation = Quaternion.Euler(r);

            if (Mathf.Abs(player.position.x - transform.position.x) > idleDis)
            {
                return false;
            }
            if (Mathf.Abs(player.position.x - transform.position.x) <walkDis)
            {
                return false;
            }
            if (isDied == false)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime);
            }
            return true;
        }
        return false;
    }

    public bool Attack()
    {
        timer += Time.deltaTime;
        if (timer >= attackTime)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if(go !=null)
            {
                if (canAttack == false || go.GetComponent<Player>().GetCurState()==false)
                {
                    return false;
                }
                if ((this.transform.position - go.transform.position).magnitude <= 1.5f)
                {
                    //To do
                    HurtPlayer();
                    SoundMgr.Instance.PlayMusicByName("knife");

                    timer = 0;
                    return true;
                }
             }
        }
        return false;
    }

    public void HurtPlayer()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        go.GetComponent<Player>().Hurt();
    }
}
