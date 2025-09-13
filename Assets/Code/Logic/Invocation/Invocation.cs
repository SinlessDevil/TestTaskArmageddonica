using UnityEngine;

namespace Code.Logic.Invocation
{
    public class Invocation : MonoBehaviour
    {
        public void Initialize(string uniqueId)
        {
            UniqueId = uniqueId;
        }
        
        public string UniqueId { get; private set; }
        
        // Методы для воспроизведения эффектов скиллов
        public virtual void PlayAttackBuffEffect()
        {
            // Базовая реализация - можно переопределить в наследниках
            Debug.Log($"[Invocation] PlayAttackBuffEffect for {UniqueId}");
        }
        
        public virtual void PlayHealthBuffEffect()
        {
            // Базовая реализация - можно переопределить в наследниках
            Debug.Log($"[Invocation] PlayHealthBuffEffect for {UniqueId}");
        }
        
        public virtual void PlaySpeedBuffEffect()
        {
            // Базовая реализация - можно переопределить в наследниках
            Debug.Log($"[Invocation] PlaySpeedBuffEffect for {UniqueId}");
        }
        
        public virtual void PlayCapacityEffect()
        {
            // Базовая реализация - можно переопределить в наследниках
            Debug.Log($"[Invocation] PlayCapacityEffect for {UniqueId}");
        }
    }
}