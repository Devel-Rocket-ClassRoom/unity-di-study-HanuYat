using System;
using System.Collections;
using UnityEngine;
using VContainer;

public class MyMole : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_CatchClip;

    private IScoreService m_Score;
    private IAudioService m_Audio;
    private MyGameConfig m_Config;

    private bool m_Resolved;
    private bool m_Done;

    private Vector3 m_UpPos;
    private Vector3 m_DownPos;

    public event Action<MyMole, bool> Despawned;

    [Inject]
    public void Construct(IScoreService score, IAudioService audio, MyGameConfig config)
    {
        m_Score = score;
        m_Audio = audio;
        m_Config = config;
    }

    public void Init(float upTime)
    {
        if (m_Score == null)
        {
            Debug.LogWarning("[Mole] 주입되지 않았습니다 — IObjectResolver.Instantiate로 생성했는지 확인하세요.");
            return;
        }

        m_Resolved = true;

        m_UpPos = transform.position;
        m_DownPos = m_UpPos - Vector3.up * m_Config.RiseHeight;
        transform.position = m_DownPos;

        StartCoroutine(LifeRoutine(upTime));
    }

    public void Catch()
    {
        if (m_Done || !m_Resolved)
            return;

        m_Done = true;
        m_Score.Add(m_Config.MoleValue);
        m_Audio.PlaySoundEffect(m_CatchClip);
        Despawned?.Invoke(this, true);
        Destroy(gameObject);
    }

    private IEnumerator LifeRoutine(float upTime)
    {
        yield return Slide(m_DownPos, m_UpPos);

        float t = 0f;
        while (t < upTime)
        {
            if (m_Done)
                yield break;
            t += Time.deltaTime;
            yield return null;
        }

        if (m_Done)
            yield break;

        yield return Slide(m_UpPos, m_DownPos);

        if (m_Done)
            yield break;

        m_Done = true;
        Despawned?.Invoke(this, false);
        Destroy(gameObject);
    }

    private IEnumerator Slide(Vector3 from, Vector3 to)
    {
        float dur = m_Config.RiseTime;
        if (dur <= 0f)
        {
            transform.position = to;
            yield break;
        }

        float t = 0f;
        while (t < dur)
        {
            if (m_Done)
                yield break;
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(from, to, t / dur);
            yield return null;
        }
        transform.position = to;
    }
}
