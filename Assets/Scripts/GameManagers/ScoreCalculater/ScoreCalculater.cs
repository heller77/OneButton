using System;

namespace GameManagers.ScoreCalculater
{
    /// <summary>
    ///     スコア毎の重みを保持
    /// </summary>
    [Serializable]
    public struct ScoreWeight
    {
        public int knockdownMob_ScoreWeight;
        public int survivePlayerSideRobot_ScoreWeight;
        public int bossDamage_ScoreWeight;
        public int consumeBulletMinux_ScoreWeight;

        public ScoreWeight(int knockdownMobScoreWeight, int survivePlayerSideRobotScoreWeight,
            int bossDamageScoreWeight, int consumeBulletMinuxScoreWeight)
        {
            knockdownMob_ScoreWeight = knockdownMobScoreWeight;
            survivePlayerSideRobot_ScoreWeight = survivePlayerSideRobotScoreWeight;
            bossDamage_ScoreWeight = bossDamageScoreWeight;
            consumeBulletMinux_ScoreWeight = consumeBulletMinuxScoreWeight;
        }
    }

    /// <summary>
    ///     スコア構造体
    /// </summary>
    public struct Score
    {
        public int knockdownMobScore;
        public int survivePlayerSideRobotScore;
        public int bossDamageScore;
        public int consumeBulletMinuxScore;

        public long sum;

        public Score(int knockdownMobScore, int survivePlayerSideRobotScore, int bossDamageScore,
            int consumeBulletMinuxScore, long sum)
        {
            this.knockdownMobScore = knockdownMobScore;
            this.survivePlayerSideRobotScore = survivePlayerSideRobotScore;
            this.bossDamageScore = bossDamageScore;
            this.consumeBulletMinuxScore = consumeBulletMinuxScore;
            this.sum = sum;
        }
    }

    /// <summary>
    ///     スコアを計算
    /// </summary>
    public class ScoreCalculater
    {
        private readonly BattleResultData _battleResultData;

        private Score _score;

        private readonly ScoreWeight _scoreWeight;

        public ScoreCalculater(BattleResultData data, ScoreWeight scoreWeight)
        {
            _battleResultData = data;
            _scoreWeight = scoreWeight;
        }

        public BattleResultData GetBattleData()
        {
            return _battleResultData;
        }

        public Score GetScore()
        {
            return _score;
        }

        /// <summary>
        ///     スコアを計算
        /// </summary>
        /// <returns></returns>
        public Score Calculate()
        {
            int knockdownMobScore = _scoreWeight.knockdownMob_ScoreWeight * _battleResultData.knockDownMobEnemyCount;
            int survivePlayerSideRobotScore = _scoreWeight.survivePlayerSideRobot_ScoreWeight *
                                              _battleResultData.survivePlayerSideRobot;
            int bossDamageScore = _scoreWeight.bossDamage_ScoreWeight * _battleResultData.bossDamage;
            int consumeBulletMinuxScore =
                _scoreWeight.consumeBulletMinux_ScoreWeight * _battleResultData.consumeBulletNums;


            long sum = knockdownMobScore + survivePlayerSideRobotScore + bossDamageScore + consumeBulletMinuxScore;
            Score score = new Score(knockdownMobScore, survivePlayerSideRobotScore, bossDamageScore,
                consumeBulletMinuxScore, sum);

            _score = score;
            return score;
        }
    }
}