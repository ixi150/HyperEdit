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
        [SerializeField] Playable[] playablesOnJump;
        [SerializeField] FloatReference jumpCooldown;
        [SerializeField] float sceneReloadDelay = 3;

        void OnEnable()
        {
            StartCoroutine(InputCoorutine());
        }

        void Jump()
        {
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