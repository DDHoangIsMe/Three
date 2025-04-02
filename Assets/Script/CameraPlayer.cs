using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject player_;
    private readonly int distance_;
    private float verticalRotation = 0f;
    private Vector3 camDirect;
    private LayerMask layerMask_;
    // mono part
    public CameraPlayer() {
        distance_ = SettingData.DISTANCE_SIGHT;
    }
    void Start()
    {
        if (player_ == null) {
            player_ = transform.parent.gameObject;
        }
        camDirect = player_.transform.rotation.eulerAngles.normalized;
        layerMask_ = LayerMask.GetMask("Block");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2)) {
            if (Cursor.lockState == CursorLockMode.None) 
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else 
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        if (Cursor.lockState == CursorLockMode.Locked) {
            float mouseX = Input.GetAxis("Mouse X") * SettingData.PLAYER_SPIN * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * SettingData.PLAYER_SPIN * Time.deltaTime;
            player_.GetComponent<Player>().FacingCamera(mouseX);
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
            this.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
    }
    void FixedUpdate()
    {
        this.transform.position = player_.transform.position + camDirect;
    }

    //
    public RaycastHit GetRayHit() {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        Physics.Raycast(ray, out hit, distance_, layerMask_);
        return hit;
    }
    public Vector3 GetDirection() {
        return transform.forward;
    }
}
