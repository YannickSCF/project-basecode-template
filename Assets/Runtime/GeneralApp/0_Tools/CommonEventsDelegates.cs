
namespace YannickSCF.GeneralApp {
    public class CommonEventsDelegates {
        public delegate void SimpleEventDelegate();
        public delegate void StringEventDelegate(string strValue);
        public delegate void IntegerEventDelegate(int intValue);
        public delegate void FloatEventDelegate(float floatValue);
        public delegate void BooleanEventDelegate(bool boolValue);
    }
}
