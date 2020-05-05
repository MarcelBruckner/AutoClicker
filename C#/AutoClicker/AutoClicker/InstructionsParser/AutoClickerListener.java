// Generated from .\AutoClicker.g4 by ANTLR 4.7.2
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link AutoClickerParser}.
 */
public interface AutoClickerListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#intTuple}.
	 * @param ctx the parse tree
	 */
	void enterIntTuple(AutoClickerParser.IntTupleContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#intTuple}.
	 * @param ctx the parse tree
	 */
	void exitIntTuple(AutoClickerParser.IntTupleContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#doubleTuple}.
	 * @param ctx the parse tree
	 */
	void enterDoubleTuple(AutoClickerParser.DoubleTupleContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#doubleTuple}.
	 * @param ctx the parse tree
	 */
	void exitDoubleTuple(AutoClickerParser.DoubleTupleContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#trueFalse}.
	 * @param ctx the parse tree
	 */
	void enterTrueFalse(AutoClickerParser.TrueFalseContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#trueFalse}.
	 * @param ctx the parse tree
	 */
	void exitTrueFalse(AutoClickerParser.TrueFalseContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#instructions}.
	 * @param ctx the parse tree
	 */
	void enterInstructions(AutoClickerParser.InstructionsContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#instructions}.
	 * @param ctx the parse tree
	 */
	void exitInstructions(AutoClickerParser.InstructionsContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#instruction}.
	 * @param ctx the parse tree
	 */
	void enterInstruction(AutoClickerParser.InstructionContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#instruction}.
	 * @param ctx the parse tree
	 */
	void exitInstruction(AutoClickerParser.InstructionContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#click}.
	 * @param ctx the parse tree
	 */
	void enterClick(AutoClickerParser.ClickContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#click}.
	 * @param ctx the parse tree
	 */
	void exitClick(AutoClickerParser.ClickContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#xPos}.
	 * @param ctx the parse tree
	 */
	void enterXPos(AutoClickerParser.XPosContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#xPos}.
	 * @param ctx the parse tree
	 */
	void exitXPos(AutoClickerParser.XPosContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#yPos}.
	 * @param ctx the parse tree
	 */
	void enterYPos(AutoClickerParser.YPosContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#yPos}.
	 * @param ctx the parse tree
	 */
	void exitYPos(AutoClickerParser.YPosContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#button}.
	 * @param ctx the parse tree
	 */
	void enterButton(AutoClickerParser.ButtonContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#button}.
	 * @param ctx the parse tree
	 */
	void exitButton(AutoClickerParser.ButtonContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#common}.
	 * @param ctx the parse tree
	 */
	void enterCommon(AutoClickerParser.CommonContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#common}.
	 * @param ctx the parse tree
	 */
	void exitCommon(AutoClickerParser.CommonContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#delay}.
	 * @param ctx the parse tree
	 */
	void enterDelay(AutoClickerParser.DelayContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#delay}.
	 * @param ctx the parse tree
	 */
	void exitDelay(AutoClickerParser.DelayContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#repetitions}.
	 * @param ctx the parse tree
	 */
	void enterRepetitions(AutoClickerParser.RepetitionsContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#repetitions}.
	 * @param ctx the parse tree
	 */
	void exitRepetitions(AutoClickerParser.RepetitionsContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#speed}.
	 * @param ctx the parse tree
	 */
	void enterSpeed(AutoClickerParser.SpeedContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#speed}.
	 * @param ctx the parse tree
	 */
	void exitSpeed(AutoClickerParser.SpeedContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#shift}.
	 * @param ctx the parse tree
	 */
	void enterShift(AutoClickerParser.ShiftContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#shift}.
	 * @param ctx the parse tree
	 */
	void exitShift(AutoClickerParser.ShiftContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#ctrl}.
	 * @param ctx the parse tree
	 */
	void enterCtrl(AutoClickerParser.CtrlContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#ctrl}.
	 * @param ctx the parse tree
	 */
	void exitCtrl(AutoClickerParser.CtrlContext ctx);
	/**
	 * Enter a parse tree produced by {@link AutoClickerParser#alt}.
	 * @param ctx the parse tree
	 */
	void enterAlt(AutoClickerParser.AltContext ctx);
	/**
	 * Exit a parse tree produced by {@link AutoClickerParser#alt}.
	 * @param ctx the parse tree
	 */
	void exitAlt(AutoClickerParser.AltContext ctx);
}