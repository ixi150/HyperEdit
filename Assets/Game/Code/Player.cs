using System.Collections;
using UnityEngine;
using Xunity.Behaviours;
using Xunity.Playables;
using Xunity.ScriptableEvents;
using Xunity.ScriptableReferences;

namespace Game.Code
{
    public class Player : GameBehaviour
    {
        [SerializeField] Playable[] playablesOnJump;
        [SerializeField] FloatReference jumpCooldown;
        [SerializeField] float sceneReloadDelay = 3;
        [SerializeField] GameEvent playerJumped, playerCollided, playerDied;
        
        
        void OnEnable()
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Position = GameObject.FindWithTag("PlayerSpawn").transform.position;
            StartCoroutine(InputCoorutine());
        }

        void Jump()
        {
            playerJumped.Raise();
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
                    Deactivate();
                    Invoke(ReloadScene, sceneReloadDelay);
                    break;
                case "Lose":
                    Deactivate();
                    Invoke(ReloadScene, sceneReloadDelay);
                    playerDied.Raise();
                    break;
                default:
                    
                    break;
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            playerCollided.Raise();
        }

        void ReloadScene()
        {
            Activate();
        }

        IEnumerator InputCoorutine()
        {
            while (true)
            {
                if (ShouldJump())
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