using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "CoinWaypoint", menuName = "Scriptable/CoinWaypoint")]
    public class CoinWaypoint: ScriptableObject
    {
        public Vector3 waypointStart;
        public Vector3 waypointEnd;
    }
}