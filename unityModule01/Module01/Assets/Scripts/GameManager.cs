using UnityEngine.SceneManagement;

namespace Module01
{
    public class GameManager
    {
        private static GameManager _instance;
        
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameManager();
                return _instance;
            }
        }

        private int _numberOfStages = 5;
        
        public void GoToNextLevel()
        {
            int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (nextLevelIndex < _numberOfStages)
                SceneManager.LoadScene(nextLevelIndex);
            
            else
                SceneManager.LoadScene(0);
        }
    }
}
