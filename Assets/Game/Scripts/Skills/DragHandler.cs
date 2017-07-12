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
		Vector2 pos = new Vector2(0,0);
		Canvas myCanvas = GameData.Instance.gameCanvas; 
		RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
		transform.position =myCanvas.transform.TransformPoint(pos);
		//transform.localPosition = new Vector3(Input.mousePosition.x -578f,Input.mousePosition.y - 745f,0);
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