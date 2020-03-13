using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "cargo", menuName = "ScriptableObjects/Cargo", order = 1)]
public class Cargo : ScriptableObject
{
    [SerializeField]
    private string m_cargoName;
    [SerializeField]
    private Texture m_cargoIcon;
    [SerializeField]
    private int m_baseBuyValue;
    [SerializeField]
    private int m_baseSellValue;

    public string GetName() => m_cargoName;
    public Texture GetIcon() => m_cargoIcon;
    public int GetBuyValue() => m_baseBuyValue;
    public int GetSellValue() => m_baseSellValue;
}