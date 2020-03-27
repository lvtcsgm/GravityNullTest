using UnityEngine;

namespace GDN.ASCore
{
    [RequireComponent(typeof(UnityStandardAssets.Cameras.FreeLookCam))]
    [RequireComponent(typeof(UnityStandardAssets.Cameras.ProtectCameraFromWallClip))]
    public class FirstAndThirdCameraModifier : MonoBehaviour
    {
        public UnityStandardAssets.Cameras.FreeLookCam freeLookCam { get; protected set; }
        public UnityStandardAssets.Cameras.ProtectCameraFromWallClip protectCameraFromWallClip { get; protected set; }
        public new Camera camera { get; protected set; }
        public Transform pivot { get; protected set; }
        public Vector3 thirdPersonCameraOffset;
        public Vector3 thirdPersonPivotOffset;

        void Start()
        {
            freeLookCam = GetComponent<UnityStandardAssets.Cameras.FreeLookCam>();
            protectCameraFromWallClip = GetComponent<UnityStandardAssets.Cameras.ProtectCameraFromWallClip>();
            camera = GetComponentInChildren<Camera>();

            pivot = camera.transform.parent;
            thirdPersonPivotOffset = pivot.localPosition;
            thirdPersonCameraOffset = camera.transform.localPosition;
        }

        void Update()
        {
            if (null != firstPersonNode)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    protectCameraFromWallClip.enabled = false;
                    camera.transform.localRotation =
                    pivot.localRotation = Quaternion.identity;
                    camera.transform.localPosition =
                    pivot.localPosition = Vector3.zero;
                    pivot.SetParent(firstPersonNode, false);
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    camera.transform.localRotation =
                    pivot.localRotation = Quaternion.identity;
                    camera.transform.localPosition = thirdPersonCameraOffset;
                    pivot.localPosition = thirdPersonPivotOffset;
                    pivot.SetParent(transform, false);
                    protectCameraFromWallClip.enabled = true;
                }
            }

            if (null != freeLookCam.Target)
            {
                freeLookCam.Target.localRotation = freeLookCam.transform.localRotation;

                /*
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    // In 1st person mode
                }
                else
                {
                    // in 3rd person mode
                }
                //*/
            }
        }
        public Transform firstPersonNode;
    }
}