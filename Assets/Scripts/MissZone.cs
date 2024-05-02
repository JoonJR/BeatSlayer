using UnityEngine;

public class MissZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            Debug.Log("Combo reset");
            ScoreManager.Instance.MissedNote();
            ScoreManager.Instance.ResetCombo();
        }
    }
}
