using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    public bool IsPlayer;
    public string CharacterName;
    public int Score;
    public bool TurnEnded = false;
    public int Health;
    public int MaxHealth;
    public int Shield;
    public int Ammo;
    public int MaxAmmo;

    [SerializeField] private List<IDurationalEffect> activeEffects = new List<IDurationalEffect>();

    [SerializeField] Slider healthbar;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Image shieldIcon;
    [SerializeField] TMP_Text shieldText;
    [SerializeField] Image strengthIcon;
    [SerializeField] TMP_Text strengthText;

    [SerializeField] Animator animator;

    protected virtual void Awake()
    {
        if (!IsPlayer) Health = MaxHealth;
        UpdateUI();
    }

    public void UpdateUI()
    {
        healthbar.minValue = 0;
        healthbar.maxValue = MaxHealth;
        healthbar.value = Health;

        healthText.text = Health + "/" + MaxHealth;

        shieldIcon.gameObject.SetActive(Shield > 0);
        shieldText.text = Shield.ToString();
    }

    public void ApplyDurationalEffects()
    {
        foreach (var effect in activeEffects.ToArray()) // ToArray to avoid modifying during iteration
        {
            if (effect.IsExpired())
            {
                Debug.Log($"Effect expired: {effect.GetType()}");
                RemoveDurationalEffect(effect);
                continue;
            }

            StartCoroutine(effect.Apply(this, this)); // Assuming the target is self for now
            effect.DecrementDuration();

            // Debug log to track the effect's duration
            Debug.Log($"Effect: {effect.GetType()}, Remaining Duration: {effect.GetDuration()}");

        }

        // Update UI after all effects have been applied
        UpdateUI();
    }



    // Add a durational effect
    public void AddDurationalEffect(IDurationalEffect effect)
    {
        activeEffects.Add(effect);
    }

    private void RemoveDurationalEffect(IDurationalEffect effect)
    {
        if (effect is ShieldEffect shieldEffect)
        {
            Debug.Log($"Removing shield effect with value: {shieldEffect.shieldValue}. Current Shield: {Shield}. On Player {IsPlayer}");

            // Decrease the shield value by the shield effect's value
            Shield = Mathf.Max(0, Shield - shieldEffect.shieldValue);

            Debug.Log($"New Shield Value: {Shield}");

            // Update the UI after the shield has been modified
            UpdateUI();

            // Trigger any shield removal animations if necessary
            // TriggerAnimation("ShieldExpired");
        }

        // Remove the effect from the list of active effects
        activeEffects.Remove(effect);
    }




    public virtual void TakeDamage(int amount)
    {
        if (Shield > 0)
        {
            int damageToShield = Mathf.Min(Shield, amount);
            Shield -= damageToShield;

            int remainingDamage = amount - damageToShield;
            Health -= remainingDamage;
        }
        else
        {
            Health -= amount;
        }

        Health = Mathf.Max(0, Health);
    }

    public virtual void TakePenetratingDamage(int amount)
    {
        Health -= amount;
        Health = Mathf.Max(0, Health);
    }

    public void TriggerAnimation(string animationName)
    {
        animator.SetTrigger(animationName);
    }
}
