using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TheMagician
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioMixer audioMixer;

        public static AudioManager INSTANCE;

        void Awake()
        {
            if (!INSTANCE) INSTANCE = this;
            HubAPI.OnVolumeChanged += HandleVolumeChanged;
        }

        void OnDestroy()
        {
            HubAPI.OnVolumeChanged -= HandleVolumeChanged;
        }

        void HandleVolumeChanged()
        {
            // change the volume of all three channels i.e,
            audioMixer.SetFloat("Sound", -80f + 80f * HubAPI.SoundVolume);
            audioMixer.SetFloat("Music", -80f + 80f * HubAPI.MusicVolume);
        }
    }
}
