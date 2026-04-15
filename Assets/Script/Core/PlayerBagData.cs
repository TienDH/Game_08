using System.Collections.Generic;

[System.Serializable]
public class PlayerBagData
{
    public List<ItemData> items = new();

    public void Add(ItemData item)
    {
        items.Add(item);
    }

    public void Clear()
    {
        items.Clear();
    }
}