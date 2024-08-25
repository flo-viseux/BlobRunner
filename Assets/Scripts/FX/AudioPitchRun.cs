using System;
using System.Collections;
using DG.Tweening;
using Runner.Player;
using UnityEngine;
using Random = UnityEngine.Random;


public class AudioPitchRun : MonoBehaviour
    {
        private AudioSource audioSource;
        private Coroutine pitchRoutine;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            pitchRoutine = StartCoroutine(VaryPitchRoutine());
            EventManager.ShrinkEvent += ShrinkPitch;
            EventManager.StopRunEvent += StopRun;
        }
        

        private void OnDisable()
        {
            StopAllCoroutines();
            EventManager.ShrinkEvent -= ShrinkPitch;
            EventManager.StopRunEvent -= StopRun;
        }

        private IEnumerator VaryPitchRoutine()
        {
            while (true)
            {
                float targetPitch = Random.Range(-0.2f, 0.1f);
                float interval = Random.Range(2, 3);
                audioSource.DOPitch(1+targetPitch, interval).SetEase(Ease.InOutQuad);

                yield return new WaitForSeconds(interval);
            }
        }
        
        private void ShrinkPitch()
        {
            StopCoroutine(pitchRoutine);
            Debug.Log("Shrink");
            audioSource.pitch = 1.6f;
            //audioSource.DOPitch(1.6f, 0.2f).SetEase(Ease.InOutQuad);
        }
        private void StopRun()
        {
            StopCoroutine(pitchRoutine);
            audioSource.Stop();
        }

        public void Play()
        {
            audioSource.Play();
            audioSource.pitch = 1f;
            pitchRoutine = StartCoroutine(VaryPitchRoutine());
        }

        public void Stop()
        {
            StopCoroutine(pitchRoutine);
            audioSource.Stop();
        }
    }
