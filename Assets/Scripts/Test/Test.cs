using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        RectTransform transform = GetComponent<RectTransform>();
        Debug.Log(transform.anchoredPosition);
    }
}