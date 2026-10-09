using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FallDetector : MonoBehaviour
{
    void Reset() { GetComponent<Collider>().isTrigger = true; }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance != null)
            GameManager.Instance.PlayerFell();
    }
}
