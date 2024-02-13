using System;
using UnityEngine;

namespace GameManagers.ScoreCalculater
{
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

    public class ScoreCalculater
    {
        private BattleResultData _battleResultData;

        private ScoreWeight _scoreWeight;

        private Score _score;

        public ScoreCalculater(BattleResultData data, ScoreWeight scoreWeight)
        {
            this._battleResultData = data;
            this._scoreWeight = scoreWeight;
        }

        public BattleResultData GetBattleData()
        {
            return this._battleResultData;
        }

        public Score GetScore()
        {
            return this._score;
        }

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
            Debug.Log($"culcuate sum {score.sum}");

            this._score = score;
            return score;
        }
    }
}