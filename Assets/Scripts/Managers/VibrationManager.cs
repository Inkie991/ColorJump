using UnityEngine;

public class VibrationManager: MonoBehaviour, IGameManager
{
        
    public ManagerStatus Status { get; private set; }
    
    public void Startup()
    {
        Debug.Log("Vibration manager starting...");
        
        Status = ManagerStatus.Started;
    }

    public void AttachSectorVibration(Sector sector)
    {
        sector.SectorIsBroken += (sender, color) => {
           if (Managers.Settings.DisableVibration)
               return;
           Handheld.Vibrate();
        };
    }
}