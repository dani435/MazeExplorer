using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class MazeGenerator : MonoBehaviour
{

    [SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize;
    [SerializeField] PlayerCharacter playerPrefab;
    [SerializeField] Collectable item;
    [SerializeField] SpikeTrap spikeTrap;
    [SerializeField] Collectable shield;
    [SerializeField] ExitDoor exitDoor;

    PlayerCharacter newPlayer;

    public static int keyNum = 10;
    public static bool allKeys;
    public static int death;
    private int trapsNum = 20;
    private int shieldNum = 8;
    private Vector3 spawnPoint;
    public static bool canDie;
    public static bool isInDoor;


    private void Start()
    {
        isInDoor = false;
        canDie = true;
        death = 0;
        allKeys = false;
        GenerateMazeInstant(mazeSize);
    }

    private void Update()
    {
        if (allKeys && isInDoor)
        {
            SceneManager.LoadScene("EndMenu");
        }
    }

    private void OnEnable()
    {
        SpikeTrap.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        SpikeTrap.OnDeath -= OnDeath;
    }

    private void OnDeath()
    {
        newPlayer.gameObject.SetActive(false);
        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(0.5f);

        newPlayer.transform.position = spawnPoint;
        newPlayer.gameObject.SetActive(true); 
    }

    void GenerateMazeInstant(Vector2Int size)
    {
        Timer.canTime = true;
        Timer.elapsedTime = 0f;
        Timer.minutes = 0;

        List<MazeNode> nodes = new List<MazeNode>();
        int multiSize = 2;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 nodePos = new Vector3(x - (size.x / 2f), 0, y - (size.y / 2f)) * multiSize;
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                nodes.Add(newNode);
            }
        }

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        currentPath.Add(nodes[UnityEngine.Random.Range(0, nodes.Count)]);

        while (completedNodes.Count < nodes.Count)
        {
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            if (currentNodeX < size.x - 1)
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0)
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            if (possibleDirections.Count > 0)
            {
                int chosenDirection = UnityEngine.Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }

                currentPath.Add(chosenNode);
            }
            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);
                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }

        int randomDoorPos = UnityEngine.Random.Range(0, completedNodes.Count);
        Vector3 doorPos = completedNodes[randomDoorPos].transform.position + new Vector3(0, 1f, 0);
        completedNodes.RemoveAt(randomDoorPos);
        ExitDoor newDoor = Instantiate(exitDoor, doorPos, Quaternion.identity, transform);        

        int randomPlayerPos = UnityEngine.Random.Range(0, completedNodes.Count);
        Vector3 playerPos = completedNodes[randomPlayerPos].transform.position;
        completedNodes.RemoveAt(randomPlayerPos);
        newPlayer = Instantiate(playerPrefab, playerPos, Quaternion.identity, transform);
        float playerHeight = newPlayer.GetHeight();
        newPlayer.transform.position += new Vector3(0, playerHeight / 2, 0);
        spawnPoint = newPlayer.transform.position;

        int keyRemaining = keyNum;
        while (keyRemaining > 0)
        {
            int randomKeyPos = UnityEngine.Random.Range(0, completedNodes.Count);
            Vector3 keyPos = completedNodes[randomKeyPos].transform.position + new Vector3 (0, 1, 0);
            completedNodes.RemoveAt(randomKeyPos);
            Collectable newKey = Instantiate(item, keyPos, Quaternion.identity, transform);
            keyRemaining--;
        }

        while (shieldNum > 0)
        {
            int randomShieldPos = UnityEngine.Random.Range(0, completedNodes.Count);
            Vector3 shieldPos = completedNodes[randomShieldPos].transform.position + new Vector3(0, 1f, 0);
            completedNodes.RemoveAt(randomShieldPos);
            Collectable newShield = Instantiate(shield, shieldPos, Quaternion.identity, transform);
            shieldNum--;
        }

        while (trapsNum > 0)
        {
            int randomTrapPos = UnityEngine.Random.Range(0, completedNodes.Count);
            Vector3 trapPos = completedNodes[randomTrapPos].transform.position + new Vector3(0, -1.1f, 0);
            completedNodes.RemoveAt(randomTrapPos);
            SpikeTrap newTrap = Instantiate(spikeTrap, trapPos, Quaternion.identity, transform);
            trapsNum--;
        }
    }
}
