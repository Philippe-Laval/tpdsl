grammar Cymbol;

compilationUnit
    :   (methodDeclaration | varDeclaration)+
    ;

// START: method
methodDeclaration
    :   type ID '(' formalParameters? ')' block
    ;
// END: method

formalParameters
    :   type ID (',' type ID)*
    ;

type:   'float'
    |   'int'
    |	'void'
    ;

// START: block
block
    :   '{' statement* '}'
    ;
// END: block

// START: var
varDeclaration
    :   type ID ('=' expression)? ';'
    ;
// END: var

statement
    :   block
    |	varDeclaration
    |   'return' expression? ';'
    |   postfixExpression // handles function calls like f(i);
        (   '=' expression
        |
        )
        ';'       
    ;

expressionList
    :   expression (',' expression)*
    |   
    ;

expression
    :   addExpression
    ;
    
addExpression
	:	postfixExpression ('+' postfixExpression)*
	;

// START: call
postfixExpression
    :   primary ( '(' expressionList ')' )*
    ;
// END: call

primary
    :   ID
    |   INT
    |   '(' expression ')'
    ;

// LEXER RULES

ID  :   LETTER (LETTER | '0'..'9')*
    ;

fragment
LETTER  :   ('a'..'z' | 'A'..'Z')
    ;

INT :   '0'..'9'+
    ;

WS  :   (' '|'\r'|'\t'|'\n') -> channel(HIDDEN)
    ;

SL_COMMENT
    :   '//' ~('\r'|'\n')* '\r'? '\n' -> channel(HIDDEN)
    ;
