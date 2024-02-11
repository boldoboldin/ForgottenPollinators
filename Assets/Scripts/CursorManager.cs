using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor, collectPollenCursor, collectNectarCursor, forageCursor;
    private Vector2 cursorHotspot;

    public static string cursorMode = "Default";
    public static bool somethingSelected = false;

    public static CursorManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        cursorHotspot = new Vector2(16, 16);

        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.ForceSoftware);
    }

    public void ChangeCursor(string cursorTexture)
    {
        if (cursorTexture == "Default")
        {
            Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.ForceSoftware);
        }

        if (cursorTexture == "CollectPollen")
        {
            Cursor.SetCursor(collectPollenCursor, cursorHotspot, CursorMode.ForceSoftware);
        }

        if (cursorTexture == "CollectNectar")
        {
            Cursor.SetCursor(collectNectarCursor, cursorHotspot, CursorMode.ForceSoftware);
        }

        cursorMode = cursorTexture;
    }

}
