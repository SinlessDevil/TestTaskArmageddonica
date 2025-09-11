using UnityEngine;

namespace Code.StaticData
{
    [System.Serializable]
    public struct HeadRotation
    {
        public Quaternion PlayerRotation;
        public Quaternion EnemyRotation;
    }
}