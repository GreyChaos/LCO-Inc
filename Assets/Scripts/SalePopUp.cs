using TMPro;
using System.Collections;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class SalePopUp : MonoBehaviour
{
    // Float value for length of time popup remains on screen.
    [SerializeField] float popUpLifetime = 2f;

    // Initial velocity of the RigidBody2d behind the popups.
    [SerializeField] Vector2 initialVelocity;

    public Color color;
    private float currentAlpha = 1;
    private float alphaInterval;
    private float preFadeLifetime;
    private float fadeLifetime;

    // [SerializeField] TMP_Text popUpText;

    public Rigidbody2D rigidBody2d;

    // Sets the initialVelocity of the RigidBody2d to inspector value and sets to destroy at end of lifetime.
    // Uses unity inspector for initial velocity of (0, 2), and linear damping of 1.3. Arbitrary but these feel the best I've found so far.
    void Start()
    {
        rigidBody2d.linearVelocity = initialVelocity;
        Destroy(gameObject, popUpLifetime);

        color = gameObject.GetComponentInChildren<TMP_Text>().color;
        preFadeLifetime = popUpLifetime * (2/3);
        fadeLifetime = popUpLifetime - preFadeLifetime;
        alphaInterval = currentAlpha / 10;
        StartCoroutine("FadeText");
    }

    // Fades the pop up text over the course of its lifetime.
    IEnumerator FadeText(   )
    {
        // Yields for 2/3 of the pop up lifetime before beginning fading the text.
        yield return new WaitForSecondsRealtime(preFadeLifetime);

        while(true)
        {
            // Increases the opacity in 10 steps over the remainder of the pop ups lifetime.
            yield return new WaitForSeconds(fadeLifetime / 10);

            currentAlpha -= alphaInterval;
            color.a = currentAlpha;
            gameObject.GetComponentInChildren<TMP_Text>().color = color;
        }
    }

}