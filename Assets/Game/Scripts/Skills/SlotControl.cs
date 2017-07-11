using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SlotControl : MonoBehaviour , IDropHandler{
	public int position;
	public GameObject item
	{
		get
		{
			if (transform.childCount > 0)
			{
				return transform.GetChild(0).gameObject;
			}
			return null;
		}
	}

	#region IdropHandler implementation
	public void OnDrop(PointerEventData eventData)
	{
		if (!item)
		{
			DragHandler.item.transform.SetParent(transform);
			SkillManagerComponent.Instance.SetSkill (position, 
				SkillManagerComponent.Instance.GetSkill (
					item.GetComponent<DragHandler> ().GetSkillNumber ()));
			
		}
	}
	#endregion

}﻿