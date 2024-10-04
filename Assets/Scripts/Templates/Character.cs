using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    public int Health;
    public int MaxHealth;
    public int Shield;
    public int Strength;
    public bool TurnEnded = false;

    [SerializeField] Slider healthbar;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Image shieldIcon;
    [SerializeField] TMP_Text shieldText;
    [SerializeField] Image strengthIcon;
    [SerializeField] TMP_Text strengthText;

    protected List<TemporaryEffect> activeEffects = new List<TemporaryEffect>();

    protected virtual void Awake()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        healthbar.minValue = 0;
        healthbar.maxValue = MaxHealth;
        healthbar.value = Health;

        healthText.text = Health + "/" + MaxHealth;

        shieldIcon.gameObject.SetActive(Shield > 0);
        shieldText.text = Shield.ToString();

        strengthIcon.gameObject.SetActive(Strength > 0);
        strengthText.text = Strength.ToString();
    }

    // Apply damage, considering shield
    public virtual void TakeDamage(int amount)
    {
        int finalDamage = Mathf.Max(0, amount - Shield);
        Health -= finalDamage;
        Shield = Mathf.Max(0, Shield - amount);
        UpdateUI();
    }

    public virtual void GainHealth(int amount)
    {
        Health = Mathf.Min(MaxHealth, Health + amount);
        UpdateUI();
    }

    public virtual void GainShield(int amount, int duration)
    {
        Shield += amount;
        AddTemporaryEffect(new TemporaryEffect(EffectType.Shield, amount, duration));
        UpdateUI();
    }

    public virtual void ModifyStrength(int amount, int duration)
    {
        Strength += amount;
        AddTemporaryEffect(new TemporaryEffect(EffectType.Strength, amount, duration));
        UpdateUI();
    }

    // Adds temporary effects for persistent effects like strength or shield
    public void AddTemporaryEffect(TemporaryEffect effect)
    {
        activeEffects.Add(effect);
        UpdateUI();
    }

    // Called at the start of each turn to reduce effect durations
    public virtual void TickEffects()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].Tick();
            if (activeEffects[i].IsExpired)
            {
                RemoveEffect(activeEffects[i]);
                activeEffects.RemoveAt(i);
            }
        }
        UpdateUI();
    }

    // Remove effect when duration ends
    protected virtual void RemoveEffect(TemporaryEffect effect)
    {
        switch (effect.EffectType)
        {
            case EffectType.Shield:
                Shield -= effect.Value;
                break;
            case EffectType.Strength:
                Strength -= effect.Value;
                break;
            default:
                break;
        }
        UpdateUI();
    }

    public abstract void PlayTurn(); // Implemented by Player and Enemy
}
