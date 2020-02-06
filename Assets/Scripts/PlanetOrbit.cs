using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    [SerializeField]
    private float m_orbitPeriod = 24;
    [SerializeField]
    private float m_speedCoefficient = 1;

    private Transform m_transform;

    void Awake()
    {
        m_transform = transform;
    }

    void Update()
    {
        m_transform.Rotate(new Vector3(0, 360f) / 3600f / m_orbitPeriod * m_speedCoefficient * Time.deltaTime);
    }
}
