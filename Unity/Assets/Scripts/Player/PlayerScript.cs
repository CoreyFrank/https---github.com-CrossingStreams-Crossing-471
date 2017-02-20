using UnityEngine;


public class PlayerScript : MonoBehaviour
{
	// Health related (billy)
	public const int MAX_HEALTH_CAP = 8;
	public int health = 2;
	public int max_health = 2;

	// Coin related (billy)
	public int coins = 0;

	// Cap for speed (billy)
	public const float MAX_SPEED = 30.0f;

    public Vector2 speed = new Vector2(10, 10);

    private Vector2 movement;
    private Rigidbody2D rigidbodyComponent;

	// variables for handling damage multipliers (billy)
	public float damageMultiplier = 1.0f;
	public const float MAX_DAMAGE_MULTIPLIER = 4.0f;

    void Update()
    {
        // 3 - Retrieve axis information
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // 4 - Movement per direction
        movement = new Vector2(
            speed.x * inputX,
            speed.y * inputY);

    }

    void FixedUpdate()
    {
        // 5 - Get the component and store the reference
        if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody2D>();

        // 6 - Move the game object
        rigidbodyComponent.velocity = movement;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
			// remove health on hit by enemy
			TakeDamage(1);
         }
    }

	/* Handle damage when hit by an enemy (billy) */
	public void TakeDamage(int amount) {
		health -= amount;
		Debug.Log ("Took damage. Health is now " + health + "/" + max_health);
		if (health <= 0)
			Destroy (gameObject);
	}

	/* Handle picking up coins or spending coins.
	 * use negative amount for buying things. 
	 * Returns false if transaction drops account below 0 (in which case transaction ignored)
	*/
	public bool AdjustCoins(int amount) {
		// make adjustment to coins
		int newBalance = coins + amount;

		// if adjustment valid, make it so, otherwise ignore transaction and return false
		if (newBalance >= 0) {
			coins = newBalance;
			Debug.Log ("Coins now: " + coins);
			return true;
		} else
			return false;
	}
}

