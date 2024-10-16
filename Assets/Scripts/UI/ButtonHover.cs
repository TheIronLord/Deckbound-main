using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour
{
    [SerializeField] Image leftImage;
    [SerializeField] Image rightImage;
    [SerializeField] float fadeDuration = 0.3f; // Duration for animations (fade/scale)
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        SetImagesVisible(false);

        // Add event listeners
        var eventTrigger = gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        
        if(leftImage && rightImage){
            // hover start
            entry.callback.AddListener(data => OnHover(true));
            eventTrigger.triggers.Add(entry);

            // hover end
            entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            entry.callback.AddListener(data => OnHover(false));
            eventTrigger.triggers.Add(entry);
        }

        // mouse down
        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entry.callback.AddListener(data => OnMouseDown());
        eventTrigger.triggers.Add(entry);

        // mouse up
        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener(data => OnMouseUp());
        eventTrigger.triggers.Add(entry);
    }

    private void OnHover(bool isHovered)
    {
        SetImagesVisible(isHovered);
    }

    private void SetImagesVisible(bool visible)
    {
        if (visible)
        {
            StartCoroutine(FadeImage(leftImage, 1));
            StartCoroutine(FadeImage(rightImage, 1));
        }
        else
        {
            StartCoroutine(FadeImage(leftImage, 0));
            StartCoroutine(FadeImage(rightImage, 0));
        }
    }

    private IEnumerator FadeImage(Image image, float targetAlpha)
    {
        Color color = image.color;
        float startAlpha = color.a;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            image.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        image.color = color;
    }

    private IEnumerator ScaleGameObject(GameObject obj, float targetScale) {
        float elapsedTime = 0;

        while(elapsedTime < fadeDuration){
            elapsedTime += Time.deltaTime;
            float xScale = Mathf.Lerp(obj.transform.localScale.x, targetScale, elapsedTime);
            float yScale = Mathf.Lerp(obj.transform.localScale.y, targetScale, elapsedTime);
            obj.transform.localScale = new Vector2(xScale, yScale);

            yield return null;
        }
    }

    private void OnClick()
    {
        // Handle button click
        Debug.Log("Button clicked!");
    }
    private void OnMouseDown(){
        StartCoroutine(ScaleGameObject(gameObject, 0.75f));
    }
    private void OnMouseUp(){
        StartCoroutine(ScaleGameObject(gameObject, 1f));
    }
}
