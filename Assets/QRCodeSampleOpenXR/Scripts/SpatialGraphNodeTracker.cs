// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine.UI;
using UnityEngine.UIElements;

#if MIXED_REALITY_OPENXR
using Microsoft.MixedReality.OpenXR;
#else
using SpatialGraphNode = Microsoft.MixedReality.SampleQRCodes.WindowsXR.SpatialGraphNode;
#endif

namespace Microsoft.MixedReality.SampleQRCodes
{
    internal class SpatialGraphNodeTracker : MonoBehaviour
    {
        private SpatialGraphNode node;

        public System.Guid Id { get; set; }

        [SerializeField] private Vector3 qrPosition;
        [SerializeField] private Quaternion qrRotation;
        [SerializeField] private TextMesh QRText;

        void Update()
        {
            if (node == null || node.Id != Id)
            {
                node = (Id != System.Guid.Empty) ? SpatialGraphNode.FromStaticNodeId(Id) : null;
                Debug.Log("Initialize SpatialGraphNode Id= " + Id);
            }

            if (node != null)
            {
#if MIXED_REALITY_OPENXR
                if (node.TryLocate(FrameTime.OnUpdate, out Pose pose))
#else
                if (node.TryLocate(out Pose pose))
#endif
                {
                    // If there is a parent to the camera that means we are using teleport and we should not apply the teleport
                    // to these objects so apply the inverse
                    if (CameraCache.Main.transform.parent != null)
                    {
                        pose = pose.GetTransformedBy(CameraCache.Main.transform.parent);
                    }
                    
                    qrPosition = pose.position;
                    qrRotation = pose.rotation;
                    gameObject.transform.SetPositionAndRotation(pose.position, pose.rotation);
                    Debug.Log("Id= " + Id + " QRPose = " + pose.position.ToString("F7") + " QRRot = " + pose.rotation.ToString("F7"));
                }
                else
                {
                    Debug.LogWarning("Cannot locate " + Id);
                }
            }
        }

        public void SetPosition()
        {
            GameObject merge = new GameObject();
            if (QRText.text == "bogie")
            {
                merge = GameObject.Find("mix6");
            } else if (QRText.text == "bogie8")
            {
                merge = GameObject.Find("mix8");
            }
            if (merge != null)
            {
                merge.transform.parent = gameObject.transform;
                merge.transform.localPosition = new Vector3(0, 0, 0);
                //merge.transform.localRotation = Quaternion.Euler(90,0,0);//QR码水平放置
                merge.transform.localRotation = Quaternion.Euler(180,-105,0);//QR码垂直放置
                merge.transform.parent = null;
                
                Vector3 mergeEulerAngles = merge.transform.localRotation.eulerAngles;
                mergeEulerAngles.x = 0;
                mergeEulerAngles.z = 0;
                merge.transform.localRotation = Quaternion.Euler(mergeEulerAngles);//只保留y轴旋转
            }
        }
    }
}