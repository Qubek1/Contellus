using Cinemachine;
using Entities;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private GameObject playerPrefab;
    public GameObject mainCamera;
    public GameObject kamikazeEnemyPrefab;
    public GameObject tankEnemyPrefab;
    public GameObject enemyBulletPrefab;
    public CinemachineVirtualCamera virtualCamera;

    [Header("Spawning")] public float spawningAcceleration;
    public float spawnInterval;
    public float enemyAmount;
    public float enemySpawnIncrease;
    public float minSpawnRange;
    public float maxSpawnRange;

    /* !!! WARNING: CAN BE NULL (when player is dead) !!! */
    public GameObject CurrentPlayer { get; private set; }

    public bool IsGameRunning { get; private set; }

    public int points = 0;

    private float _spawningSpeed;
    private float _timeTillNextSpawn;
    private Transform _currentPlayerTransform;

    private void Awake()
    {
        Instance = this;

        SpawnPlayer();
        //SpawnEnemy();
    }

    private void Start()
    {
        StartGame();
        // Follow camera
        var playerTransform = CurrentPlayer.GetComponent<Transform>();
        virtualCamera.Follow = playerTransform;
        //virtualCamera.LookAt = playerTransform;
    }

    private void SpawnPlayer()
    {
        var player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        CurrentPlayer = player;
        _currentPlayerTransform = player.GetComponent<Transform>();
    }

    private void StartGame()
    {
        IsGameRunning = true;
        //_timeTillNextSpawn = initialSpawnSpeed;
        _timeTillNextSpawn = 0;
        points = 0;
    }

    private void Update()
    {
        if (IsGameRunning)
        {
            /*_spawningSpeed += spawningAcceleration * Time.deltaTime;
            _timeTillNextSpawn -= Time.deltaTime;
            if (_timeTillNextSpawn <= 0)
            {
                _timeTillNextSpawn = 1 / (initialSpawnSpeed + _spawningSpeed);
                SpawnRandomEnemy();
            }*/

            _timeTillNextSpawn -= Time.deltaTime;
            if(_timeTillNextSpawn <= 0)
            {
                _timeTillNextSpawn = spawnInterval;
                for(int i=0; i<(int)enemyAmount; i++)
                {
                    SpawnRandomEnemy();
                }
                enemyAmount += enemySpawnIncrease;
            }
        }
    }

    private void SpawnRandomEnemy()
    {
        var randomCircle = Random.onUnitSphere;
        randomCircle.y = 0;
        var location = randomCircle * Random.Range(minSpawnRange, maxSpawnRange) + _currentPlayerTransform.position;

        if ((location.x < 0 || location.x > BoardGenerator.boardSize * 6) ||
            (location.z < 0 || location.z > BoardGenerator.boardSize * 6))
        {
            return;
        }

        var rotation = new Vector3(0, Random.rotation.eulerAngles.y, 0);
        var enemyId = Random.Range(0, 2);
        switch (enemyId)
        {
            case 0:
                KamikazeEnemy.Spawn(CurrentPlayer, location, rotation);
                break;
            case 1:
                TankEnemy.Spawn(CurrentPlayer, location, rotation);
                break;
        }
    }

    /*
     * =============================================================
     * ========================= SINGLETON =========================
     * =============================================================
     */

    /// <summary>
    ///     Main instance getter
    /// </summary>
    public static Game Instance { get; private set; }
}