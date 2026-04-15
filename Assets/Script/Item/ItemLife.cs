using UnityEngine;
using System;

public class ItemLife : MonoBehaviour
{
    public ItemData data;
    public bool useLifetime = true;

    public Action onDespawn;

    private void Start()
    {
        if (!useLifetime || data == null) return;

        if (data.lifeTime > 0)
        {
            Invoke(nameof(Despawn), data.lifeTime);
        }
    }

    private void Despawn()
    {
        onDespawn?.Invoke();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        onDespawn?.Invoke();
    }
}