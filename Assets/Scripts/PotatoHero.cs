using System;
using System.Collections;
using UnityEngine;

public class PotatoHero : BasePlayer, IDamagable
{
    private const string JumpParamName = "Jumping";
    private const string HorizontalParamName = "HorizontalMove";
    private const string AttackParamName = "Attack";
    private const string DamageParamName = "Damage";

    private int damageParamId;
    private int jumpParamId;
    private int horizontalParamId;
    private int attackParamId;

    private const float IsGroundedThreshold = 0.05f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private int damage;
    [SerializeField] private int initialHealth;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;

    private float horizontalMove;
    private int health;
    private bool facingLeft;
    private bool isAttacking;
    private float initialY;


    private bool IsGrounded => transform.position.y - initialY <= IsGroundedThreshold;


    private void Awake()
    {
        damageParamId = Animator.StringToHash(DamageParamName);
        jumpParamId = Animator.StringToHash(JumpParamName);
        horizontalParamId = Animator.StringToHash(HorizontalParamName);
        attackParamId = Animator.StringToHash(AttackParamName);
        
        initialY = transform.position.y;
        health = initialHealth;
    }

    private void Update()
    {
        UpdateFlip();
        UpdateAnimation();
    }

    public override void UpdateDirection(float value)
    {
        horizontalMove = value * speed;
    }

    public override void Jump()
    {
        if (!IsGrounded) return;

        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    public override void Attack()
    {
        if (isAttacking) return;

        animator.SetTrigger(attackParamId);
        
        StartCoroutine(DoAttack());

        var enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);

        if (enemiesToDamage != null && enemiesToDamage.Length > 0)
        {
            foreach (var enemy in enemiesToDamage)
            {
                var damagable = enemy.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(damage);
                }
            }
        }
        
    }

    private void HitBoxActivate(bool isActive)
    {
        attackHitBox.SetActive(isActive);
    }

    private void UpdateFlip()
    {
        if (horizontalMove > 0 && facingLeft)
        {
            Flip();
        }

        else if (horizontalMove < 0 && !facingLeft)
        {
            Flip();
        }
    }

    private void UpdateAnimation()
    {
        animator.SetFloat(horizontalParamId, Mathf.Abs(horizontalMove));

        if (IsGrounded == false)
        {
            animator.SetBool(jumpParamId, true);
        }
        else
        {
            animator.SetBool(jumpParamId, false);
        }
    }

    private void FixedUpdate()
    {
        var targetVelocity = new Vector2(horizontalMove * 10f, rb.velocity.y);
        rb.velocity = targetVelocity;
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        
        var scale = Vector3.one;
        scale.x = facingLeft ? -1 : 1;
        transform.localScale = scale;
    }


    private IEnumerator DoAttack()
    {
        isAttacking = true;
        attackHitBox.SetActive(true);

        yield return new WaitForSeconds(attackCooldown);

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
        healthBar.UpdateFill(health / 100f);
        animator.SetTrigger(damageParamId);

        if (health <= 0) Dead();
    }

    private void Dead()
    {
        SceneManager.ExitGame();
    }
}