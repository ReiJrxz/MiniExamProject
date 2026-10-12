using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace ZiddikJari
{
    public class PlayerItemInteractor : MonoBehaviour
    {
        private readonly HashSet<ItemPickup> availableItems =
            new HashSet<ItemPickup>();

        private ItemPickup currentTarget;
        private bool interactPressed;
        private void Update()
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsPlaying)
            {
                SetCurrentTarget(null);
                return;
            }

            // กำจัด reference ของไอเท็มที่ถูกทำลายแล้ว
            availableItems.RemoveWhere(item => item == null);

            ItemPickup nearestItem = FindNearestItem();
            SetCurrentTarget(nearestItem);

            if (!interactPressed)
                return;

            interactPressed = false;

            if (currentTarget != null)
            {
                ItemPickup itemToCollect = currentTarget;

                availableItems.Remove(itemToCollect);
                SetCurrentTarget(null);

                itemToCollect.Collect();
            }
        }

        private ItemPickup FindNearestItem()
        {
            ItemPickup nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (ItemPickup item in availableItems)
            {
                if (item == null || !item.CanCollect)
                    continue;

                float distance =
                    (item.transform.position - transform.position).sqrMagnitude;

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = item;
                }
            }

            return nearest;
        }
        public void OnInteract(InputValue value)
        {
            if (value.isPressed)
                interactPressed = true;
        }
        private void SetCurrentTarget(ItemPickup newTarget)
        {
            if (currentTarget == newTarget)
                return;

            if (currentTarget != null)
                currentTarget.SetPromptVisible(false);

            currentTarget = newTarget;

            if (currentTarget != null)
                currentTarget.SetPromptVisible(true);
        }

        public void RegisterItem(ItemPickup item)
        {
            if (item != null)
                availableItems.Add(item);
        }

        public void UnregisterItem(ItemPickup item)
        {
            if (item == null)
                return;

            availableItems.Remove(item);

            if (currentTarget == item)
                SetCurrentTarget(null);
        }

        private void OnDisable()
        {
            SetCurrentTarget(null);
            availableItems.Clear();
        }
    }
}