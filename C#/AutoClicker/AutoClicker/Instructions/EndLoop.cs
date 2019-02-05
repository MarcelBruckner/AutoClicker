
namespace AutoClicker.Instructions
{
    class EndLoop : Instruction
    {
        public EndLoop() : base(Action.END_LOOP) {      }

        public override string ToString()
        {
            return Type + "";
        }
    }
}