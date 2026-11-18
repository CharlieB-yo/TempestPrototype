using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Track track;
    public int lineIndex;

    void Awake()
    {
        var body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.name = "Body";
        body.transform.SetParent(transform, false);
        body.transform.localPosition = Vector3.zero;
        body.transform.localRotation = Quaternion.identity;
        body.transform.localScale = new Vector3(1.05f, 0.32f, 0.12f);
        Destroy(body.GetComponent<Collider>());

        var material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        material.SetColor("_BaseColor", new Color(1f, 0.8f, 0.15f));
        body.GetComponent<MeshRenderer>().sharedMaterial = material;
    }

    void Update()
    {
        if (track == null || track.lines == null || track.lines.Count == 0)
            return;

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.aKey.wasPressedThisFrame)
            lineIndex--;
        if (keyboard.dKey.wasPressedThisFrame)
            lineIndex++;
    }

    void LateUpdate()
    {
        if (track == null || track.lines == null || track.lines.Count == 0)
            return;

        int count = track.lines.Count;
        lineIndex = ((lineIndex % count) + count) % count;
        Line line = track.lines[lineIndex];
        transform.SetPositionAndRotation(line.Midpoint, line.Rotation);
    }
}
