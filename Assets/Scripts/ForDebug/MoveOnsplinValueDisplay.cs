using Utils;

namespace ForDebug
{
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