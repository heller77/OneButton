namespace GameLoops
{
    /// <summary>
    /// 毎フレーム呼ばれるが、unity側に呼ばれたくない時に使う。
    /// </summary>
    public interface ITickable
    {
        void Tick();
    }
}