using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CargoUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_cargoName;
    [SerializeField]
    private RawImage m_cargoIcon;

    public void LoadCargo(Cargo cargo)
    {
        m_cargoName.text = cargo.GetName();
        m_cargoIcon.texture = cargo.GetIcon();
    }
}