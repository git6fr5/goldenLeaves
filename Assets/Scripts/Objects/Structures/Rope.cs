using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : Structure {

    /* --- Components --- */
    LineRenderer lineRenderer;

    public int segmentCount;
    public float lineWidth;
    [SerializeField] protected Vector3[] ropeSegments;
    [SerializeField] protected Vector3[] prevRopeSegments;

    private float dist = 0.2f;

    void OnTriggerStay2D(Collider2D collider) {
        if (collider.transform.parent.GetComponent<Rigidbody2D>() != null) {
            Rigidbody2D body = collider.transform.parent.GetComponent<Rigidbody2D>();
            // get closest segment
            Vector3 pos = collider.transform.position;
            int index = 1;
            float minDist = 1e9f;
            for (int i = 1; i < segmentCount; i++) {
                if ((pos - ropeSegments[i]).magnitude < minDist) {
                    index = i;
                    minDist = (pos - ropeSegments[i]).magnitude;
                }
            }
            ropeSegments[index] += (Vector3)body.velocity * Time.deltaTime;
        }
    }

    void Awake() {
        SetRope();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        DrawRope();
    }

    void FixedUpdate() {
        Simulation();
    }

    EdgeCollider2D collisionFrame;

    void SetRope() {
        ropeSegments = new Vector3[segmentCount];
        ropeSegments[0] = Vector3.zero;
        for (int i = 1; i < segmentCount; i++) {
            ropeSegments[i] = ropeSegments[i - 1] + new Vector3(1, -1, 0);
            float newDist = Vector3.Distance(ropeSegments[i], ropeSegments[i - 1]);
            Vector3 newNorm = (ropeSegments[i] - ropeSegments[i - 1]).normalized;
            ropeSegments[i] = ropeSegments[i - 1] + newNorm * dist;
        }
        prevRopeSegments = new Vector3[segmentCount];
        for (int i = 0; i < segmentCount; i++) {
            prevRopeSegments[i] = ropeSegments[i];
        }
        collisionFrame = GetComponent<EdgeCollider2D>();
        collisionFrame.edgeRadius = lineWidth / 2f;
    }

    void DrawRope() {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = segmentCount;
        lineRenderer.SetPositions(ropeSegments);

        Vector2[] points = new Vector2[segmentCount];
        for (int i = 0; i < segmentCount; i++) {
            points[i] = (Vector2)ropeSegments[i];
        }

        collisionFrame.points = points;
    }

    public float swingDirection = 1f;
    private float timeInterval = 1.5f;
    public float swingTime = 3f;

    void _Swing() {
        timeInterval += Time.fixedDeltaTime;
        if (timeInterval >= swingTime) {
            timeInterval = 0f;
            swingDirection *= -1f;
        }

        //float force = 50f;
        //float distForce = force * dist;
        //for (int i = segmentCount - 1; i >= 1; i--) {
        //    // move
        //    Vector3 norm = (ropeSegments[i] - ropeSegments[i - 1]).normalized;
        //    Vector3 rotated = new Vector3(-norm.y, norm.x, 0);

        //    // makes a cool spiral pattern.
        //    //ropeSegments[i] = ropeSegments[i] + rotated * swingDirection * Time.fixedDeltaTime * i;
        //    //Vector3 newNorm = (ropeSegments[i] - ropeSegments[i - 1]).normalized;
        //    //ropeSegments[i] = ropeSegments[i - 1] + newNorm * dist;

        //    // makes a cool box pattern. // fun values are force = 100f
        //    ropeSegments[i] = ropeSegments[i] + rotated * swingDirection * Time.fixedDeltaTime * distForce;
        //    float newDist = Vector3.Distance(ropeSegments[i], ropeSegments[i - 1]);
        //    Vector3 newNorm = (ropeSegments[i] - ropeSegments[i - 1]).normalized;
        //    ropeSegments[i] = ropeSegments[i - 1] + newNorm * dist;
        //    distForce = Mathf.Max(0, newDist - dist) * force;

        //}

    }

    void Simulation() {

        Vector3 forceGravity = new Vector3(0f, -1.5f, 0f);

        for (int i = 1; i < segmentCount; i++) {
            Vector3 velocity = ropeSegments[i] - prevRopeSegments[i];
            prevRopeSegments[i] = ropeSegments[i];
            ropeSegments[i] += velocity;
            ropeSegments[i] += forceGravity * Time.fixedDeltaTime;
        }

        for (int i = 0; i < 50; i++) {
            Constraints();
        }

    }

    void Constraints() {

        for (int i = 1; i < segmentCount; i++) {

            float newDist = (ropeSegments[i-1] - ropeSegments[i]).magnitude;
            Vector3 changeDir = (ropeSegments[i - 1] - ropeSegments[i]).normalized;

            float error = newDist - dist;
            Vector3 changeAmount = changeDir * error;

            if (i != 1) {
                ropeSegments[i - 1] -= changeAmount * 0.5f;
            }
            ropeSegments[i] += changeAmount * 0.5f;
        }
    }

}
