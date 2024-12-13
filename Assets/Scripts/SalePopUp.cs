using TMPro;
using UnityEngine;

public class SalePopUp : MonoBehaviour
{
    // Float value for length of time popup remains on screen.
    [SerializeField] float popUpLifetime = 1.5f;

    // Initial velocity of the RigidBody2d behind the popups.
    [SerializeField] Vector2 initialVelocity;

    public Rigidbody2D rigidBody2d;

    // Sets the initialVelocity of the RigidBody2d to inspector value and sets to destroy at end of lifetime.
    // Uses unity inspector for initial velocity of (0, 2), and linear damping of 1.3. Arbitrary but these feel the best I've found so far.
    void Start()
    {
        rigidBody2d.linearVelocity = initialVelocity;
        Destroy(gameObject, popUpLifetime);
    }

}