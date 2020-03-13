using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tappable : MonoBehaviour
{
    /// <summary>
    /// Function called when tappable object is tapped on.
    /// </summary>
    public abstract void OnTap();
}