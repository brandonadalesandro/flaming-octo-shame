using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private static float MAX_SPEED = 1.0f;

	// changable constants
	private float jump_speed = 10.0f;
	private float walk_speed = .9f;

	// should remain private to class
	protected Animator animator;
	protected bool facing_right = true;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float input_x = Input.GetAxis ("Horizontal");
		if (Mathf.Abs(rigidbody2D.velocity.x) < MAX_SPEED) {
			rigidbody2D.velocity += new Vector2(input_x * walk_speed, 0);
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			animator.SetBool("grounded", false);
			rigidbody2D.AddForce(new Vector2(0, jump_speed), ForceMode2D.Impulse);
		}

		// Control the flipping of the sprite when going left/right
		if (rigidbody2D.velocity.x < 0 && facing_right) {
			Flip ();
		} else if (rigidbody2D.velocity.x > 0 && !facing_right) {
			Flip ();
		}

		animator.SetFloat ("speed", Mathf.Abs (rigidbody2D.velocity.x));
	}

	void OnCollisionEnter2D(Collision2D collision) {
		print("colliding");
		animator.SetBool("grounded", true);
	}

	void Flip() {
		facing_right = !facing_right;

		Vector3 scalar = transform.localScale;
		scalar.x *= -1;
		transform.localScale = scalar;

	}
}
