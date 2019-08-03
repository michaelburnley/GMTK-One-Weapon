using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PhysicsObject
{
    public float jumpTakeOffSpeed = 7f;
    public float fallMultiplier = 4f;
    public float maxSpeed = 7f;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected override void ComputeVelocity() {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded) {
            velocity.y = jumpTakeOffSpeed;
        } else if (Input.GetButtonUp("Jump")) {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if (velocity.y < 0) {
            velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
        }

        animator.SetBool("grounded", grounded);

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flipSprite) 
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        if(Input.GetKeyDown(KeyCode.S)) {
            animator.SetBool("crouching", true);
            boxCollider.size = new Vector2(0.32f, 0.2f);
        } else {
            animator.SetBool("crouching", false);
            boxCollider.size = new Vector2(0.32f, 0.32f);
        }
        targetVelocity = move * maxSpeed;
    }
}
