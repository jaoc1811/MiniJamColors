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
    [SerializeField] Vector2 verticalSpeed;
    [SerializeField] Vector2 speed;
    [SerializeField] float gravity = -10;
    [SerializeField] SpriteRenderer bodySprite;
    [SerializeField] SpriteRenderer shadowSprite;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] bool isGrounded = true;
    public int numOfDivisions = 2;
    bool invulnerable;
    bool dead = false;
    Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(JumpCorroutine((target.position - transform.position).normalized, false));
    }

    private void Awake() {
        GetComponent<Enemy>().enabled = true;
        transBody.GetComponent<Animator>().enabled = true;
        transShadow.GetComponent<Animator>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
    }

    private void Update() {
        rb.velocity = new Vector2(0,0);
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
        verticalVelocity = spawn ? 2f : Random.Range(verticalSpeed.x,verticalSpeed.y);
        groundVelocity = direction.normalized * Random.Range(speed.x,speed.y);
        isGrounded = false;
    }


    void UpdatePosition() {
        if (!isGrounded && !dead){
            transform.position += (Vector3)groundVelocity * Time.deltaTime;
            verticalVelocity += gravity * Time.deltaTime;
            transBody.position += new Vector3(0,verticalVelocity,0) * Time.deltaTime;
        }
    }

    void CheckGroundHit() {
        if(transBody.position.y < transform.position.y && !isGrounded && !dead) {
            transBody.position = transform.position;
            isGrounded = true;
            bodyAnimator.SetBool("jumping", false);
            shadowAnimator.SetBool("jumping", false);
            StartCoroutine(JumpCorroutine((target.position - transform.position).normalized, false));
        }
    }

    [ContextMenu("Dividir")]
    public void Divide(Vector3 direction) {
        if (!invulnerable) {
            Destroy(gameObject);
            GameObject leftHalf = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            leftHalf.transform.localScale = transform.localScale - Vector3.one;
            leftHalf.GetComponent<Enemy>().Spawn(new Vector2(direction.x + 0.05f,direction.y));
            leftHalf.GetComponent<Enemy>().numOfDivisions -= 1; 
            GameObject rightHalf = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            rightHalf.transform.localScale = transform.localScale - Vector3.one;
            rightHalf.GetComponent<Enemy>().Spawn(new Vector2(direction.x - 0.05f,direction.y));
            rightHalf.GetComponent<Enemy>().numOfDivisions -= 1; 

        }
    }

    public void Die() {
        if (!invulnerable) {
            StartCoroutine(DieCoroutine());
        }
    }

    
    IEnumerator DieCoroutine() {
        dead = true;
        bodyAnimator.Play("Die");
        shadowAnimator.Play("Die");
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void Spawn(Vector2 direction) {
        invulnerable = true;
        StartCoroutine(spawnProtection());
        StartCoroutine(JumpCorroutine(direction, true));
    }

    IEnumerator spawnProtection() {
        yield return new WaitForSeconds(1);
        invulnerable = false;
    }
}
