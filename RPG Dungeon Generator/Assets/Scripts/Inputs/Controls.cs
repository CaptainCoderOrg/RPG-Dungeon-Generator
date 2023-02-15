namespace CaptainCoder.Dungeoneering
{
    public static class Controls
    {
        private static UserInputs _userInput;
        public static UserInputs.MovementActions MovementActions {
            get
            {
                if(_userInput == null)
                {
                    _userInput = new UserInputs();
                    _userInput.Enable();
                }
                return _userInput.Movement;
            }
        }

        public static void Enable() => _userInput.Movement.Enable();
        public static void Disable() => _userInput.Movement.Disable();
    }
}