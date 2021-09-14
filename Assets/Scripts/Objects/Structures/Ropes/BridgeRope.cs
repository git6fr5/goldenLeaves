using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeRope : Rope {

    [SerializeField] protected Transform endpoint;

    void Start() {
        edgeCollider.isTrigger = false;
    }

    protected override void Constraints() {

        ropeSegments[0] = startpoint.position;
        ropeSegments[segmentCount-1] = endpoint.position;

        for (int i = 1; i < segmentCount; i++) {
            // Get the distance and direction between the segments.
            float newDist = (ropeSegments[i - 1] - ropeSegments[i]).magnitude;
            Vector3 direction = (ropeSegments[i - 1] - ropeSegments[i]).normalized;

            // Get the error term.
            float error = newDist - SegmentLength;
            Vector3 errorVector = direction * error;

            // Adjust the segments by the error term.
            if (i != 1) {
                ropeSegments[i - 1] -= errorVector * 0.5f;
            }
            ropeSegments[i] += errorVector * 0.5f;
        }
    }
}
