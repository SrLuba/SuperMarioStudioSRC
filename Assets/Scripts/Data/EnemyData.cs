using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] public class EnemyData : ScriptableObject
{
    [Header("Flags Principales")] public List<string> type;
    [Header("Base Speed")] public float baseSpeed = 1.0f;
    [Header("Base Animation Speed")] public float baseAnimSpeed = 1.0f;
    [Header("Flags para especificar cosas secundaras (E_Wings, E_FlyExplicit, E_Shell)")] public List<string> flags;
    [Header("Define Sounds")] public AudioClip PLandAC, DieAC, ChangeSideAC, DestroyAC;
    [Header("Hits")] public int hits = 0;
    [Header("Spawn when death")] public GameObject spawnDeath;
    [Header("Animator Controllers")] public RuntimeAnimatorController ACNormal, ACRed, ACRedWings, ACWings;
    [Header("Bullet")] public GameObject bullet = null;
    [Header("Bullet | Timer")] public float bulletTime = 1f;
    [Header("Bullet | Count")] public int bulletCount = 1;
}
