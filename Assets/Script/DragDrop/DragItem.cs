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
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null)
                Debug.Log("Click trúng: " + hit.collider.name);
        }
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
        foreach (var hit in hits)
        {
            Debug.Log("Hit: " + hit.name);
        }
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

    private void TryDrop()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);

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

        StartCoroutine(Return());
    }
    private void TryDropToBin()
    {
        Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapPointAll(pos);

        foreach (var hit in hits)
        {
            CollectionBin bin = hit.GetComponent<CollectionBin>() ?? hit.GetComponentInParent<CollectionBin>();

            if (bin != null)
            {
                bool success = bin.TryCollect(data);

                if (success)
                {
                    Destroy(gameObject); // ✅ bán xong → biến mất
                }
                else
                {
                    StartCoroutine(Return());
                }

                return;
            }
        }

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