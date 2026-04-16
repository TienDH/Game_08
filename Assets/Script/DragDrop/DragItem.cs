using UnityEngine;
using System.Collections;

public class DragItem : MonoBehaviour
{
    public ItemData data;

    private Camera cam;
    private Vector3 startPos;
    private bool dragging;

    public float returnSpeed = 8f;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        dragging = true;
        startPos = transform.position;
    }

    private void OnMouseDrag()
    {
        if (!dragging) return;

        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;
    }

    private void OnMouseUp()
    {
        if (!dragging) return;

        dragging = false;
        TryDropAll();
    }
    private void TryDropAll()
    {
        Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapPointAll(pos);

        // 🔥 ƯU TIÊN BIN TRƯỚC
        foreach (var hit in hits)
        {
            CollectionBin bin = hit.GetComponent<CollectionBin>() ?? hit.GetComponentInParent<CollectionBin>();

            if (bin != null)
            {
                bool success = bin.TryCollect(data);

                if (success)
                {
                    Destroy(gameObject);
                }
                else
                {
                    StartCoroutine(Return());
                }

                return;
            }
        }

        // 🔥 SAU ĐÓ MỚI TỚI BAG
        foreach (var hit in hits)
        {
            Bag bag = hit.GetComponent<Bag>() ?? hit.GetComponentInParent<Bag>();

            if (bag != null)
            {
                bag.AddItem(data);
                Destroy(gameObject);
                return;
            }
        }

        // ❌ KHÔNG TRÚNG GÌ
        StartCoroutine(Return());
    }

    private IEnumerator Return()
    {
        while (Vector3.Distance(transform.position, startPos) > 0.02f)
        {
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * returnSpeed);
            yield return null;
        }

        transform.position = startPos;
    }
}
