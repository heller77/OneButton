using UnityEngine;

namespace GameManagers
{
    public struct BattleResultData
    {
        public int knockDownMobEnemyCount;
        public int survivePlayerSideRobot;
        public int bossDamage;
        public int consumeBulletNums;

        public BattleResultData(int knockDownMobEnemyCount, int survivePlayerSideRobot, int bossDamage,
            int consumeBulletNums)
        {
            this.knockDownMobEnemyCount = knockDownMobEnemyCount;
            this.survivePlayerSideRobot = survivePlayerSideRobot;
            this.bossDamage = bossDamage;
            this.consumeBulletNums = consumeBulletNums;
        }

        public override string ToString()
        {
            return $"倒したモブ敵　: {knockDownMobEnemyCount} , 生き残った味方　: {survivePlayerSideRobot}+" +
                   $"ボスへのダメージ : {bossDamage} , 消費した弾  : {consumeBulletNums}";
        }
    }

    public class BattleResultManager
    {
        private static BattleResultManager _instance;

        public static BattleResultManager GetInstance()
        {
            if (_instance is null)
            {
                _instance = new BattleResultManager();
            }

            return _instance;
        }

        private BattleResultData data;

        public BattleResultData GetBattleResultData()
        {
            return this.data;
        }

        public BattleResultManager()
        {
            this.data = new BattleResultData(0, 0, 0, 0);
        }

        public void AddKnockMobEnemy()
        {
            data.knockDownMobEnemyCount++;
        }

        public void SetSurvivePlayerSideRobot(int survivePlayerSideRobotCount)
        {
            data.survivePlayerSideRobot = survivePlayerSideRobotCount;
        }

        public void SetBossDamage(int bossdamage)
        {
            data.bossDamage = bossdamage;
        }

        public void AddConsumeBullet()
        {
            data.consumeBulletNums++;
        }
    }
}