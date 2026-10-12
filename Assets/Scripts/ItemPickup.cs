using UnityEngine;
using TMPro;

namespace ZiddikJari
{
    public class ItemPickup : MonoBehaviour
    {
        [Header("Item data")]
        [SerializeField] private PickupItemData itemData;

        [Header("World-space prompt")]
        [SerializeField] private GameObject promptObject;
        [SerializeField] private TMP_Text promptLabel;

        private bool collected;

        public bool CanCollect => !collected && isActiveAndEnabled;

        private void Awake()
        {
            if (promptLabel != null && itemData != null)
                promptLabel.text = itemData.promptText;

            if (promptObject != null)
                promptObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerItemInteractor player =
                other.GetComponentInParent<PlayerItemInteractor>();

            if (player != null)
                player.RegisterItem(this);
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerItemInteractor player =
                other.GetComponentInParent<PlayerItemInteractor>();

            if (player != null)
                player.UnregisterItem(this);
        }

        public void SetPromptVisible(bool visible)
        {
            if (promptObject != null)
                promptObject.SetActive(visible && CanCollect);
        }

        public void Collect()
        {
            if (!CanCollect)
                return;

            if (itemData == null)
            {
                Debug.LogError("Pickup Item Data is not assigned.", this);
                return;
            }

            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager was not found in this scene.", this);
                return;
            }

            int amount = Mathf.Abs(itemData.amount);

            switch (itemData.effect)
            {
                case PickupEffect.AddTime:
                    GameManager.Instance.AddTime(amount);
                    break;

                case PickupEffect.RemoveTime:
                    GameManager.Instance.AddTime(-amount);
                    break;

                case PickupEffect.AddScore:
                    GameManager.Instance.AddScore(amount);
                    break;

                case PickupEffect.RemoveScore:
                    GameManager.Instance.AddScore(-amount);
                    break;
            }

            collected = true;
            SetPromptVisible(false);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayItemPickupSfx(itemData.pickupSfx);

            Destroy(gameObject);
        }
    }
}