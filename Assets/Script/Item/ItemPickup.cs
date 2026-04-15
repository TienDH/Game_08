using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData data;

    public void Pick(Bag bag)
    {
        if (bag == null || data == null) return;

        bag.AddItem(data);
        Destroy(gameObject);
    }
}
