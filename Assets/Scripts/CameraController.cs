using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float m_secondsForLerpToPlanetFocus = 1f;

    [Header("Pan")]
    [SerializeField]
    private float m_panSpeedCoefficient = 1f;

    [Header("Zoom")]
    [SerializeField]
    private float m_zoomSpeedCoefficient = 1f;
    [SerializeField]
    private float m_minCameraZoomFOV = 10f;
    [SerializeField]
    private float m_maxCameraZoomFOV = 90f;

    [Header("Touch")]
    [SerializeField]
    private float m_touchDistanceThreshold = 5f;

    private Transform m_transform;
    private Camera m_camera;

    private Vector3 m_touchStartPos;
    private Vector3 m_panDiff;
    private float m_zoomDiff;
    private Vector3 m_positionBeforeFocus;
    private Quaternion m_rotationBeforeFocus;
    private float m_lastPinchDifference;
    private bool m_bCanMoveCamera = true;
    private int m_panTouchID;

    public static CameraController instance;

    void Awake()
    {
        m_transform = transform;
        m_camera = Camera.main;
        if (!instance)
            instance = this;
    }

    void Update()
    {
        TouchInput();

        m_transform.position -= m_panDiff;

        // TODO: math to minus from zoom diff to reach perfect FOV min/max val
        if ((m_camera.fieldOfView + m_zoomDiff) <= m_maxCameraZoomFOV
            && (m_camera.fieldOfView + m_zoomDiff >= m_minCameraZoomFOV))
        {
            m_camera.fieldOfView += m_zoomDiff;
        }

    }

    private void FixedUpdate()
    {
        m_panDiff *= 0.9f;
        m_zoomDiff *= 0.9f;
    }

    private void TouchInput()
    {
        if (m_bCanMoveCamera)
        {
            // pinch zoom
            if (Input.touchCount >= 2)
            {
                Touch t1 = Input.GetTouch(0);
                Touch t2 = Input.GetTouch(1);

                if (t2.phase == TouchPhase.Began)
                {
                    m_lastPinchDifference = Vector3.Distance(t1.position, t2.position);
                }
                else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
                {
                    float dist = Vector3.Distance(t1.position, t2.position);
                    m_zoomDiff -= ((dist - m_lastPinchDifference) * m_zoomSpeedCoefficient * Time.deltaTime);

                    m_lastPinchDifference = dist;
                }
            }
            else if (Input.touchCount == 1)
            {
                Touch t1 = Input.GetTouch(0);

                switch (t1.phase)
                {
                    case TouchPhase.Began:
                    {
                            m_panTouchID = t1.fingerId;
                            m_touchStartPos = t1.position;

                            // stop zooming
                            m_zoomDiff = 0f;

                            break;
                    }
                    case TouchPhase.Moved:
                    {
                            if (t1.fingerId != m_panTouchID)
                                break;

                            m_panDiff = new Vector3(t1.deltaPosition.x, 0, t1.deltaPosition.y) * m_transform.position.y * m_panSpeedCoefficient * Time.deltaTime;

                            break;
                    }
                    case TouchPhase.Ended:
                    {
                        if (t1.fingerId != m_panTouchID)
                            break;

                        if (Vector3.Distance(t1.position, m_touchStartPos) < m_touchDistanceThreshold)
                        {
                                // user has tapped
                                ProcessTap(t1.position);
                        }

                        break;
                    }
                }
            }
        }
    }

    private void ProcessTap(Vector3 tapPos)
    {
        RaycastHit hit;
        if (Physics.Raycast(m_camera.ScreenPointToRay(tapPos), out hit))
        {
            if (hit.collider.GetComponent<Planet>())
            {
                StartCoroutine(FocusPlanet(hit.collider.GetComponent<Planet>(), m_secondsForLerpToPlanetFocus));
            }
        }
    }

    /// <summary>
    /// Function to lerp camera from position when coroutine is started to designated position and rotation in seconds amount of time.
    /// Disables camera movement on start.
    /// </summary>
    /// <param name="targetPos">Target position to lerp to.</param>
    /// <param name="targetRot">Target rotation to lerp to.</param>
    /// <param name="seconds">Seconds to take lerping to the target.</param>
    public IEnumerator LerpCamera(Vector3 targetPos, Quaternion targetRot, float seconds)
    {
        m_bCanMoveCamera = false;
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

    private IEnumerator FocusPlanet(Planet planet, float secondsUntilEnablingUI)
    {
        m_positionBeforeFocus = m_transform.position;
        m_rotationBeforeFocus = m_transform.rotation;

        Invoke("FocusPlanet", m_secondsForLerpToPlanetFocus);

        Transform t = planet.GetFocusAngle();
        StartCoroutine(LerpCamera(t.position, t.rotation, m_secondsForLerpToPlanetFocus));

        // wait for seconds until enabling UI
        yield return new WaitForSeconds(secondsUntilEnablingUI);

        UIManager.instance.EnablePlanetOverlay(planet);
    }

    /// <summary>
    /// Hides planet overlay and lerps camera back to position before focusing on planet.
    /// </summary>
    public void UnfocusPlanet()
    {
        UIManager.instance.DisablePlanetOverlay();
        StartCoroutine(LerpCamera(m_positionBeforeFocus, m_rotationBeforeFocus, m_secondsForLerpToPlanetFocus));
        Invoke("EnableCameraMovement", m_secondsForLerpToPlanetFocus);
    }

    public void DisableCameraMovement()
    {
        m_bCanMoveCamera = false;
        m_panDiff = Vector3.zero;
        m_zoomDiff = 0f;
    }

    public void EnableCameraMovement()
    {
        m_bCanMoveCamera = true;
    }
}