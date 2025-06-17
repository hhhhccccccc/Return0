public static class GameConst
{
    public const string AssemblyNameForController = "Game.Controller";
    public const string AssemblyNameForModel = "Game.Model";
    public const string AssemblyNameForView = "Game.View";
    public const string AssemblyNameForManager = "Game.Manager";
    public const string AssemblyNameForConfig = "Game.Config";
    public const string AssemblyNameForInterface = "Game.Interface";
    public const string AssemblyNameForMessage = "Game.Message";

    public const int ReferenceResolutionX = 1920;
    public const int ReferenceResolutionY = 1080;
    public const int MatchWidthOrHeight = 1;
    
    public static class Battle
    {
        public static int KeyMax = 10;
        public static float CalculateSpeedOffset = 0.1f;
        public static int CalculateActionWheelNormal = 5;//默认计算为5息
    }
}