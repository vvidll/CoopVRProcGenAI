using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] InputActionAsset actionAsset;
        [SerializeField] InputAction inputActions;

        [SerializeField] PlayerAnimation playerAnimation;

        [SerializeField] CharacterController characterController;

        [SerializeField] float speedMove = 2f;

        private void Start()
        {
            inputActions = actionAsset.FindAction("Player/Move");
            inputActions.Enable();
        }

        void Update()
        {

            Vector2 inputVector = inputActions.ReadValue<Vector2>();

            Vector3 move = transform.TransformDirection(new Vector3(inputVector.x, 0f, inputVector.y));

            characterController.Move(move * speedMove * Time.deltaTime);

            playerAnimation.ChangeAnimationWalk(move);
        }
    }
}