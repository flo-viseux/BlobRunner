using System;
using UnityEngine;

namespace Gameplay.Obstacles
{
    [RequireComponent(typeof(AudioSource))]
    public class ActiveObjectSFX : MonoBehaviour
    {
        private AudioSource _source;
        [SerializeField] private AudioSource sourceForStart;
        [SerializeField] private bool isTruck = false;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            if (isTruck && sourceForStart != null)
            {
                sourceForStart.PlayOneShot(sourceForStart.clip);
            }
            _source.Play();
        }

        public void OnDisable()
        {
            _source.Stop();
        }
    }
}