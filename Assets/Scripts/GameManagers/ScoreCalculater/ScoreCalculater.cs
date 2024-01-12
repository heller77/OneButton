namespace GameManagers.ScoreCalculater
{
    public class ScoreCalculater
    {
        public static long Calculate(BattleResultData battleResultData)
        {
            int knockdownMobScore = 1000 * battleResultData.knockDownMobEnemyCount;
            int survivePlayerSideRobotScore = 3000 * battleResultData.survivePlayerSideRobot;
            int bossDamageScore = battleResultData.bossDamage;
            int consumeBulletMinuxScore = -1 * battleResultData.consumeBulletNums;


            long score = knockdownMobScore + survivePlayerSideRobotScore + bossDamageScore + consumeBulletMinuxScore;
            return score;
        }
    }
}