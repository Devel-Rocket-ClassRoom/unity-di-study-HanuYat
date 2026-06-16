using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MyMoleSpawner : MonoBehaviour
{
    [SerializeField]
    private MyMole m_MolePrefab;

    [SerializeField]
    private Transform[] m_Holes;

    private IObjectResolver m_Resolver;
    private MyGameConfig m_Config;
    private IRoundService m_Round;

    private readonly HashSet<int> m_Occupied = new HashSet<int>();
    private Coroutine m_SpawnLoop;

    [Inject]
    public void Construct(IObjectResolver resolver, MyGameConfig config, IRoundService round)
    {
        m_Resolver = resolver;
        m_Config = config;
        m_Round = round;
    }

    private void Start()
    {
        if (m_MolePrefab == null || m_Resolver == null || m_Round == null)
        {
            Debug.LogWarning("[MoleSpawner] 프리팹/주입 설정을 확인하세요.");
            return;
        }

        m_Round.RoundStarted += OnRoundStarted;
        m_Round.RoundEnded += OnRoundEnded;

        if (m_Round.IsPlaying)
            OnRoundStarted();
    }

    private void OnDestroy()
    {
        if (m_Round != null)
        {
            m_Round.RoundStarted -= OnRoundStarted;
            m_Round.RoundEnded -= OnRoundEnded;
        }
    }

    private void OnRoundStarted()
    {
        m_Occupied.Clear();
        if (m_SpawnLoop != null)
            StopCoroutine(m_SpawnLoop);
        m_SpawnLoop = StartCoroutine(SpawnLoop());
    }

    private void OnRoundEnded(int finalScore)
    {
        if (m_SpawnLoop != null)
        {
            StopCoroutine(m_SpawnLoop);
            m_SpawnLoop = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        var wait = new WaitForSeconds(m_Config.SpawnInterval);
        while (m_Round.IsPlaying)
        {
            if (m_Occupied.Count < m_Config.MaxActiveMoles)
                TrySpawn();
            yield return wait;
        }
    }

    private void TrySpawn()
    {
        int hole = PickFreeHole();
        if (hole < 0)
            return;

        Transform point = m_Holes[hole];
        MyMole mole = m_Resolver.Instantiate(m_MolePrefab, point.position, point.rotation);
        m_Occupied.Add(hole);

        mole.Despawned += (m, caught) => m_Occupied.Remove(hole);
        mole.Init(m_Config.RandomUpTime());
    }

    private int PickFreeHole()
    {
        if (m_Holes == null || m_Holes.Length == 0)
            return -1;

        int start = Random.Range(0, m_Holes.Length);
        for (int i = 0; i < m_Holes.Length; i++)
        {
            int idx = (start + i) % m_Holes.Length;
            if (m_Holes[idx] != null && !m_Occupied.Contains(idx))
                return idx;
        }
        return -1;
    }
}
