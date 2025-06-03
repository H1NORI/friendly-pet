using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float scaleDuration = 1f;

    private Vector3 targetScale;
    private float scaleTimer;

    private void Awake()
    {
        targetScale = transform.localScale;
        transform.localScale = Vector3.one * 0.01f;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        scaleTimer = 0f;
    }

    private void Update()
    {
        if (scaleTimer < scaleDuration)
        {
            scaleTimer += Time.deltaTime;
            float t = Mathf.Clamp01(scaleTimer / scaleDuration);
            transform.localScale = Vector3.Lerp(Vector3.one * 0.01f, targetScale, t);
        }

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnMouseDown()
    {
        Debug.Log("Coin clicked!");
        CurrencyManager.Instance.AddCoins(1);
        Destroy(gameObject);
    }
}