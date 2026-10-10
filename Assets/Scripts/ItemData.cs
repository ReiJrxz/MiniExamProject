using UnityEngine;

namespace ZiddikJari
{
public enum ItemEffect
    {
        AddTime,
        RemoveTime,
        AddScore,
        RemoveScore
    }

    [CreateAssetMenu(
        fileName = "NewItemData",
        menuName = "Practical Exam/Item Data"
    )]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public ItemEffect effect;

        [Min(0)]
        public int amount = 500;

        public AudioClip collectSFX;
    }
}
