using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Xunity.Extensions;
using Xunity.Morphables;
using Xunity.Playables;
using Xunity.ScriptableReferences;

namespace Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField] new Vector2Reference constantForce;
        [SerializeField] Vector2Reference jumpForce;
        [SerializeField] ForceMode2D forceMode2D = ForceMode2D.Impulse;
        [SerializeField] Vector2Reference maxSpeed;
        [SerializeField] Playable[] playablesOnJump;
        [SerializeField] FloatReference jumpCooldown;
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

        void OnEnable()
        {
            StartCoroutine(InputCoorutine());
        }

        void Update()
        {

        }

        void FixedUpdate()
        {
            rb.AddForce((Vector2) constantForce * Time.deltaTime);
            rb.velocity = rb.velocity.LimitedTo(maxSpeed);
        }

        void Jump()
        {
            rb.AddForce(jumpForce, forceMode2D);
            foreach (var playable in playablesOnJump)
                playable.Play();
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

        IEnumerator InputCoorutine()
        {
            while (true)
            {
                if (ShouldJump() && CanJump)
                {
                    Jump();
                    yield return new WaitForSeconds(jumpCooldown);
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
}