using UnityEngine;
using System.Collections;

public class DragBag : MonoBehaviour
{
    private Camera cam;
    private bool dragging;

    private Vector3 originalPos;

    public float returnSpeed = 8f;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance == null ||
            GameManager.Instance.bagData.items.Count == 0)
        {
            Debug.Log("Túi rỗng!");
            return;
        }

        dragging = true;
        originalPos = transform.position;
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

        // ❌ nếu KHÔNG ở dropArea → quay về
        if (!IsInDropArea())
        {
            StartCoroutine(Return());
        }
    }

    private bool IsInDropArea()
    {
        // tìm HomeManager
        HomeManager hm = FindObjectOfType<HomeManager>();
        if (hm == null) return false;

        Vector2 local = (Vector2)transform.position - (Vector2)hm.dropArea.position;

        return Mathf.Abs(local.x) <= hm.dropAreaSize.x / 2 &&
               Mathf.Abs(local.y) <= hm.dropAreaSize.y / 2;
    }

    private IEnumerator Return()
    {
        while (Vector3.Distance(transform.position, originalPos) > 0.02f)
        {
            transform.position = Vector3.Lerp(transform.position, originalPos, Time.deltaTime * returnSpeed);
            yield return null;
        }

        transform.position = originalPos;
    }
}