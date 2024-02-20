using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerPlanner : MonoBehaviour
{
    public static GameManagerPlanner Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject UIEndGame = null;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndGame()
    {
        UIEndGame.SetActive(true);
        StopAllCoroutines();
        Time.timeScale = 0f;
    }
}
