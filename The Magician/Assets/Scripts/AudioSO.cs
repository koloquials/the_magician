using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TheMagician
{
    [CreateAssetMenu(menuName = "TheMagician/AudioSO")]
    public class AudioSO : ScriptableObject
    {
        [SerializeField] AudioClip audioClip;
        [SerializeField] AudioMixerGroup audioMixerGroup;
        public void Play()
        {

        }
    }
}
