using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static GameObject item;   
	Transform startParent;
	Vector3 startPosition;
	bool start = true;
	public int skillNumber = 1;
	public int GetSkillNumber(){
		return skillNumber;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		item = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;

		GetComponent<CanvasGroup>().blocksRaycasts = false;
		item.transform.SetParent(item.transform.parent.parent);
	}


	public void OnDrag(PointerEventData eventData)
	{

		transform.localPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		//transform.position =  GetComponentInParent<Canvas>().worldCamera.ScreenToWorldPoint( Input.mousePosition);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		item = null;
		if(transform.parent == startParent)
		{
			transform.position = startPosition;
		}
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

}