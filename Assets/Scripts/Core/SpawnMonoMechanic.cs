using System;
using System.Collections.Generic;
using Abstract;
using Level;
using UnityEngine;
using UnityEngine.AI;

public class SpawnMonoMechanic : GameModeMechanic
{
    public void SpawnAll(ISpawnable spawnable)
    {
        Type spawnPointType = spawnable.GetSpawnPointType();
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        List<SpawnPoint> validSpawnPoints = new List<SpawnPoint>();
        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.GetType() == spawnPointType)
            {
                validSpawnPoints.Add(spawnPoint);
            }
        }
        
        Debug.Log(validSpawnPoints.Count);
        if (validSpawnPoints.Count > 0)
        {
            foreach (var validSpawnPoint in validSpawnPoints)
            {
                if (spawnable.IsSpawnOnNavMesh())
                {
                    SpawnOnNavMesh(spawnable,validSpawnPoint);
                }
                else
                {
                    DefaultSpawn(spawnable,validSpawnPoint);
                }
            }
        }
    }


    private void DefaultSpawn(ISpawnable spawnable, SpawnPoint validSpawnPoint)
    {
        GameObject gameObject = Instantiate(spawnable.GetObjectToSpawn().gameObject, validSpawnPoint.transform.position,
            Quaternion.identity);
        gameObject.GetComponent<ISpawnable>().Initialize();
    }


    private void SpawnOnNavMesh(ISpawnable spawnable, SpawnPoint validSpawnPoint)
    {
        NavMesh.SamplePosition(validSpawnPoint.transform.position, out NavMeshHit navMeshHit, 5, NavMesh.AllAreas);
        var myRandomPositionInsideNavMesh = navMeshHit.position;

        if (myRandomPositionInsideNavMesh == Vector3.positiveInfinity || myRandomPositionInsideNavMesh == Vector3.negativeInfinity)
        {
            Debug.LogError("The enemy spawn point is infinity ! Check is navmesh surface is on map");
            return;
        }

        GameObject gameObject = Instantiate(spawnable.GetObjectToSpawn().gameObject, myRandomPositionInsideNavMesh, Quaternion.identity);

        if (gameObject == null)
        {
            Debug.LogError("There is error with spawning object");
            return;
        }
        gameObject.GetComponent<ISpawnable>().Initialize();

    }

    public void SpawnAtSpawnPoints(ISpawnable enemyPrefab, SpawnPoint[] spawnPoint)
    {
        foreach (var validSpawnPoint in spawnPoint)
        {
            if (enemyPrefab.IsSpawnOnNavMesh())
            {
                SpawnOnNavMesh(enemyPrefab,validSpawnPoint);
            }
            else
            {
                DefaultSpawn(enemyPrefab,validSpawnPoint);
            }
        }
    }
}


public interface ISpawnable
{
    public Type GetSpawnPointType();

    public GameObject GetObjectToSpawn();
    public bool IsSpawnOnNavMesh();
    public void Initialize();
}
