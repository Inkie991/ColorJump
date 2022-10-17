using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[RequireComponent(typeof(SavesManager))]
[RequireComponent(typeof(SettingsManager))]
[RequireComponent(typeof(LevelManager))]
[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(VibrationManager))]
public class Managers : MonoBehaviour
{
    public static SavesManager Saves { get; private set; }
    public static SettingsManager Settings { get; private set; }
    public static AudioManager Audio { get; private set; }
    public static VibrationManager Vibration { get; private set; }
    public static LevelManager Levels { get; private set; }
    public static UIManager UI { get; private set; }

    private List<IGameManager> _startSequence;

    void Awake()
    {
        Saves = GetComponent<SavesManager>();
        Settings = GetComponent<SettingsManager>();
        Audio = GetComponent<AudioManager>();
        Vibration = GetComponent<VibrationManager>();
        Levels = GetComponent<LevelManager>();
        UI = GetComponent<UIManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Audio);
        _startSequence.Add(Vibration);
        _startSequence.Add(Saves);
        _startSequence.Add(Levels);
        _startSequence.Add(UI);
        _startSequence.Add(Settings);

        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartupManagers());
    }

    private IEnumerator<Object> StartupManagers()
    {
        foreach (IGameManager manager in _startSequence)
        {
            manager.Startup();
        }

        yield return null;

        int numModules = _startSequence.Count;
        int numReady = 0;

        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach (IGameManager manager in _startSequence)
            {
                if (manager.Status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }

            if (numReady > lastReady)
                Debug.Log("Progress: " + numReady + "/" + numModules);
            yield return null;
        }

        Debug.Log("All managers started up");
    }
}