using UnityEngine;

namespace ZiddikJari
{
    public enum PickupEffect
    {
        AddTime,
        RemoveTime,
        AddScore,
        RemoveScore
    }

    [CreateAssetMenu(
        fileName = "NewPickupItem",
        menuName = "Ziddik Jari/Item/Pickup Data"
    )]
    public class PickupItemData : ScriptableObject
    {
        public string itemName = "Item";
        public string promptText = "กด E เพื่อเก็บ";

        public PickupEffect effect;
        public int amount = 500;

        public AudioClip pickupSfx;
    }
}