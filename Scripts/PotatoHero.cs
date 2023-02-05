using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PotatoHero : MonoBehaviour
{
    public Rigidbody2D rb;
    private float HorizontalMove = 0f;
    public float speed = 1f;
    public int health;
    public float jumpForce = 8f;
    private bool FacingRight = true;
    public bool isGrounded = false;
    public float checkGroundOffsetY = -1.8f;
    public float checkGroundRadius = 0.3f;
    public Animator animator;
    bool isAttacking = false;
    [SerializeField]
    GameObject attackHitBox;
    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    public int damage;
    public GameObject bloodEffect;
    public Image Heal;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        attackHitBox.SetActive(false);
    }


    void Update()
    {
        if (Input.GetMouseButton(0) && !isAttacking)
        {
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
            isAttacking = true;
            animator.SetTrigger("Attack");

            StartCoroutine(DoAttack());
        
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
            }
        }


        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
   
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        HorizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        animator.SetFloat("HorizontalMove", Mathf.Abs(HorizontalMove));

        if (isGrounded == false)
        {
            animator.SetBool("Jumping", true);
        }
        else
        {
            animator.SetBool("Jumping", false);
        }

        if (HorizontalMove < 0 && FacingRight)
        {
            Flip();
        }
        else if (HorizontalMove > 0 && !FacingRight)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(HorizontalMove * 10f, rb.velocity.y);
        rb.velocity = targetVelocity;

        CheckGround();

    }

    private void Flip()
    {
        FacingRight = !FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;  
    }
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll
            (new Vector2(transform.position.x, transform.position.y + checkGroundOffsetY), checkGroundRadius);

        if (colliders.Length > 1)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    IEnumerator DoAttack()
    {
        attackHitBox.SetActive(true);
        yield return new WaitForSeconds(1f);
        attackHitBox.SetActive(false);

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        health -= damage;
        Heal.fillAmount = health / 100;
       
        animator.SetTrigger("Damage");
        Debug.Log("damage TAKEN !");
        if (health <= 0)
        {
            Debug.Log("YOU DEAD");
            SceneManager.LoadScene(0);
        }
    }


}
