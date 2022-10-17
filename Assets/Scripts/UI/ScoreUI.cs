using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text currentLevel;
    [SerializeField] private Text nextLevel;
    [SerializeField] private Slider progressBar;
    private Player _player;
    
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (_player == null) return;
        
        scoreLabel.text = _player.Score.ToString();
        progressBar.value = Managers.Levels.Progress;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Preloader" ) return;

        _player = FindObjectOfType<Player>();
        currentLevel.text = Managers.Levels.Level.ToString();
        nextLevel.text = (Managers.Levels.Level + 1).ToString();
    }
}
