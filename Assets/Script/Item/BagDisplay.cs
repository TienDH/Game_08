using TMPro;
using UnityEngine;

public class BagDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Update()
    {
        if (GameManager.Instance == null) return;

        int current = GameManager.Instance.bagData.items.Count;
        int max = 20;

        text.text = current + "/" + max;
    }
}