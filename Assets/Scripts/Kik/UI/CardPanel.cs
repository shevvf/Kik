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
            Rotate(Quaternion.Euler(0, 0, 60));
        }

        private void RotateRight()
        {
            Rotate(Quaternion.Euler(0, 0, -60));
        }

        private void Rotate(Quaternion quaternion)
        {
            CurrentSolider.transform.rotation *= quaternion;
        }

        private void Close()
        {
            CurrentSolider = null;
            gameObject.SetActive(false);
        }
    }
}