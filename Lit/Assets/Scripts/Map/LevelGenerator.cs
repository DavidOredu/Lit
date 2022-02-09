using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<LevelPart> levelParts = new List<LevelPart>();
    [SerializeField] private int totalPartToSpawn;

    [SerializeField] private Transform higherStartPosition;
    [SerializeField] private Transform middleStartPosition;
    [SerializeField] private Transform lowerStartPosition;
    private List<Vector3> lastEndPositions = new List<Vector3>();
    // Start is called before the first frame update
    private void Awake()
    {
        lastEndPositions.Add(lowerStartPosition.position);
        lastEndPositions.Add(middleStartPosition.position);
        lastEndPositions.Add(higherStartPosition.position);

        for (int i = 0; i < totalPartToSpawn; i++)
        {
            foreach (var levelPart in levelParts)
            {
                SpawnLevelPart(levelPart, levelParts.IndexOf(levelPart));
            }
        }
    }
    
    private Transform SpawnLevelPart(Transform levelPart, Vector3 spawnPosition)
    {
        Transform levelPartTransform = Instantiate(levelPart, spawnPosition, Quaternion.identity);
        return levelPartTransform;
    }

    private void SpawnLevelPart(LevelPart levelPart, int index)
    {
        Transform chosenLevelPart = levelPart.parts[Random.Range(0, levelPart.parts.Count)];
        Transform lastLevelPartTransform = SpawnLevelPart(chosenLevelPart, lastEndPositions[index]);
        lastEndPositions[index] = lastLevelPartTransform.Find("EndPosition").position;
    }
}
