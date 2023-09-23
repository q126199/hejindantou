using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    private Animator anim;

    
    
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void PlayIdleAnim()
    {
        anim.SetBool("walk", false);
    }
    public void PlayWalkleAnim()
    {
        anim.SetBool("walk", true);
    }
    public void PlayKillAnim()
    {
        anim.SetTrigger("kill");
    }
    public void PlayDieAnim()
    {
        anim.SetTrigger("die");
    }



}
