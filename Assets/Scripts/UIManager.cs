using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private GameObject m_planetOverlayCanvasObject;
    [SerializeField]
    private TextMeshProUGUI m_planetNameText;
    [SerializeField]
    private RectTransform m_cargoUIContent;
    [SerializeField]
    private CargoUI[] m_cargoUIElements;
    [SerializeField]
    private float m_cargoUIHeight = 128f;

    void Start()
    {
        if (!instance)
            instance = this;
    }

    public void EnablePlanetOverlay(Planet planet)
    {
        m_planetNameText.text = planet.GetName();
        LoadCargo(planet);

        m_planetOverlayCanvasObject.SetActive(true);
    }

    /// <summary>
    /// Loads the specified planet's available cargo into the cargo UI slots
    /// </summary>
    /// <param name="planet">The planet to copy the cargo from.</param>
    private void LoadCargo(Planet planet)
    {
        Cargo[] availableCargo = planet.GetAvailableCargo();
        for (int i = 0; i < availableCargo.Length; ++i)
        {
            m_cargoUIElements[i].enabled = true;
            m_cargoUIElements[i].LoadCargo(availableCargo[i]);
        }
        for (int i = availableCargo.Length; i < m_cargoUIElements.Length; ++i)
        {
            m_cargoUIElements[i].enabled = false;
        }
        Vector2 newSizeDelta = m_cargoUIContent.sizeDelta;
        newSizeDelta.y = availableCargo.Length * m_cargoUIHeight;
        m_cargoUIContent.sizeDelta = newSizeDelta;
    }

    public void DisablePlanetOverlay()
    {
        m_planetOverlayCanvasObject.SetActive(false);
    }
}