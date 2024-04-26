using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] int AttackDamage = 5;
    public int Hp = 10;
    [SerializeField] int MaxAmountOfHitPerSecond = 1;
    [SerializeField] Animator anim;
    [SerializeField] float AttackRadius = 2f;
    [SerializeField] LayerMask enemyLayers;
    private float NextAttackTime = 0f;
   
    void OnTriggerEnter2D(Collider2D col)
    {
        RunAttackAnimation2(col);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        RunAttackAnimation2(col);
    }

    public void RunAttackAnimation2(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && IsAttackTime())
        {
                anim.SetTrigger("Attack");
        }
    }

    public void Attack()
    {
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(gameObject.transform.position, AttackRadius, enemyLayers);
        foreach (Collider2D enemy in hitEnemys)
        {
            if (enemy.gameObject == GameObject.Find("Player"))
            {
                if(IsAttackTime()){
                    enemy.GetComponent<PlayerMove>().TakeDamage(AttackDamage);
                }
                CalculateNextAttackTime();    
                Debug.Log(enemy.GetComponent<PlayerMove>().HP);
            }
        }
        anim.SetTrigger("EndAttack"); 
    }

    void CalculateNextAttackTime()
    {
        NextAttackTime = Time.time + 1f / MaxAmountOfHitPerSecond;
    }

    bool IsAttackTime()
    {
        if (Time.time >= NextAttackTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(int amountOfDamage)
    {
        this.Hp -= amountOfDamage;
        if (this.Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}