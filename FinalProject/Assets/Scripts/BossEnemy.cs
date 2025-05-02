using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour
{
    [Header("Boss Stats")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Health Bar")]
    public GameObject healthBarPrefab;  // Assign in Inspector
    private Image healthFill;
    private Transform barTransform;

    void Start()
    {
        currentHealth = maxHealth;

        // Instantiate health bar
        if (healthBarPrefab != null)
        {
            Vector3 offsetPos = transform.position + Vector3.up * 2f;
            GameObject bar = Instantiate(healthBarPrefab, offsetPos, Quaternion.identity);
            barTransform = bar.transform;
            barTransform.SetParent(transform);  // Attach to boss so it follows

            // Get reference to fill image
            healthFill = barTransform.Find("Background/HealthFill").GetComponent<Image>();
        }
        else
        {
            Debug.LogWarning("BossEnemy: healthBarPrefab not assigned!");
        }
    }

    public void TakeDamage(float amount)
{
    currentHealth -= amount;
    Debug.Log("Boss took damage: " + amount + " | Remaining Health: " + currentHealth);

    UpdateHealthBar();

    if (currentHealth <= 0f)
    {
        Die();
    }
}


    void UpdateHealthBar()
    {
        if (healthFill != null)
        {
            healthFill.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        // Optional: play death animation or sound here
        Destroy(gameObject);
    }
}
