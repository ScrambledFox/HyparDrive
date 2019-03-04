using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour {

    // File that contains the position of the cubes
    public TextAsset hyparPositionFile;

    // Cube setup logic
    int cubeAmount = 192;
    public GameObject cubePrefab;
    List<GameObject> cubes = new List<GameObject>();

    private void Awake () {
        SetupCubes(ReadHyparPositionsFromFile());
    }

    void SetupCubes ( Vector3[] positions ) {
        foreach (Vector3 position in positions) {
            GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
            cube.transform.parent = transform;
            cubes.Add(cube);
            cube.name = "Cube " + cubes.Count;
        }
    }

    Vector3[] ReadHyparPositionsFromFile () {
        string[] lines = hyparPositionFile.text.Split(
            new[] { System.Environment.NewLine },
            System.StringSplitOptions.None);

        List<Vector3> positions = new List<Vector3>();
        foreach (string line in lines) {
            string[] coords = line.Split(',');
            positions.Add(new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2])));
        }

        return positions.ToArray();
    }

}