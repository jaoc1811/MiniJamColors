using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimesMovement : MonoBehaviour
{
    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator shadowAnimator;
    [SerializeField] Transform target;
    [SerializeField] Transform transBody;
    [SerializeField] Vector2 groundVelocity;
    [SerializeField] float verticalVelocity;
    [SerializeField] float verticalSpeed;
    [SerializeField] float speed;
    [SerializeField] float gravity = -10;
    [SerializeField] SpriteRenderer bodySprite;
    [SerializeField] SpriteRenderer shadowSprite;

    bool isGrounded = true;

    private void Start() {
        StartCoroutine(JumpCorroutine());
    }

    private void Update() {
        UpdatePosition();
        CheckGroundHit();
    }

    IEnumerator JumpCorroutine() {
        if (target.position.x < transform.position.x) {
            bodySprite.flipX = true;
            shadowSprite.flipX = true;
        } else {
            bodySprite.flipX = false;
            shadowSprite.flipX = false;
        }
        yield return new WaitForSeconds(1);
        bodyAnimator.SetBool("jumping", true);
        shadowAnimator.SetBool("jumping", true);
        this.groundVelocity = (target.position - transform.position).normalized * speed;
        isGrounded = false;
    }


    void UpdatePosition() {
        if (!isGrounded){
            transform.position += (Vector3)groundVelocity * Time.deltaTime;
            verticalVelocity += gravity * Time.deltaTime;
            transBody.position += new Vector3(0,verticalVelocity,0) * Time.deltaTime;
        }
    }

    void CheckGroundHit() {
        if(transBody.position.y < transform.position.y && !isGrounded) {
            transBody.position = transform.position;
            isGrounded = true;
            bodyAnimator.SetBool("jumping", false);
            shadowAnimator.SetBool("jumping", false);
            verticalVelocity = verticalSpeed;
            StartCoroutine(JumpCorroutine());
        }
    }
}
