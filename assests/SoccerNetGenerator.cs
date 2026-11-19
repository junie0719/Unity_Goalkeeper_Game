
using UnityEngine;

public class SoccerNetGenerator : MonoBehaviour
{
    public Material netMaterial;

    [Header("Net dimensions")]
    public float width = 7.32f;
    public float height = 2.44f;
    public float depth = 1.5f;
    public float spacing = 0.20f;
    public float thickness = 0.012f;

    [ContextMenu("Generate Net")]
    public void GenerateNet()
    {
        // Remove previously generated strands.
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        float frontZ = 27f;
        float backZ = frontZ + depth;

        // Back net: vertical and horizontal strands
        for (float x = -width / 2; x <= width / 2 + 0.001f; x += spacing)
            Strand(new Vector3(x, 0, backZ),
                   new Vector3(x, height, backZ));

        for (float y = 0; y <= height + 0.001f; y += spacing)
            Strand(new Vector3(-width / 2, y, backZ),
                   new Vector3(width / 2, y, backZ));

        // Left and right side nets
        foreach (float x in new float[] { -width / 2, width / 2 })
        {
            for (float z = frontZ; z <= backZ + 0.001f; z += spacing)
                Strand(new Vector3(x, 0, z),
                       new Vector3(x, height, z));

            for (float y = 0; y <= height + 0.001f; y += spacing)
                Strand(new Vector3(x, y, frontZ),
                       new Vector3(x, y, backZ));
        }

        // Top net
        for (float x = -width / 2; x <= width / 2 + 0.001f; x += spacing)
            Strand(new Vector3(x, height, frontZ),
                   new Vector3(x, height, backZ));

        for (float z = frontZ; z <= backZ + 0.001f; z += spacing)
            Strand(new Vector3(-width / 2, height, z),
                   new Vector3(width / 2, height, z));
    }

    void Strand(Vector3 start, Vector3 end)
    {
        GameObject strand = GameObject.CreatePrimitive(PrimitiveType.Cube);
        strand.name = "NetStrand";
        strand.transform.SetParent(transform, false);

        Vector3 direction = end - start;

        strand.transform.localPosition = (start + end) / 2f;
        strand.transform.localRotation =
            Quaternion.FromToRotation(Vector3.up, direction.normalized);
        strand.transform.localScale =
            new Vector3(thickness, direction.magnitude, thickness);

        Collider collider = strand.GetComponent<Collider>();
        if (collider != null)
            DestroyImmediate(collider);

        if (netMaterial != null)
            strand.GetComponent<Renderer>().sharedMaterial = netMaterial;
    }
}
