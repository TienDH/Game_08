using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public void Setup(ItemData data)
    {
        if (spriteRenderer != null && data != null)
        {
            spriteRenderer.sprite = data.icon;
        }
    }
}