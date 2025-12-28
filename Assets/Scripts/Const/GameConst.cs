using System.Collections.Generic;

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
        public static int ImmunityCounterBuffID = 30421;
        public static int ShieldBuffID = 80001;
        public static int ArmorBuffID = 81001;
        public static int CounterBuffID = 99999;
        public static int MaxRandomCount = 3;//最大随机次数为3 超过3跳出去
        /// <summary>
        /// ↑类留劲buffID
        /// </summary>
        public static List<int> BuffUpFirstSkillList = new()
        {
            76901,77301,77701,78101
        };
        /// <summary>
        /// ↓类留劲buffID
        /// </summary>
        public static List<int> BuffDownFirstSkillList = new()
        {
            77101,77501,77901,78301
        };
        /// <summary>
        /// ←类留劲buffID
        /// </summary>
        public static List<int> BuffLeftFirstSkillList = new()
        {
            77201,77601,78001,78401
        };
        /// <summary>
        /// →类留劲buffID
        /// </summary>
        public static List<int> BuffRightFirstSkillList = new()
        {
            77001,77401,77801,78201
        };
        /// <summary>
        /// 化身类buff
        /// </summary>
        public static List<int> BuffAvatarList = new()
        {
            30371,30381,30391
        };
        
        //敷宵剑
        public static int SkillFuXiaoJian = 2023;
        //反击
        public static int SkillCounterattack = 3024;

        public static int Buff10011 = 10011;
        public static int Buff10021 = 10021;
        public static int Buff10041 = 10041;
        public static int Buff10071 = 10071;
        public static int Buff10091 = 10091;
        public static int Buff10101 = 10101;
        public static int Buff10121 = 10121;
        public static int Buff10131 = 10131;
        public static int Buff10161 = 10161;
        public static int Buff10171 = 10171;
        public static int Buff10191 = 10191;
        public static int Buff10201 = 10201;
        
        
        public static int Buff20011 = 20011;
        public static int Buff20021 = 20021;
        public static int Buff20071 = 20071;
        public static int Buff20081 = 20081;
        public static int Buff20111 = 20111;
        public static int Buff20121 = 20121;
        public static int Buff20131 = 20131;
        public static int Buff20141 = 20141;
        public static int Buff20151 = 20151;
        public static int Buff20161 = 20161;
        public static int Buff20181 = 20181;
        public static int Buff20191 = 20191;
        public static int Buff20201 = 20201;
        public static int Buff20211 = 20211;
        public static int Buff20221 = 20221;
        public static int Buff20231 = 20231;
        public static int Buff20331 = 20331;
        public static int Buff20351 = 20351;
        //毒瘴
        public static int Buff20341 = 20341;
        
        public static int Buff30011 = 30011;
        public static int Buff30031 = 30031;
        public static int Buff30041 = 30041;
        public static int Buff30051 = 30051;
        public static int Buff30071 = 30071;
        public static int Buff30091 = 30091;
        //留劲
        public static int Buff30111 = 30111;
        public static int Buff30121 = 30121;
        public static int Buff30131 = 30131;
        public static int Buff30141 = 30141;
        
        public static int Buff30151 = 30151;
        public static int Buff30161 = 30161;
        public static int Buff30171 = 30171;
        public static int Buff30181 = 30181;
        
        public static int Buff30191 = 30191;
        public static int Buff30201 = 30201;
        public static int Buff30211 = 30211;
        public static int Buff30221 = 30221;
        
        public static int Buff30231 = 30231;
        public static int Buff30241 = 30241;
        public static int Buff30251 = 30251;
        public static int Buff30261 = 30261;
        
        public static int Buff30301 = 30301;
        public static int Buff30361 = 30361;
        public static int Buff30371 = 30371;
        public static int Buff30381 = 30381;
        public static int Buff30391 = 30391;
        
        //失重效果
        public static int Buff90019 = 90019;
        //满欲效果
        public static int Buff90020 = 90020;
        

        public static int HeartMethod10058 = 10058;
        public static int HeartMethod10060 = 10060;
        public static int HeartMethod10067 = 10067;
        public static int HeartMethod10090 = 10090;
        public static int HeartMethod10091 = 10091;
        public static int HeartMethod10095 = 10095;
        public static int HeartMethod10106 = 10106;
        public static int HeartMethod10116 = 10116;
        public static int HeartMethod10123 = 10123;
        public static int HeartMethod10124 = 10124;
        public static int HeartMethod10125 = 10125;
        public static int HeartMethod10136 = 10136;
        public static int HeartMethod10153 = 10053;
        
        //技能
        public static List<int> UseItemSkillIDList = new List<int>
        {   
            1013,
            2070,
            3016,
            4018
        };
        public static int Skill1013 = 1013;
        public static int Skill2070 = 2070;
        
    }
    
    public static class View
    {
        public static string SceneRoot = "Assets/GameResource/Prefab/Scene/Scene/";
        public static string PasserbyRoot = "Assets/GameResource/Prefab/Scene/Passerby/";
    }
}