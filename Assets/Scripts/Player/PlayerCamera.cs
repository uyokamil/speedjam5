using DG.Tweening;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;

    public Transform playerOrientation;
    public Transform camHolder;
    
    float xRotation;
    float yRotation;

    [Header("FOV")]
    public bool useFluentFov = true;
    public Rigidbody rb;
    public Camera cam;
    public float minMovementSpeed;
    public float maxMovementSpeed;
    public float minFov;
    public float maxFov;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Get the mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -87f, 87f);

        // Clamp rotation and set the rotation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);

        if (useFluentFov) HandleFov();
    }

    private void HandleFov()
    {
        float moveSpeedDif = maxMovementSpeed - minMovementSpeed;
        float fovDif = maxFov - minFov;

        float rbFlatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;
        float currMoveSpeedOvershoot = rbFlatVel - minMovementSpeed;
        float currMoveSpeedProgress = currMoveSpeedOvershoot / moveSpeedDif;

        float fov = (currMoveSpeedProgress * fovDif) + minFov;

        float currFov = cam.fieldOfView;

        float lerpedFov = Mathf.Lerp(fov, currFov, Time.deltaTime * 200);

        cam.fieldOfView = lerpedFov;
    }

    public void SetFov(float endValue)
    {
        cam.DOFieldOfView(endValue, 0.25f);
    }

    public void SetTilt(float zTilt)
    {
        camHolder.transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}
