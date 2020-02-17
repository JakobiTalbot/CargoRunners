using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float m_secondsForLerpToPlanetFocus = 1f;

    [Header("Pan")]
    [SerializeField]
    private float m_panSpeedCoefficient = 1f;

    [Header("Zoom")]
    [SerializeField]
    private float m_zoomSpeedCoefficient = 1f;
    [SerializeField]
    private float m_minCameraYZoom = 50f;
    [SerializeField]
    private float m_maxCameraYZoom = 500f;


    private Transform m_transform;
    private Camera m_camera;


    private Vector3 m_lastPos;
    private Vector3 m_panDiff;
    private Vector3 m_zoomDiff;
    private Vector3 m_positionBeforeFocus;
    private Quaternion m_rotationBeforeFocus;
    private bool m_bCanMoveCamera = true;

    void Awake()
    {
        m_transform = transform;
        m_camera = Camera.main;
    }

    void Update()
    {
        #if UNITY_EDITOR
            MouseInput();
        #endif

        m_transform.position += m_panDiff;
        if (!(m_transform.position.y + m_zoomDiff.y < m_minCameraYZoom)
            && !(m_transform.position.y + m_zoomDiff.y > m_maxCameraYZoom))
        {
            m_transform.position += m_zoomDiff;
        }
    }

    private void FixedUpdate()
    {
        m_panDiff *= 0.9f;
        m_zoomDiff *= 0.9f;
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_bCanMoveCamera)
            {
                m_lastPos = Input.mousePosition;
                m_zoomDiff = Vector3.zero;

                RaycastHit hit;
                if (Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    Planet p;
                    if (p = hit.collider.GetComponent<Planet>())
                    {
                        // store position and rotation to return to after leaving planet focus
                        m_positionBeforeFocus = m_transform.position;
                        m_rotationBeforeFocus = m_transform.rotation;

                        DisableCameraMovement();
                        Transform angle = p.GetFocusAngle();
                        StartCoroutine(LerpCamera(angle.position, angle.rotation, m_secondsForLerpToPlanetFocus));
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (m_bCanMoveCamera)
            {
                m_panDiff = new Vector3(m_lastPos.x - Input.mousePosition.x, 0, m_lastPos.y - Input.mousePosition.y) * m_transform.position.y * m_panSpeedCoefficient * Time.deltaTime;
                m_lastPos = Input.mousePosition;
            }
        }

        // zoom
        if (m_bCanMoveCamera)
        {
            m_zoomDiff += m_transform.forward * Input.mouseScrollDelta.y * m_zoomSpeedCoefficient * Time.deltaTime;
        }

        // return to original position from planet focus
        if (!m_bCanMoveCamera && Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(LerpCamera(m_positionBeforeFocus, m_rotationBeforeFocus, m_secondsForLerpToPlanetFocus));
            Invoke("EnableCameraMovement", m_secondsForLerpToPlanetFocus);
        }
    }

    private IEnumerator LerpCamera(Vector3 targetPos, Quaternion targetRot, float seconds)
    {
        float lerp = 0f;
        Vector3 startPos = m_transform.position;
        Quaternion startRot = m_transform.rotation;

        while (lerp < 1f)
        {
            lerp += Time.deltaTime / seconds;

            m_transform.position = Vector3.Lerp(startPos, targetPos, lerp);
            m_transform.rotation = Quaternion.Lerp(startRot, targetRot, lerp);

            yield return null;
        }

        m_transform.position = targetPos;
        m_transform.rotation = targetRot;
    }

    public void DisableCameraMovement()
    {
        m_bCanMoveCamera = false;
        m_panDiff = Vector3.zero;
        m_zoomDiff = Vector3.zero;
    }

    public void EnableCameraMovement()
    {
        m_bCanMoveCamera = true;
    }
}