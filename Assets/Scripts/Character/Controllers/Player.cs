using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Controller {

    KeyCode jumpKey = KeyCode.Space;

    protected override void Think() {
        moveSpeed = baseSpeed;
        moveDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(jumpKey)) {
            Jump();
        }
    }

}
