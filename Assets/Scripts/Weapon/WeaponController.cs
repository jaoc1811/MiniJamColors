using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator anim;
    Collider2D coll;
    bool attacking;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
    }

    public void Enable(){
        spriteRenderer.enabled = true;
        anim.enabled = true;
        coll.enabled = true;
        attacking = true;
    }

    public void Disable(){
        spriteRenderer.enabled = false;
        anim.enabled = false;
        coll.enabled = false;
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            if (other.GetComponent<Enemy>().numOfDivisions > 0 ){
                other.GetComponent<Enemy>().TakeDamage(new  Vector3(transform.localPosition.x, transform.localPosition.y - 0.2f, 0));
            } else {
                other.GetComponent<Enemy>().Die();
            }
        }
    }

    public bool isAttacking() {
        return attacking;
    }
}
