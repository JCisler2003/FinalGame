using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage to deal
    //public string enemyTag = "Enemy"; // Make sure your enemies are tagged appropriately

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag(enemyTag))
        //{
            // Try to get a component on the enemy that can take damage
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
        //}
    }
}
