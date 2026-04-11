using System;
using UnityEngine;

public class OutlineController : MonoBehaviour {
    [Header("Color Settings")]
    [SerializeField] private Color baseColor = Color.white;
    [SerializeField] private float baseSize = 0.05f;
    [SerializeField] private Color borderColor = Color.black;
    [SerializeField] private float borderSize = 0.01f;

    private SpriteRenderer mainSr;
    private GameObject outlineParent;

    void Awake() {
        mainSr = GetComponent<SpriteRenderer>();
        CreateOutline();
        SetOutlineActive(false);
    }

    void CreateOutline() {
        outlineParent = new GameObject("Outline_Group");
        outlineParent.transform.SetParent(transform);
        outlineParent.transform.localPosition = Vector3.zero;

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, new Vector3(1, 1, 0), new Vector3(-1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0)};

        CreateLayers(directions, baseSize + borderSize, borderColor, -2, "Border_Black");
        CreateLayers(directions, baseSize, baseColor, -1, "Base_White");
    }

    private void CreateLayers(Vector3[] dirs, float size, Color color, int orderOffset, string layerName) {
        foreach (Vector3 dir in dirs) {
            GameObject go = new GameObject(layerName);
            go.transform.SetParent(outlineParent.transform);

            go.transform.localPosition = dir.normalized * size;

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = mainSr.sprite;
            sr.color = color;
            sr.sortingOrder = mainSr.sortingOrder + orderOffset;

            sr.material = new Material(Shader.Find("GUI/Text Shader"));
        }
    }
    public void SetOutlineActive(bool isActive) {
        if (outlineParent != null)
            outlineParent.SetActive(isActive);
    }
}