using UnityEngine;

// !!! Credit: Unity in Action by Joesph Hocking, Chapter 2 for mouse camera control code
public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float horizontalSensitivity = 9.0f;
    public float verticalSensitivity = 9.0f;

    public float minimumVertical = -45.0f;
    public float maximumVertical = 45.0f;

    private float verticalRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * horizontalSensitivity, 0);
        }
        else if (axes == RotationAxes.MouseY)
        {
            verticalRotation -= Input.GetAxis("Mouse Y") * verticalSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, minimumVertical, maximumVertical);

            float horizontalRotation = transform.localEulerAngles.y;

            transform.localEulerAngles = new Vector3(verticalRotation, horizontalRotation, 0);
        }
        else
        {
            verticalRotation -= Input.GetAxis("Mouse Y") * verticalSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, minimumVertical, maximumVertical);

            float delta = Input.GetAxis("Mouse X") * horizontalSensitivity;
            float horizontalRot = transform.localEulerAngles.y + delta;

            transform.localEulerAngles = new Vector3(verticalRotation, horizontalRot, 0);
        }
    }
}
