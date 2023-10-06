 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Sprite[] animationScripts;
    public float animationTime = 1.0f;

    public System.Action killed;

    private SpriteRenderer spriteRender;
    private int animationFrame;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    private void AnimateSprite()
    {
        animationFrame++;

        if(animationFrame >= this.animationScripts.Length) 
        {
            animationFrame = 0;
        }

        spriteRender.sprite = this.animationScripts[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            this.killed.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}
