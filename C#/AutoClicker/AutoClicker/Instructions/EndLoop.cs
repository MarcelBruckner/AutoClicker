
namespace AutoClicker.Instructions
{
    class EndLoop : Instruction
    {
        public EndLoop() : base(Action.END_LOOP, 0, 1, false, false, false) {      }

        public override string ToString()
        {
            return Type + "";
        }
    }
}