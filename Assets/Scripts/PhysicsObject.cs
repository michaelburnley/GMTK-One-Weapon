using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float minGroundNormalY = 0.65f;
    public float gravityModifier = 1f;

    protected Vector2 targetVelocity;
    protected Vector2 groundNormal;
    protected bool grounded;
    protected Vector2 velocity;
    protected Rigidbody2D rigidbody;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitbuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitbufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    private void OnEnable() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity() {

    }

    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useTriggers = false;
        // Check LayerMask collision matrix
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    private void FixedUpdate() {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;
        Movement(move, false);
        move = Vector2.up * deltaPosition.y;
        Movement(move, true);
    }

    private void Movement (Vector2 move, bool yMovement) {
        float distance = move.magnitude;

        if (distance > minMoveDistance) {
            int count = rigidbody.Cast (move, contactFilter, hitbuffer, distance + shellRadius);
            hitbufferList.Clear();
            for (int i = 0; i < count; i++) {
                hitbufferList.Add(hitbuffer[i]);
            }

            for (int i = 0; i < hitbufferList.Count; i++) {
                Vector2 currentNormal = hitbufferList[i].normal;
                if (currentNormal.y > minGroundNormalY) {
                    grounded = true;
                    if (yMovement) {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0) {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitbufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        rigidbody.position = rigidbody.position + move.normalized * distance;
    }
}
