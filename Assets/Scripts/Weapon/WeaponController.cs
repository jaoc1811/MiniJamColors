using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator anim;
    bool attacking;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void Enable(){
        spriteRenderer.enabled = true;
        anim.enabled = true;
        attacking = true;
    }

    public void Disable(){
        spriteRenderer.enabled = false;
        anim.enabled = false;
        attacking = false;
    }

    public bool isAttacking() {
        return attacking;
    }
}
