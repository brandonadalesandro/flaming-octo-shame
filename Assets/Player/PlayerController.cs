using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private static float MAX_SPEED = 2.0f;

	// changable constants
	private float jump_speed = 200.0f;
	private float move_speed = .9f;
	private int jumps_left = 2;

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
			rigidbody2D.velocity += new Vector2(input_x * move_speed, 0);
		}

		if (Input.GetKeyDown(KeyCode.Space) && jumps_left >= 1) {
			animator.SetBool("grounded", false);
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
			rigidbody2D.AddForce(new Vector2(0, jump_speed));
			jumps_left--;
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
		if (collision.gameObject.tag == "Climbable") {
			animator.SetBool ("hanging", true);
			jumps_left = 1;
			rigidbody2D.gravityScale = 0.0f;
		} else {
			animator.SetBool ("grounded", true);
			jumps_left = 2;
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.tag == "Climbable") {
			animator.SetBool ("hanging", false);
		} else {
			jumps_left = 2;
		}
	}

	void Flip() {
		facing_right = !facing_right;

		Vector3 scalar = transform.localScale;
		scalar.x *= -1;
		transform.localScale = scalar;

	}
}
