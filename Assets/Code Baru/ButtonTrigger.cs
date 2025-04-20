using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public UnityEngine.UI.Button buttonToPress;  // Reference to the UI button

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))  // Ensure this only triggers when a hand collides
        {
            buttonToPress.onClick.Invoke();
        }
    }
}
