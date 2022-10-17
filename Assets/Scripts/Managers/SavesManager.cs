using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavesManager: MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }

    private string _filename;

    public void Startup()
    {
        Debug.Log("Saves manager starting...");

        _filename = Path.Combine(Application.persistentDataPath, "save");
        Load();

        Status = ManagerStatus.Started;
    }

    private void OnDestroy()
    {
        Save();
    }

    public void Load()
    {
        if (!File.Exists(_filename)) {
            Debug.Log("No saved game");
            return;
        }

        Dictionary<string, object> gamestate;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(_filename, FileMode.Open);
        gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();

        Managers.Levels.Level = (int) (gamestate.TryGetValue("level", out var level) ? level : 1);
        Managers.Settings.DisableMusic = (bool) (gamestate.TryGetValue("disableMusic", out var disableMusic) ? disableMusic : false);
        Managers.Settings.DisableSound = (bool) (gamestate.TryGetValue("disableSound", out var disableSound) ? disableSound : false);
        Managers.Settings.DisableVibration = (bool) (gamestate.TryGetValue("disableVibration", out var disableVibration) ? disableVibration : false);
    }

    public void Save()
    {
        Dictionary<string, object> gameState = new Dictionary<string, object>();
        gameState.Add("level", Managers.Levels.Level);
        gameState.Add("disableSound", Managers.Settings.DisableSound);
        gameState.Add("disableMusic", Managers.Settings.DisableMusic);
        gameState.Add("disableVibration", Managers.Settings.DisableVibration);
        
        FileStream stream = File.Create(_filename);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, gameState);
        stream.Close();
    }
}