using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    public List<Character> Characters = new List<Character>();
    public List<Character> AliveCharacters
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
        } else
        {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad(gameObject);
    }

    public void RegisterCharacter(Character character)
    {
        if(!Characters.Contains(character))
        {
            Characters.Add(character);
            CameraFollow.Instance.RegisterTarget(character.CharacterTransform);
        }
    }

    public List<Character> GetOpponents(Character myself)
    {
        return AliveCharacters.FindAll(c => c != myself);
    }
}
