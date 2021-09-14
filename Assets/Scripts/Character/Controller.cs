using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the actions of a character.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour {

    /* --- Movement States --- */
    public enum Movement {
        Idle,
        Moving,
        Dashing,
    }

    /* --- Falling States --- */
    public enum Airborne {
        Grounded,
        Rising,
        Falling
    }

    /* --- Components --- */
    protected Rigidbody2D body; // Handles physics calculations.
    public Mesh mesh; // Handles the collision frame and animation.

    /* --- External Variables --- */
    [Space(5)][Header("External")]
    [Range(0, 20)] public int maxHealth; // The maximum health of this character.
    [Range(0, 20)] public float baseSpeed; // The base speed at which this character moves.
    [Range(0, 100)] public float baseAcceleration; // The base acceleration at which this character moves.
    [Range(0, 20)] public float baseWeight; // The base weight that the character has.
    [Range(0, 20)] public float baseJump; // The base weight that the character has.

    /* --- Internal Variables --- */
    [Space(5)][Header("Internal")]
    [SerializeField] protected int health; // The character's health.
    [SerializeField] protected float moveSpeed; // The character's movement speed.
    [SerializeField] protected float moveDirection; // The direction the character is moving.

    /* --- Flags --- */
    [Space(5)][Header("Flags")]
    [SerializeField] protected Movement movementFlag = Movement.Idle; // Flags what type of movement this controller is in.
    [SerializeField] protected Airborne airborneFlag = Airborne.Grounded; // Flags what type of movement this controller is in

    /* --- Unity --- */
    void Awake() {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = baseWeight * GameRules.GravityScale;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update() {
        Think();
        Check();
    }

    void FixedUpdate() {
        Move();
    }

    /* --- Input --- */
    protected virtual void Think() {
        // Determined by the particular type of controller.
    }

    void Check() {
        movementFlag = Movement.Idle;
        if (moveDirection != 0 && moveSpeed != 0) {
            movementFlag = Movement.Moving;
        }
        airborneFlag = Airborne.Grounded;
        if (mesh.feetbox.IsEmpty()) {
            airborneFlag = Airborne.Rising;
            if (body.velocity.y < 0) {
                airborneFlag = Airborne.Falling;
            }
        }
    }

    /* --- Internal Methods --- */
    void Move() {
        float targetVelocity = moveSpeed * moveDirection;
        if (Mathf.Abs(targetVelocity - body.velocity.x) >= baseAcceleration * Time.fixedDeltaTime) {
            float deltaVelocity = Mathf.Sign(targetVelocity - body.velocity.x) * baseAcceleration * Time.fixedDeltaTime;
            body.velocity = new Vector2(body.velocity.x + deltaVelocity, body.velocity.y);
        }
        else {
            body.velocity = new Vector2(targetVelocity, body.velocity.y);
        }
    }

    /* --- Internal Events --- */
    protected void Jump() {
        if (!mesh.feetbox.IsEmpty()) {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y + baseJump);
        }
    }

}
