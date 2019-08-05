using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PhysicsObject
{
    [SerializeField]
    public WeaponData data;
    public SpriteRenderer renderer;
    public float maxSpeed = 100f;
    public bool start_moving = false;
    public Vector2 direction;
    public bool isDangerous = false;

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = data.Icon;
    }

    protected override void Collisions(RaycastHit2D collider) {
        if (collider.collider.tag == "wall" && start_moving) {
            start_moving = false;
            isDangerous = false;
            GameObject instantiated_projectile = Instantiate(this.gameObject, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void Update() {
        if (start_moving) {
            targetVelocity = Vector2.right * maxSpeed;
            Debug.Log(targetVelocity);
        }
    }


}