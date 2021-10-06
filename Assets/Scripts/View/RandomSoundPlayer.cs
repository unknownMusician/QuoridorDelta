using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace QuoridorDelta.View
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class RandomSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _clips;

        private readonly Random _random = new Random();
        private AudioSource _source;

        private void Awake() => _source = GetComponent<AudioSource>();

        public void PlayNext()
        {
            _source.clip = _clips[_random.Next(_clips.Length)];
            _source.Play();
        }
    }
}
