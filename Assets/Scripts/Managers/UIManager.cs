using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGameManager
{
    [SerializeField] private GameObject uiContainer;
    public ManagerStatus Status { get; private set; }

    private Dictionary<String, Canvas> _canvasMap;
        
    public void Startup()
    {
        DontDestroyOnLoad(uiContainer.gameObject);
        CollectCanvas();
        SceneManager.sceneLoaded += OnSceneLoaded;
        Status = ManagerStatus.Started;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Preloader" ) return;
        
        ToggleSettings(true);
        ToggleScore(true);
        ToggleWin(false);
        ToggleLose(false);
    }

    void CollectCanvas()
    {
        _canvasMap = new Dictionary<string, Canvas>();
        List<Canvas> canvasArray = uiContainer.transform.GetComponentsInChildren<Canvas>().ToList();
        canvasArray.ForEach(canvas => _canvasMap.Add(canvas.name, canvas));
    }
    
    public void ToggleScore(bool value)
    {
        _canvasMap["Score"].gameObject.SetActive(value);
    }
    
    public void ToggleSettings(bool value)
    {
        _canvasMap["Settings"].gameObject.SetActive(value);
    }

    public void ToggleWin(bool value)
    {
        _canvasMap["Win"].gameObject.SetActive(value);
    }

    public void ToggleLose(bool value)
    {
        _canvasMap["Lose"].gameObject.SetActive(value);
    }

    public void OnRestart()
    {
        Managers.Levels.Reload();
    }

    public void OnContinue()
    {
        Managers.Levels.NextLevel();
    }

    public void OnMusicClick()
    {
        Managers.Audio.PlayButtonSound();
        Managers.Settings.OnMusicMute();
    }

    public void OnSoundClick()
    {
        Managers.Audio.PlayButtonSound();
        Managers.Settings.OnSoundMute();
    }

    public void OnVibrationClick()
    {
        Managers.Audio.PlayButtonSound();
        Managers.Settings.OnVibrationDisable();
    }
}
