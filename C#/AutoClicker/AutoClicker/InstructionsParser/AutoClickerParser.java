// Generated from .\AutoClicker.g4 by ANTLR 4.7.2
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class AutoClickerParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.7.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		DIGIT=1, X=2, Y=3, CLICK=4, BUTTON=5, LEFT=6, RIGHT=7, MIDDLE=8, DELAY=9, 
		REPETITIONS=10, SPEED=11, SHIFT=12, CTRL=13, ALT=14, EQ=15, SLASH=16, 
		IS=17, TRUE=18, FALSE=19, NUMBER=20, WORD=21, WHITESPACE=22, NEWLINE=23, 
		TEXT=24;
	public static final int
		RULE_intTuple = 0, RULE_doubleTuple = 1, RULE_trueFalse = 2, RULE_instructions = 3, 
		RULE_instruction = 4, RULE_click = 5, RULE_xPos = 6, RULE_yPos = 7, RULE_button = 8, 
		RULE_common = 9, RULE_delay = 10, RULE_repetitions = 11, RULE_speed = 12, 
		RULE_shift = 13, RULE_ctrl = 14, RULE_alt = 15;
	private static String[] makeRuleNames() {
		return new String[] {
			"intTuple", "doubleTuple", "trueFalse", "instructions", "instruction", 
			"click", "xPos", "yPos", "button", "common", "delay", "repetitions", 
			"speed", "shift", "ctrl", "alt"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, null, null, null, null, 
			null, null, null, "'='", "'/'", "':'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "DIGIT", "X", "Y", "CLICK", "BUTTON", "LEFT", "RIGHT", "MIDDLE", 
			"DELAY", "REPETITIONS", "SPEED", "SHIFT", "CTRL", "ALT", "EQ", "SLASH", 
			"IS", "TRUE", "FALSE", "NUMBER", "WORD", "WHITESPACE", "NEWLINE", "TEXT"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "AutoClicker.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public AutoClickerParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	public static class IntTupleContext extends ParserRuleContext {
		public TerminalNode EQ() { return getToken(AutoClickerParser.EQ, 0); }
		public List<TerminalNode> DIGIT() { return getTokens(AutoClickerParser.DIGIT); }
		public TerminalNode DIGIT(int i) {
			return getToken(AutoClickerParser.DIGIT, i);
		}
		public TerminalNode SLASH() { return getToken(AutoClickerParser.SLASH, 0); }
		public IntTupleContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_intTuple; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterIntTuple(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitIntTuple(this);
		}
	}

	public final IntTupleContext intTuple() throws RecognitionException {
		IntTupleContext _localctx = new IntTupleContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_intTuple);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(32);
			match(EQ);
			setState(34); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(33);
				match(DIGIT);
				}
				}
				setState(36); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==DIGIT );
			setState(44);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==SLASH) {
				{
				setState(38);
				match(SLASH);
				setState(40); 
				_errHandler.sync(this);
				_la = _input.LA(1);
				do {
					{
					{
					setState(39);
					match(DIGIT);
					}
					}
					setState(42); 
					_errHandler.sync(this);
					_la = _input.LA(1);
				} while ( _la==DIGIT );
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class DoubleTupleContext extends ParserRuleContext {
		public TerminalNode EQ() { return getToken(AutoClickerParser.EQ, 0); }
		public List<TerminalNode> NUMBER() { return getTokens(AutoClickerParser.NUMBER); }
		public TerminalNode NUMBER(int i) {
			return getToken(AutoClickerParser.NUMBER, i);
		}
		public TerminalNode SLASH() { return getToken(AutoClickerParser.SLASH, 0); }
		public DoubleTupleContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_doubleTuple; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterDoubleTuple(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitDoubleTuple(this);
		}
	}

	public final DoubleTupleContext doubleTuple() throws RecognitionException {
		DoubleTupleContext _localctx = new DoubleTupleContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_doubleTuple);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(46);
			match(EQ);
			setState(48); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(47);
				match(NUMBER);
				}
				}
				setState(50); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==NUMBER );
			setState(58);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==SLASH) {
				{
				setState(52);
				match(SLASH);
				setState(54); 
				_errHandler.sync(this);
				_la = _input.LA(1);
				do {
					{
					{
					setState(53);
					match(NUMBER);
					}
					}
					setState(56); 
					_errHandler.sync(this);
					_la = _input.LA(1);
				} while ( _la==NUMBER );
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TrueFalseContext extends ParserRuleContext {
		public TerminalNode EQ() { return getToken(AutoClickerParser.EQ, 0); }
		public TerminalNode TRUE() { return getToken(AutoClickerParser.TRUE, 0); }
		public TerminalNode FALSE() { return getToken(AutoClickerParser.FALSE, 0); }
		public TrueFalseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_trueFalse; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterTrueFalse(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitTrueFalse(this);
		}
	}

	public final TrueFalseContext trueFalse() throws RecognitionException {
		TrueFalseContext _localctx = new TrueFalseContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_trueFalse);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(60);
			match(EQ);
			setState(61);
			_la = _input.LA(1);
			if ( !(_la==TRUE || _la==FALSE) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class InstructionsContext extends ParserRuleContext {
		public List<InstructionContext> instruction() {
			return getRuleContexts(InstructionContext.class);
		}
		public InstructionContext instruction(int i) {
			return getRuleContext(InstructionContext.class,i);
		}
		public List<TerminalNode> NEWLINE() { return getTokens(AutoClickerParser.NEWLINE); }
		public TerminalNode NEWLINE(int i) {
			return getToken(AutoClickerParser.NEWLINE, i);
		}
		public InstructionsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_instructions; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterInstructions(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitInstructions(this);
		}
	}

	public final InstructionsContext instructions() throws RecognitionException {
		InstructionsContext _localctx = new InstructionsContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_instructions);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(68);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==CLICK) {
				{
				{
				setState(63);
				instruction();
				setState(64);
				match(NEWLINE);
				}
				}
				setState(70);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class InstructionContext extends ParserRuleContext {
		public ClickContext click() {
			return getRuleContext(ClickContext.class,0);
		}
		public InstructionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_instruction; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterInstruction(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitInstruction(this);
		}
	}

	public final InstructionContext instruction() throws RecognitionException {
		InstructionContext _localctx = new InstructionContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_instruction);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(71);
			click();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ClickContext extends ParserRuleContext {
		public TerminalNode CLICK() { return getToken(AutoClickerParser.CLICK, 0); }
		public TerminalNode IS() { return getToken(AutoClickerParser.IS, 0); }
		public List<XPosContext> xPos() {
			return getRuleContexts(XPosContext.class);
		}
		public XPosContext xPos(int i) {
			return getRuleContext(XPosContext.class,i);
		}
		public List<YPosContext> yPos() {
			return getRuleContexts(YPosContext.class);
		}
		public YPosContext yPos(int i) {
			return getRuleContext(YPosContext.class,i);
		}
		public List<ButtonContext> button() {
			return getRuleContexts(ButtonContext.class);
		}
		public ButtonContext button(int i) {
			return getRuleContext(ButtonContext.class,i);
		}
		public List<CommonContext> common() {
			return getRuleContexts(CommonContext.class);
		}
		public CommonContext common(int i) {
			return getRuleContext(CommonContext.class,i);
		}
		public ClickContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_click; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterClick(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitClick(this);
		}
	}

	public final ClickContext click() throws RecognitionException {
		ClickContext _localctx = new ClickContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_click);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(73);
			match(CLICK);
			setState(74);
			match(IS);
			setState(81);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << X) | (1L << Y) | (1L << BUTTON) | (1L << DELAY) | (1L << REPETITIONS) | (1L << SPEED) | (1L << SHIFT) | (1L << CTRL) | (1L << ALT))) != 0)) {
				{
				setState(79);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case X:
					{
					setState(75);
					xPos();
					}
					break;
				case Y:
					{
					setState(76);
					yPos();
					}
					break;
				case BUTTON:
					{
					setState(77);
					button();
					}
					break;
				case DELAY:
				case REPETITIONS:
				case SPEED:
				case SHIFT:
				case CTRL:
				case ALT:
					{
					setState(78);
					common();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				setState(83);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class XPosContext extends ParserRuleContext {
		public TerminalNode X() { return getToken(AutoClickerParser.X, 0); }
		public IntTupleContext intTuple() {
			return getRuleContext(IntTupleContext.class,0);
		}
		public XPosContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_xPos; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterXPos(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitXPos(this);
		}
	}

	public final XPosContext xPos() throws RecognitionException {
		XPosContext _localctx = new XPosContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_xPos);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(84);
			match(X);
			setState(85);
			intTuple();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class YPosContext extends ParserRuleContext {
		public TerminalNode Y() { return getToken(AutoClickerParser.Y, 0); }
		public IntTupleContext intTuple() {
			return getRuleContext(IntTupleContext.class,0);
		}
		public YPosContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_yPos; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterYPos(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitYPos(this);
		}
	}

	public final YPosContext yPos() throws RecognitionException {
		YPosContext _localctx = new YPosContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_yPos);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(87);
			match(Y);
			setState(88);
			intTuple();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ButtonContext extends ParserRuleContext {
		public TerminalNode BUTTON() { return getToken(AutoClickerParser.BUTTON, 0); }
		public TerminalNode EQ() { return getToken(AutoClickerParser.EQ, 0); }
		public TerminalNode LEFT() { return getToken(AutoClickerParser.LEFT, 0); }
		public TerminalNode RIGHT() { return getToken(AutoClickerParser.RIGHT, 0); }
		public TerminalNode MIDDLE() { return getToken(AutoClickerParser.MIDDLE, 0); }
		public ButtonContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_button; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterButton(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitButton(this);
		}
	}

	public final ButtonContext button() throws RecognitionException {
		ButtonContext _localctx = new ButtonContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_button);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(90);
			match(BUTTON);
			setState(91);
			match(EQ);
			setState(92);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << LEFT) | (1L << RIGHT) | (1L << MIDDLE))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class CommonContext extends ParserRuleContext {
		public DelayContext delay() {
			return getRuleContext(DelayContext.class,0);
		}
		public RepetitionsContext repetitions() {
			return getRuleContext(RepetitionsContext.class,0);
		}
		public SpeedContext speed() {
			return getRuleContext(SpeedContext.class,0);
		}
		public ShiftContext shift() {
			return getRuleContext(ShiftContext.class,0);
		}
		public CtrlContext ctrl() {
			return getRuleContext(CtrlContext.class,0);
		}
		public AltContext alt() {
			return getRuleContext(AltContext.class,0);
		}
		public CommonContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_common; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterCommon(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitCommon(this);
		}
	}

	public final CommonContext common() throws RecognitionException {
		CommonContext _localctx = new CommonContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_common);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(100);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case DELAY:
				{
				setState(94);
				delay();
				}
				break;
			case REPETITIONS:
				{
				setState(95);
				repetitions();
				}
				break;
			case SPEED:
				{
				setState(96);
				speed();
				}
				break;
			case SHIFT:
				{
				setState(97);
				shift();
				}
				break;
			case CTRL:
				{
				setState(98);
				ctrl();
				}
				break;
			case ALT:
				{
				setState(99);
				alt();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class DelayContext extends ParserRuleContext {
		public TerminalNode DELAY() { return getToken(AutoClickerParser.DELAY, 0); }
		public IntTupleContext intTuple() {
			return getRuleContext(IntTupleContext.class,0);
		}
		public DelayContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_delay; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterDelay(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitDelay(this);
		}
	}

	public final DelayContext delay() throws RecognitionException {
		DelayContext _localctx = new DelayContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_delay);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(102);
			match(DELAY);
			setState(103);
			intTuple();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class RepetitionsContext extends ParserRuleContext {
		public TerminalNode REPETITIONS() { return getToken(AutoClickerParser.REPETITIONS, 0); }
		public IntTupleContext intTuple() {
			return getRuleContext(IntTupleContext.class,0);
		}
		public RepetitionsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_repetitions; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterRepetitions(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitRepetitions(this);
		}
	}

	public final RepetitionsContext repetitions() throws RecognitionException {
		RepetitionsContext _localctx = new RepetitionsContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_repetitions);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(105);
			match(REPETITIONS);
			setState(106);
			intTuple();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class SpeedContext extends ParserRuleContext {
		public TerminalNode SPEED() { return getToken(AutoClickerParser.SPEED, 0); }
		public DoubleTupleContext doubleTuple() {
			return getRuleContext(DoubleTupleContext.class,0);
		}
		public SpeedContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_speed; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterSpeed(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitSpeed(this);
		}
	}

	public final SpeedContext speed() throws RecognitionException {
		SpeedContext _localctx = new SpeedContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_speed);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(108);
			match(SPEED);
			setState(109);
			doubleTuple();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ShiftContext extends ParserRuleContext {
		public TerminalNode SHIFT() { return getToken(AutoClickerParser.SHIFT, 0); }
		public TrueFalseContext trueFalse() {
			return getRuleContext(TrueFalseContext.class,0);
		}
		public ShiftContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_shift; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterShift(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitShift(this);
		}
	}

	public final ShiftContext shift() throws RecognitionException {
		ShiftContext _localctx = new ShiftContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_shift);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(111);
			match(SHIFT);
			setState(112);
			trueFalse();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class CtrlContext extends ParserRuleContext {
		public TerminalNode CTRL() { return getToken(AutoClickerParser.CTRL, 0); }
		public TrueFalseContext trueFalse() {
			return getRuleContext(TrueFalseContext.class,0);
		}
		public CtrlContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_ctrl; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterCtrl(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitCtrl(this);
		}
	}

	public final CtrlContext ctrl() throws RecognitionException {
		CtrlContext _localctx = new CtrlContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_ctrl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(114);
			match(CTRL);
			setState(115);
			trueFalse();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class AltContext extends ParserRuleContext {
		public TerminalNode ALT() { return getToken(AutoClickerParser.ALT, 0); }
		public TrueFalseContext trueFalse() {
			return getRuleContext(TrueFalseContext.class,0);
		}
		public AltContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_alt; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).enterAlt(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof AutoClickerListener ) ((AutoClickerListener)listener).exitAlt(this);
		}
	}

	public final AltContext alt() throws RecognitionException {
		AltContext _localctx = new AltContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_alt);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(117);
			match(ALT);
			setState(118);
			trueFalse();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3\32{\4\2\t\2\4\3\t"+
		"\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t\13\4"+
		"\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\3\2\3\2\6\2%\n"+
		"\2\r\2\16\2&\3\2\3\2\6\2+\n\2\r\2\16\2,\5\2/\n\2\3\3\3\3\6\3\63\n\3\r"+
		"\3\16\3\64\3\3\3\3\6\39\n\3\r\3\16\3:\5\3=\n\3\3\4\3\4\3\4\3\5\3\5\3\5"+
		"\7\5E\n\5\f\5\16\5H\13\5\3\6\3\6\3\7\3\7\3\7\3\7\3\7\3\7\7\7R\n\7\f\7"+
		"\16\7U\13\7\3\b\3\b\3\b\3\t\3\t\3\t\3\n\3\n\3\n\3\n\3\13\3\13\3\13\3\13"+
		"\3\13\3\13\5\13g\n\13\3\f\3\f\3\f\3\r\3\r\3\r\3\16\3\16\3\16\3\17\3\17"+
		"\3\17\3\20\3\20\3\20\3\21\3\21\3\21\3\21\2\2\22\2\4\6\b\n\f\16\20\22\24"+
		"\26\30\32\34\36 \2\4\3\2\24\25\3\2\b\n\2z\2\"\3\2\2\2\4\60\3\2\2\2\6>"+
		"\3\2\2\2\bF\3\2\2\2\nI\3\2\2\2\fK\3\2\2\2\16V\3\2\2\2\20Y\3\2\2\2\22\\"+
		"\3\2\2\2\24f\3\2\2\2\26h\3\2\2\2\30k\3\2\2\2\32n\3\2\2\2\34q\3\2\2\2\36"+
		"t\3\2\2\2 w\3\2\2\2\"$\7\21\2\2#%\7\3\2\2$#\3\2\2\2%&\3\2\2\2&$\3\2\2"+
		"\2&\'\3\2\2\2\'.\3\2\2\2(*\7\22\2\2)+\7\3\2\2*)\3\2\2\2+,\3\2\2\2,*\3"+
		"\2\2\2,-\3\2\2\2-/\3\2\2\2.(\3\2\2\2./\3\2\2\2/\3\3\2\2\2\60\62\7\21\2"+
		"\2\61\63\7\26\2\2\62\61\3\2\2\2\63\64\3\2\2\2\64\62\3\2\2\2\64\65\3\2"+
		"\2\2\65<\3\2\2\2\668\7\22\2\2\679\7\26\2\28\67\3\2\2\29:\3\2\2\2:8\3\2"+
		"\2\2:;\3\2\2\2;=\3\2\2\2<\66\3\2\2\2<=\3\2\2\2=\5\3\2\2\2>?\7\21\2\2?"+
		"@\t\2\2\2@\7\3\2\2\2AB\5\n\6\2BC\7\31\2\2CE\3\2\2\2DA\3\2\2\2EH\3\2\2"+
		"\2FD\3\2\2\2FG\3\2\2\2G\t\3\2\2\2HF\3\2\2\2IJ\5\f\7\2J\13\3\2\2\2KL\7"+
		"\6\2\2LS\7\23\2\2MR\5\16\b\2NR\5\20\t\2OR\5\22\n\2PR\5\24\13\2QM\3\2\2"+
		"\2QN\3\2\2\2QO\3\2\2\2QP\3\2\2\2RU\3\2\2\2SQ\3\2\2\2ST\3\2\2\2T\r\3\2"+
		"\2\2US\3\2\2\2VW\7\4\2\2WX\5\2\2\2X\17\3\2\2\2YZ\7\5\2\2Z[\5\2\2\2[\21"+
		"\3\2\2\2\\]\7\7\2\2]^\7\21\2\2^_\t\3\2\2_\23\3\2\2\2`g\5\26\f\2ag\5\30"+
		"\r\2bg\5\32\16\2cg\5\34\17\2dg\5\36\20\2eg\5 \21\2f`\3\2\2\2fa\3\2\2\2"+
		"fb\3\2\2\2fc\3\2\2\2fd\3\2\2\2fe\3\2\2\2g\25\3\2\2\2hi\7\13\2\2ij\5\2"+
		"\2\2j\27\3\2\2\2kl\7\f\2\2lm\5\2\2\2m\31\3\2\2\2no\7\r\2\2op\5\4\3\2p"+
		"\33\3\2\2\2qr\7\16\2\2rs\5\6\4\2s\35\3\2\2\2tu\7\17\2\2uv\5\6\4\2v\37"+
		"\3\2\2\2wx\7\20\2\2xy\5\6\4\2y!\3\2\2\2\f&,.\64:<FQSf";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}