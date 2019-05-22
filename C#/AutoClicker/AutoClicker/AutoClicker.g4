grammar AutoClicker;

/*
 * Lexer Rules
 */

fragment C          : ('C'|'c') ;
fragment L          : ('L'|'l') ;
fragment I          : ('I'|'i') ;
fragment K          : ('K'|'k') ;

fragment B          : ('B'|'b') ;
fragment U          : ('U'|'u') ;
fragment T          : ('T'|'t') ;
fragment O          : ('O'|'o') ;
fragment N          : ('N'|'n') ;

fragment E          : ('E'|'e') ;
fragment F          : ('F'|'f') ;

fragment R          : ('R'|'r') ;
fragment G          : ('G'|'g') ;
fragment H          : ('H'|'h') ;

fragment M          : ('M'|'m') ;
fragment D          : ('D'|'d') ;

fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
fragment DIGIT      : [0-9] ;


CLICK               : C L I C K;
X                   : ('X'|'x') ;
Y                   : ('Y'|'y') ;
BUTTON              : B U T T O N;
LEFT                : L E F T;
RIGHT               : R I G H T;
MIDDLE              : M I D D L E;

NUMBER              : DIGIT+ ([.,] DIGIT+)? ;
WORD                : (LOWERCASE | UPPERCASE | '_')+ ;
WHITESPACE          : [ \t\r\n] -> skip;
NEWLINE             : ('\r'? '\n' | '\r')+ ;
TEXT                : ('['|'(') .*? (']'|')');

/*
 * Parser Rules
 */

instructions        : (instruction NEWLINE)*;
instruction         : click ;

/* click: x=3 y=5 button=left */
click               : CLICK ':' (xPos | yPos | button)*; 
xPos				: (X '=' NUMBER);
yPos				: (Y '=' NUMBER);
button              : (BUTTON '=' (LEFT | RIGHT | MIDDLE));
