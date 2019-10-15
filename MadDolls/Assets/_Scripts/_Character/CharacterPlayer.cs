using UnityEngine;

namespace Gameplay.Character
{
    public class CharacterPlayer : ACharacter
    {
        public int PlayerID = 0;
        public bool UseKeyboardMouse = true;

        protected override void PreInitializeComponents()
        {
            CharacterController = new CharacterComponentControllerPlayer();
            base.PreInitializeComponents();
        }

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
        }
    }
}