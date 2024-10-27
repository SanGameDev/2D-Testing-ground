using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    public PlanetGravity attractor;
    [HideInInspector] public bool inPlanet = false;

    private void FixedUpdate() 
    {
        if (attractor != null)
        {
            GravityController();
            return;
        }
    }

    void GravityController()
    {
        attractor.Attract(transform); 
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Planet")) {
            attractor = other.GetComponent<PlanetGravity>();
            inPlanet = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Planet")) {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            attractor = null;
            inPlanet = false;
        }
    }
}