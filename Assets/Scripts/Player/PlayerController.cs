using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{   
    float minx = -35f;
    float maxx = 35f;
    float miny = -45f;
    float maxy = 45f;

    float x = 0;
    float y = 0;

    float basex = 0;

    float fov = 60;
    float minfov = 20;
    float maxfov = 60;


    float turnSpeed = 45f;

    public Camera playerCamera;

    private PlayerControls playerControls;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    Ray ray;
    RaycastHit hit;

    // Update is called once per frame
    void Update() {

        ray = playerCamera.ScreenPointToRay(Input.mousePosition);
	    if(Physics.Raycast(ray, out hit)) {
            if (hit.collider.name.StartsWith("door")) {
                if (playerControls.Inside.LeftClick.triggered) {
                    if (basex == 0) {
                        transform.Translate(0, 0, 10);
                    }
                    if (basex == 180 || basex == -180) {
                        transform.Translate(0, 0, -10);
                    }
                    if (basex == 90 || basex == -270) {
                        transform.Translate(10, 0, 0);
                    }
                    if (basex == 270 || basex == -90) {
                        transform.Translate(-10, 0, 0);
                    }
                }
            }
	    }
        
        if (playerControls.Inside.TurnLeft.triggered) {
            Debug.Log("Turning Left");
            HandleLeftTurn();
        }
        if (playerControls.Inside.TurnRight.triggered) {
            Debug.Log("Turning Right");
            HandleRightTurn();
        }
        fov -= playerControls.Inside.Zoom.ReadValue<float>() * 0.05f;
        fov = Mathf.Clamp(fov, minfov, maxfov);
        playerCamera.fieldOfView = fov;

        HandleRotation();
    }

    void HandleRotation() {
        float mouseXRotation = playerControls.Inside.Look.ReadValue<Vector2>().x;
        float mouseYRotation = playerControls.Inside.Look.ReadValue<Vector2>().y;

        x += mouseXRotation;
        y += mouseYRotation;

        x = Mathf.Clamp(x, basex + minx, basex + maxx);
        y = Mathf.Clamp(y, miny, maxy);

        // transform.Rotate(0, mouseXRotation, 0);

        // mouseYRotation = Mathf.Clamp(mouseYRotation, -45f, 45f);
        //transform.localRotation = Quaternion.Euler(mouseYRotation, 0, 0);
        // playerCamera.transform.Rotate(mouseYRotation, 0, 0);
        playerCamera.transform.localRotation = Quaternion.Euler(y, x, 0);
    }

    void HandleLeftTurn() {
        basex -= 90;
        basex %= 360;
        StartCoroutine(TurnAnimation());
    }

    void HandleRightTurn() {
        basex += 90;
        basex %= 360;
        StartCoroutine(TurnAnimation());
    }

    IEnumerator TurnAnimation() {
        OnDisable();

        while (x != basex){
            x = Mathf.MoveTowards(x, basex, turnSpeed * Time.deltaTime);
            playerCamera.transform.localRotation = Quaternion.Euler(y, x, 0);
            yield return null;
        }
        OnEnable();
    }
}
