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

        public static UserInputs.CameraActions CameraActions {
            get
            {
                if(_userInput == null)
                {
                    _userInput = new UserInputs();
                    _userInput.Enable();
                }
                return _userInput.Camera;
            }
        }

        public static void Enable() => _userInput.Enable();
        public static void Disable() => _userInput.Disable();
    }
}