using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager: MonoBehaviour, IGameManager
{
    [SerializeField] private AudioClip mainMusic;
    private AudioSource musicSource;
    private AudioSource audioSource;
    private AudioClip jumpClip;
    private AudioClip buttonClip;
    private List<AudioClip> sectorBrokenClips;
    private List<AudioClip> deathClips;
    public ManagerStatus Status { get; private set; }

    public void Startup()
    {
        Debug.Log("Audio manager starting...");
        audioSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        
        jumpClip = Resources.Load<AudioClip>("Sounds/jump");
        buttonClip = Resources.Load<AudioClip>("Sounds/button");
        sectorBrokenClips = new List<AudioClip>();
        sectorBrokenClips.Add(Resources.Load<AudioClip>("Sounds/sector_broken_1"));
        sectorBrokenClips.Add(Resources.Load<AudioClip>("Sounds/sector_broken_2"));

        deathClips = new List<AudioClip>();
        deathClips.Add(Resources.Load<AudioClip>("Sounds/death_1"));
        deathClips.Add(Resources.Load<AudioClip>("Sounds/death_2"));
        
        musicSource.clip = mainMusic;
        musicSource.loop = true;
        musicSource.Play();
        
        Status = ManagerStatus.Started;
    }

    public void AttachBounceableSounds(Bounceable bounceable)
    {
        bounceable.BouncedOffPlatform += (sender, y) => Play(jumpClip);
    }

    public void AttachPlayerSounds(Player player)
    {
        player.Died += (sender, args) => PlayRandom(deathClips);
    }

    public void AttachSectorSounds(Sector sector)
    {
        sector.SectorIsBroken += (sender, color) => PlayRandom(sectorBrokenClips);
    }

    private void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void PlayRandom(List<AudioClip> clips)
    {
       Play(Utils.GetRandomItem(clips)); 
    }

    public void MuteSounds(bool shouldMute)
    {
        audioSource.volume = shouldMute ? 0f : 1f;
    }

    public void MuteMusic(bool shouldMute)
    {
        musicSource.volume = shouldMute ? 0f : 1f;
    }

    public void PlayButtonSound()
    {
        Play(buttonClip);
    }
}
