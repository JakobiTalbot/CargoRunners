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
    [SerializeField]
    private TextMeshProUGUI m_cargoBuyPrice;
    [SerializeField]
    private TextMeshProUGUI m_cargoSellPrice;

    public void LoadCargo(Cargo cargo)
    {
        m_cargoName.text = cargo.GetName();
        m_cargoIcon.texture = cargo.GetIcon();
        m_cargoBuyPrice.text = cargo.GetBuyValue().ToString();
        m_cargoSellPrice.text = cargo.GetSellValue().ToString();
    }
}