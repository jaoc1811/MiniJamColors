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

    [Header ("Attack")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.33f;
    [SerializeField] float attackRadius = 0.33f;
    [SerializeField] float attackDelay = 0.25f;
    [SerializeField] LayerMask enemyLayers;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        rb.velocity = movementVector * speed;
    }

    private void OnMove(InputValue movementValue) {
        movementVector = movementValue.Get<Vector2>();
        if (movementVector.magnitude != 0) {
            attackPoint.localPosition = movementVector.normalized * attackRange;
            attackPoint.localPosition += new Vector3(0, 0.2f, 0);
        }
    }

    private void OnFire() {
        if (canAttack) {
            attack();
        }
    }

    void attack() {
        // Play animation

        // Swing blade / detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);

            

        // Damage
        foreach(Collider2D enemy in hitEnemies) {
            Debug.Log("hit " + enemy.name);
        }
        canAttack = false;
        StartCoroutine(attackRecharge());
    }

    // Draws hit area in editor
    private void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    IEnumerator attackRecharge() {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }
}
