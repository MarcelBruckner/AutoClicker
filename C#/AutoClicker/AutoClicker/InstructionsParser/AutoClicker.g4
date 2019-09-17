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
ENDX				: E N D X;
ENDY				: E N D Y;
HOVER				: H O V E R;
CLICK               : C L I C K;
DRAG				: D R A G;
TEXT				: T E X T;
INPUT				: I N P U T;
BUTTON              : B U T T O N;
LEFT                : L E F T;
RIGHT               : R I G H T;
MIDDLE              : M I D D L E;
MOVEMENT			: M O V E M E N T;
SINUS				: S I N U S;
SPRING				: S P R I N G;
JUMP				: J U M P;
WHEEL				: W H E E L;
SCROLL				: S C R O L L;

KEYSTROKE			: K E Y S T R O K E;
KEY					: K E Y;

WAIT				: W A I T;

DELAY				: D E L A Y;
REPETITIONS			: R E P E T I T I O N S;
SPEED				: S P E E D;
SHIFT				: S H I F T;
CTRL				: C T R L;
ALT					: A L T;

EQ					: '=';
SLASH				: '/';
IS					: ':';
QUOTE				: '"';
TRUE				: T R U E;
FALSE				: F A L S E;
DECIMAL             : ('-' | '+')? DIGIT+ ([.,] DIGIT+)? ;
// WORD                : (LOWERCASE | UPPERCASE | '_')+ ;
WORD				: (LOWERCASE | UPPERCASE | '_')+;
WHITESPACE          : [ \t\r\n] -> skip;
NEWLINE             : ('\r'? '\n' | '\r')+ ;
STRING				: '"' ~('\r' | '\n' | '"')* '"' ;

/*
 * Parser Rules
 */
decimalTuple		: EQ DECIMAL (SLASH DECIMAL)?;
trueFalse			: EQ (TRUE | FALSE);

instructions        : (instruction? NEWLINE)*;
instruction         : click | hover | drag | keystroke | text | wheel | wait;

commons				: (delay | repetitions | speed | shift | ctrl | alt);
delay				: (DELAY decimalTuple);
repetitions			: (REPETITIONS decimalTuple);
speed				: (SPEED decimalTuple);
shift				: (SHIFT trueFalse);
ctrl				: (CTRL trueFalse);
alt					: (ALT trueFalse);
scroll				: (SCROLL decimalTuple);

hover				: HOVER IS (xPos | yPos | movement | commons)*;
click               : CLICK IS (button | xPos | yPos | movement | commons)*; 
drag				: DRAG IS (endX | endY | button | xPos | yPos | movement | commons)*;
wheel				: WHEEL IS (scroll | xPos | yPos | movement | commons)*;

xPos				: (X decimalTuple);
yPos				: (Y decimalTuple);
endX				: (ENDX decimalTuple);
endY				: (ENDY decimalTuple);
button              : (BUTTON EQ (LEFT | RIGHT | MIDDLE));
movement			: (MOVEMENT EQ (SINUS | SPRING | JUMP));

stringInput			: (INPUT EQ STRING);
keystroke			: KEY IS (stringInput | commons)*;
text				: TEXT IS (stringInput | commons)*;

wait				: WAIT IS (commons)*;