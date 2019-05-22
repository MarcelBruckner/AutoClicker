// Generated from .\AutoClicker.g4 by ANTLR 4.7.2
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class AutoClickerLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.7.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		DIGIT=1, X=2, Y=3, CLICK=4, BUTTON=5, LEFT=6, RIGHT=7, MIDDLE=8, DELAY=9, 
		REPETITIONS=10, SPEED=11, SHIFT=12, CTRL=13, ALT=14, EQ=15, SLASH=16, 
		IS=17, TRUE=18, FALSE=19, NUMBER=20, WORD=21, WHITESPACE=22, NEWLINE=23, 
		TEXT=24;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", 
			"O", "P", "Q", "R", "S", "T", "U", "V", "W", "Z", "LOWERCASE", "UPPERCASE", 
			"DIGIT", "X", "Y", "CLICK", "BUTTON", "LEFT", "RIGHT", "MIDDLE", "DELAY", 
			"REPETITIONS", "SPEED", "SHIFT", "CTRL", "ALT", "EQ", "SLASH", "IS", 
			"TRUE", "FALSE", "NUMBER", "WORD", "WHITESPACE", "NEWLINE", "TEXT"
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


	public AutoClickerLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "AutoClicker.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\2\32\u0122\b\1\4\2"+
		"\t\2\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4"+
		"\13\t\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22"+
		"\t\22\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31"+
		"\t\31\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37\4 \t"+
		" \4!\t!\4\"\t\"\4#\t#\4$\t$\4%\t%\4&\t&\4\'\t\'\4(\t(\4)\t)\4*\t*\4+\t"+
		"+\4,\t,\4-\t-\4.\t.\4/\t/\4\60\t\60\4\61\t\61\4\62\t\62\4\63\t\63\3\2"+
		"\3\2\3\3\3\3\3\4\3\4\3\5\3\5\3\6\3\6\3\7\3\7\3\b\3\b\3\t\3\t\3\n\3\n\3"+
		"\13\3\13\3\f\3\f\3\r\3\r\3\16\3\16\3\17\3\17\3\20\3\20\3\21\3\21\3\22"+
		"\3\22\3\23\3\23\3\24\3\24\3\25\3\25\3\26\3\26\3\27\3\27\3\30\3\30\3\31"+
		"\3\31\3\32\3\32\3\33\3\33\3\34\3\34\3\35\3\35\3\36\3\36\3\37\3\37\3\37"+
		"\3\37\3\37\3\37\3 \3 \3 \3 \3 \3 \3 \3!\3!\3!\3!\3!\3\"\3\"\3\"\3\"\3"+
		"\"\3\"\3#\3#\3#\3#\3#\3#\3#\3$\3$\3$\3$\3$\3$\3%\3%\3%\3%\3%\3%\3%\3%"+
		"\3%\3%\3%\3%\3&\3&\3&\3&\3&\3&\3\'\3\'\3\'\3\'\3\'\3\'\3(\3(\3(\3(\3("+
		"\3)\3)\3)\3)\3*\3*\3+\3+\3,\3,\3-\3-\3-\3-\3-\3.\3.\3.\3.\3.\3.\3/\6/"+
		"\u00fa\n/\r/\16/\u00fb\3/\3/\6/\u0100\n/\r/\16/\u0101\5/\u0104\n/\3\60"+
		"\3\60\3\60\6\60\u0109\n\60\r\60\16\60\u010a\3\61\3\61\3\61\3\61\3\62\5"+
		"\62\u0112\n\62\3\62\3\62\6\62\u0116\n\62\r\62\16\62\u0117\3\63\3\63\7"+
		"\63\u011c\n\63\f\63\16\63\u011f\13\63\3\63\3\63\3\u011d\2\64\3\2\5\2\7"+
		"\2\t\2\13\2\r\2\17\2\21\2\23\2\25\2\27\2\31\2\33\2\35\2\37\2!\2#\2%\2"+
		"\'\2)\2+\2-\2/\2\61\2\63\2\65\2\67\39\4;\5=\6?\7A\bC\tE\nG\13I\fK\rM\16"+
		"O\17Q\20S\21U\22W\23Y\24[\25]\26_\27a\30c\31e\32\3\2#\4\2CCcc\4\2DDdd"+
		"\4\2EEee\4\2FFff\4\2GGgg\4\2HHhh\4\2IIii\4\2JJjj\4\2KKkk\4\2LLll\4\2M"+
		"Mmm\4\2NNnn\4\2OOoo\4\2PPpp\4\2QQqq\4\2RRrr\4\2SSss\4\2TTtt\4\2UUuu\4"+
		"\2VVvv\4\2WWww\4\2XXxx\4\2YYyy\4\2\\\\||\3\2c|\3\2C\\\3\2\62;\4\2ZZzz"+
		"\4\2[[{{\4\2..\60\60\5\2\13\f\17\17\"\"\4\2**]]\4\2++__\2\u0111\2\67\3"+
		"\2\2\2\29\3\2\2\2\2;\3\2\2\2\2=\3\2\2\2\2?\3\2\2\2\2A\3\2\2\2\2C\3\2\2"+
		"\2\2E\3\2\2\2\2G\3\2\2\2\2I\3\2\2\2\2K\3\2\2\2\2M\3\2\2\2\2O\3\2\2\2\2"+
		"Q\3\2\2\2\2S\3\2\2\2\2U\3\2\2\2\2W\3\2\2\2\2Y\3\2\2\2\2[\3\2\2\2\2]\3"+
		"\2\2\2\2_\3\2\2\2\2a\3\2\2\2\2c\3\2\2\2\2e\3\2\2\2\3g\3\2\2\2\5i\3\2\2"+
		"\2\7k\3\2\2\2\tm\3\2\2\2\13o\3\2\2\2\rq\3\2\2\2\17s\3\2\2\2\21u\3\2\2"+
		"\2\23w\3\2\2\2\25y\3\2\2\2\27{\3\2\2\2\31}\3\2\2\2\33\177\3\2\2\2\35\u0081"+
		"\3\2\2\2\37\u0083\3\2\2\2!\u0085\3\2\2\2#\u0087\3\2\2\2%\u0089\3\2\2\2"+
		"\'\u008b\3\2\2\2)\u008d\3\2\2\2+\u008f\3\2\2\2-\u0091\3\2\2\2/\u0093\3"+
		"\2\2\2\61\u0095\3\2\2\2\63\u0097\3\2\2\2\65\u0099\3\2\2\2\67\u009b\3\2"+
		"\2\29\u009d\3\2\2\2;\u009f\3\2\2\2=\u00a1\3\2\2\2?\u00a7\3\2\2\2A\u00ae"+
		"\3\2\2\2C\u00b3\3\2\2\2E\u00b9\3\2\2\2G\u00c0\3\2\2\2I\u00c6\3\2\2\2K"+
		"\u00d2\3\2\2\2M\u00d8\3\2\2\2O\u00de\3\2\2\2Q\u00e3\3\2\2\2S\u00e7\3\2"+
		"\2\2U\u00e9\3\2\2\2W\u00eb\3\2\2\2Y\u00ed\3\2\2\2[\u00f2\3\2\2\2]\u00f9"+
		"\3\2\2\2_\u0108\3\2\2\2a\u010c\3\2\2\2c\u0115\3\2\2\2e\u0119\3\2\2\2g"+
		"h\t\2\2\2h\4\3\2\2\2ij\t\3\2\2j\6\3\2\2\2kl\t\4\2\2l\b\3\2\2\2mn\t\5\2"+
		"\2n\n\3\2\2\2op\t\6\2\2p\f\3\2\2\2qr\t\7\2\2r\16\3\2\2\2st\t\b\2\2t\20"+
		"\3\2\2\2uv\t\t\2\2v\22\3\2\2\2wx\t\n\2\2x\24\3\2\2\2yz\t\13\2\2z\26\3"+
		"\2\2\2{|\t\f\2\2|\30\3\2\2\2}~\t\r\2\2~\32\3\2\2\2\177\u0080\t\16\2\2"+
		"\u0080\34\3\2\2\2\u0081\u0082\t\17\2\2\u0082\36\3\2\2\2\u0083\u0084\t"+
		"\20\2\2\u0084 \3\2\2\2\u0085\u0086\t\21\2\2\u0086\"\3\2\2\2\u0087\u0088"+
		"\t\22\2\2\u0088$\3\2\2\2\u0089\u008a\t\23\2\2\u008a&\3\2\2\2\u008b\u008c"+
		"\t\24\2\2\u008c(\3\2\2\2\u008d\u008e\t\25\2\2\u008e*\3\2\2\2\u008f\u0090"+
		"\t\26\2\2\u0090,\3\2\2\2\u0091\u0092\t\27\2\2\u0092.\3\2\2\2\u0093\u0094"+
		"\t\30\2\2\u0094\60\3\2\2\2\u0095\u0096\t\31\2\2\u0096\62\3\2\2\2\u0097"+
		"\u0098\t\32\2\2\u0098\64\3\2\2\2\u0099\u009a\t\33\2\2\u009a\66\3\2\2\2"+
		"\u009b\u009c\t\34\2\2\u009c8\3\2\2\2\u009d\u009e\t\35\2\2\u009e:\3\2\2"+
		"\2\u009f\u00a0\t\36\2\2\u00a0<\3\2\2\2\u00a1\u00a2\5\7\4\2\u00a2\u00a3"+
		"\5\31\r\2\u00a3\u00a4\5\23\n\2\u00a4\u00a5\5\7\4\2\u00a5\u00a6\5\27\f"+
		"\2\u00a6>\3\2\2\2\u00a7\u00a8\5\5\3\2\u00a8\u00a9\5+\26\2\u00a9\u00aa"+
		"\5)\25\2\u00aa\u00ab\5)\25\2\u00ab\u00ac\5\37\20\2\u00ac\u00ad\5\35\17"+
		"\2\u00ad@\3\2\2\2\u00ae\u00af\5\31\r\2\u00af\u00b0\5\13\6\2\u00b0\u00b1"+
		"\5\r\7\2\u00b1\u00b2\5)\25\2\u00b2B\3\2\2\2\u00b3\u00b4\5%\23\2\u00b4"+
		"\u00b5\5\23\n\2\u00b5\u00b6\5\17\b\2\u00b6\u00b7\5\21\t\2\u00b7\u00b8"+
		"\5)\25\2\u00b8D\3\2\2\2\u00b9\u00ba\5\33\16\2\u00ba\u00bb\5\23\n\2\u00bb"+
		"\u00bc\5\t\5\2\u00bc\u00bd\5\t\5\2\u00bd\u00be\5\31\r\2\u00be\u00bf\5"+
		"\13\6\2\u00bfF\3\2\2\2\u00c0\u00c1\5\t\5\2\u00c1\u00c2\5\13\6\2\u00c2"+
		"\u00c3\5\31\r\2\u00c3\u00c4\5\3\2\2\u00c4\u00c5\5;\36\2\u00c5H\3\2\2\2"+
		"\u00c6\u00c7\5%\23\2\u00c7\u00c8\5\13\6\2\u00c8\u00c9\5!\21\2\u00c9\u00ca"+
		"\5\13\6\2\u00ca\u00cb\5)\25\2\u00cb\u00cc\5\23\n\2\u00cc\u00cd\5)\25\2"+
		"\u00cd\u00ce\5\23\n\2\u00ce\u00cf\5\37\20\2\u00cf\u00d0\5\35\17\2\u00d0"+
		"\u00d1\5\'\24\2\u00d1J\3\2\2\2\u00d2\u00d3\5\'\24\2\u00d3\u00d4\5!\21"+
		"\2\u00d4\u00d5\5\13\6\2\u00d5\u00d6\5\13\6\2\u00d6\u00d7\5\t\5\2\u00d7"+
		"L\3\2\2\2\u00d8\u00d9\5\'\24\2\u00d9\u00da\5\21\t\2\u00da\u00db\5\23\n"+
		"\2\u00db\u00dc\5\r\7\2\u00dc\u00dd\5)\25\2\u00ddN\3\2\2\2\u00de\u00df"+
		"\5\7\4\2\u00df\u00e0\5)\25\2\u00e0\u00e1\5%\23\2\u00e1\u00e2\5\31\r\2"+
		"\u00e2P\3\2\2\2\u00e3\u00e4\5\3\2\2\u00e4\u00e5\5\31\r\2\u00e5\u00e6\5"+
		")\25\2\u00e6R\3\2\2\2\u00e7\u00e8\7?\2\2\u00e8T\3\2\2\2\u00e9\u00ea\7"+
		"\61\2\2\u00eaV\3\2\2\2\u00eb\u00ec\7<\2\2\u00ecX\3\2\2\2\u00ed\u00ee\5"+
		")\25\2\u00ee\u00ef\5%\23\2\u00ef\u00f0\5+\26\2\u00f0\u00f1\5\13\6\2\u00f1"+
		"Z\3\2\2\2\u00f2\u00f3\5\r\7\2\u00f3\u00f4\5\3\2\2\u00f4\u00f5\5\31\r\2"+
		"\u00f5\u00f6\5\'\24\2\u00f6\u00f7\5\13\6\2\u00f7\\\3\2\2\2\u00f8\u00fa"+
		"\5\67\34\2\u00f9\u00f8\3\2\2\2\u00fa\u00fb\3\2\2\2\u00fb\u00f9\3\2\2\2"+
		"\u00fb\u00fc\3\2\2\2\u00fc\u0103\3\2\2\2\u00fd\u00ff\t\37\2\2\u00fe\u0100"+
		"\5\67\34\2\u00ff\u00fe\3\2\2\2\u0100\u0101\3\2\2\2\u0101\u00ff\3\2\2\2"+
		"\u0101\u0102\3\2\2\2\u0102\u0104\3\2\2\2\u0103\u00fd\3\2\2\2\u0103\u0104"+
		"\3\2\2\2\u0104^\3\2\2\2\u0105\u0109\5\63\32\2\u0106\u0109\5\65\33\2\u0107"+
		"\u0109\7a\2\2\u0108\u0105\3\2\2\2\u0108\u0106\3\2\2\2\u0108\u0107\3\2"+
		"\2\2\u0109\u010a\3\2\2\2\u010a\u0108\3\2\2\2\u010a\u010b\3\2\2\2\u010b"+
		"`\3\2\2\2\u010c\u010d\t \2\2\u010d\u010e\3\2\2\2\u010e\u010f\b\61\2\2"+
		"\u010fb\3\2\2\2\u0110\u0112\7\17\2\2\u0111\u0110\3\2\2\2\u0111\u0112\3"+
		"\2\2\2\u0112\u0113\3\2\2\2\u0113\u0116\7\f\2\2\u0114\u0116\7\17\2\2\u0115"+
		"\u0111\3\2\2\2\u0115\u0114\3\2\2\2\u0116\u0117\3\2\2\2\u0117\u0115\3\2"+
		"\2\2\u0117\u0118\3\2\2\2\u0118d\3\2\2\2\u0119\u011d\t!\2\2\u011a\u011c"+
		"\13\2\2\2\u011b\u011a\3\2\2\2\u011c\u011f\3\2\2\2\u011d\u011e\3\2\2\2"+
		"\u011d\u011b\3\2\2\2\u011e\u0120\3\2\2\2\u011f\u011d\3\2\2\2\u0120\u0121"+
		"\t\"\2\2\u0121f\3\2\2\2\f\2\u00fb\u0101\u0103\u0108\u010a\u0111\u0115"+
		"\u0117\u011d\3\b\2\2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}