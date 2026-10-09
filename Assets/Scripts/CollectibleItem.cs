using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectibleItem : MonoBehaviour
{
    public enum EffectType { AddTime, RemoveTime, AddScore, RemoveScore }
    [SerializeField] EffectType effect = EffectType.AddScore;
    [SerializeField] float timeAmount = 3f;
    [SerializeField] int scoreAmount = 500;
    [SerializeField] AudioClip collectSfx;
    [SerializeField, Range(0f, 1f)] float sfxVolume = 1f;
    [SerializeField] GameObject collectVfx;
    bool collected;

    void Reset() { GetComponent<Collider>().isTrigger = true; }

    void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player")) return;
        GameManager gm = GameManager.Instance;
        if (gm == null || !gm.IsPlaying) return;
        collected = true;

        switch (effect)
        {
            case EffectType.AddTime: gm.AddTime(Mathf.Abs(timeAmount)); break;
            case EffectType.RemoveTime: gm.AddTime(-Mathf.Abs(timeAmount)); break;
            case EffectType.AddScore: gm.AddScore(Mathf.Abs(scoreAmount)); break;
            case EffectType.RemoveScore: gm.AddScore(-Mathf.Abs(scoreAmount)); break;
        }
        if (collectSfx != null) AudioSource.PlayClipAtPoint(collectSfx, transform.position, sfxVolume);
        if (collectVfx != null) Instantiate(collectVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
