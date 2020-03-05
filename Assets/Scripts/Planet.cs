using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private string m_planetName;
    [SerializeField]
    private Transform m_focusAngle;

    public string GetName() => m_planetName;
    public Transform GetFocusAngle() => m_focusAngle;
}