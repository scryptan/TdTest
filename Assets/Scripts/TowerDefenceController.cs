using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TdTest.Input;
using TdTest.Targets;
using UnityEngine;

namespace TdTest
{
    public class TowerDefenceController : MonoBehaviour
    {
        [SerializeField] private GameState currentState = GameState.Pause;
        [SerializeField] private int targetCountOnCurrentStageDefault = 5;
        [SerializeField] private int currentWave;
        [SerializeField] private int allWaves = 3;
        [SerializeField] private int chillSecondsDefault = 15;
        [SerializeField] private List<Transform> wayPoints;

        private TargetSpawner _targetSpawner;
        private TargetSpawnPoint _targetSpawnPoint;
        private List<Target> _waveTargets = new List<Target>();
        private List<InputControllerBase> _controllers = new List<InputControllerBase>();
        private Coroutine _chillTimer;
        private int _chillSeconds;        
        private int targetCountOnCurrentStage;


        private void Awake()
        {
            _targetSpawner = FindObjectOfType<TargetSpawner>();
            _targetSpawnPoint = FindObjectOfType<TargetSpawnPoint>();
            _controllers = FindObjectsOfType<InputControllerBase>().ToList();
        }

        private void Update()
        {
            _controllers.ForEach(x => x.enabled = true);
            switch (currentState)
            {
                case GameState.Pause:
                    _controllers.ForEach(x => x.enabled = false);
                    break;
                case GameState.Attack:
                    if (targetCountOnCurrentStage <= 0 && _waveTargets.All(x => x.Health <= 0))
                    {
                        _waveTargets.Clear();
                        currentState = GameState.Chill;
                        if (currentWave >= allWaves)
                            currentState = GameState.Pause;
                    }

                    break;
                case GameState.Chill:
                    _chillTimer ??= StartCoroutine(StartChillTimer());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnEnable()
        {
            _targetSpawnPoint.OnTargetExit += SpawnEnemy;
        }

        private void SpawnEnemy()
        {
            if (targetCountOnCurrentStage <= 0 || currentState != GameState.Attack) return;

            targetCountOnCurrentStage--;
            var target = _targetSpawner.Spawn();
            target.Init(wayPoints);
            _waveTargets.Add(target);
        }
        
        private void SpawnEnemyImmediately()
        {
            targetCountOnCurrentStage--;
            var target = _targetSpawner.Spawn();
            target.Init(wayPoints);
            _waveTargets.Add(target);
        }

        private IEnumerator StartChillTimer()
        {
            _chillSeconds = chillSecondsDefault;
            while (_chillSeconds >= 0)
            {
                Debug.Log(_chillSeconds);
                _chillSeconds--;
                yield return new WaitForSecondsRealtime(1);
            }

            _chillTimer = null;
            SetAttackState();
        }

        private void SetAttackState()
        {
            SpawnEnemyImmediately();
            targetCountOnCurrentStage = targetCountOnCurrentStageDefault;
            currentState = GameState.Attack;
            currentWave++;
        }

        private void OnDisable()
        {
            _targetSpawnPoint.OnTargetExit -= SpawnEnemy;
        }
    }
}