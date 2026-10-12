using UnityEngine;

namespace ZiddikJari
{
    public class ObstacleCollisionSfx : MonoBehaviour
    {
        [SerializeField] private string obstacleTag = "Obstacle";
        [SerializeField] private float soundCooldown = 0.4f;

        private float nextSoundTime;

        // สำหรับตัวละครที่ใช้ CharacterController
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            TryPlaySound(hit.collider);
        }

        // สำหรับวัตถุที่ใช้ Rigidbody และ Collider ปกติ
        private void OnCollisionEnter(Collision collision)
        {
            TryPlaySound(collision.collider);
        }

        // รองรับสิ่งกีดขวางที่ตั้ง Collider เป็น Is Trigger
        private void OnTriggerEnter(Collider other)
        {
            TryPlaySound(other);
        }

        private void TryPlaySound(Collider other)
        {
            if (!other.CompareTag(obstacleTag)) return;
            if (Time.time < nextSoundTime) return;

            nextSoundTime = Time.time + soundCooldown;
            AudioManager.Instance?.PlayObstacleHitSfx();
        }
    }
}