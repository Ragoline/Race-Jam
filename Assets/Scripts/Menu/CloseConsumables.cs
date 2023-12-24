using UnityEngine;
using UnityEngine.EventSystems;

public class CloseConsumables : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.SetActive(false);
        Debug.Log("click");
    }
}
