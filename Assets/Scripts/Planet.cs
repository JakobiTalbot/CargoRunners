using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : Tappable
{
    [SerializeField]
    private string m_planetName;
    [SerializeField]
    private Transform m_focusAngle;
    [SerializeField]
    private Cargo[] m_availableCargo;

    public override void OnTap()
    {
        CameraController cc = CameraController.instance;
        StartCoroutine(CameraController.instance.FocusPlanet(this, cc.m_secondsForLerpToPlanetFocus));
    }

    public string GetName() => m_planetName;
    public Transform GetFocusAngle() => m_focusAngle;
    public Cargo[] GetAvailableCargo() => m_availableCargo;
}