using System;
using UnityEngine;

[Serializable]
public class MyGameConfig
{
    [SerializeField]
    private int m_MoleValue = 1;

    [SerializeField]
    private float m_RoundDuration = 30f;

    [SerializeField]
    private int m_MaxActiveMoles = 3;

    [SerializeField]
    private float m_MoleUpTimeMin = 0.7f;

    [SerializeField]
    private float m_MoleUpTimeMax = 1.5f;

    [SerializeField]
    private float m_SpawnInterval = 0.5f;

    [SerializeField]
    private float m_RiseHeight = 1f;

    [SerializeField]
    private float m_RiseTime = 0.15f;

    public int MoleValue => m_MoleValue;
    public float RoundDuration => m_RoundDuration;
    public int MaxActiveMoles => m_MaxActiveMoles;
    public float MoleUpTimeMin => m_MoleUpTimeMin;
    public float MoleUpTimeMax => m_MoleUpTimeMax;
    public float SpawnInterval => m_SpawnInterval;
    public float RiseHeight => m_RiseHeight;
    public float RiseTime => m_RiseTime;

    public float RandomUpTime()
    {
        return UnityEngine.Random.Range(m_MoleUpTimeMin, m_MoleUpTimeMax);
    }
}
