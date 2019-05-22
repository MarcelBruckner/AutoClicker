grammar AutoClicker;

/*
 * Lexer Rules
 */

fragment A			: [aA]; // match either an 'a' or 'A'
fragment B			: [bB];
fragment C			: [cC];
fragment D			: [dD];
fragment E			: [eE];
fragment F			: [fF];
fragment G			: [gG];
fragment H			: [hH];
fragment I			: [iI];
fragment J			: [jJ];
fragment K			: [kK];
fragment L			: [lL];
fragment M			: [mM];
fragment N			: [nN];
fragment O			: [oO];
fragment P			: [pP];
fragment Q			: [qQ];
fragment R			: [rR];
fragment S			: [sS];
fragment T			: [tT];
fragment U			: [uU];
fragment V			: [vV];
fragment W			: [wW];
fragment Z			: [zZ];

fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
fragment DIGIT      : [0-9] ;


X                   : ('X'|'x') ;
Y                   : ('Y'|'y') ;
CLICK               : C L I C K;
BUTTON              : B U T T O N;
LEFT                : L E F T;
RIGHT               : R I G H T;
MIDDLE              : M I D D L E;
MOVEMENT			: M O V E M E N T;
SINUS				: S I N U S;
SPRING				: S P R I N G;
JUMP				: J U M P;

DELAY				: D E L A Y;
REPETITIONS			: R E P E T I T I O N S;
SPEED				: S P E E D;
SHIFT				: S H I F T;
CTRL				: C T R L;
ALT					: A L T;

EQ					: '=';
SLASH				: '/';
IS					: ':';
TRUE				: T R U E;
FALSE				: F A L S E;
NUMBER				: DIGIT+;
DECIMAL             : NUMBER ([.,] NUMBER)? ;
WORD                : (LOWERCASE | UPPERCASE | '_')+ ;
WHITESPACE          : [ \t\r\n] -> skip;
NEWLINE             : ('\r'? '\n' | '\r')+ ;
TEXT                : ('['|'(') .*? (']'|')');

/*
 * Parser Rules
 */

intTuple			: EQ NUMBER (SLASH NUMBER)?;
doubleTuple			: EQ DECIMAL (SLASH DECIMAL)?;
trueFalse			: EQ (TRUE | FALSE);

instructions        : (instruction? NEWLINE)*;
instruction         : click ;

commons				: (delay | repetitions | speed | shift | ctrl | alt);
delay				: (DELAY intTuple);
repetitions			: (REPETITIONS intTuple);
speed				: (SPEED doubleTuple);
shift				: (SHIFT trueFalse);
ctrl				: (CTRL trueFalse);
alt					: (ALT trueFalse);

/* click: x=3/5 y=5/7 button=left */
click               : CLICK IS (xPos | yPos | button | movement | commons)*; 
xPos				: (X intTuple);
yPos				: (Y intTuple);
button              : (BUTTON EQ (LEFT | RIGHT | MIDDLE));
movement			: (MOVEMENT EQ (SINUS | SPRING | JUMP));
