using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    private Button pb;
    private Sprite oldSprite;
    public Sprite newSprite;

    void Start()
    {
        pb = GetComponent<Button>();
        oldSprite = pb.image.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pb.image.sprite = newSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pb.image.sprite = oldSprite;
    }
}