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
    :   formalParameter (',' formalParameter)*
    ;

formalParameter
    :   type ID
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
    :   type ID ('=' expr)? ';'
    ;
// END: var

statement
    :   block
    |	varDeclaration
    |   'return' expr? ';'
    |   expr '=' expr ';' // assignment
    |   expr ';'          // func call      
    ;

expr:   ID '(' exprList? ')'    # Call
    |   expr '[' expr ']'       # Index
    |   '-' expr                # Negate
    |   '!' expr                # Not
    |   expr '*' expr           # Mult
    |   expr ('+'|'-') expr     # AddSub
    |   expr '==' expr          # Equal
    |   ID                      # Var
    |   INT                     # Int
    |   '(' expr ')'            # Parens
    ;

exprList : expr (',' expr)* ;   // arg list

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
