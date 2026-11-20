using UnityEngine;

namespace _Project.Scripts.PlayerScripts
{ 
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] Animator playerAnimator;

        public void ChangeAnimationWalk(Vector3 move) 
        {
            if (move.x != 0 || move.z != 0) 
            {
                playerAnimator.SetFloat("Blend", .25f);
            }

            if (move.x == 0 && move.z == 0) 
            {
                playerAnimator.SetFloat("Blend", 0f);
            }
        }

        public void ChangeAnimationRun(Vector3 move) 
        {
            if (move.z > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                playerAnimator.SetFloat("Blend", .5f);
            }
        }
    }
}