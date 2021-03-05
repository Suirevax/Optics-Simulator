using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Canvas canvas;

    private RectTransform rectTransform;

    SelectionManager selectionManager;

    GameObject thisObject;

    GameObject linkedObject;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
        thisObject = gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Clicked");
        selectionManager.newSelection(thisObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       // Debug.Log("OnEndDrag");
    }

    public void SetLinkedObject(GameObject newLinkedObject)
    {
        linkedObject = newLinkedObject;
    }

    public GameObject GetLinkedObject()
    {
        return linkedObject;
    }
}
