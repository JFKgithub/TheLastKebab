using System.Collections;
using UnityEngine;

public class combineTracker : MonoBehaviour
{
    [Header("Combine List")]
    public int objectNumber;
    public GameObject lastCollided;
    [Header("Push Force")]
    [Range(0f, 5f)]
    public float force;
    [Range(0f, 1f)]
    public float time;

    private bool isPushed;

    void OnTriggerStay2D(Collider2D col)
    {
        moveIngredient moveIngredient = FindFirstObjectByType<moveIngredient>();
        if (gameObject == moveIngredient.lastMovedObject && !moveIngredient.isDragging && !isPushed)
        {
            lastCollided = col.gameObject;
            CombineIngredient combineIngredient = FindFirstObjectByType<CombineIngredient>();
            combineIngredient.TryCombine();
        }
    }

    public void MoveAway()
    {
        Debug.Log("Keine gültige Kombination für diese Objekte.");

        // Move objects apart if no valid combination
        Rigidbody2D lastMovedRb = GetComponent<Rigidbody2D>();
        Rigidbody2D collidedRb = lastCollided.GetComponent<Rigidbody2D>();

        if (lastMovedRb != null && collidedRb != null)
        {
            // Calculate direction to move objects apart
            Vector2 direction = (transform.position - lastCollided.transform.position).normalized;

            // Calculate distance
            float distance = Vector2.Distance(transform.position, lastCollided.transform.position);

            // If distance is less than threshold, push objects apart
            if (distance < 5f)
            {
                StartCoroutine(Waiter());
                isPushed = true;
                float forceAmount = Mathf.Lerp(force, 5f, distance / 5f);
                lastMovedRb.AddForce(direction * forceAmount, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator Waiter()
    {
        Rigidbody2D lastMovedRb = GetComponent<Rigidbody2D>();
        yield return new WaitForSeconds(time);
        Debug.LogWarning("Stop");
        lastMovedRb.linearVelocity = Vector2.zero;
        isPushed = false;
    }
}
