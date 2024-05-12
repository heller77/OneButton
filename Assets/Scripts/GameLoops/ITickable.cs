namespace GameLoops
{
    /// <summary>
    ///     GameLoopMonoで毎フレーム関数呼ばれたい時に使うインターフェイス
    /// </summary>
    public interface ITickable
    {
        void Tick();
    }
}