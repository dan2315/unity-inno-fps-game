using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Pickups;
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

        private int _waveCounter = 1;
        private int _enemiesCounter;
        private int _enemiesForWave;

        private static GameController _instance;
        
        public static GameController Instance => _instance;
        public Action<int, int, int> WaveInfoUpdated;
        
        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            SpawnWave();
        }

        private void SpawnWave()
        {
            _enemiesForWave = 2 + _waveCounter;
            int shieldedEnemies = Mathf.Max(-2 + _waveCounter, 0);
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
                EndWave();
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

        private void HandleLose()
        {
            SceneManager.LoadScene("Main");
        }
    }
}