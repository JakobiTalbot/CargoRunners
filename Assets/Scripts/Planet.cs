using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private Transform m_focusAngle;

    public Transform GetFocusAngle() => m_focusAngle;
}