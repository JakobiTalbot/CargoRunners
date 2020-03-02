using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed = 10f;
    [SerializeField]
    private float m_turnSpeed = 1f;
    [SerializeField]
    private float m_yawToRollCoefficient = 1f;
    [SerializeField]
    private float m_tapThreshold = 3f;

    private Coroutine m_currentFlight;

    private Transform m_transform;
    private Camera m_camera;

    private Vector3 m_tapStartPos;
    private bool m_bIsFlying = false;

    private void Awake()
    {
        m_transform = transform;
        m_camera = Camera.main;
    }

    private void Update()
    {

    }

    public IEnumerator FlyToPlanet(Planet targetPlanet)
    {
        // TODO: ensure ship doesn't fly through other planets (steering behaviour)
        m_bIsFlying = true;

        float turnLerp = 0f;
        bool bRotationLerped = false;

        float lastYaw = m_transform.rotation.y;

        while (Vector3.Distance(m_transform.position, targetPlanet.transform.position) > (targetPlanet.transform.localScale.x + m_transform.localScale.z) * 0.6f)
        {
            Vector3 dir = (targetPlanet.transform.position - m_transform.position).normalized;
            Quaternion newRot = new Quaternion();

            if (turnLerp < 1f)
            {
                turnLerp += Time.deltaTime * m_turnSpeed;
                newRot = Quaternion.Lerp(m_transform.rotation, Quaternion.LookRotation(dir), turnLerp);
            }
            else if (!bRotationLerped)
            {
                newRot = Quaternion.LookRotation(dir);
                bRotationLerped = true;
            }

            float yawDiff = newRot.eulerAngles.y - lastYaw;
            m_transform.rotation = Quaternion.Euler(newRot.eulerAngles.x, newRot.eulerAngles.y, -yawDiff * m_yawToRollCoefficient);

            lastYaw = m_transform.rotation.eulerAngles.y;

            m_transform.position += m_transform.forward * m_moveSpeed * Time.deltaTime;

            yield return null;
        }

        m_bIsFlying = false;
    }
}