using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    [Header("References")]
    public Bag homeBag;

    [Header("Drop Area")]
    public Transform dropArea;
    public Vector2 dropAreaSize = new Vector2(5.5f, 4f);

    [Header("Hiệu ứng đổ rác")]
    public GameObject itemVisualPrefab;
    public float spawnDelay = 0.08f;
    public float fallHeight = 1.5f;
    public float fallDuration = 0.38f;

    [Header("Hiệu ứng túi")]
    public float tiltAngle = 65f;
    public float tiltDuration = 0.28f;
    public float waitBeforeReturn = 0.8f;
    public float returnDuration = 0.5f;

    private Vector3 originalBagPosition;
    private Quaternion originalBagRotation;

    private bool isDumping = false;

    private void Start()
    {
        if (homeBag != null)
        {
            originalBagPosition = homeBag.transform.position;
            originalBagRotation = homeBag.transform.rotation;
        }

        LoadBagData();
    }

    private void LoadBagData()
    {
        if (homeBag == null || GameManager.Instance == null) return;
        homeBag.LoadFromData();
    }

    private void Update()
    {
        if (isDumping) return;

        // Thả chuột
        if (Input.GetMouseButtonUp(0))
        {
            if (homeBag == null) return;

            if (IsBagInDropArea())
            {
                var data = GameManager.Instance?.bagData;

                if (data != null && data.items.Count > 0)
                {
                    StartCoroutine(DumpSequence());
                }
                else
                {
                    Debug.Log("Túi rỗng!");
                }
            }
        }
    }

    private bool IsBagInDropArea()
    {
        if (dropArea == null || homeBag == null) return false;

        Vector2 local = (Vector2)homeBag.transform.position - (Vector2)dropArea.position;

        return Mathf.Abs(local.x) <= dropAreaSize.x / 2 &&
               Mathf.Abs(local.y) <= dropAreaSize.y / 2;
    }

    // ================== MAIN FLOW ==================
    private IEnumerator DumpSequence()
    {
        isDumping = true;

        // disable drag
        if (homeBag.TryGetComponent(out DragItem drag))
        {
            drag.enabled = false;
        }

        var bagData = GameManager.Instance?.bagData;
        if (bagData == null)
        {
            Debug.LogError("[HomeManager] BagData null!");
            yield break;
        }

        // copy data
        List<ItemData> itemsToDump = new List<ItemData>(bagData.items);

        Vector3 dumpPos = homeBag.transform.position;

        // 1. nghiêng túi
        yield return StartCoroutine(TiltBag());

        // 2. clear data + UI
        homeBag.ClearAll();

        // 3. spawn rác rơi
        for (int i = 0; i < itemsToDump.Count; i++)
        {
            Vector3 startPos = dumpPos + new Vector3(Random.Range(-0.65f, 0.65f), fallHeight, 0);
            Vector3 endPos = GetRandomPosition();

            StartCoroutine(FallItem(itemsToDump[i], startPos, endPos));
            yield return new WaitForSeconds(spawnDelay);
        }

        // 4. đợi
        yield return new WaitForSeconds(waitBeforeReturn);

        // 5. bay về
        yield return StartCoroutine(ReturnBag());

        Debug.Log($"✅ ĐÃ ĐỔ {itemsToDump.Count} ITEM");

        // enable drag lại
        if (homeBag.TryGetComponent(out DragItem drag2))
        {
            drag2.enabled = true;
        }

        isDumping = false;
    }

    // ================== EFFECT ==================

    private IEnumerator TiltBag()
    {
        Quaternion target = Quaternion.Euler(0, 0, tiltAngle);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / tiltDuration;
            homeBag.transform.rotation = Quaternion.Lerp(originalBagRotation, target, t);
            yield return null;
        }

        homeBag.transform.rotation = target;
    }

    private IEnumerator ReturnBag()
    {
        Vector3 startPos = homeBag.transform.position;
        Quaternion startRot = homeBag.transform.rotation;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / returnDuration;

            homeBag.transform.position = Vector3.Lerp(startPos, originalBagPosition, t);
            homeBag.transform.rotation = Quaternion.Lerp(startRot, originalBagRotation, t);

            yield return null;
        }

        homeBag.transform.position = originalBagPosition;
        homeBag.transform.rotation = originalBagRotation;
    }

    private IEnumerator FallItem(ItemData data, Vector3 start, Vector3 end)
    {
        GameObject go = Instantiate(itemVisualPrefab, start, Quaternion.identity);

        // ✅ GÁN DATA (QUAN TRỌNG NHẤT)
        var drag = go.GetComponent<DragItem>();
        if (drag != null)
        {
            drag.data = data;
        }

        // sprite
        var sr = go.GetComponent<SpriteRenderer>();
        if (sr != null && data != null)
        {
            sr.sprite = data.icon;
        }

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / fallDuration;

            float curve = 1 - Mathf.Pow(1 - t, 3);
            go.transform.position = Vector3.Lerp(start, end, curve);
            yield return null;
        }

        go.transform.position = end;
    }

    private Vector3 GetRandomPosition()
    {
        if (dropArea == null) return Vector3.zero;

        float x = Random.Range(-dropAreaSize.x / 2f, dropAreaSize.x / 2f);
        float y = Random.Range(-dropAreaSize.y / 2f, dropAreaSize.y / 2f);

        return dropArea.position + new Vector3(x, y, 0);
    }

    // ================== SCENE ==================

    public void GoToMap()
    {
        SceneManager.LoadScene("MapScene");
    }
}