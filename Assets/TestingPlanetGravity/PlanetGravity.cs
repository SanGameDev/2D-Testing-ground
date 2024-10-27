using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public float gravity = 0;
    public float jumpForceMultiplier;

    public void Attract(Transform player)
    {
        player.GetComponent<PlayerMovement>().jumpForceMultiplier = jumpForceMultiplier;

        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.position - transform.position).normalized;
        Vector3 bodyUp = player.up;

        //playerRB.AddForce(-direction * gravity);
        Debug.DrawRay(player.position, -direction * gravity, Color.green, 0.1f);
        playerRB.gravityScale = 0;

        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, direction) * player.rotation;
        player.rotation = Quaternion.Slerp(player.rotation, targetRotation, 50 * Time.fixedDeltaTime);
    }
}
