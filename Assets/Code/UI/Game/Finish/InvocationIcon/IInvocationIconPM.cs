using UnityEngine;

namespace Code.UI.Game.Finish.InvocationIcon
{
    public interface IInvocationIconPM
    {
        string GetName();
        Sprite GetSprite();
        int GetQuantity();
    }
}