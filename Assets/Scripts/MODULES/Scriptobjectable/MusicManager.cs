using System.Collections.Generic;
using MANAGERS;
using UnityEngine;

namespace MODULES.Scriptobjectable
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance;
        AudioSource m_audioSource;
        public List<AudioClip> LIST_SOUND_TRACK;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
            DontDestroyOnLoad(this.gameObject);

            m_audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {
            Mute = GetValueMuteFormData();
            Play();
        }

        public bool Mute
        {
            get
            {
                return m_audioSource.mute;
            }
            set
            {
                m_audioSource.mute = value;
                if (m_audioSource.mute)
                    PlayerPrefs.SetString("Music", "off");
                else
                    PlayerPrefs.SetString("Music", "on");
                PlayerPrefs.Save();
            }
        }

        private bool GetValueMuteFormData()
        {
            if (PlayerPrefs.GetString("Music") == "off")
                return true;
            else return false;
        }

        public void Stop()
        {
            m_audioSource.Stop();
        }

        public void Play()
        {
            if (m_audioSource.isPlaying) return;

            int _total = LIST_SOUND_TRACK.Count;
            _total = UnityEngine.Random.Range(0, _total);

            m_audioSource.Stop();
            m_audioSource.clip = LIST_SOUND_TRACK[_total];
            m_audioSource.Play();
        }

        public void PlayHard()
        {
            int _total = LIST_SOUND_TRACK.Count;
            _total = UnityEngine.Random.Range(0, _total);

            m_audioSource.Stop();
            m_audioSource.clip = LIST_SOUND_TRACK[_total];
            m_audioSource.Play();
        }
        
    }
}
