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

    void Start()
    {
        if (!instance)
            instance = this;
    }

    public void EnablePlanetOverlay(Planet planet)
    {
        m_planetNameText.text = planet.GetName();
        m_planetOverlayCanvasObject.SetActive(true);
    }

    public void DisablePlanetOverlay()
    {
        m_planetOverlayCanvasObject.SetActive(false);
    }
}