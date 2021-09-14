using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the collision framework and animation
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Mesh : MonoBehaviour {

    /* --- Components --- */
    protected SpriteRenderer spriteRenderer;
    protected BoxCollider2D collisionBox;
    protected CircleCollider2D collisionBall;

    /* --- Variables --- */
    public Hurtbox hurtbox; // Handles the ground collision checks.
    public Feetbox feetbox; // Handles the ground collision checks.

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collisionBox = GetComponent<BoxCollider2D>();
        collisionBall = GetComponent<CircleCollider2D>();
    }

}
