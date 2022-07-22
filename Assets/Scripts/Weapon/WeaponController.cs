using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator anim;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void Enable(){
        spriteRenderer.enabled = true;
        anim.enabled = true;
    }

    public void Disable(){
        spriteRenderer.enabled = false;
        anim.enabled = false;
    }

    public void FlipX(bool flip){
        spriteRenderer.flipX = flip;
    }

}
