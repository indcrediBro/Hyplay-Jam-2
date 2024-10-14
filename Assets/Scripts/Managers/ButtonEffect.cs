using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private float duration = 0.2f;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound("SFX_ButtonClick");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one * 1.1f, duration).SetEase(Ease.OutBack);
        AudioManager.Instance.PlaySound("SFX_ButtonHover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}
