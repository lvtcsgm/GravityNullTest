using UnityEngine;

namespace GDN.ASCore
{
    // Adaptation of UnityStandardAssets.CharacterController
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class AvatarController : MonoBehaviour
    {
        public Rigidbody rigidBody { get; protected set; }
        public CapsuleCollider capsuleCollider { get; protected set; }
        public Animator animator { get; protected set; }
        public Transform fpsNode;
        public bool isGrounded { get; protected set; }
        public bool isCrouching { get; protected set; }
        public float aiming { get; protected set; }

        void OnAnimatorIK(int layer)
        {
            if (float.Epsilon < aiming)// && !isCrouching)
            {
                animator.SetLayerWeight(animator.GetLayerIndex("Aiming Layer"), aiming);

                if (null != gun)
                {
                    //if (0 < gun.ItemCount)
                    //{
                        Vector3 P = gun.transform.position;// AverageItemPosition();

                        animator.SetLookAtWeight(1.0F, 1.0F, 1.0F);
                        animator.SetLookAtPosition(P);

                        animator.SetIKPosition(AvatarIKGoal.RightHand, P);
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, aiming);
                    //}
                }
            }
            else
            {
                animator.SetLayerWeight(animator.GetLayerIndex("Aiming Layer"), 0.0F);

                animator.SetLookAtWeight(0.0F, 0.0F, 0.0F);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.0F);

            }

            animator.SetFloat(Animator.StringToHash("Aiming"), aiming /2);
        }

        void Start()
        {
            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = capsuleCollider.height;
            m_CapsuleCenter = capsuleCollider.center;

            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            m_OrigGroundCheckDistance = m_GroundCheckDistance;

            m_fpsNodePos = fpsNode.localPosition;
        }

        public void Move(Vector3 move, bool crouch, bool jump, bool aim)
        {
            //move FPS node down during crouching
            if (isCrouching)
                fpsNode.localPosition = new Vector3(m_fpsNodePos.x, m_fpsNodePos.y - 0.5f, m_fpsNodePos.z);
            else
                fpsNode.localPosition = m_fpsNodePos;

            aiming = Mathf.Clamp01(aiming + m_aimSpeed * (aim ? Time.deltaTime : -Time.deltaTime));

            move = Vector3.ProjectOnPlane(move, m_GroundNormal);

            if (CheckGroundStatus())
                HandleGroundedMovement(crouch, jump);
            else
                HandleAirborneMovement(move);

            ScaleCapsuleForCrouching(crouch);
            PreventStandingInLowHeadroom();

            UpdateAnimator(move.z, move.x);

        }

        void ScaleCapsuleForCrouching(bool crouch)
        {
            if (isGrounded && crouch)
            {
                if (isCrouching) return;
                capsuleCollider.height = capsuleCollider.height / 2f;
                capsuleCollider.center = capsuleCollider.center / 2f;
                isCrouching = true;
            }
            else
            {
                Ray crouchRay = new Ray(rigidBody.position + Vector3.up * capsuleCollider.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - capsuleCollider.radius * k_Half;
                if (Physics.SphereCast(crouchRay, capsuleCollider.radius * k_Half, crouchRayLength))
                {
                    //isCrouching = true;
                    return;
                }
                capsuleCollider.height = m_CapsuleHeight;
                capsuleCollider.center = m_CapsuleCenter;
                isCrouching = false;
            }
        }

        void PreventStandingInLowHeadroom()
        {
            // prevent standing up in crouch-only zones
            if (!isCrouching)
            {
                Ray crouchRay = new Ray(rigidBody.position + Vector3.up * capsuleCollider.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - capsuleCollider.radius * k_Half;
                if (Physics.SphereCast(crouchRay, capsuleCollider.radius * k_Half, crouchRayLength))
                {
                    //isCrouching = true;
                }
            }
        }

        void UpdateAnimator(float forward, float strafe)
        {
            // update the animator parameters
            animator.SetFloat("Forward", forward, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", strafe, 0.1f, Time.deltaTime);
            animator.SetBool("Crouch", isCrouching);
            animator.SetBool("OnGround", isGrounded);
            if (!isGrounded)
            {
                animator.SetFloat("Jump", rigidBody.velocity.y);
            }

            /* ********************************************** */
            /* ** READ THIS BEFORE YOU CHANGE ANYTHING! ** */
            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            /* ** READ THIS BEFORE YOU CHANGE ANYTHING! ** */
            /* ********************************************** */
            float runCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
            float jumpLeg = (runCycle < k_Half ? 1 : -1) * forward;
            if (isGrounded)
            {
                animator.SetFloat("JumpLeg", jumpLeg);
            }

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            Vector3 move = new Vector3(strafe, 0, forward);
            if (isGrounded && move.magnitude > 0)
            {
                animator.speed = m_AnimSpeedMultiplier;
            }
            else
            {
                // don't use that while airborne
                animator.speed = 1;
            }
        }

        void HandleAirborneMovement(Vector3 move)
        {
            Vector3 M = Quaternion.Euler(0.0F, transform.localEulerAngles.y, 0.0F) * move;
            // we allow some movement in air, but it's very different to when on ground
            // (typically allowing a small change in trajectory)
            Vector3 airMove = new Vector3(M.x * airSpeed, rigidBody.velocity.y, M.z * airSpeed);
            rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, airMove, Time.deltaTime * airControl);
            GetComponent<Rigidbody>().useGravity = true;

            // apply extra gravity from multiplier:
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
            rigidBody.AddForce(extraGravityForce);

            m_GroundCheckDistance = rigidBody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
        }

        void HandleGroundedMovement(bool crouch, bool jump)
        {
            // check whether conditions are right to allow a jump:
            if (jump && !crouch && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                // jump!
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, m_JumpPower, rigidBody.velocity.z);
                isGrounded = false;
                animator.applyRootMotion = false;
                m_GroundCheckDistance = 0.01F;//0.1f;  //I don't know why this is not 0.01F
                GetComponent<Avatar_SFX>().PlayExert();
            }
        }

        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (isGrounded && Time.deltaTime > 0)
            {
                Vector3 v = (animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = rigidBody.velocity.y;
                rigidBody.velocity = v;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 origin = transform.position + (Vector3.up * sphereCastOffset);
            Vector3 endPoint = origin - (Vector3.up * m_GroundCheckDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(endPoint, sphereCastRadius);
        }

        bool CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            //Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.SphereCast(transform.position + (Vector3.up * sphereCastOffset), sphereCastRadius, Vector3.down, out hitInfo, m_GroundCheckDistance, sphereCastMask))
            {
                m_GroundNormal = hitInfo.normal;
                isGrounded = true;
                animator.applyRootMotion = true;
            }
            /*if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
            {
                m_GroundNormal = hitInfo.normal;
                isGrounded = true;
                animator.applyRootMotion = true;
            }*/
            else
            {
                isGrounded = false;
                m_GroundNormal = Vector3.up;
                animator.applyRootMotion = false;
            }

            return isGrounded;
        }

        [SerializeField] LayerMask sphereCastMask = 0;
        [SerializeField] float sphereCastRadius = 0;
        [SerializeField] float sphereCastOffset = 0;

        [SerializeField] float m_JumpPower = 12f;
        [SerializeField] private float airSpeed = 6; // determines the max speed of the character while airborne
        [SerializeField] private float airControl = 2; // determines the response speed of controlling the character while airborne
	    [Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
	    [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
	    [SerializeField] float m_MoveSpeedMultiplier = 1f;
	    [SerializeField] float m_AnimSpeedMultiplier = 1f;
	    [SerializeField] float m_GroundCheckDistance = 0.1f;
        [Range(0.1F, 4.0F)][SerializeField] float m_aimSpeed = 2.0F;
        [SerializeField] GameObject gun = null;

        float m_OrigGroundCheckDistance;
        const float k_Half = 0.5f;
        Vector3 m_GroundNormal;
        float m_CapsuleHeight;
        Vector3 m_CapsuleCenter;
        Vector3 m_fpsNodePos;
    }
}