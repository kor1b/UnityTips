using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
	[Header("Controllers")]
	public int itemsCount;
	[Range(0, 500)]
	public int itemsOffset;
	[Tooltip("How early items start change their size")]
	public float scaleOffset;
	public float snapSpeed;
	public float scaleSpeed;
	[Tooltip("Select with brute force by scroll rect velocity'/n' less number means more long scrolling")]
	public float maxVelocity;

	[Header("Other Objects")]
	public GameObject prefab;
	public ScrollRect scrollRect;

	RectTransform contentRect;
	Vector2 contentVector;

	GameObject[] instItems;     //instantiated items
	Vector2[] itemsPos;
	Vector2[] itemsScale;

	Image[] itemsImage;
	Color[] itemsColor;

	int selectedItemID;
	bool isScrolling = false;

	void Start()
	{
		contentRect = GetComponent<RectTransform>();

		instItems = new GameObject[itemsCount];
		itemsPos = new Vector2[itemsCount];
		itemsScale = new Vector2[itemsCount];
		itemsImage = new Image[itemsCount];
		itemsColor = new Color[itemsCount];

		for (int i = 0; i < itemsCount; i++)
		{
			instItems[i] = Instantiate(prefab, transform, false);
			itemsImage[i] = instItems[i].GetComponent<Image>();
			itemsColor[i] = itemsImage[i].color;

			if (i == 0) continue;
			instItems[i].transform.localPosition = new Vector2(
				instItems[i - 1].transform.localPosition.x + prefab.GetComponent<RectTransform>().sizeDelta.x + itemsOffset,
				instItems[i].transform.localPosition.y);

			itemsPos[i] = -instItems[i].transform.localPosition;
		}
	}

	private void FixedUpdate()
	{
		if (contentRect.anchoredPosition.x >= itemsPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= itemsPos[itemsCount - 1].x && !isScrolling)
			scrollRect.inertia = false;

		float nearestPos = float.MaxValue;

		for (int i = 0; i < itemsCount; i++)
		{
			float distance = Mathf.Abs(contentRect.anchoredPosition.x - itemsPos[i].x);
			if (distance < nearestPos)
			{
				nearestPos = distance;
				selectedItemID = i;
			}
			float scale = Mathf.Clamp(1 / (distance / itemsOffset) * scaleOffset, 0.5f, 1.3f);
			itemsScale[i].x = Mathf.SmoothStep(instItems[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
			itemsScale[i].y = Mathf.SmoothStep(instItems[i].transform.localScale.y, scale, scaleSpeed * Time.fixedDeltaTime);
			instItems[i].transform.localScale = itemsScale[i];

			//alpha change together with scale
			float alpha = Mathf.Clamp(1 / (distance / itemsOffset) * scaleOffset, 0.5f, 1);
			itemsColor[i].a = Mathf.SmoothStep(itemsImage[i].color.a, alpha, scaleSpeed * Time.fixedDeltaTime);
			itemsImage[i].color = itemsColor[i];
		}

		float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
		if (scrollVelocity < maxVelocity && !isScrolling)
			scrollRect.inertia = false;

		if (isScrolling || scrollVelocity > maxVelocity) return;
		contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, itemsPos[selectedItemID].x, snapSpeed * Time.fixedDeltaTime);
		contentRect.anchoredPosition = contentVector;
	}

	public void Scrolling(bool scroll)
	{
		isScrolling = scroll;
		if (scroll)
			scrollRect.inertia = true;
	}
}
