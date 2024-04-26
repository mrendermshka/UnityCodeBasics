using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] int AttackDamage = 5;
    public int Hp = 10;
    [SerializeField] int MaxAmountOfHitPerSecond = 2;
    private float NextAttackTime = 0f;
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Player")&& IsAttackTime()){
            col.gameObject.GetComponent<PlayerMove>().TakeDamage(AttackDamage);
            CalculateNextAttackTime();
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if(col.gameObject.CompareTag("Player")&& IsAttackTime()){
           col.gameObject.GetComponent<PlayerMove>().TakeDamage(AttackDamage); 
           CalculateNextAttackTime();
        }
    }

    void CalculateNextAttackTime(){
        NextAttackTime = Time.time + 1f / MaxAmountOfHitPerSecond;
    }

    bool IsAttackTime(){
        if (Time.time >= NextAttackTime){
            return true;
        }else{
            return false;
        }
    }

    public void TakeDamage(int amountOfDamage){
        this.Hp -= amountOfDamage;
        if (this.Hp  <= 0 ){
           Destroy(gameObject); 
        }
    }
}
