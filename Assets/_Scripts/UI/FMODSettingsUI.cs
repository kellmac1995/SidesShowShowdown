using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODSettingsUI : MonoBehaviour {


    public FMOD.Studio.Bus Master;
    public FMOD.Studio.Bus Music;
    public FMOD.Studio.Bus SFX;

    float masterVolume = 1.0f;
    float musicVolume = 1.0f;
    float sfxVolume = 1.0f;

    private void Awake()
    {
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/FX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");

    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }

    }


    public void MasterVolumeLevel(float newVolume)
    {
        masterVolume = newVolume;
        Master.setVolume(masterVolume);
    }


    public void MusicVolumeLevel(float newVolume)
    {
        musicVolume = newVolume;
        Music.setVolume(musicVolume);
    }

    public void SFXVolumeLevel(float newVolume)
    {
        sfxVolume = newVolume;
        SFX.setVolume(sfxVolume);
    }






}
