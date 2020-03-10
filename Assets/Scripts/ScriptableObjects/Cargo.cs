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
    private int m_baseValue;

    public string GetName() => m_cargoName;
    public Texture GetIcon() => m_cargoIcon;
    public int GetValue() => m_baseValue;
}