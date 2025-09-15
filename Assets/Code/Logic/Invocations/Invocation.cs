using UnityEngine;

namespace Code.Logic.Invocations
{
    public class Invocation : MonoBehaviour
    {
        public void Initialize(string uniqueId) => 
            UniqueId = uniqueId;

        public string UniqueId { get; private set; }
    }
}