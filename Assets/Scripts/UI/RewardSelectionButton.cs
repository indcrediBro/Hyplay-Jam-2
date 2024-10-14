using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSelectionButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void LateUpdate()
    {
        if (GameDataReferences.Instance.selectedRewardCard == null)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
