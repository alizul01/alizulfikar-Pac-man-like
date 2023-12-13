using UnityEditor;
using UnityEngine;

namespace Managers
{
    [System.Serializable]
    public class CoinWaypoint
    {
        public Transform waypointStart;
        public Transform waypointEnd;
    }

    public enum SpawnAxis
    {
        X,
        Z
    }

    public class CoinSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private GameObject powerUpPrefab;
        [SerializeField] private CoinWaypoint[] coinWaypoints;
        [SerializeField] private float spacing = 1.0f;
        [SerializeField] private SpawnAxis spawnAxis = SpawnAxis.X;

        // HideInInspector prevents the array from being visible in the Inspector
        [HideInInspector] public bool coinWaypointsInitialized;

        private void Start()
        {
            if (!coinWaypointsInitialized)
            {
                Debug.LogError("Don't forget to assign CoinWaypoints in the Inspector!");
            }

            SpawnCoinsAlongWaypoints();
            
            var pickableManager = FindObjectOfType<PickableManager>();
            if (pickableManager != null)
            {
                pickableManager.InitPickableList();
            }
        }

        private void SpawnCoinsAlongWaypoints()
        {
            foreach (var coinWaypoint in coinWaypoints)
            {
                float distance = Vector3.Distance(coinWaypoint.waypointStart.position, coinWaypoint.waypointEnd.position);
                int numberOfCoins = Mathf.FloorToInt(distance / spacing);

                // Always spawn power-up at the start
                Instantiate(powerUpPrefab, coinWaypoint.waypointStart.position, Quaternion.identity);

                for (int i = 1; i < numberOfCoins - 1; i++)
                {
                    float t = i / (float)(numberOfCoins - 1);
                    Vector3 spawnPosition = GetSpawnPosition(coinWaypoint, t);

                    Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
                }

                // Always spawn power-up at the end
                Instantiate(powerUpPrefab, coinWaypoint.waypointEnd.position, Quaternion.identity);
            }
        }

        private Vector3 GetSpawnPosition(CoinWaypoint waypoint, float t)
        {
            if (spawnAxis == SpawnAxis.X)
            {
                return Vector3.Lerp(waypoint.waypointStart.position, waypoint.waypointEnd.position, t);
            }
            else
            {
                return Vector3.Lerp(waypoint.waypointStart.position, new Vector3(waypoint.waypointEnd.position.x, waypoint.waypointStart.position.y, waypoint.waypointEnd.position.z), t);
            }
        }

        private void OnDrawGizmos()
        {
            if (coinWaypoints == null || coinWaypoints.Length == 0) return;
            foreach (var coinWaypoint in coinWaypoints)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(coinWaypoint.waypointStart.position, 0.2f);

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(coinWaypoint.waypointEnd.position, 0.2f);

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(coinWaypoint.waypointStart.position, coinWaypoint.waypointEnd.position);
            }
        }

        // Custom editor script to handle initialization
#if UNITY_EDITOR
        [CustomEditor(typeof(CoinSpawner))]
        public class CoinSpawnerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                CoinSpawner coinSpawner = (CoinSpawner)target;

                // Draw the default inspector
                DrawDefaultInspector();

                // Check if coinWaypoints is null or empty
                if (coinSpawner.coinWaypoints == null || coinSpawner.coinWaypoints.Length == 0)
                {
                    EditorGUILayout.HelpBox("Don't forget to assign CoinWaypoints in the Inspector!", MessageType.Warning);
                    coinSpawner.coinWaypointsInitialized = false;
                }
                else
                {
                    coinSpawner.coinWaypointsInitialized = true;
                }
            }
        }
#endif
    }
}
