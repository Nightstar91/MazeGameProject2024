using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class PlayerMovement : MonoBehaviour
{
    // States enum
    public enum PlayerState
    {
        STATE_WALKING,
        STATE_SPRINTING,
        STATE_DEAD
    }

    // Declaring variable
    public PlayerState playerState = PlayerState.STATE_WALKING;

    // Variable involving regular movement
    public float speed = 6.0f;
    public float speedMultiplier = 1f;
    public float gravity = -9.8f;

    // Variable involving sprinting
    public bool isSprinting = false;
    public float stamina = 1.0f;
    private const float sprintSpeed = 1.75f;
    private const float regenerateStamina = 0.09375f;
    private const float degenerateStamina = 0.125f;

    private CharacterController charController;
    private Player player;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        switch (playerState)
        {
            // Default state
            case PlayerState.STATE_WALKING:
                // regular movement using the base movement speed
                HandleMovement();
                StaminaRegenerate();

                // TESTING FOR DEADSTATE
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    player.didPlayerDie = true;
                }

                if (isSprinting == true)
                {
                    playerState = PlayerState.STATE_SPRINTING;
                }
                else if (player.didPlayerDie == true)
                {
                    playerState = PlayerState.STATE_DEAD;
                }

                break;

            case PlayerState.STATE_SPRINTING:
                // Overloaded method that handle movement while sprinting
                HandleMovement(sprintSpeed);
                StaminaDegenerate();

                if (isSprinting == false)
                {
                    playerState = PlayerState.STATE_WALKING;
                }
                else if (player.didPlayerDie == true)
                {
                    playerState = PlayerState.STATE_DEAD;
                }

                Debug.Log("SPRINT!");
                break;

            case PlayerState.STATE_DEAD:
                // Implement function so that the player is considered dead
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    player.didPlayerDie = false;
                    playerState = PlayerState.STATE_WALKING;
                }
                break;
        }


    }

    private void OnEnable()
    {
        
    }

    private void OnDisable ()
    {

    }

    private void Sprint()
    {

    }


    private void HandleMovement()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed; 
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement.y = gravity;

        movement *= Time.deltaTime * speedMultiplier;
        movement = transform.TransformDirection(movement);
        charController.Move(movement);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    private void HandleMovement(float sprintSpeedAmount)
    {
        // Code handling movement while sprinting
        float deltaX = Input.GetAxis("Horizontal") * speed; 
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement.y = gravity;

        // Checking to see if the player has stamina
        if(stamina > 0)
        {
            // if so, use the sprint speed
            movement *= Time.deltaTime * speedMultiplier * sprintSpeedAmount;
        }
        else
        {
            // otherwise use regular walking speed
            movement *= Time.deltaTime * speedMultiplier;
        }


        movement = transform.TransformDirection(movement);
        charController.Move(movement);



        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    private void StaminaRegenerate()
    {
        // Only regenerate stamina when the player is not sprinting
        if(isSprinting == false)
        {
            // Making sure stamina doesn't go beyond the cap of 1
            if (stamina >= 0f && stamina < 1f)
            {
                stamina += Time.deltaTime * regenerateStamina;
            }
            else
            {
                stamina = 1f;
            }

        }
    }

    private void StaminaDegenerate()
    {
        if (isSprinting == true)
        {
            stamina -= Time.deltaTime * degenerateStamina;

            // Cap the stamina at 0 
            if (stamina < 0)
            {
                stamina = 0;
            }
        }
    }
}