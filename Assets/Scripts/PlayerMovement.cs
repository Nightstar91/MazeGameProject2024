using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class PlayerMovement : MonoBehaviour
{
    public int score = 0;

    public float speed = 6.0f;
    public float gravity = -9.8f;

    public bool isSprinting = false;
    public float stamina = 1.0f;

    private CharacterController charController;

    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement.y = gravity;

        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        charController.Move(movement);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
            Debug.Log("SPRINT!");
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

    private void StaminaManagement()
    {
        if(isSprinting) 
        {
            if(stamina >= 0)
            {
                stamina -= 0.125f;
            }
        }
        else
        {
            if(stamina <= 1.0)
            {
                stamina += 0.0625f;
            }

        }
    }
}