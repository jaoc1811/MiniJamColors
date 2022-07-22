using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator shadowAnimator;
    Transform target;
    [SerializeField] Transform transBody;
    [SerializeField] Transform transShadow;
    [SerializeField] Vector2 groundVelocity;
    [SerializeField] float verticalVelocity;
    [SerializeField] float verticalSpeed;
    [SerializeField] float speed;
    [SerializeField] float gravity = -10;
    [SerializeField] SpriteRenderer bodySprite;
    [SerializeField] SpriteRenderer shadowSprite;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] bool isGrounded = true;

    private void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(JumpCorroutine((target.position - transform.position).normalized, false));
    }

    private void Awake() {
        GetComponent<Enemy>().enabled = true;
        transBody.GetComponent<Animator>().enabled = true;
        transShadow.GetComponent<Animator>().enabled = true;
    }

    private void Update() {
        UpdatePosition();
        CheckGroundHit();
    }

    IEnumerator JumpCorroutine(Vector2 direction, bool spawn) {
        if (direction.x < 0) {
            bodySprite.flipX = true;
            shadowSprite.flipX = true;
        } else {
            bodySprite.flipX = false;
            shadowSprite.flipX = false;
        }
        if (!spawn)
            yield return new WaitForSeconds(1);
        bodyAnimator.SetBool("jumping", true);
        shadowAnimator.SetBool("jumping", true);
        verticalVelocity = verticalSpeed;
        groundVelocity = direction.normalized * speed;
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
            StartCoroutine(JumpCorroutine((target.position - transform.position).normalized, false));
        }
    }

    [ContextMenu("Dividir")]
    public void Divide() {
        Destroy(gameObject);
        GameObject leftHalf = Instantiate(enemyPrefab,transform.position,Quaternion.identity);
        leftHalf.transform.localScale = transform.localScale / 2;
        leftHalf.GetComponent<Enemy>().Spawn(true);
        GameObject rightHalf = Instantiate(enemyPrefab,transform.position,Quaternion.identity);
        rightHalf.transform.localScale = transform.localScale / 2;
        rightHalf.GetComponent<Enemy>().Spawn(false);
    }

    public void Spawn(bool right) {
        StartCoroutine(JumpCorroutine(new Vector2(right ? 1 : -1, 0), true));
    }
}
