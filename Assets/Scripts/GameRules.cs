using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour {

    [SerializeField] public static float GravityScale = 1f;
    [SerializeField] public static float MovementAcceleration = 5000f;
    [SerializeField] public static float MovementDamp = 0.5f;
    [SerializeField] public static float MovementPrecision = 0.05f;

    [SerializeField] public static string GroundTag = "Ground";

}
