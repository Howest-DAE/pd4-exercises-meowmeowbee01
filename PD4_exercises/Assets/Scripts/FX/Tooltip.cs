using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	[SerializeField]
	private GameObject _toolTipObject;

	private void Start()
	{
		_toolTipObject?.SetActive(false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_toolTipObject?.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_toolTipObject?.SetActive(false);
	}
}
