using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WavaData
{
    [Tooltip("Wave name")]
    public string name;

    [Tooltip("List of enemies that can appear, the pawn gameobjects")]
    public List<GameObject> spawnableEnemies = new();

    [Tooltip("How many enemies to spawn within this wave")]
    public int spawnCount;

    [Tooltip("Enemy weaponry this wave")]
    public List<GameObject> enemyWeapons = new();
}
