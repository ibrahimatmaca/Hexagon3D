using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchConrolScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IDragHandler
{

    private float startPos;
    private float lastPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = Input.mousePosition.x;
        GameControlScript.instance.SwipeFunction(false, startPos, lastPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        lastPos = Input.mousePosition.x;
        GameControlScript.instance.SwipeFunction(true, startPos,lastPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        lastPos = Input.mousePosition.x;
        GameControlScript.instance.SwipeFunction(false, startPos, lastPos);
    }

}
