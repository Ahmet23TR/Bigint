using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform Target;
    public float Distance = 5.0f;
    public float Height = 2.0f;
    public float MouseSensitivity = 10f;
    public float WebGLMouseSensitivity = 3f;
    public float RotationSmoothTime = 0.12f;

    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;
    private float verticalRotation;
    private float horizontalRotation;

    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            MouseSensitivity = WebGLMouseSensitivity;
        }
    }

    void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * MouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -70f, 70f);

        horizontalRotation += mouseX * MouseSensitivity;

        Vector3 targetRotation = new Vector3(verticalRotation, horizontalRotation);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, RotationSmoothTime);

        transform.position = Target.position - Quaternion.Euler(currentRotation) * Vector3.forward * Distance + Vector3.up * Height;
        transform.LookAt(Target.position + Vector3.up * Height);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            Target.rotation = Quaternion.Euler(0, horizontalRotation, 0);
        }
    }

    public void OnStartFollowing()
    {
        if (Target != null)
        {
            transform.position = Target.position - Target.forward * Distance + Vector3.up * Height;
            transform.LookAt(Target.position + Vector3.up * Height);
        }
    }
}
