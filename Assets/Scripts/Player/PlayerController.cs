using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private float movementX;
    private float movementY;
    private Vector2 movementVector;

    [SerializeField] float speed = 2f;

    private float attackDelay = 0.25f;
    private bool canAttack;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMove(InputValue movementValue) {
        movementVector = movementValue.Get<Vector2>();
    }

    private void OnFire() {
        if (canAttack) {
            canAttack = false;
            // Swing blade
            StartCoroutine(attackRecharge());
        }
    }

    private void FixedUpdate() {
        rb.velocity = movementVector;
    }

    IEnumerator attackRecharge() {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }
}
