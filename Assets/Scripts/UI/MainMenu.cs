using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        
        [SerializeField] private RectTransform settingsPanel;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button closeSettingsButton;

        [SerializeField] private Slider masterVolume;
        [SerializeField] private Slider musicVolume;

        [SerializeField] private AudioSource soundtrackPlayer;
        
        
        private void Start()
        {
            var audioSources = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            if (audioSources.Count >= 2)
            {
                audioSources.Remove(soundtrackPlayer);
                Destroy(soundtrackPlayer);
                soundtrackPlayer = audioSources[0];
            }
            else DontDestroyOnLoad(soundtrackPlayer);
            
            playButton.onClick.AddListener(() => SceneManager.LoadScene("Main"));
            settingsButton.onClick.AddListener(() => settingsPanel.gameObject.SetActive(true));
            closeSettingsButton.onClick.AddListener(() => settingsPanel.gameObject.SetActive(false));
            
            masterVolume.onValueChanged.AddListener(SaveMasterVolume);
            musicVolume.onValueChanged.AddListener(value => soundtrackPlayer.volume = value);
        }

        private void SaveMasterVolume(float value)
        {
            AudioListener.volume = value;
            PlayerPrefs.SetFloat("masterVolume", value);
            PlayerPrefs.Save();
        }
    }
}