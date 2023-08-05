using System.Collections;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public float fadeOutDuration = 2f;
    public float fadeInDuration = 1f;
    float elapsedTime = 0f;
    private bool isFadingOut = false;
    // Reference to the CanvasGroup component of the popup panel.
    public CanvasGroup canvasGroup;
    //public GameObject copyToClipBoardPopup;

    private void Awake()
    {
        // Get the CanvasGroup component on the popup panel (or add one if not present).
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }


    private IEnumerator FadeOut()
    {
        isFadingOut = true;
        // Fade out over fadeOutDuration second (adjust the time as needed).
        
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeOutDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        // Deactivate the popup panel when the fade-out is complete.
        gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(float fadeOutDuration)
    {
        isFadingOut = true;isFadingOut = true;
        // Fade out over fadeOutDuration second (adjust the time as needed).

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeOutDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        // Deactivate the popup panel when the fade-out is complete.
        gameObject.SetActive(false);
        canvasGroup.alpha = 1f;
        isFadingOut = false;
    }


    private IEnumerator FadeIn()
    {
        // Reset the CanvasGroup alpha to fully transparent at the beginning of the fade-in.
        canvasGroup.alpha = 0f;

        // Fade in over fadeInDuration seconds.
        
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        // Ensure the alpha is set to fully opaque (1) after the fade-in.
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeIn(float fadeInDuration)
    {
        // Reset the CanvasGroup alpha to fully transparent at the beginning of the fade-in.
        canvasGroup.alpha = 0f;

        // Fade in over fadeInDuration seconds.

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        // Ensure the alpha is set to fully opaque (1) after the fade-in.
        canvasGroup.alpha = 1f;
    }


    public void PanelFadeOut(float fadeOutDuration)
    {
        if (!isFadingOut)
        {
            canvasGroup.alpha = 1f;
            // Set the timeVisible to the current time when the popup is shown.
            elapsedTime = 0f;
            //StopCoroutine(FadeOut(fadeOutDuration));
            StartCoroutine(FadeOut(fadeOutDuration));
        }
    }

    public void PanelFadeIn(float fadeInDuration)
    {
        //StopCoroutine(FadeIn(fadeInDuration));
        StartCoroutine(FadeIn(fadeInDuration));
    }


    // Call this method to show the popup.
    public void ShowPopup()
    {
        // Activate the popup panel.
        gameObject.SetActive(true);

        // Reset the CanvasGroup alpha to fully opaque when the popup is shown.
        canvasGroup.alpha = 1f;
        // Set the timeVisible to the current time when the popup is shown.
        elapsedTime = 0f;
        // Reset the fading state.
        isFadingOut = false;

        StopCoroutine(FadeOut());

        if (!isFadingOut)
        {
            // Start fading out the popup panel.
            StartCoroutine(FadeOut());
        }
    }
}
