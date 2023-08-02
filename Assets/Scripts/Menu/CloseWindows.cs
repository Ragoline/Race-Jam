using UnityEngine;
using UnityEngine.EventSystems;

public class CloseWindows : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        MenuManager.Instance.CloseWindows();
        Debug.Log("click");
    }
}
