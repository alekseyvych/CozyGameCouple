using System.Collections.Generic;
using UnityEngine;

public class PreviewManager : MonoBehaviour
{
    public GameObject previewPrefab;
    private List<GameObject> previewObjects;
    private const int poolSize = 10;

    void Start()
    {
        previewObjects = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject preview = Instantiate(previewPrefab);
            preview.SetActive(false);
            previewObjects.Add(preview);
        }
    }

    public void UpdatePreviews(List<Vector3> positions, Material material)
    {
        int i = 0;
        for (; i < positions.Count && i < poolSize; i++)
        {
            previewObjects[i].transform.position = positions[i];
            previewObjects[i].GetComponent<Renderer>().material = material;
            previewObjects[i].SetActive(true);
        }
        for (; i < poolSize; i++)
        {
            previewObjects[i].SetActive(false);
        }
    }
}
