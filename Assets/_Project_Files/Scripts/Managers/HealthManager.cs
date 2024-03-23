using System;
using UnityEngine;

public static class HealthManager
{
    public static event Action<GameObject, int> OnHealthChanged;
    public static event Action<GameObject> OnEntityDeath;

    public static void RaiseHealthChanged(GameObject entity, int newHealth)
    {
        OnHealthChanged?.Invoke(entity, newHealth);
    }

    public static void RaiseEntityDeath(GameObject entity)
    {
        OnEntityDeath?.Invoke(entity);
    }
}
