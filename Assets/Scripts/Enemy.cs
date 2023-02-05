using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour, IDamagable
{
    Rigidbody2D rb;
    public int health;
    public float speed;
    private float dazedTime;
    public float startDazedTime;
    [SerializeField]
    GameObject attackHitBox;
    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    public int damage;
    bool isAttacking = true;

    private Animator animator;
    public GameObject bloodEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", true);

        attackHitBox.SetActive(false);
    }

    private void Update()
    {
        if (isAttacking == true)
        {

            Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
            isAttacking = false;
            animator.SetTrigger("Attack");

            StartCoroutine(DoAttack());

            for (int i = 0; i < playerToDamage.Length; i++)
            {
                var damagable = playerToDamage[i].GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(damage);
                }
            }

        }
        if (dazedTime <= 0)
        {
            speed = 2;
        } else
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
        }

        if(health <= 0)
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        dazedTime = startDazedTime;
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        health -= damage;
        animator.SetTrigger("Damage");
        Debug.Log("damage TAKEN !");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    IEnumerator DoAttack()
    {
        attackHitBox.SetActive(true);
        yield return new WaitForSeconds(1f);
        attackHitBox.SetActive(false);

        isAttacking = true;
    }

   
}