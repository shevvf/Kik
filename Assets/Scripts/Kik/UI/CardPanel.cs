using Unity.Netcode;

using UnityEngine;
using UnityEngine.UI;


namespace Kik.UI
{
    public class CardPanel : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button rotateLeftButton;
        [SerializeField] private Button rotateRightButton;

        public GameObject CurrentSolider { get; set; }

        private void Start()
        {
            rotateLeftButton.onClick.AddListener(RotateLeft);
            rotateRightButton.onClick.AddListener(RotateRight);
            closeButton.onClick.AddListener(Close);
        }

        private void RotateLeft()
        {
            RequestRotationServerRpc(Quaternion.Euler(0, 0, 60));
        }

        private void RotateRight()
        {
            RequestRotationServerRpc(Quaternion.Euler(0, 0, -60));
        }

        [ServerRpc]
        private void RequestRotationServerRpc(Quaternion rotation, ServerRpcParams rpcParams = default)
        {
            RotateClientRpc(rotation);
        }

        [ClientRpc]
        private void RotateClientRpc(Quaternion rotation)
        {
            if (CurrentSolider != null)
            {
                CurrentSolider.transform.rotation *= rotation;
            }
        }

        private void Close()
        {
            CurrentSolider = null;
            gameObject.SetActive(false);
        }
    }
}