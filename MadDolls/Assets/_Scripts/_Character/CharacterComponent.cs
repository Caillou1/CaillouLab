using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class CharacterComponent : MonoBehaviour
{
    protected Character controlledCharacter;

    public virtual void InitializeComponent(Character controlledChar)
    {
        controlledCharacter = controlledChar;
    }
}
