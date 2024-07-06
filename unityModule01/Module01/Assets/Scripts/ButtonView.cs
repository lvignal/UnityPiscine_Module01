using Module01.Doors;
using UnityEngine;

namespace Module01.Button
{
    public class ButtonView : MonoBehaviour
    {
        [SerializeField] private ButtonType _buttonType;
        
        private bool _isButtonPressed = false;
        private Material _buttonMaterial;
        private ButtonsManager _buttonsManager;

        private void Start()
        {
            _buttonMaterial = GetComponent<Renderer>().material;
            _buttonsManager = FindObjectOfType<ButtonsManager>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isButtonPressed || !other.gameObject.CompareTag("Character"))
                return;
            
            _isButtonPressed = true;
            gameObject.transform.Translate(Vector3.down * 0.1f);
            
            if (_buttonType == ButtonType.Doors)
            {
                if (_buttonMaterial.color == Color.white)
                    _buttonMaterial.color = other.gameObject.GetComponent<Renderer>().material.color;
                
                _buttonsManager.OpenColorDoors(_buttonMaterial.color);
            }
            
            if (_buttonType == ButtonType.Platform)
                _buttonsManager.ChangePlatformsColor(other.gameObject.layer);
        }

        public enum ButtonType
        {
            Doors, 
            Platform
        }
    }
}
