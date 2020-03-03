using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private GameObject m_planetOverlayCanvasObject;

    void Start()
    {
        if (!instance)
            instance = this;
    }

    void Update()
    {
        
    }

    public void EnablePlanetOverlay(Planet planet)
    {
        m_planetOverlayCanvasObject.SetActive(true);
    }

    public void DisablePlanetOverlay()
    {
        m_planetOverlayCanvasObject.SetActive(false);
    }
}