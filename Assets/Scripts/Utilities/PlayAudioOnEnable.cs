using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnEnable : MonoBehaviour
{
    [SerializeField] string clipName;
    void OnEnable()
    {
        AudioManager.Instance.PlaySound(clipName);
    }
}
