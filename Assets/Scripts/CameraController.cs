using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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

    private Vector3 m_lastPos;

    private Vector3 m_panDiff;
    private Vector3 m_zoomDiff;

    void Awake()
    {
        m_transform = transform;
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
            m_lastPos = Input.mousePosition;
            m_zoomDiff = Vector3.zero;
        }
        else if (Input.GetMouseButton(0))
        {
            m_panDiff = new Vector3(m_lastPos.x - Input.mousePosition.x, 0, m_lastPos.y - Input.mousePosition.y) * m_transform.position.y * m_panSpeedCoefficient * Time.deltaTime;
            m_lastPos = Input.mousePosition;
        }

        // zoom
        m_zoomDiff += m_transform.forward * Input.mouseScrollDelta.y * m_zoomSpeedCoefficient * Time.deltaTime;
    }
}