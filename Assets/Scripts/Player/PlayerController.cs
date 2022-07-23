using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movementVector;
    bool canAttack = true;

    [Header ("Movement")]
    [SerializeField] float speed = 2f;
    float initialSpeed;
    SpriteRenderer bodySprite;
    [SerializeField] bool isInvincible;
    [SerializeField] float invincibleDelay = 1f;

    [Header ("Attack")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.33f;
    [SerializeField] float attackRadius = 0.45f;
    [SerializeField] float attackDelay = 0.25f;
    [SerializeField] LayerMask enemyLayers;

    // [Header ("Animation")]
    Animator anim;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        bodySprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        initialSpeed = speed;
    }

    private void FixedUpdate() {
        rb.velocity = movementVector * speed;
        anim.SetFloat("Speed", rb.velocity.magnitude);

        if (movementVector.x > 0) {
            bodySprite.flipX = false;
        }
        else if (movementVector.x < 0) {
            bodySprite.flipX = true;
        }

         // AttackPoint move and rotate
        if (movementVector.magnitude != 0 && !attackPoint.GetComponent<WeaponController>().isAttacking()) {
            attackPoint.localPosition = movementVector.normalized * attackRange;
            attackPoint.localPosition += new Vector3(0, 0.2f, 0);

            float angle = Mathf.Atan2(movementVector.y, movementVector.x) * Mathf.Rad2Deg;
            attackPoint.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (isInvincible && rb.velocity.magnitude < 0.1){ // Idle, move to looking side
            float direction = attackPoint.localPosition.x > 0 ? 1 : -1;
            rb.velocity = new Vector2(direction, 0) * speed;
        }
    }

    private void OnMove(InputValue movementValue) {
        // Player Move
        movementVector = movementValue.Get<Vector2>();
        anim.SetFloat("Vertical", movementVector.y);
    }

    private void OnFire() {
        if (canAttack) {
            attack();
        }
    }

    void attack() {
        // Play animation
        attackPoint.GetComponent<WeaponController>().Enable();
        canAttack = false;
        StartCoroutine(attackRecharge());
    }

    // Draws hit area in editor
    private void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;

        //Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    IEnumerator attackRecharge() {
        yield return new WaitForSeconds(attackDelay);
        if (!isInvincible){ // To prevent attacks when dodging
            canAttack = true;
        }
        
    }

    private void OnDodge() {
        if (!isInvincible){
            isInvincible = true;
            canAttack = false;
            anim.SetBool("Dodge", true);
            speed *= 2;
            StartCoroutine(DodgeRecharge());
        }
    }

    IEnumerator DodgeRecharge() {
        yield return new WaitForSeconds(invincibleDelay);
        isInvincible = false;
    }

    void StopDodge(){
        speed = initialSpeed;
        canAttack = true;
        anim.SetBool("Dodge", false);
    }
}
