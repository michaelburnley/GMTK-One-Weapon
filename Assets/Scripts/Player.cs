using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : PhysicsObject
{
    public float jumpTakeOffSpeed = 7f;
    public float fallMultiplier = 4f;
    public float maxSpeed = 7f;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private GameObject weapon;
    private GameObject hand;
    public Text scoreField;
    public int score = 0;
    public string playerName;
    public GameObject roundManager;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        hand = transform.GetChild(0).gameObject;
    }

    private void UpdateScore() {
        score++;
        roundManager.GetComponent<RoundManager>().UpdateScore(playerName);
    }

    protected override void Collisions(RaycastHit2D collider) {
        if (collider.collider.tag == "weapon") {
            weapon = collider.collider.gameObject;
            Projectile instance = weapon.GetComponent<Projectile>();
            if (instance.isDangerous) {
                Debug.Log("I'm dead, Jim.");
                animator.SetBool("dead", true);
            } else {
                Debug.Log("got weapon: " + instance.data.WeaponName);
                weapon.transform.SetParent(transform);
                weapon.transform.position = hand.transform.position;
                weapon.transform.rotation = Quaternion.Euler(0,0,-89.145f);
            }
        }
    }

    protected override void ComputeVelocity() {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.W) && grounded) {
            velocity.y = jumpTakeOffSpeed;
        } else if (Input.GetKeyUp(KeyCode.W)) {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if (velocity.y < 0) {
            velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
        }

        animator.SetBool("grounded", grounded);

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.0f));
        if (flipSprite) 
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            if (weapon) {
                weapon.GetComponent<Projectile>().renderer.flipX = spriteRenderer.flipX;
            }
        }

        if(Input.GetKeyDown(KeyCode.S)) {
            animator.SetBool("crouching", true);
            boxCollider.size = new Vector2(0.32f, 0.2f);
        } else {
            animator.SetBool("crouching", false);
            boxCollider.size = new Vector2(0.32f, 0.32f);
        }
        targetVelocity = move * maxSpeed;
    
        if(Input.GetKeyDown(KeyCode.Space)) {
            if (weapon) {
                weapon.transform.parent = null;
                weapon.GetComponent<Projectile>().start_moving = true;
                weapon.GetComponent<Projectile>().direction = Vector2.left;
                weapon.GetComponent<Projectile>().isDangerous = true;
            }
        }
    }
}
