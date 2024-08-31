using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Obstacles
{
    [RequireComponent(typeof(AudioSource))]
    public class ActiveObjectSFX : MonoBehaviour
    {
        private AudioSource _source;
        [SerializeField] private AudioSource sourceForStart;
        [Header("Truck")]
        [SerializeField] private bool isTruck = false;
        [SerializeField] private AudioClip[] truckClips;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            if (isTruck && sourceForStart != null && truckClips.Length != 0)
            {
                int rand = Random.Range(0, truckClips.Length);
                AudioClip clip = truckClips[rand];
                sourceForStart.PlayOneShot(clip);
            }
            _source.Play();
        }

        public void OnDisable()
        {
            _source.Stop();
        }
    }
}