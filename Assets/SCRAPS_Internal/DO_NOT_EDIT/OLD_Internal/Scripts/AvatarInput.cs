using UnityEngine;

namespace GDN.ASCore
{
    [RequireComponent(typeof(AvatarController))]
    public class AvatarInput : MonoBehaviour
    {
        public AvatarController avatar { get; protected set; }
        public bool jump { get; protected set; }

        public bool aiming { get; protected set; }

        private void Start()
        {
            avatar = GetComponent<AvatarController>();
        }

        private void Update()
        {
            if (!jump)
                jump = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButtonDown("Jump");

            aiming = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButton("Fire1");
        }

        private void FixedUpdate()
        {
            float h = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Horizontal");
            float v = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Vertical");

            bool crouch = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButton("Crouch");

            Vector3 move = new Vector3(h, 0, v);

            //print(move);

            if (1.0F < move.magnitude)
                move.Normalize();

            if (UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButton("Walk"))
                move *= 0.5F;

            avatar.Move(move, crouch, jump, aiming);
            jump = false;
        }
    }
}