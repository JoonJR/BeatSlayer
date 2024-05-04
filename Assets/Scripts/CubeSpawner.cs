using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    public TextAsset spawnPatternFile; 
    public GameObject[] cubePrefabs; // Array of cube prefabs to spawn
    public Transform[] spawnPoints; // Array of spawn points
    Quaternion rotation;
    private double startTime = AudioSettings.dspTime;
    private SpawnPattern spawnPattern; // Stores the parsed spawn pattern
    public float CubeSpawnDelay = -1f;
 
    private bool allEventsProcessed = false;
    public float spawnRate = 1.0f; // Base spawn rate
    public bool stopSpawning = false;

    public delegate void OnAllCubesSpawned();
    public event OnAllCubesSpawned AllCubesSpawned;

    public float postSpawnDelay = 5.0f; // Delay after the last cube spawn before declaring win

    void Start()
    {
        ParseData();
        
    }
    void ParseData()
    {
        // Load the JSON text from the TextAsset
        string jsonData = spawnPatternFile.text;

        try
        {
            // Parse the JSON string into the SpawnPattern object using Newtonsoft.Json
            spawnPattern = JsonConvert.DeserializeObject<SpawnPattern>(jsonData);

            // Debug log to print the entire spawnPattern object
            Debug.Log("spawnPattern: " + spawnPattern);

            // Check if events list is not null
            if (spawnPattern.events != null)
            {
                // Debug log to print the number of events in the list
                Debug.Log("Number of events: " + spawnPattern.events.Count);

                // Debug log to print each event individually
                /*foreach (SpawnEvent e in spawnPattern.events)
                {
                   Debug.Log("Event time: " + e.time + ", Line Index: " + e.lineIndex + ", Line Layer: " + e.lineLayer + ", Type: " + e.type + ", Cut Direction: " + e.cutDirection);
                }*/
            }
            else
            {
                Debug.LogWarning("Events list is null.");
            }
        }
        catch (JsonException e)
        {
            // Handle parsing error
            Debug.LogError("Error parsing JSON data: " + e.Message);
        }
    }
    void Update()
    {
        
        if (!allEventsProcessed)
            CheckSpawnEvents();
    }
    private void CheckSpawnEvents()
    {
        if (spawnPattern != null && spawnPattern.events != null && !allEventsProcessed)
        {
            float bpm = 126f;
            // Get the current DSP time
            double currentTime = AudioSettings.dspTime - startTime;

            // Iterate through the list in reverse order
            for (int i = spawnPattern.events.Count - 1; i >= 0; i--)
            {
                SpawnEvent currentEvent = spawnPattern.events[i];

                double beatTime = (currentEvent.time * (60.0 / bpm)) + CubeSpawnDelay;

                if (!stopSpawning & currentTime >= beatTime)
                {
                    SpawnCube(currentEvent.lineIndex, currentEvent.lineLayer, currentEvent.type, currentEvent.cutDirection);
                    spawnPattern.events.RemoveAt(i);


                }

            }
            if (spawnPattern.events.Count == 0)
            {
                allEventsProcessed = true;
                Debug.Log("All cubes spawned.");
                StartCoroutine(WaitAndDeclareWin());
            }

        }
        else
        {
            Debug.Log("All events processed. Spawning stopped.");
        }
    }
    private void SpawnCube(int lineIndex, int lineLayer, int type, int cutDirection)
    {
        // Select spawn point based on line index and layer (3x4)
        Transform spawnPoint = spawnPoints[lineLayer * 4 + lineIndex];

        // Determine cube prefab based on type
        GameObject cubePrefab = null; // Initialize with null

        if (type >= 0 && type < cubePrefabs.Length) // Check for valid index within array bounds
        {
            cubePrefab = cubePrefabs[type];
        }
        else
        {
            Debug.LogError("Invalid type value in SpawnEvent: " + type); // Handle invalid type
        }
        

        switch (cutDirection)
        {
            case 0:
                rotation = Quaternion.Euler(0f, 0f, 180f);  // Up
                break;
            case 1:
                rotation = Quaternion.Euler(0f, 0f, 0f); // Down
                break;
            case 2:
                rotation = Quaternion.Euler(0f, 0f, -90f); // Left
                break;
            case 3:
                rotation = Quaternion.Euler(0f, 0f, 90f);   // Right
                break;
            case 4:
                rotation = Quaternion.Euler(0f, 0f, 225f);  // Diagonal Up-Left
                break;
            case 5:
                rotation = Quaternion.Euler(0f, 0f, -225f); // Diagonal Up-Right
                break;
            case 6:
                rotation = Quaternion.Euler(-0f, 0f, -45f); // Diagonal Down-Left
                break;
            case 7:
                rotation = Quaternion.Euler(0f, 0f, 45f); // Diagonal Down-Right
                break;
            default:
                Debug.LogError("Invalid cutDirection value: " + cutDirection);
                break;
        }

        // Instantiate the cube at the chosen spawn point with the calculated rotation
        GameObject cube = Instantiate(cubePrefab, spawnPoint.position, rotation);

        // Get the MoveTowardsPlayer component 
        MoveTowardsPlayer mover = cube.GetComponent<MoveTowardsPlayer>();
        if (mover)
        {
            // Increase speed after passing the player
            if (cube.transform.position.z < 0.5) 
            {
                mover.speed *= 5.0f; // Increase speed by a factor of 5
            }
        }
    }

    
    public void StopSpawning()
    {
        stopSpawning = true;
    }
    IEnumerator WaitAndDeclareWin()
    {
        yield return new WaitForSeconds(postSpawnDelay);
        if (AllCubesSpawned != null)
            AllCubesSpawned();

        ScoreManager.Instance.DisplayWinUI(); // This will trigger the display of the win menu and final score
    }
}


// Define the data structures (can be placed in a separate script if preferred)
public class SpawnEvent
{
    public float time;
    public int lineIndex;
    public int lineLayer;
    public int type;
    public int cutDirection;
}

public class SpawnPattern
{
    public List<SpawnEvent> events;
}