using UnityEngine;

/// <summary>
/// Manages map zones, regions and navigation data.
/// (To be implemented later)
/// </summary>
public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // TODO: Implement map logic
}
