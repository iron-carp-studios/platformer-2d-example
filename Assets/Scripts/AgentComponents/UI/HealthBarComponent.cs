using IronCarpStudios.AES.Agents;
using IronCarpStudios.AES.Events;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarComponent : AgentComponent
{

    public Image fill;
    public float CurrentFillAmount;

    protected override void Subscribe()
    {
        GlobalEvent.AddListener(new EventRegistrationData(UiEvent.PlayerHealthChanged.ToString(), OnPlayerHealthChanged));
    }

    protected override void Unsubscribe()
    {
        GlobalEvent.RemoveListener(new EventRegistrationData(UiEvent.PlayerHealthChanged.ToString(), OnPlayerHealthChanged));
    }

    private void OnPlayerHealthChanged(Agent sender, AgentEventArgs args)
    {
        UiBarChangedEventArgs parameter = args as UiBarChangedEventArgs;

        var maxValue = parameter?.MaximumValue ?? 1;
        var current = parameter?.CurrentValue ?? maxValue;

        CurrentFillAmount = GetRemainingHealthAsPercentage(current, maxValue);
        fill.fillAmount = CurrentFillAmount;
    }

    private float GetRemainingHealthAsPercentage(int current, int max)
    {
        float percentage = (float)current / (float)max;
        return Mathf.Clamp(percentage, 0f, 1f);
    }
}
