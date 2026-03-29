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

        public static int BuffXinYan = 10011;
        public static int BuffHuiBi = 10021;
        public static int BuffFanJi = 10031;
        public static int BuffXunSu = 10041;
        public static int BuffGangJu = 10051;
        public static int BuffXuanJu = 10061;
        public static int BuffLiZeng = 10071;
        public static int BuffJiZeng = 10081;
        public static int BuffWuZeng = 10091;
        public static int BuffShuZeng = 10101;
        public static int BuffQiaoZeng = 10111;
        public static int BuffCangShen = 10121;
        public static int BuffYinHun = 10131;
        public static int BuffLeiXing = 10141;
        public static int BuffWenShi = 10151;
        public static int BuffNiXing = 10161;
        public static int BuffBiYang = 10171;
        public static int BuffJieFa = 10181;
        public static int BuffYueHuanJiaShi = 10191;
        public static int BuffYuShouJiaShi = 10201;
        public static int BuffLongTeng = 10211;
        public static int BuffHuiChun = 10221;
        
        
        public static int BuffHuanSu = 20011;
        public static int BuffShiHeng = 20021;
        public static int BuffFengXueShang = 20031;
        public static int BuffFengXueXia = 20041;
        public static int BuffFengXueZuo = 20051;
        public static int BuffFengXueYou = 20061;
        public static int BuffPoZhan = 20071;
        public static int BuffShangKou = 20081;
        public static int BuffGangPing = 20091;
        public static int BuffXuanPing = 20101;
        public static int BuffLiShuai = 20111;
        public static int BuffJiShuai = 20121;
        public static int BuffWuShuai = 20131;
        public static int BuffShuShuai = 20141;
        public static int BuffJiangYing = 20151;
        public static int BuffGuoJin = 20161;
        public static int BuffMangMu = 20171;
        public static int BuffWuShiJin = 20181;
        public static int BuffShuShiJin = 20191;
        public static int BuffJiShiJin = 20201;
        public static int BuffFaShiJin = 20211;
        public static int BuffYaoDu = 20221;
        public static int BuffYaoDuQinShi = 20231;
        public static int BuffDuXiangShang = 20241;
        public static int BuffDuXiangXia = 20251;
        public static int BuffDuXiangZuo = 20261;
        public static int BuffDuXiangYou = 20271;
        public static int BuffShiChi = 20281;
        public static int BuffGongSheng = 20291;
        public static int BuffChiFei = 20301;
        public static int BuffHanXin = 20311;
        public static int BuffXuanYun = 20321;
        public static int BuffKuYin = 20331;
        public static int BuffDuZhang = 20341;
        public static int BuffHunChen = 20351;
        public static int BuffZuiQi = 20361;
        
        public static int BuffJiaoMing = 30011;
        public static int BuffXianFaZhiRen = 30021;
        public static int BuffNiSha = 30031;
        public static int BuffNiLin = 30041;
        public static int BuffTaoTieWanWu = 30051;
        public static int BuffFuXiaoJian = 30071;
        public static int BuffDuanJinShi = 30081;
        public static int BuffBangJian = 30091;
        public static int BuffYuDiShi = 30101;
        //留劲
        public static int BuffLiuJinWuShaShiShang = 30111;
        public static int BuffLiuJinWuShaShiXia = 30121;
        public static int BuffLiuJinWuShaShiZuo = 30131;
        public static int BuffLiuJinWuShaShiYou = 30141;
        
        public static int BuffLiuJinShuShaShiShang = 30151;
        public static int BuffLiuJinShuShaShiXia = 30161;
        public static int BuffLiuJinShuShaShiZuo = 30171;
        public static int BuffLiuJinShuShaShiYou = 30181;
        
        public static int BuffLiuJinJiYuShiShang = 30191;
        public static int BuffLiuJinJiYuShiXia = 30201;
        public static int BuffLiuJinJiYuShiZuo = 30211;
        public static int BuffLiuJinJiYuShiYou = 30221;
        
        public static int BuffLiuJinFaZhouShiShang = 30231;
        public static int BuffLiuJinFaZhouShiXia = 30241;
        public static int BuffLiuJinFaZhouShiZuo = 30251;
        public static int BuffLiuJinFaZhouShiYou = 30261;
        public static int BuffSiQi = 30271;
        public static int BuffRanXi = 30281;
        public static int BuffPing = 30291;
        
        public static int BuffChe = 30301;
        public static int BuffQi = 30311;
        public static int BuffGuZai = 30321;
        public static int BuffYuSheNian = 30331;
        public static int BuffDaiMian = 30341;
        public static int BuffQiaoLaiFangJi = 30351;
        public static int BuffCanQue = 30361;
        public static int BuffShouHuaShen = 30371;
        public static int BuffQinHuaShen = 30381;
        public static int BuffZuHuaShen = 30391;
        public static int BuffLieMing = 30401;
        public static int BuffZhangGuiBenYuan = 30411;
        public static int BuffPoZhaoDiMian = 30421;
        
        //药力Buff 400系列
        public static int Buff40011 = 40011;
        public static int Buff40021 = 40021;
        public static int Buff40031 = 40031;
        public static int Buff40041 = 40041;
        public static int Buff40051 = 40051;
        public static int Buff40061 = 40061;
        public static int Buff40071 = 40071;
        public static int Buff40081 = 40081;
        public static int Buff40091 = 40091;
        public static int Buff40101 = 40101;
        public static int Buff40111 = 40111;
        public static int Buff40121 = 40121;
        public static int Buff40131 = 40131;
        public static int Buff40141 = 40141;
        public static int Buff40151 = 40151;
        public static int Buff40161 = 40161;
        public static int Buff40171 = 40171;
        public static int Buff40181 = 40181;
        public static int Buff40191 = 40191;
        public static int Buff40201 = 40201;
        public static int Buff40211 = 40211;
        public static int Buff40221 = 40221;
        public static int Buff40231 = 40231;
        public static int Buff40241 = 40241;
        public static int Buff40251 = 40251;
        public static int Buff40261 = 40261;
        public static int Buff40271 = 40271;
        public static int Buff40281 = 40281;
        public static int Buff40291 = 40291;
        public static int Buff40301 = 40301;
        public static int Buff40311 = 40311;
        
        //特殊Buff
        public static int Buff72008 = 72008;
        public static int Buff72053 = 72053;
        public static int Buff72065 = 72065;
        public static int Buff74041 = 74041;
        public static int Buff74046 = 74046;
        public static int Buff74073 = 74073;
        
        //护体/甲
        public static int Buff80001 = 80001;
        public static int Buff80002 = 80002;
        public static int Buff81001 = 81001;
        public static int Buff81002 = 81002;
        
        //效果Buff 900系列
        public static int Buff90003 = 90003;
        public static int Buff90004 = 90004;
        public static int Buff90005 = 90005;
        public static int Buff90006 = 90006;
        public static int Buff90007 = 90007;
        public static int Buff90008 = 90008;
        public static int Buff90009 = 90009;
        public static int Buff90010 = 90010;
        public static int Buff90011 = 90011;
        public static int Buff90012 = 90012;
        public static int Buff90013 = 90013;
        public static int Buff90014 = 90014;
        public static int Buff90015 = 90015;
        public static int Buff90016 = 90016;
        public static int Buff90017 = 90017;
        public static int Buff90018 = 90018;
        //失重效果
        public static int Buff90019 = 90019;
        //满欲效果
        public static int Buff90020 = 90020;
        
        //破招
        public static int Buff99999 = 99999;
        

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