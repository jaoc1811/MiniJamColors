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

    [SerializeField] Vector3 currentScale;
    [SerializeField] Vector3 maxScale;
    [SerializeField] float scaleDuration;

    [Header ("Movement")]
    [SerializeField] float speed = 2f;
    float initialSpeed;
    SpriteRenderer bodySprite;
    [SerializeField] bool isInvincible;
    [SerializeField] float invincibleDelay = 1f;

    [Header ("Attack")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.33f;
    [SerializeField] float attackDelay = 0.25f;
    [SerializeField] LayerMask enemyLayers;

    [Header("Audio")]
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip mergeSound;
    [SerializeField] AudioClip dodgeSound;

    // [Header ("Animation")]
    Animator anim;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        bodySprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        initialSpeed = speed;
        maxScale = transform.localScale;
        currentScale = transform.localScale;
        StartCoroutine(scaleOverTime());
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
        AudioSource.PlayClipAtPoint(attackSound, transform.position);
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
            AudioSource.PlayClipAtPoint(dodgeSound, transform.position);
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

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Enemy") && !isInvincible) {
            // Trigger Animation
            // Merge
            Vector3 newScale = currentScale + other.transform.localScale;
            currentScale = newScale.x > maxScale.x ? maxScale : newScale;
            anim.Play("Hit");
            AudioSource.PlayClipAtPoint(mergeSound, transform.position);
            //StartCoroutine(scaleOverTime(transform.localScale + other.transform.localScale));
            Destroy(other.gameObject);
        }
    }

    IEnumerator scaleOverTime() {
        while (true) {
            if (currentScale.x > transform.localScale.x) {
                Vector3 originalScale = transform.localScale;
                float counter = 0;
                while (counter < scaleDuration) {
                    transform.localScale = Vector3.Lerp(originalScale, currentScale, counter/scaleDuration);
                    counter += Time.deltaTime;
                    yield return null;
                }
                transform.localScale = currentScale;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
