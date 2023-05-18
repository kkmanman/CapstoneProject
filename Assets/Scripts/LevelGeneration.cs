using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] private Transform levelPart_StartPoint;
    [SerializeField] private Transform levelPart_EndPoint;
    [SerializeField] private List<Transform> levelPartList;

    private Vector3 lastEndPosition;

    private void Awake()
    {
        //Spawn the start part of the terrain
        lastEndPosition = levelPart_StartPoint.Find("EndPosition").position;

        //Spawn the main part of the terrain
        for (int i = 0; i < 10; i++)
        {
            SpawnLevelPart();
        }

        //Spawn the end part of the terrain
        SpawnLevelPart(levelPart_EndPoint, lastEndPosition);
        lastEndPosition = levelPart_EndPoint.Find("EndPosition").position;
    }

    private void SpawnLevelPart()
    {
        Transform chosenLevelPart = levelPartList[Random.Range(0, levelPartList.Count)];
        Transform lastLevelPartTransform = SpawnLevelPart(chosenLevelPart, lastEndPosition);
        lastEndPosition = lastLevelPartTransform.Find("EndPosition").position;
    }

    private Transform SpawnLevelPart(Transform levelPart, Vector3 spawnPosition)
    {
        Transform levelPartTransform = Instantiate(levelPart, spawnPosition, Quaternion.identity);
        return levelPartTransform;
    }
}
