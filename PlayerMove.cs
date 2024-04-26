using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMove : MonoBehaviour
{
   
    public Rigidbody2D rb;
    public Animator anim;
    public int HP;
    [SerializeField] int AttackDamage; 
    [SerializeField] HealthBar hpBar;
    [SerializeField] float AttackRadius = 5f;
    [SerializeField] LayerMask enemyLayers;
    private float NextAttackTime = 0f; 
    int MaxAmountOfHitPerSecond = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        hpBar.SetMaxHealth(HP);
        GroundCheckRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
    }

    void Update()
    {
        Walk();
        Reflect();
        Jump();
        CheckingGround();
        RunAttackAnimation();
    }
  
    public Vector2 moveVector;
    public int speed = 3;
    void Walk()
    {
        moveVector.x = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
        anim.SetFloat("moveX", Mathf.Abs(moveVector.x));
    }
  
    public bool faceRight = true;
    void Reflect()
    {
        if ((moveVector.x > 0 && !faceRight) || (moveVector.x < 0 && faceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;
        }
    }
    
    public int jumpForce = 10;
    void Jump()
    {
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public bool onGround;
    public LayerMask Ground;
    public Transform GroundCheck;
    private float GroundCheckRadius;
    void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, Ground);
        anim.SetBool("onGround", onGround);
    }
    //-----------------------------------------------------------------

    public void TakeDamage(int amountOfDamage){
        this.HP -= amountOfDamage;
        hpBar.SetHealth(HP);
        if (this.HP  <= 0 ){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    public void Attack(){
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(gameObject.transform.position, AttackRadius, enemyLayers);   
        foreach(Collider2D enemy in hitEnemys){
            enemy.GetComponent<BasicEnemy>().TakeDamage(AttackDamage);
            Debug.Log(enemy.GetComponent<BasicEnemy>().Hp);
        }
        anim.SetTrigger("EndAttack");

    }

    public void RunAttackAnimation(){
        if (Input.GetKeyDown(KeyCode.Return) && IsAttackTime())
        anim.SetTrigger("Attack");
    }

    public void OnDrawGizmosSelected(){
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
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

}