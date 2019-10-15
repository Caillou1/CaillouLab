using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Gameplay.Character;

namespace Gameplay.Management
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        public List<ACharacter> Characters = new List<ACharacter>();
        public List<ACharacter> AliveCharacters
        {
            get
            {
                return Characters.FindAll(c => c.CharacterHealth.IsAlive);
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            //DontDestroyOnLoad(gameObject);
        }

        public void RegisterCharacter(ACharacter character)
        {
            if (!Characters.Contains(character))
            {
                Characters.Add(character);
                CameraFollow.Instance.RegisterTarget(character.CharacterTransform);
            }
        }

        public List<ACharacter> GetOpponents(ACharacter myself)
        {
            return AliveCharacters.FindAll(c => c != myself);
        }
    }
}