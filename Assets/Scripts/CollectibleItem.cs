
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player"))
            return;

        if (itemData == null)
        {
            Debug.LogError(
                $"{name}: ItemData has not been assigned.",
                this
            );
            return;
        }

        GameManager manager = GameManager.Instance;

        if (manager == null)
        {
            Debug.LogError("GameManager.Instance is null.");
            return;
        }

        collected = true;

        switch (itemData.effect)
        {
            case ItemEffect.AddTime:
                manager.AddTime(itemData.amount);
                break;

            case ItemEffect.RemoveTime:
                manager.AddTime(-itemData.amount);
                break;

            case ItemEffect.AddScore:
                manager.AddScore(itemData.amount);
                break;

            case ItemEffect.RemoveScore:
                manager.AddScore(-itemData.amount);
                break;
        }

        if (itemData.collectSFX != null)
        {
            AudioSource.PlayClipAtPoint(
                itemData.collectSFX,
                transform.position
            );
        }

        Destroy(gameObject);
    }
}
