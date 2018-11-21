using UnityEngine;
using UnityEngine.SceneManagement;
using Xunity.Playables;

namespace Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float jumpVelocity = 1;
        [SerializeField] float maxSpeed = 1;
        [SerializeField] Playable[] playablesOnJump;
        [SerializeField] float sceneReloadDelay = 3;

        Rigidbody2D rb;

        bool CanJump
        {
            get { return true; }
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            if (ShouldJump() && CanJump)
            {
                Jump();
            }
        }

        void Jump()
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            foreach (var playable in playablesOnJump)
            {
                playable.Play();
            }
        }

        bool ShouldJump()
        {
            return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.tag)
            {
                case "Win":
                    Invoke("ReloadScene", sceneReloadDelay);
                    break;
                case "Lose":
                    gameObject.SetActive(false);
                    Invoke("ReloadScene", sceneReloadDelay);
                    break;
                default:
                    break;
            }
        }

        void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

