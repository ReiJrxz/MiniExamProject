using UnityEngine;
using UnityEngine.EventSystems;

namespace ZiddikJari
{
    public class UIButtonSfx : MonoBehaviour,
        IPointerEnterHandler,
        IPointerClickHandler
    {
        [SerializeField] private AudioClip hoverSfx;
        [SerializeField] private AudioClip clickSfx;

        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance?.PlayUISfx(hoverSfx);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.Instance?.PlayUISfx(clickSfx);
        }
    }
}