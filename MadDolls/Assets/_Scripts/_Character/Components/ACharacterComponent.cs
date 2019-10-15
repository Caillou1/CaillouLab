namespace Gameplay.Character
{
    [System.Serializable]
    public abstract class ACharacterComponent
    {
        protected ACharacter controlledCharacter;

        /// <summary>
        /// Pre-initialization of character component called on Awake
        /// </summary>
        /// <param name="controlledChar">Character using this component</param>
        public virtual void PreInitComponent(ACharacter controlledChar)
        {
            controlledCharacter = controlledChar;
        }

        /// <summary>
        /// Initialization of character component called on Start
        /// </summary>
        public virtual void InitComponent() { }

        /// <summary>
        /// Update of character component called on Update
        /// </summary>
        /// <param name="deltaTime">Time between each frame</param>
        public virtual void UpdateComponent(float deltaTime) { }

        /// <summary>
        /// Late update of character component called on LateUpdate
        /// </summary>
        /// <param name="deltaTime">Time between each frame</param>
        public virtual void LateUpdateComponent(float deltaTime) { }

        /// <summary>
        /// Fixed update of character component called on FixedUpdate
        /// </summary>
        /// <param name="fixedDeltaTime">Fixed time between each frame</param>
        public virtual void FixedUpdateComponent(float fixedDeltaTime) { }
    }
}