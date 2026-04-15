using UnityEngine;

public class Bag : MonoBehaviour
{
    public Transform content;
    public GameObject itemPrefab;

    [Header("Layout")]
    public int column = 5;
    public float spacing = 1.2f;

    private void Start()
    {
        LoadFromData();
    }

    public void ClearAll()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.bagData.Clear();
        ClearBag();
    }

    public void ClearBag()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);
    }

    public void LoadFromData()
    {
        if (GameManager.Instance == null) return;

        var data = GameManager.Instance.bagData;

        ClearBag();

        for (int i = 0; i < data.items.Count; i++)
        {
            SpawnItemUI(data.items[i], i);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item == null || GameManager.Instance == null) return;

        GameManager.Instance.bagData.Add(item);

        // 👉 chỉ spawn thêm 1 item (không reload toàn bộ)
        int index = GameManager.Instance.bagData.items.Count - 1;
        SpawnItemUI(item, index);
    }

    private void SpawnItemUI(ItemData item, int index)
    {
        var obj = Instantiate(itemPrefab, content);

        // 👉 set vị trí dạng grid
        int row = index / column;
        int col = index % column;

        obj.transform.localPosition = new Vector3(
            col * spacing,
            -row * spacing,
            0
        );

        var ui = obj.GetComponent<ItemUI>();
        if (ui != null)
            ui.Setup(item);
    }
}