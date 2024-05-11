using Utils;

namespace ForDebug
{
    /// <summary>
    /// MoveOnsplineクラスのあるオブジェクトにつけると、今のMoveOnsplineの値を表示
    /// </summary>
    public class MoveOnsplinValueDisplay : DisplayTextInEditor
    {
#if UNITY_EDITOR
        protected override string DecideDisplayText()
        {
            return this.displayText + this.gameObject.GetComponent<MoverOnSpline>().GetT().ToString();
        }
#endif
    }
}