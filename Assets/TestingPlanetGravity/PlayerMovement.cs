using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables privadas
    private float horizontal;
    private bool isFacingRight = true;
    private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform frontMov;
    [SerializeField] private Transform jumpMov;
    
    public bool grounded;
    public float speed = 7.0f, maxSpeed, acceleration, decceleration;
    public float jetPackForce; //variable que controla la fuerza del jet pack
    public int jetPackCount; //variable que define el numero de "saltos" que el jugador hizo 

    //Variables publicas
    [Header("Movement Settings")]
    //public float speed = 8f;
    public float jumpingPower = 16f;


    private PlayerGravity playerPlanetInteraction;

    [Space(10)]
    public float jumpForceMultiplier;

    //private Animator animatorPlayer;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //animatorPlayer = GetComponent<Animator>(); 
        playerPlanetInteraction = GetComponent<PlayerGravity>();
    }

    void Update()
    {
        //Obtener los inputs respectivos de movimiento horizontal (0 o 1, izquierda o derecha)
        horizontal = Input.GetAxisRaw("Horizontal");
        //animatorPlayer.SetFloat("Moving", Mathf.Abs(horizontal));
        Jump();
        Flip();
        JetPack();

        if (!grounded && IsGrounded())
        {
            jetPackCount = 0;
            //animatorPlayer.SetBool("Jump", false);
            grounded = true;
        }
    }

    private void FixedUpdate()
    {
        //animatorPlayer.SetBool("IsGrounded", IsGrounded());

        if (horizontal != 0 && IsGrounded()) 
        { 
            transform.position = Vector2.MoveTowards(transform.position, frontMov.position, speed * Time.deltaTime);
        }
        else if(horizontal != 0 && !IsGrounded())
        {
            transform.position = Vector2.MoveTowards(transform.position, frontMov.position, speed / 1.75f * Time.deltaTime);
        }
    }

    //Funcion para rotar al personaje en funcion a la direccion en la que se mueve
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;

            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    //Funcion booleana que identifica si el jugador esta tocando el suelo (groundLayer)
    public bool IsGrounded()
    {
        //jetPackCount = 0;
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        
    }

    //Funcion para que el jugador salte determinando si esta en el suelo o no
    private void Jump()
    {
        //Obtener los inputs de salto y evaluar si es posible saltar
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            if(playerPlanetInteraction.inPlanet)
            {
                //Debug.Log("JUMP Planet");
                //animatorPlayer.SetBool("Jump", true);
                Vector2 direction = (jumpMov.position - playerPlanetInteraction.attractor.transform.position);
                //playerPlanetInteraction.TurnOffAtractor();
                //Debug.Log("Direction " + direction + " JumpPow " + jumpingPower + " JumpForceMult " + jumpForceMultiplier + " All multiplied " + direction * jumpingPower * jumpForceMultiplier);
                rb.AddForce(direction * jumpingPower * jumpForceMultiplier, ForceMode2D.Impulse);
                Debug.DrawLine(jumpMov.position, direction * jumpingPower * jumpForceMultiplier, Color.red, 5f);
            }
            else
            {
                //Debug.Log("JUMP");
                //animatorPlayer.SetBool("Jump", true);
                rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
            }

            StartCoroutine(JumpResetCoroutine());
        }

        //Disminuir la velocidad con la que cae el personaje al soltar el boton de salto
        if (Input.GetButton("Jump") && rb.velocity.y < 0)
        {
            Vector2 rbVel;
            rbVel.y = Mathf.Lerp(rb.velocity.y, -2.5f, 0.25f * Time.deltaTime);
            rb.velocity = Mathf.Clamp(rbVel.y, -2.5f, 0) * Vector2.up;

            //Debug.Log("SlowFall" + rb.velocity.y);
        }
    }

    private void JetPack()
    {
        /*Implementacion del Jet Pack*/
        if (Input.GetKeyDown(KeyCode.Space) && !IsGrounded() && jetPackCount < 1)
        {
            if(playerPlanetInteraction.inPlanet)
            {
                //Debug.Log("JETPACK Planet");
                jetPackCount += 1;
                //animatorPlayer.SetBool("JetPack", true);
                Vector2 direction = (jumpMov.position - playerPlanetInteraction.attractor.transform.position).normalized;
                rb.AddForce(direction * jetPackForce, ForceMode2D.Impulse);
            }
            else
            {
                //Debug.Log("JETPACK");
                jetPackCount += 1;
                //animatorPlayer.SetBool("JetPack", true);
                rb.AddForce(Vector2.up * jetPackForce, ForceMode2D.Impulse);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && jetPackCount >= 1)
        {
            //animatorPlayer.SetBool("JetPack", false);
        }
    }

    IEnumerator JumpResetCoroutine()
    {
        //Coorutina que reestablece la animacion de salto
        yield return new WaitForSeconds(0.2f);

        grounded = false;
    }
}
