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
    [SerializeField] int lives;
    bool invulnerable = false;
    bool dead = false;
    Rigidbody2D rb;

    [Header("Audio")]
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip death;

    [Header("SlimePower")]
    SlimeColor slimeColor;
    int directionMultiplier = 1;
    float velocityMultiplier = 1;
    float jumpDelay = 1;
    int numOfTwinChildren = 1;

    enum SlimeColor {
        Yellow,     // 0
        Red,        // 1
        Blue,       // 2
        Green,      // 3
        Magenta,    // 4
        Cyan,       // 5
        COUNT       // max
    }

    Color[] colors = {Color.yellow, Color.red, Color.blue, Color.green, Color.magenta, Color.cyan};
    
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        lives = (int)transform.localScale.x;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        speed = new Vector2(6,9);

        slimeColor = (SlimeColor)Random.Range(0, ((int)SlimeColor.COUNT));
        transBody.GetComponent<SpriteRenderer>().color = colors[(int)slimeColor];
        colorPower();
        
        StartCoroutine(JumpCorroutine((target.position - transform.position).normalized * directionMultiplier, false));
    }

    [ContextMenu("Stop")]
    public void Stop() {
        GetComponent<Enemy>().enabled = false;
        foreach (Transform child in transform)
        {
            child.GetComponent<Animator>().enabled = false;
        }
    }

    [ContextMenu("Play")]
    public void Play() {
        GetComponent<Enemy>().enabled = true;
        foreach (Transform child in transform)
        {
            child.GetComponent<Animator>().enabled = true;
        }
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
            yield return new WaitForSeconds(jumpDelay);
        AudioSource.PlayClipAtPoint(jump, transform.position);
        bodyAnimator.SetBool("jumping", true);
        shadowAnimator.SetBool("jumping", true);
        verticalVelocity = spawn ? 2f : Random.Range(verticalSpeed.x,verticalSpeed.y);
        groundVelocity = direction.normalized * Random.Range(speed.x,speed.y) * velocityMultiplier;
        isGrounded = false;
    }

    void UpdatePosition() {
        if (!isGrounded && !dead){
            Vector3 newPosition = transform.position + (Vector3)groundVelocity * Time.deltaTime;
            transform.position = new Vector3(Mathf.Clamp(newPosition.x,-8.9f,8.9f), Mathf.Clamp(newPosition.y, -4.9f, 4.4f), 0);
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
            AudioSource.PlayClipAtPoint(jump, transform.position);
            StartCoroutine(JumpCorroutine((target.position - transform.position).normalized * directionMultiplier, false));
        }
    }

    [ContextMenu("Dividir")]
    public void Divide(Vector3 direction) {
        for (int i = 0; i < numOfTwinChildren; i++) {
            GameObject leftHalf = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            spawnHalfEnemy(leftHalf, transform.localScale,new Vector2(direction.x + 0.05f,direction.y));
            GameObject rightHalf = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            spawnHalfEnemy(rightHalf, transform.localScale, new Vector2(direction.x - 0.05f,direction.y));
        }
        target.GetComponent<PlayerController>().enemiesKilled++;
        Destroy(gameObject);
    }

    public void spawnHalfEnemy(GameObject enemy, Vector3 scale, Vector2 enemyKnockBackDirection){
        enemy.transform.localScale = Vector3Int.RoundToInt(scale / 2);
        enemy.GetComponent<Enemy>().KnockBack(enemyKnockBackDirection);
    }

    public void TakeDamage(Vector3 direction) {
        if (!invulnerable) {
            if(lives > 1) {
                KnockBack(direction);
                bodyAnimator.Play("Hit");
                shadowAnimator.Play("Hit");
                lives--;
            } else {
                Divide(direction);
            }
        }
    }

    public void KnockBack(Vector2 direction) {
        invulnerable = true;
        StopAllCoroutines();
        StartCoroutine(damageProtection());
        StartCoroutine(JumpCorroutine(direction, true));
    }

    IEnumerator damageProtection() {
        yield return new WaitForSeconds(0.5f);
        invulnerable = false;
    }

    public void Die() {
        if (!invulnerable) {
            AudioSource.PlayClipAtPoint(death, transform.position);
            GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(DieCoroutine());
        }
    }

    IEnumerator DieCoroutine() {
        dead = true;
        bodyAnimator.Play("Die");
        shadowAnimator.Play("Die");
        yield return new WaitForSeconds(1);
        target.GetComponent<PlayerController>().enemiesKilled++;
        Destroy(gameObject);
    }

    private void colorPower() {
        switch(slimeColor) {
            case SlimeColor.Yellow:
                directionMultiplier = -1;
                break;
            case SlimeColor.Red:
                lives = (int)(lives * 1.5f);
                break;
            case SlimeColor.Blue:
                velocityMultiplier = 1.5f;
                break;
            case SlimeColor.Green:
                velocityMultiplier = 0.001f;
                break;
            case SlimeColor.Magenta:
                jumpDelay = 0.5f;
                break;
            case SlimeColor.Cyan:
                numOfTwinChildren = 2;
                break;
        }
    }
}
