using System;
using System.Collections.Generic;
using UnityEngine;

namespace Module01.Doors
{
    public class ButtonsManager : MonoBehaviour
    {
        [SerializeField] private float _ceilingPosition;
        [SerializeField] private Transform _doorsContainer;
        [SerializeField] private Transform _platformsContainer;
        [SerializeField] private List<PlatformColor> _platformColors;
        
        private float _slidingSpeed = 4f;
        private List<GameObject> _doors = new();
        private List<GameObject> _platforms = new();
        private List<GameObject> _doorsToOpen = new();

        private void Start()
        {
            foreach (Transform door in _doorsContainer)
                _doors.Add(door.gameObject);
            
            foreach (Transform platform in _platformsContainer)
                _platforms.Add(platform.gameObject);
        }

        public void OpenColorDoors(Color buttonColor)
        {
            foreach (GameObject door in _doors)
            {
                if (door.gameObject.activeSelf && door.GetComponent<Renderer>().material.color == buttonColor)
                    _doorsToOpen.Add(door);
            }
        }
        
        public void ChangePlatformsColor(LayerMask characterLayer)
        {
            foreach (GameObject platform in _platforms)
            {
                platform.GetComponent<Renderer>().material.color = GetPlatformColor(characterLayer);
                platform.layer = characterLayer;
            }
        }
        
        private Color GetPlatformColor(LayerMask characterLayer)
        {
            return _platformColors.Find(platformColor => platformColor.LayerMask == characterLayer).Color;
        }

        private void Update()
        {
            if (_doorsToOpen == null || _doorsToOpen.Count == 0)
                return;
            
            // start loop from the end to avoid "collection was modified" exception
            for (int i = _doorsToOpen.Count - 1; i >= 0; i--)
            {
                if (_doorsToOpen[i] == null)
                    continue;
                
                _doorsToOpen[i].transform.Translate(Vector3.up * _slidingSpeed * Time.deltaTime);
                
                if (_doorsToOpen[i].transform.localPosition.y >= _ceilingPosition)
                {
                    Destroy(_doorsToOpen[i]);
                    _doors.Remove(_doorsToOpen[i]);
                    _doorsToOpen.RemoveAt(i);
                }
            }
        }
        
        [Serializable]
        public struct PlatformColor
        {
            public Color Color;
            public int LayerMask;
        }
    }
}