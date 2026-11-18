using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Track : MonoBehaviour
{
    public List<Line> lines = new List<Line>();
    public float thickness = 0.08f;

    readonly List<LineRenderer> visuals = new List<LineRenderer>();
    Material lineMaterial;
    bool cross;

    void Awake()
    {
        ApplyShape();
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null || !keyboard.qKey.wasPressedThisFrame)
            return;

        cross = !cross;
        ApplyShape();
    }

    void ApplyShape()
    {
        Vector3[] points = cross ? CrossPoints() : CirclePoints();
        if (lines == null)
            lines = new List<Line>();

        while (lines.Count < points.Length)
            lines.Add(new Line());
        while (lines.Count > points.Length)
            lines.RemoveAt(lines.Count - 1);

        for (int i = 0; i < points.Length; i++)
        {
            lines[i].start = points[i];
            lines[i].end = points[(i + 1) % points.Length];
        }
    }

    static Vector3[] CirclePoints()
    {
        var points = new Vector3[16];
        for (int i = 0; i < points.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / points.Length;
            points[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * 4f;
        }

        return points;
    }

    static Vector3[] CrossPoints()
    {
        const float outer = 4f;
        const float inner = 1.15f;
        const float cut = 1f;
        float bevel = inner + cut;

        return new[]
        {
            new Vector3(-inner, outer, 0f),
            new Vector3(inner, outer, 0f),
            new Vector3(inner, bevel, 0f),
            new Vector3(bevel, inner, 0f),
            new Vector3(outer, inner, 0f),
            new Vector3(outer, -inner, 0f),
            new Vector3(bevel, -inner, 0f),
            new Vector3(inner, -bevel, 0f),
            new Vector3(inner, -outer, 0f),
            new Vector3(-inner, -outer, 0f),
            new Vector3(-inner, -bevel, 0f),
            new Vector3(-bevel, -inner, 0f),
            new Vector3(-outer, -inner, 0f),
            new Vector3(-outer, inner, 0f),
            new Vector3(-bevel, inner, 0f),
            new Vector3(-inner, bevel, 0f)
        };
    }

    void LateUpdate()
    {
        if (lines == null)
            return;

        while (visuals.Count < lines.Count)
            visuals.Add(CreateVisual());

        while (visuals.Count > lines.Count)
        {
            int last = visuals.Count - 1;
            Destroy(visuals[last].gameObject);
            visuals.RemoveAt(last);
        }

        for (int i = 0; i < lines.Count; i++)
        {
            Line line = lines[i];
            LineRenderer visual = visuals[i];
            visual.startWidth = thickness;
            visual.endWidth = thickness;
            visual.SetPosition(0, line.start);
            visual.SetPosition(1, line.end);
        }
    }

    LineRenderer CreateVisual()
    {
        var go = new GameObject("Line");
        go.transform.SetParent(transform, false);
        var visual = go.AddComponent<LineRenderer>();
        visual.positionCount = 2;
        visual.useWorldSpace = true;
        visual.sharedMaterial = LineMaterial();
        visual.startColor = new Color(0.3f, 0.9f, 1f);
        visual.endColor = visual.startColor;
        return visual;
    }

    Material LineMaterial()
    {
        if (lineMaterial == null)
        {
            lineMaterial = new Material(Shader.Find("Sprites/Default"));
            lineMaterial.color = Color.white;
        }

        return lineMaterial;
    }
}
