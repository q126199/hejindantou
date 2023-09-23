using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//判断子弹方向
public enum Dir
{
    Up,
    Forward,
}
public class Player : MonoBehaviour
{
    //移动速度
    private float speed = 300f;
    //刚体
    private Rigidbody2D rd;
    //动画脚本
    public PlayerAnim playerAnim;
    //是否在地上
    private bool isOnGround = true;
    //允许跳跃次数
    private int jumpNum = 1;

    public Transform[] points;//0 向前发射子弹...1向上发射子弹
    public Transform curPoint;

    public int maxHealth = 3;
    public int curHealth;
    public bool isDead = false;
    public bool isResume = false;
    private void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<PlayerAnim>();
        curHealth = maxHealth;
    }

    private void Update()
    {
        if (isResume == true || isDead == true)
        {
            return;
        }
        PlayerMove();
        PlayerJump();

        if (Input.GetKeyDown(KeyCode.J))
        {
            Fire(Dir.Forward);
            playerAnim.PlayShootAnim();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Fire(Dir.Up);
            playerAnim.PlayShootUpAnim();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Throw();
        }
    }

    //玩家移动按A D移动
    public void PlayerMove()
    {

        //限制角色跳跃到空中时，不能AD左右移动
        if (isOnGround == false)
        {
            return;
        }

        //获取A D健输入
        //返回值-1-1....大于0 按D....朝右
        //返回值-1-1....小于0 按A....朝左
        float h = Input.GetAxis("Horizontal");
        rd.velocity = new Vector2(h * speed * Time.fixedDeltaTime, rd.velocity.y);
        playerAnim.PlayWalkAnim();
        if (h < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rd.velocity = new Vector2(h * speed * Time.fixedDeltaTime, rd.velocity.y);
            playerAnim.PlayWalkAnim();
        }
        if (h > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            rd.velocity = new Vector2(h * speed * Time.fixedDeltaTime, rd.velocity.y);
            playerAnim.PlayWalkAnim();
        }
        if (h == 0)
        {
            playerAnim.PlayIdleAnim();
        }
    }

    public void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.K) && jumpNum > 0)
        {
            isOnGround = false;
            jumpNum -= 1;
            rd.AddForce(Vector2.up * 400F);
            playerAnim.PlayJumpAnim();
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Ground")
        {
            isOnGround = true;
            jumpNum = 1;
        }
    }

    public void Fire(Dir dir)
    {
        SoundMgr.Instance.PlayMusicByName("shoot");
        
        GameObject temp = Resources.Load<GameObject>("Prefabs/Bullet");

        switch (dir)
        {
            case Dir.Forward:
                curPoint = points[0];
                break;
            case Dir.Up:
                curPoint = points[1];
                break;
        }
        GameObject go = Instantiate(temp, curPoint.transform.position, Quaternion.identity);

        //初始化子弹方向
        go.GetComponent<Bullet>().InitDir(dir);

        /*Bullet bullet = go.GetComponent<Bullet>();

        // 使用中间变量存储结果  
        Bullet storedBullet = bullet;

        // 在调试时输出中间变量的值  
        Debug.Log("Stored Bullet: " + storedBullet);

        // 调用 InitDir 方法  
        bullet.InitDir(dir);*/
    }

    public void Throw()
    {
        GameObject temp = Resources.Load<GameObject>("Prefabs/Grenade");
        GameObject go = Instantiate(temp, points[2].transform.position, Quaternion.identity);
    }
    public void Hurt()
    {
        curHealth -= 1;

        if (curHealth <= 0)
        {
            isDead = true;
            playerAnim.PlayDieAnim();
            SoundMgr.Instance.PlayMusicByName("soliderDie");
            //Todo
        }
        else
        {
            isResume = true;

            StartCoroutine(Resume());
        }
    }

    public IEnumerator Resume()
    {
        yield return new WaitForSeconds(1);
        playerAnim.PlayResumeAnim();

        transform.position = new Vector3(transform.position.x- 3f, transform.position.y + 5f);
        playerAnim.PlayIdleAnim();

        StartCoroutine(ResetState());

    }
    public IEnumerator ResetState()
    {
        yield return new WaitForSeconds(1f);
        isResume = false;
    }

    public bool GetCurState()
    { 
        if(isDead==true)
        {
            return false;
        }
        if (isResume == true)
        {
            return false;
        }
        return true;
    }
}
