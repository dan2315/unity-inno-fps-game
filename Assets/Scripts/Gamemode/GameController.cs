using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using DG.Tweening;
using Pickups;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Gamemode
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private int waveCount;
        [SerializeField] private List<Transform> enemySpawnPoints;
        [SerializeField] private Enemy enemyPrefab;

        [SerializeField] private Transform buildMarker;
        [SerializeField] private TurretBuilder turretBuilder;
        [SerializeField] private AudioListener audioListener;
        
        


        private int _waveCounter = 1;
        private int _enemiesCounter;
        private int _enemiesForWave;

        private bool _isPreparationStage;
        private bool _isPlayerDead;

        private static GameController _instance;
        public bool IsPreparationStage => _isPreparationStage;
        public bool IsPlayerDead => _isPlayerDead;
        public static GameController Instance => _instance;
        public Action<int, int, int> WaveInfoUpdated;
        
        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            StartPreparationStage();
            AudioListener.volume = PlayerPrefs.GetFloat("masterVolume", 1);
        }

        private void Update()
        {
            if (_isPreparationStage && Input.GetKeyDown(KeyCode.F))
            {
                EndPreparationStage();
            }

            if (_isPlayerDead && Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene("Main");
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                SceneManager.LoadScene("MainMenu");
            }
        }

        private void StartPreparationStage()
        {
            _isPreparationStage = true;
        }

        private void EndPreparationStage()
        {
            _isPreparationStage = false;
            buildMarker.gameObject.SetActive(false);
            turretBuilder.enabled = false;
            UIController.UiController.HidePreparationUI();
            SpawnWave();
        }
        
        private void SpawnWave()
        {
            _enemiesForWave = 2 + _waveCounter;
            int shieldedEnemies = Mathf.RoundToInt(_enemiesForWave * 0.33f);
            _enemiesCounter = _enemiesForWave;
            
            var temp = new Transform[enemySpawnPoints.Count];
            enemySpawnPoints.CopyTo(temp);
            var duplicatePoints = temp.ToList();

            for (int i = 0; i < _enemiesForWave; i++)
            {
                int index = Random.Range(0 ,duplicatePoints.Count);
                var position = duplicatePoints[index].position;
                duplicatePoints.RemoveAt(index);

                var enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
                enemy.OnDeath += HandleEnemyDeath;
                if (shieldedEnemies > 0)
                {
                    enemy.SetShield(Random.value * 2 >= 1 ? ModifierType.FireModifier : ModifierType.ElectricalModifier);
                    enemy.HP.AddArmor(25);
                    shieldedEnemies--;
                }
            }
            
            WaveInfoUpdated?.Invoke(_enemiesCounter,_enemiesForWave, _waveCounter);
        }

        private void HandleEnemyDeath()
        {
            _enemiesCounter--;
            WaveInfoUpdated?.Invoke(_enemiesCounter,_enemiesForWave, _waveCounter);

            if (_enemiesCounter <= 0)
            {
                DOTween.Sequence().AppendInterval(2).OnComplete(EndWave);
            }
        }

        private void EndWave()
        {
            _waveCounter++;
            if (_waveCounter <= waveCount)
            {
                SpawnWave();
            }
            else
            {
                HandleWin();
            }
        }

        private void HandleWin()
        {
            SceneManager.LoadScene("WinScene");
        }

        public void HandleLose()
        {
            _isPlayerDead = true;
            UIController.UiController.ShowDeathScreen();
        }
    }
}