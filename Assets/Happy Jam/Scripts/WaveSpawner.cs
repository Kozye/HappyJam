using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using Happy;

[Serializable]
public class CountStringEvent : UnityEvent<string> { }
public class WaveSpawner : MonoBehaviour
{
    const float ENEMY_SEARCH_TIME_FREQUENCY = 1f;

    public enum SpawnState
    {
        Off,
        Spawning,
        Waiting,
        Counting
    }

    public Wave[] Waves;
    public float TimeBetweenWaves = 5f;
    public bool LoopAfterComplete = false;
    public bool ClearWaveBeforeNext = false;
    public bool CheckSpawnOverlap = true;
    public bool SetEnemyRotationOnSpawn = true;

    [Header("ReadOnly")]
    [SerializeField]
    private float __WaveCountDown;
    [SerializeField]
    private SpawnState _State = SpawnState.Off;

    [Header("Events")]
    public CountStringEvent WaveTotalCallback = new CountStringEvent();
    //public CustomUnityEvent OnWavesStart = new CustomUnityEvent();
    public CustomUnityEvent OnWaveStart = new CustomUnityEvent();
    public CountStringEvent OnWaveStartStringCount = new CountStringEvent();
    public CustomUnityEvent OnWavesCompleted = new CustomUnityEvent();
    public CustomUnityEvent OnWaveCompleted = new CustomUnityEvent();

    private int _nextWave = -1;
    private float _searchCountdown = ENEMY_SEARCH_TIME_FREQUENCY;
    private Transform _transform;

    [Header("Testing")]
    public int RemainingEnemies = 1;
    public bool StartOnStart = false;

    void Awake()
    {
        __WaveCountDown = TimeBetweenWaves;
        _State = SpawnState.Off;
        _transform = transform;
        WaveTotalCallback.Invoke(Waves.Length.ToString());
        if (StartOnStart) _State = SpawnState.Waiting;
    }

    void Update()
    {
        if (_State == SpawnState.Waiting)
        {
            if (!ClearWaveBeforeNext || !IsEnemyAlive())
            {
                WaveCompleted();
            }
        }
        else if (_State != SpawnState.Off)
        {
            if (__WaveCountDown <= 0)
            {
                if (_State != SpawnState.Spawning)
                {
                    OnWaveStartStringCount.Invoke((_nextWave + 1).ToString());
                    StartCoroutine(SpawnWave(Waves[_nextWave]));
                }
            }
            else
            {
                __WaveCountDown -= Time.deltaTime;
            }
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        _State = SpawnState.Spawning;
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.GetWaveEnemy());
            yield return new WaitForSeconds(wave.delay);
        }
        _State = SpawnState.Waiting;
        yield break;
    }

    void SpawnEnemy(WaveEnemy waveEnemy)
    {
        Debug.Log("Spawning Enemy " + waveEnemy.enemy.name);
        Transform spawnTransform = waveEnemy.GetSpawn(CheckSpawnOverlap);

        if (spawnTransform == null)
        {
            Debug.LogWarning("Spawn transform is null. It may overlap with other physical gameobject");
            return;
            //spawnTransform = _transform;
        }

        Transform enemy = Instantiate(waveEnemy.enemy, spawnTransform.position, spawnTransform.rotation) as Transform;
        if (SetEnemyRotationOnSpawn)
        {
            SetTargetOrientation(GetTarget(), enemy);

        }
    }
    void WaveCompleted()
    {
        Debug.Log("Wave Completed");
        if (OnWaveCompleted != null) OnWaveCompleted.Invoke();

        __WaveCountDown = TimeBetweenWaves;
        if (_nextWave + 1 > Waves.Length - 1)
        {
            Debug.Log("All Wave's Completed");
            if (OnWaveCompleted != null) OnWavesCompleted.Invoke();
            if (LoopAfterComplete)
            {
                _nextWave = 0;
            }
            else
            {
                _State = SpawnState.Off;
            }
        }
        else
        {
            _nextWave++;
            _State = SpawnState.Counting;
        }
    }

    // Enemy must have Agent component
    void SetTargetOrientation(GameObject target, Transform enemy)
    {
        // Calculating rotation
        float targetOrientation = 0;
        if (target)
        {
            Vector3 direction = target.transform.position - enemy.position;
            targetOrientation = Mathf.Atan2(direction.y, direction.x);
            targetOrientation *= Mathf.Rad2Deg;

            AI.Agent2D agent = enemy.GetComponent<AI.Agent2D>();
            if (agent)
                agent.rotation = targetOrientation;
            else
                enemy.transform.eulerAngles = new Vector3(0, 0, targetOrientation);
        }
    }
    GameObject GetTarget()
    {
        // TODO GameManager.Instance.GetPlayer(spawnTransform.position);
        return null;
    }
    bool IsEnemyAlive()
    {
        bool alive = true;
        _searchCountdown -= Time.deltaTime;

        if (_searchCountdown <= 0)
        {
            _searchCountdown = ENEMY_SEARCH_TIME_FREQUENCY;
            if (GetCurrentEnemiesCount() == 0)
                alive = false;
        }
        return alive;
    }

    public int GetCurrentEnemiesCount()
    {
        // TODO REPLACE
        return RemainingEnemies;
        //GameManager.Instance.Enemies.Length
    }

    public void EnableSpawning(bool enable)
    {
        if (enable)
        {
            _State = SpawnState.Counting;
        }
        else
        {
            _State = SpawnState.Off;
        }
    }


    [System.Serializable]
    public class Wave
    {
        public string name;
        public WaveEnemy[] enemies;
        [Tooltip("Number of enemies to spawn")]
        public int count;
        public bool spawnRandom = false;
        public float delay;

        public WaveEnemy GetWaveEnemy()
        {
            WaveEnemy enemy = null;
            if (!spawnRandom)
            {
                int maxDiff = -1;
                // Get enemy with less spawns
                foreach (WaveEnemy e in enemies)
                {
                    if (e.Count - e.counter > maxDiff)
                    {
                        maxDiff = e.Count - e.counter;
                        enemy = e;
                    }
                }
            }
            // Get random enemy
            if (spawnRandom || enemy == null)
            {
                int random = UnityEngine.Random.Range(0, 100);
                if (enemies != null && enemies.Length > 0)
                {
                    enemy = enemies[random % enemies.Length];
                }
            }
            if (enemy != null)
                enemy.counter++;
            return enemy;
        }
    }

    [System.Serializable]
    public class WaveEnemy
    {
        public Transform enemy;
        public Transform[] spawns;
        public int Count;
        [HideInInspector]
        public int counter = 0;

        public Transform GetSpawn(bool checkOverlap)
        {
            Transform transf = null;
            if (spawns != null && spawns.Length > 0)
            {
                transf = GetRandomSpawn();
                if (IsOverlaping(transf))
                {
                    transf = null;
                    foreach (Transform t in spawns)
                    {
                        if (!IsOverlaping(t))
                        {
                            transf = t;
                            break;
                        }
                    }
                }
            }
            return transf;
        }

        public Transform GetRandomSpawn()
        {
            return spawns[UnityEngine.Random.Range(0, spawns.Length)];
        }
        public bool IsOverlaping(Transform pos)
        {
            float radius = 1f;
            RaycastHit2D hit = Physics2D.CircleCast(pos.position, radius, Vector2.zero);
            if (hit)
            {
                Debug.Log("OVERLAPING " + hit.collider);
                //if(hit.collider.tag)
                return true;
            }
            return false;
        }
    }
}
