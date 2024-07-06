using System.Collections.Generic;
using Module01.Character;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Module01
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private int _levelIndex;
        [SerializeField] private Camera _camera;
        [SerializeField] private List<PlayerController> _charactersList;
        
        private int _numberOfCharactersWhoFinished = 0;
        private PlayerController _activeCharacter;
        
        private void Start()
        {
            foreach (PlayerController character in _charactersList)
            {
                character.OnExitReached += CheckLevelEnd;
                character.OnPlayerDeath += LaunchGameOver;
            }
        }
        
        private void Update()
        {
            // handle camera focus on characters
            foreach (PlayerController character in _charactersList)
            {
                if (Input.GetKeyDown(character.FocusKeyCode) && _activeCharacter != character)
                {
                    _activeCharacter = character;
                    character.SetActive(true);
                    
                    foreach (PlayerController otherCharacter in _charactersList)
                    {
                        if (otherCharacter != character)
                            otherCharacter.SetActive(false);
                    }
                }
            }
            
            if (_activeCharacter != null)
                _camera.transform.position = new Vector3(_activeCharacter.transform.position.x, _camera.transform.position.y, _camera.transform.position.z);

            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Backspace))
                ResetLevel();
        }
        
        private void CheckLevelEnd(bool isExitReached)
        {
            if (isExitReached)
                _numberOfCharactersWhoFinished++;
            else
                _numberOfCharactersWhoFinished--;

            if (_numberOfCharactersWhoFinished == _charactersList.Count)
            {
                Debug.Log($"Level {_levelIndex} is finished !");
                GameManager.Instance.GoToNextLevel();
            }
        }

        private void ResetLevel()
        {
            SceneManager.LoadScene(_levelIndex);
        }
        
        private void LaunchGameOver()
        {
            Debug.Log("Game Over!");
            ResetLevel();
        }
        
        private void OnDestroy()
        {
            foreach (PlayerController character in _charactersList)
            {
                character.OnExitReached -= CheckLevelEnd;
                character.OnPlayerDeath -= LaunchGameOver;
            }
        }
    }
}