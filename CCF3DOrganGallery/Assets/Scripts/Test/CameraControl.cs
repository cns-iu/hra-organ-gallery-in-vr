using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Zoom")]
    public float m_ZoomSpeed = 5f;

    [Header("Rotation")]
    public float m_RotationSpeed = 5f;

    public float m_SmoothFactor = .8f;

    public Transform target;
    private Vector3 m_CameraOffset;

    // Start is called before the first frame update
    private void Start()
    {
        m_CameraOffset = this.transform.position - target.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleZoom();
        HandleRotation();
        HandleMovement();
    }

    private void HandleZoom()
    {
        this.GetComponent<Camera>().fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * m_ZoomSpeed;
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(0))
        {
            Quaternion camTurnAngleY = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * m_RotationSpeed, Vector3.up);
            Quaternion camTurnAngleX = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * m_RotationSpeed, Vector3.forward);
            m_CameraOffset = camTurnAngleY * camTurnAngleX * m_CameraOffset;
            Vector3 newPos = target.position + m_CameraOffset;
            this.transform.position = Vector3.Slerp(this.transform.position, newPos, m_SmoothFactor);
        }
        this.transform.LookAt(target.transform);
    }

    private void HandleMovement()
    {
        if (Input.GetMouseButton(1))
        {
            //Debug.Log("clicked right");
        }
    }
}