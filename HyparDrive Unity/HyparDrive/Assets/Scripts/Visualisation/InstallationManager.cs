using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Threading;

public class InstallationManager : MonoBehaviour {

    public static InstallationManager INSTANCE;

    // Cube setup logic
    public GameObject hyparCubePrefab;
    Cube[] cubes;
    LED[] LEDs;

    // Light Objects
    List<LightObject> lightObjects = new List<LightObject>();

    public delegate void handleLightObject ( LightObject lo, bool removed );
    public event handleLightObject HandleLightObject;

    // Zones
    GameObject[,,] zones;
    List<Zone> zonesActiveLastUpdate = new List<Zone>();
    public Vector3 zoneCount = new Vector3(10, 10, 10);
    public int zoneSize = 1;

    public bool doVisualisation = true;

    public bool sendArtNetData = false;

    public const float targetFPS = 40f;
    private const long frameTickLength = (long)((1f / targetFPS) * 1000 * 10000);
    private static System.DateTime currentTime;
    private static long lastTick = -1;

    private Thread lightObjectThread;

    private void Awake () {
        if (INSTANCE == null) {
            INSTANCE = this;
        }

        InstallationSaveData saveData = FileManagement.GetInstallationSaveData(FileManagement.INSTALLATION_SAVE_FOLDER + "Hypar160" + FileManagement.FILE_EXTENSION);
        LoadInstallation(saveData);

        zones = SetupZones(zoneCount, zoneSize);

        lightObjectThread = new Thread(new ThreadStart(LightObjectThread));
        lightObjectThread.Start();

#if UNITY_EDITOR
        Invoke("CheckAmountCleanedTiles", 1.0f);
#endif
    }


    long lastUpdateTicks;
    /// <summary>
    /// Threading Test
    /// </summary>
    public void LightObjectThread () {

        while (true) {
            currentTime = System.DateTime.Now;

            if (currentTime.Ticks > lastUpdateTicks + 1000000) {
                ThreadHelper.ExecuteInUpdate(() => {
                    UpdateActiveCubes();
                });

                if (sendArtNetData) {
                    ArtNetController.INSTANCE.NodeUpdateTick();
                }

                lastUpdateTicks = currentTime.Ticks;
            }

        }

    }

    private void UpdateActiveCubes () {
        for (int i = 0; i < cubes.Length; i++) {
            // Zone already gets checked in UpdateLEDs.
            cubes[i].UpdateLEDs();
        }
    }

    public void LoadInstallation ( InstallationSaveData installationSaveData ) {
        List<Cube> newCubes = new List<Cube>();

        foreach (InstallationSaveData.HyparCube hyparCube in installationSaveData.hyparCubes) {
            GameObject cube = Instantiate(hyparCubePrefab, this.transform);
            newCubes.Add(cube.GetComponent<Cube>());

            cube.transform.position = hyparCube.position;
            cube.transform.rotation = Quaternion.Euler(hyparCube.rotation);
            cube.transform.localScale = hyparCube.scale;
            cube.transform.GetComponent<Cube>().SetIndex(hyparCube.id);

            cube.gameObject.isStatic = true;
        }

        cubes = newCubes.ToArray();
    }

    /// <summary>
    /// Gets all LEDColourData.
    /// </summary>
    /// <returns>Returns 2D color data in a 1D array with x being cubes and y being led index </returns>
    private Color[] GetLEDColourData () {
        Color[] data = new Color[cubes.Length * INSTALLATION_CONFIG.LEDS_PER_STRIP * 12];

        for (int i = 0; i < cubes.Length; i++) {
            Color[] stripColourData = cubes[i].GetLEDData();

            if (stripColourData == null) {
                UnityEngine.Debug.LogWarning("StripColourData not yet initialized!");
                return null;
            }

            for (int j = 0; j < INSTALLATION_CONFIG.LEDS_PER_STRIP * 12; j++) {
                data[i + j * cubes.Length] = stripColourData[j];
            }
        }

        return data;
    }

    /// <summary>
    /// Get all the known light objects.
    /// </summary>
    /// <returns>Returns an array of all the known light objects.</returns>
    public LightObject[] GetLightObjects () {
        return lightObjects.ToArray();
    }

    /// <summary>
    /// Subscribes a light object to the known light objects list.
    /// </summary>
    /// <param name="lo">The light object to add.</param>
    public void SubscribeLightObject (LightObject lo) {
        //UnityEngine.Debug.Log("Subscribed new Light Object");
        lightObjects.Add(lo);
        lo.Moved += UpdateActiveZonesLastUpdate;
        HandleLightObject?.Invoke(lo, false);
    }

    /// <summary>
    /// Remove a light object from the known light objects list.
    /// </summary>
    /// <param name="lo">The light object to remove.</param>
    public void RemoveLightObject (LightObject lo) {
        lightObjects.Remove(lo);
        HandleLightObject?.Invoke(lo, true);
    }

    public void RegisterActiveZone ( Zone zone, bool register ) {
        if (register) {
            zonesActiveLastUpdate.Add(zone);
        }
    }

    private void UpdateActiveZonesLastUpdate ( LightObject lo ) {
        for (int i = 0; i < zonesActiveLastUpdate.Count; i++) {
            zonesActiveLastUpdate[i].NotifyCubes(lo);
        }

        zonesActiveLastUpdate.Clear();
    }


    /// <summary>
    /// Sets up the cubes of the installation.
    /// </summary>
    /// <param name="positions">The positions where the cubes are going to be located. CubeAmount depends on this array.</param>
    /// <returns>Returns the spawned cubes.</returns>
    //private Cube[] SetupCubes ( Vector3[] positions ) {
    //    Cube[] cubes = new Cube[positions.Length];
    //    for (int i = 0; i < positions.Length; i++) {
    //        GameObject cube = Instantiate(hyparCubePrefab, positions[i], Quaternion.Euler(-90, 0, 0));
    //        cube.transform.parent = transform;
    //        cube.name = "Cube " + (i + 1);

    //        cubes[i] = cube.GetComponent<Cube>().SetIndex(i);
    //    }

    //    return cubes;
    //}

    /// <summary>
    /// Reads 3D positional data from a file.
    /// </summary>
    /// <returns>Returns a Vector3 array, containing every position.</returns>
    //Vector3[] ReadHyparPositionsFromFile () {
    //    string[] lines = hyparPositionFile.text.Split(
    //        new[] { System.Environment.NewLine },
    //        System.StringSplitOptions.None);

    //    List<Vector3> positions = new List<Vector3>();
    //    foreach (string line in lines) {
    //        string[] coords = line.Split(',');
    //        positions.Add(new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2])));
    //    }

    //    return positions.ToArray();
    //}

    /// <summary>
    /// Sets up the zones around the installation. Can be controlled via the zoneCount variable.
    /// </summary>
    /// <param name="zoneCount">A Vector3 containing the dimensional count of zones.</param>
    /// <param name="zoneSize">The size of one zone.</param>
    /// <returns>Returns a 3D array of the created zones.</returns>
    private GameObject[,,] SetupZones (Vector3 zoneCount, int zoneSize) {
        GameObject[,,] zones = new GameObject[(int)zoneCount.x, (int)zoneCount.y, (int)zoneCount.z];

        GameObject zonesFolder = new GameObject("Zones");
        zonesFolder.transform.parent = this.transform.parent;

        for (int z = 0; z < zones.GetLength(2); z++) {
            for (int y = 0; y < zones.GetLength(1); y++) {
                for (int x = 0; x < zones.GetLength(0); x++) {
                    GameObject zone = new GameObject("Zone " + (1 + x + y * zones.GetLength(0) + z * (zones.GetLength(1) * zones.GetLength(1))));

                    float xz = zoneSize * (x - zones.GetLength(0) / 2f) + zoneSize / 2f;
                    float yz = zoneSize * y + zoneSize / 2f;
                    float zz = zoneSize * (z - zones.GetLength(2) / 2f) + zoneSize / 2f;

                    zone.transform.position = new Vector3(xz, yz, zz);
                    zone.transform.localScale = new Vector3(zoneSize, zoneSize, zoneSize);
                    zone.AddComponent<Zone>();
                    zone.transform.parent = zonesFolder.transform;

                    zones[x, y, z] = zone;
                }
            }
        }

#if UNITY_EDITOR
        UnityEngine.Debug.Log("Set up " + zones.Length + " zones. Cleaning up...");
#endif

        return zones;
    }

    /// <summary>
    /// Checks the amount of tiles cleaned.
    /// </summary>
    private void CheckAmountCleanedTiles () {
        int zonesActive = 0;

        for (int z = 0; z < zones.GetLength(2); z++) {
            for (int y = 0; y < zones.GetLength(1); y++) {
                for (int x = 0; x < zones.GetLength(0); x++) {
                    if (zones[x, y, z] != null) zonesActive++;
                }
            }
        }

        UnityEngine.Debug.Log("Cleaned up zones. " + zonesActive + " zones still active.");
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

            Collision.AABB a = new Collision.AABB(centre, size);
            Collision.AABB b = new Collision.AABB(this.cubes[i].transform.position, this.cubes[i].transform.localScale);

            if (Collision.HasIntersection(a, b)) {
                areaCubes.Add(this.cubes[i].GetComponent<Cube>());
            }
        }

        return areaCubes.ToArray();
    }

    private void OnDrawGizmos () {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(Vector3.zero + new Vector3(0, (zoneCount.y * zoneSize) / 2, 0), zoneCount * zoneSize);
    }

    private void OnApplicationQuit () {
        lightObjectThread.Abort();
    }

    private void OnDestroy () {
        lightObjectThread.Abort();
    }

    private void OnDisable () {
        lightObjectThread.Abort();
    }

}