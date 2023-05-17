using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
{
    [System.Serializable]
    public class CustomUIEvent : UnityEvent { }
    public CustomUIEvent OnEvent;

    public Image backgroundGraphic;

    private void Awake()
    {
        backgroundGraphic = gameObject.GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(Transition(new Vector3(1.1f, 1.1f, 1.1f), new Color(1f, 1f, 1f, 1f), 0.25f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(Transition(Vector3.one, new Color(1f, 1f, 1f, 0.75f), 0.25f));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(Transition(new Vector3(0.9f, 0.9f, 0.9f), new Color(1f, 1f, 1f, 1f), 0.25f));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(Transition(new Vector3(0.9f, 0.9f, 0.9f), new Color(1f, 1f, 1f, 1f), 0.25f));
        OnEvent.Invoke();
    }

    public IEnumerator Transition(Vector3 newSize, Color newColor, float transitionTime)
    {
        float timer = 0f;
        Vector3 startSize = transform.localScale;
        Color startColor = backgroundGraphic.color;

        while(timer < transitionTime)
        {
            timer += Time.unscaledDeltaTime;

            yield return null;

            transform.localScale = Vector3.Lerp(startSize, newSize, timer / transitionTime);
            backgroundGraphic.color = Color.Lerp(startColor, newColor, timer / transitionTime);
        }
    }
}
