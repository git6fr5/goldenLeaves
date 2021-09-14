using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class Rope : Structure {

    /* --- Components --- */
    protected LineRenderer lineRenderer;
    protected EdgeCollider2D edgeCollider;

    /* --- Static Variables --- */
    [SerializeField] protected static float SegmentLength  = 0.2f;
    [SerializeField] protected static float SegmentWeight = 1.5f;
    [SerializeField] protected static int ConstraintDepth = 50;

    /* --- Variables --- */
    [HideInInspector] protected int segmentCount; // The number of segments.
    [SerializeField] public Transform startpoint; // The width of the rope.
    [SerializeField] public float ropeLength; // The width of the rope.
    [SerializeField] public float ropeWidth; // The width of the rope.
    [SerializeField] protected Vector3[] ropeSegments; // The current positions of the segments.
    [SerializeField] protected Vector3[] prevRopeSegments; // The previous positions of the segments.

    /* --- Unity --- */
    // Runs once on initialization.
    void Awake() {
        // Cache these references.
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        // Set up these components.
        edgeCollider.edgeRadius = ropeWidth / 2f;
        RopeSegments();
    }

    // Runs once every frame.
    void Update() {
        Render();
    }

    // Runs once every set time interval.
    void FixedUpdate() {
        Simulation();
    }

    // Runs if this trigger is activated.
    void OnTriggerStay2D(Collider2D collider) {
        if (collider.transform.parent.GetComponent<Rigidbody2D>() != null) {
            Jiggle(collider);
        }
    }

    // Runs if this trigger is activated.
    void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.transform.parent.GetComponent<Rigidbody2D>() != null) {
            Jiggle(collision.collider);
        }
    }

    /* --- Methods --- */
    // Initalizes the rope segments.
    void RopeSegments() {
        // Get the number of segments for a rope of this length.
        segmentCount = (int)Mathf.Ceil(ropeLength / SegmentLength);

        // Initialize the rope segments.
        ropeSegments = new Vector3[segmentCount];
        prevRopeSegments = new Vector3[segmentCount];
        
        for (int i = 0; i < segmentCount; i++) {
            ropeSegments[i] = startpoint.position;
            prevRopeSegments[i] = ropeSegments[i];
        }
        ropeSegments[segmentCount - 1] += ropeLength * Vector3.right;
    }

    // Renders the rope using the line renderer and edge collider.
    void Render() {
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
        lineRenderer.positionCount = segmentCount;
        lineRenderer.SetPositions(ropeSegments);

        Vector2[] points = new Vector2[segmentCount];
        for (int i = 0; i < segmentCount; i++) {
            points[i] = (Vector2)ropeSegments[i] - (Vector2)transform.position;
        }

        edgeCollider.points = points;
    }

    // Adds a jiggle whenever a body collides with this.
    void Jiggle(Collider2D collider) {
        Rigidbody2D body = collider.transform.parent.GetComponent<Rigidbody2D>();
        // Get the segment closest to the collider.
        Vector3 pos = collider.transform.position;
        int index = 1;
        float minDist = 1e9f;
        for (int i = 1; i < segmentCount; i++) {
            if ((pos - ropeSegments[i]).magnitude < minDist) {
                index = i;
                minDist = (pos - ropeSegments[i]).magnitude;
            }
        }
        // Add a jiggle to this segment.
        ropeSegments[index] += (Vector3)body.velocity * SegmentWeight * Time.deltaTime; // body.gravityScale /  
    }

    void Simulation() {
        Vector3 forceGravity = new Vector3(0f, -SegmentWeight * GameRules.GravityScale, 0f);
        for (int i = 0; i < segmentCount; i++) {
            Vector3 velocity = ropeSegments[i] - prevRopeSegments[i];
            prevRopeSegments[i] = ropeSegments[i];
            ropeSegments[i] += velocity;
            ropeSegments[i] += forceGravity * Time.fixedDeltaTime;
        }
        for (int i = 0; i < ConstraintDepth; i++) {
            Constraints();
        }
    }

    protected virtual void Constraints() {
        //
    }

}
