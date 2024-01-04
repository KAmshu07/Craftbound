using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float distance = 10.0f; // Distance from the character
    [SerializeField] private float height = 1.5f; // Height above the character's head
    [SerializeField] private float smoothSpeed = 10.0f; // Smoothness of camera movement
    [SerializeField] private float rotationSpeed = 5.0f; // Speed of camera rotation
    [SerializeField] private Vector2 pitchMinMax = new Vector2(-40, 85); // Minimum and maximum pitch angles
    [SerializeField] private float zoomSpeed = 2.0f; // Speed of zooming in/out
    [SerializeField] private float minDistance = 2.0f; // Minimum distance from the character
    [SerializeField] private float maxDistance = 20.0f; // Maximum distance from the character
    [SerializeField] private LayerMask collisionLayers; // Layers to consider for camera collision detection
    [SerializeField] private float collisionOffset = 0.2f; // Offset to avoid camera clipping into objects
    [SerializeField] private float FOVOffset = 10.0f; // Field of view offset based on distance

    private Transform target; // The character's transform to follow
    private float yaw; // Camera rotation around the character
    private float pitch; // Camera rotation up and down
    private float currentDistance; // Current distance from the character
    private float desiredFOV; // Desired field of view
    private float originalFOV; // Original field of view
    private Vector3 cameraOffset; // Offset from the character's head position

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; // Lock cursor to the center of the screen
        //Cursor.visible = false; // Hide the cursor

        SetTarget(); // Set the target to the player

        currentDistance = distance;
        desiredFOV = Camera.main.fieldOfView;
        originalFOV = desiredFOV;

        cameraOffset = new Vector3(0, height, 0); // Set the camera offset to the desired height above the character's head
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("No target assigned to the camera controller.");
            return;
        }

        // Calculate the desired rotation based on player input
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        // Calculate the desired distance based on zoom input
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        currentDistance -= zoomInput * zoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        // Calculate the desired position for the camera
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = new Vector3(0, 0, -currentDistance);
        Vector3 desiredPosition = target.position + target.TransformDirection(cameraOffset) + (rotation * offset);

        // Perform collision detection to avoid camera clipping into objects
        RaycastHit hitInfo;
        if (Physics.Linecast(target.TransformPoint(cameraOffset), desiredPosition, out hitInfo, collisionLayers))
        {
            desiredPosition = hitInfo.point + hitInfo.normal * collisionOffset;
        }

        // Apply camera smoothing using SmoothDamp
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Update the camera position and rotation
        transform.position = smoothedPosition;
        transform.LookAt(target.position + target.TransformDirection(cameraOffset));

        // Adjust the field of view based on distance
        desiredFOV = originalFOV + FOVOffset * (currentDistance - distance) / (maxDistance - distance);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, desiredFOV, smoothSpeed * Time.deltaTime);
    }

    // Get Player with the player tag
    public void SetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}