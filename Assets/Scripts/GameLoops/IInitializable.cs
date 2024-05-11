namespace GameLoops
{
    /// <summary>
    /// GameLoopMonoでゲーム開始時に関数呼ばれたい時に使うインターフェイス
    /// </summary>
    public interface IInitializable
    {
        public void Initialize();
    }
}