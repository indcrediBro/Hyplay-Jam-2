using UnityEngine;
using System.Collections;

public class ContinueGameButton : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button button;
    [SerializeField] private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        button.onClick.AddListener(GameController.Instance.ContinueGame);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(GameController.Instance.ContinueGame);
    }

    private void LateUpdate()
    {
        button.interactable = PlayerPrefs.HasKey("Map");
        canvasGroup.alpha = button.interactable ? 1 : 0;
        canvasGroup.interactable = button.interactable;
    }
}
