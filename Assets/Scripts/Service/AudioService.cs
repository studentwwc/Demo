using UnityEngine;

namespace Service
{
    public class AudioService:MonoBehaviour
    {
        private static AudioService instance;

        public static AudioService Instance
        {
            get
            {
                return instance;
            }
        }

        [SerializeField] private AudioSource bgAudio;
        [SerializeField] private  AudioSource uiAudio;
        public void Init()
        {
            NetCommon.Log("AudioService Init",NetLogType.Normal);
            instance = this;
            
        }
        
        public void PlayBgAudio(string audioName,bool isLoop=true)
        {
            AudioClip ac= ResService.Instance.LoadAudio(audioName);
            if (bgAudio.clip == null || bgAudio.clip!=ac)
            {
                bgAudio.clip = ac;
                bgAudio.loop = isLoop;
                bgAudio.Play();
            }
        }

        public void PlayUIAudio(string audioName)
        {
            AudioClip ac= ResService.Instance.LoadAudio(audioName);
            uiAudio.clip = ac;
            uiAudio.Play();
        }
        public void PlayPlayerAudio(string audioName,AudioSource audioSource)
        {
            AudioClip ac= ResService.Instance.LoadAudio(audioName);
            audioSource.clip = ac;
            audioSource.Play();
        }
    }
}