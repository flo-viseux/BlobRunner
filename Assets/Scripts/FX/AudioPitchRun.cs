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

        private void Start()
        {
            UIManager.Instance.pauseEvent += PauseRunClip;
        }

        private void OnDestroy()
        {
            UIManager.Instance.pauseEvent -= PauseRunClip;
        }

        private void OnEnable()
        {
            //UIManager.Instance.pauseEvent += PauseRunClip;
            pitchRoutine = StartCoroutine(VaryPitchRoutine());
            GameManager.Instance.coroutineStorage.AddRoutine(pitchRoutine);
            
            EventManager.ShrinkEvent += ShrinkPitch;
            EventManager.StopRunEvent += StopRun;
            EventManager.DeathEvent += StopRun;
        }
        

        private void OnDisable()
        {
            //UIManager.Instance.pauseEvent -= PauseRunClip;
            EventManager.ShrinkEvent -= ShrinkPitch;
            EventManager.StopRunEvent -= StopRun;
            EventManager.DeathEvent -= StopRun;
        }

        private void PauseRunClip(bool isPaused)
        {
            audioSource.mute = isPaused;
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
            GameManager.Instance.coroutineStorage.RemoveRoutine(pitchRoutine);
            
            // //Debug.Log("Shrink");
            // audioSource.pitch = 1.6f;
            // //audioSource.DOPitch(1.6f, 0.2f).SetEase(Ease.InOutQuad);
        }
        private void StopRun()
        {
            StopCoroutine(pitchRoutine);
            GameManager.Instance.coroutineStorage.RemoveRoutine(pitchRoutine);
            
            audioSource.Stop();
        }

        public void Play()
        {
            audioSource.Play();
            audioSource.pitch = 1f;
            
            pitchRoutine = StartCoroutine(VaryPitchRoutine());
            GameManager.Instance.coroutineStorage.AddRoutine(pitchRoutine);
        }

        public void Stop()
        {
            StopCoroutine(pitchRoutine);
            GameManager.Instance.coroutineStorage.RemoveRoutine(pitchRoutine);
            
            audioSource.Stop();
        }
    }
