using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelEffect : MonoBehaviour
{
    void OnEnable()
    {
        AudioManager.Instance.PlaySound("SFX_PanelOpen");
    }

    void OnDisable()
    {
        AudioManager.Instance.PlaySound("SFX_PanelClose");
    }
}
