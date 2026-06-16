using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VContainer;

public class UiSingleton : MonoBehaviour
{
    public TMP_Text m_InfoText;
    public Button m_AddButton;
    public Button m_SceneChangeButton;

    public string m_TargetSceneName;

    private GlobalCounterService m_Counter;

    private void Start()
    {
        m_InfoText.text = $"{m_Counter.Id}: {m_Counter.Count}";
        m_AddButton.onClick.AddListener(OnClickAdd);
        m_SceneChangeButton.onClick.AddListener(OnClickSceneChange);
    }

    [Inject]
    public void Construct(GlobalCounterService counter)
    {
        m_Counter = counter;
    }

    private void OnClickAdd()
    {
        m_Counter.Add(1);
        m_InfoText.text = $"{m_Counter.Id}: {m_Counter.Count}";
    }

    private void OnClickSceneChange()
    {
        SceneManager.LoadScene(m_TargetSceneName);
    }
}