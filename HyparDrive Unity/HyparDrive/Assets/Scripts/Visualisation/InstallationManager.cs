using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class InstallationManager : MonoBehaviour {

    public static InstallationManager INSTANCE;

    // File that contains the position of the cubes
    public TextAsset hyparPositionFile;

    // Cube setup logic
    int cubeAmount = 192;
    public GameObject cubePrefab;
    GameObject[] cubes;
    LED[] LEDs;

    // Light Objects
    List<LightObject> lightObjects = new List<LightObject>();

    public delegate void handleLightObject ( LightObject lo, bool removed );
    public event handleLightObject HandleLightObject;

    // Zones
    GameObject[,,] zones;
    public Vector3 zoneCount = new Vector3(10, 10, 10);
    public int zoneSize = 1;

    private Thread lightObjectThread;

    private void Awake () {
        if (INSTANCE == null) {
            INSTANCE = this;
        }

        cubes = SetupCubes(ReadHyparPositionsFromFile());
        zones = SetupZones(zoneCount, zoneSize);

        lightObjectThread = new Thread(new ThreadStart(LightObjectThread));
        lightObjectThread.Start();

#if UNITY_EDITOR
        Invoke("CheckAmountCleanedTiles", 1.0f);
#endif
    }

    /// <summary>
    /// Threading Test
    /// </summary>
    public static void LightObjectThread () {
        
    }

    public LightObject[] GetLightObjects () {
        return lightObjects.ToArray();
    }

    public void SubscribeLightObject (LightObject lo) {
        lightObjects.Add(lo);
        HandleLightObject?.Invoke(lo, false);
    }

    public void RemoveLightObject (LightObject lo) {
        lightObjects.Remove(lo);
        HandleLightObject?.Invoke(lo, true);
    }

    /// <summary>
    /// Sets up the cubes of the installation.
    /// </summary>
    /// <param name="positions">The positions where the cubes are going to be located. CubeAmount depends on this array.</param>
    /// <returns>Returns the spawned cubes.</returns>
    private GameObject[] SetupCubes ( Vector3[] positions ) {
        GameObject[] cubes = new GameObject[positions.Length];
        for (int i = 0; i < positions.Length; i++) {
            GameObject cube = Instantiate(cubePrefab, positions[i], Quaternion.Euler(-90, 0, 0));
            cube.transform.parent = transform;
            cube.name = "Cube " + (i + 1);

            cubes[i] = cube;
        }

        return cubes;
    }

    /// <summary>
    /// Reads 3D positional data from a file.
    /// </summary>
    /// <returns>Returns a Vector3 array, containing every position.</returns>
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

    /// <summary>
    /// Sets up the zones around the installation. Can be controlled via the zoneCount variable.
    /// </summary>
    /// <param name="zoneCount">A Vector3 containing the dimensional count of zones.</param>
    /// <param name="zoneSize">The size of one zone.</param>
    /// <returns>Returns a 3D array of the created zones.</returns>
    private GameObject[,,] SetupZones (Vector3 zoneCount, int zoneSize) {
        GameObject[,,] zones = new GameObject[(int)zoneCount.x, (int)zoneCount.y, (int)zoneCount.z];

        GameObject zonesFolder = new GameObject("Zones");
        zonesFolder.transform.parent = transform.parent;

        for (int z = 0; z < zones.GetLength(0); z++) {
            for (int y = 0; y < zones.GetLength(1); y++) {
                for (int x = 0; x < zones.GetLength(2); x++) {
                    GameObject zone = new GameObject("Zone " + (1 + x + y * zones.GetLength(0) + z * (zones.GetLength(1) * zones.GetLength(1))));

                    int xz = zoneSize * (x - zones.GetLength(0) / 2);
                    int yz = zoneSize * (y) + zoneSize / 2;
                    int zz = zoneSize * (z - zones.GetLength(2) / 2);

                    zone.transform.position = new Vector3(xz, yz, zz);
                    zone.transform.localScale = new Vector3(zoneSize, zoneSize, zoneSize);
                    zone.AddComponent<Zone>();
                    zone.transform.parent = zonesFolder.transform;

                    zones[x, y, z] = zone;
                }
            }
        }

#if UNITY_EDITOR
        Debug.Log("Set up " + zones.Length + " zones. Cleaning up...");
#endif

        return zones;
    }

    /// <summary>
    /// Checks the amount of tiles cleaned.
    /// </summary>
    private void CheckAmountCleanedTiles () {
        int zonesActive = 0;

        for (int z = 0; z < zones.GetLength(0); z++) {
            for (int y = 0; y < zones.GetLength(1); y++) {
                for (int x = 0; x < zones.GetLength(2); x++) {
                    if (zones[x, y, z] != null) zonesActive++;
                }
            }
        }

        Debug.Log("Cleaned up zones. " + zonesActive + " zones still active.");
    }

    /// <summary>
    /// Check all cubes in the installation for being in a specific area. Is used for zoning.
    /// </summary>
    /// <param name="centre">The centre of the area to check.</param>
    /// <param name="size">The size of the area to check around the centre.</param>
    /// <returns>Returns the cubes found in that area. (Can return NULL!)</returns>
    public Cube[] GetCubesInArea ( Vector3 centre, Vector3 size ) {
        List<Cube> areaCubes = new List<Cube>();
        for (int i = 0; i < this.cubes.Length; i++) {

            AABB a = new AABB(centre, size);
            AABB b = new AABB(this.cubes[i].transform.position, this.cubes[i].transform.localScale);

            if (AABBCollision3D(a, b)) {
                areaCubes.Add(this.cubes[i].GetComponent<Cube>());
            }
        }

        return areaCubes.ToArray();
    }

    /// <summary>
    /// 3D Cubular Collision checking.
    /// </summary>
    /// <param name="a">First Cube</param>
    /// <param name="b">Second Cube</param>
    /// <returns>Returns a bool stating if a collision happened.</returns>
    private bool AABBCollision3D (AABB a, AABB b) {
        return (a.minX <= b.maxX && a.maxX >= b.minX) &&
         (a.minY <= b.maxY && a.maxY >= b.minY) &&
         (a.minZ <= b.maxZ && a.maxZ >= b.minZ);
    }

    private void OnDrawGizmos () {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(Vector3.zero + new Vector3(0, (zoneCount.y * zoneSize) / 2, 0), zoneCount * zoneSize);
    }

}

/// <summary>
/// Cubular Collision Data Struct (supports 3D)
/// </summary>
public struct AABB {

    public float minX;
    public float minY;
    public float minZ;

    public float maxX;
    public float maxY;
    public float maxZ;

    public AABB (Vector3 centre, Vector3 size) {
        minX = centre.x - size.x / 2;
        minY = centre.y - size.y / 2;
        minZ = centre.z - size.z / 2;

        maxX = centre.x + size.x / 2;
        maxY = centre.y + size.y / 2;
        maxZ = centre.z + size.z / 2;
    }

}