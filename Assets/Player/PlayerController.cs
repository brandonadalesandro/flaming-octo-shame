using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	

	// changable constants
	public float MAX_SPEED = 3.0f;
	public float MAX_JUMP = 3.0f;
	public float jump_speed = 100.0f;
	public float move_speed = .9f;

	// Should remain private to class
	protected Animator animator;
	protected bool facing_right = true;
	private int jumps_left = 2;
	private float gravity_scale;
	bool jumpReset = true;

	// Use this for initialization
	void Start () {	
		gravity_scale = rigidbody2D.gravityScale;
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float input_x = Input.GetAxisRaw ("Horizontal");
		if (Mathf.Abs(rigidbody2D.velocity.x) < MAX_SPEED) {
			rigidbody2D.velocity += new Vector2(input_x * move_speed, 0);
		}

		bool hanging = animator.GetBool("hanging");
		bool grounded = animator.GetBool("grounded");
		if (Input.GetKey (KeyCode.Space) && rigidbody2D.velocity.y <= MAX_JUMP && jumpReset) {
						//|| Input.GetKey(KeyCode.Space) && !isGrounded && rigidbody2D.velocity.y <= MAX_JUMP) {
			animator.SetBool ("grounded", false);
			if (hanging && jumps_left > 0) {
				if (!facing_right) {
					rigidbody2D.AddForce (new Vector2 (2 * jump_speed / 5, jump_speed), ForceMode2D.Impulse);//velocity = new Vector2(jump_speed, jump_speed);
				} else {
					rigidbody2D.AddForce (new Vector2 (-2 * jump_speed / 5, jump_speed), ForceMode2D.Impulse);//.velocity = new Vector2(-jump_speed, jump_speed);
				}
				jumps_left -= 1;
				jumpReset = false;
			} else if (jumps_left > 0) {
				rigidbody2D.AddForce (new Vector2 (rigidbody2D.velocity.x, jump_speed / 3), ForceMode2D.Impulse);//rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jump_speed);
				jumps_left -= 1;
			}
			if (jumps_left == 0) {
				jumpReset = false;
			}
		} else if (!Input.GetKey (KeyCode.Space) && (hanging || grounded)) {
				jumpReset = true;
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
		jumps_left = 3;
		print (collision.gameObject.name);
		if (collision.gameObject.name == "Goombers") {
			print ("Game Over");
			Destroy(this.gameObject);
		}
		
		if (collision.gameObject.tag == "Climbable") {
			animator.SetBool ("hanging", true);
			rigidbody2D.gravityScale = 0.0f;
		} else if (collision.gameObject.name == "Goombers_top") {
			rigidbody2D.AddForce(new Vector2(rigidbody2D.velocity.x, jump_speed), ForceMode2D.Impulse);
			jumps_left = 0;
		} else {
			animator.SetBool ("grounded", true);
		}
	}


	void OnCollisionExit2D(Collision2D collision) {
		rigidbody2D.gravityScale = gravity_scale;
		if (collision.gameObject.tag == "Climbable") {
			animator.SetBool ("hanging", false);	
			//jumps_left = 3;
		} else {
			//jumps_left = 2;
		}
	}

	void Flip() {
		facing_right = !facing_right;

		Vector3 scalar = transform.localScale;
		scalar.x *= -1;
		transform.localScale = scalar;

	}
}
