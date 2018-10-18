using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour {
    public int seed = 0;
    public GameObject cube;
    [Range(0f,1f)]
    public float threshold = .5f;
    public int width = 100;
    public int depth = 100;
    public int height = 100;

    public float scale = 1f;


    private int xCoordOrigin = 0;
    private int yCoordOrigin = 0;

    private void Start()
    {
        StartCoroutine(Test());
    }

    void Generate()
    {
        for(int x = 0; x<width; x++)
        {
            for(int y = 0; y<height; y++)
            {
                for(int z = 0; z < depth; z++)
                {
                    float sample = Perlin3D(x, y, z);
                    if (sample >= threshold)
                    {
                        Instantiate(cube, new Vector3(x, y, z), Quaternion.identity, transform);
                    }
                }
            }
        }
    }

    public float Perlin3D(float x, float y, float z)
    {
        float xy = Mathf.PerlinNoise(x, y);
        float yz = Mathf.PerlinNoise(y, z);
        float xz = Mathf.PerlinNoise(x, z);
        float yx = Mathf.PerlinNoise(y, x);
        float zy = Mathf.PerlinNoise(z, y);
        float zx = Mathf.PerlinNoise(z, x);

        return (xy + yz + xz + yx + zy + zx) / 6f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && false)
            Generate();
    }

    IEnumerator Test()
    {
        List<CombineInstance> combine = new List<CombineInstance>();
        List<GameObject> toDisable = new List<GameObject>();
        for (float x = 0; x < width; x++)
        {
            for (float y = 0; y < height; y++)
            {
                for (float z = 0; z < depth; z++)
                {
                    float sample = Perlin3D(x/width * scale, y/height * scale, z/depth * scale);
                    if (sample >= threshold)
                    {
                        var obj = Instantiate(cube, new Vector3(x, y, z), Quaternion.identity, transform);
                        var filter = obj.GetComponent<MeshFilter>();
                        CombineInstance ci = new CombineInstance();
                        ci.mesh = filter.sharedMesh;
                        ci.transform = filter.transform.localToWorldMatrix;
                        //filter.gameObject.SetActive(false);
                        combine.Add(ci);
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
        var newFilter = cube.GetComponent<MeshFilter>();
        newFilter.mesh = new Mesh();
        newFilter.mesh.CombineMeshes(combine.ToArray());
    }
}
