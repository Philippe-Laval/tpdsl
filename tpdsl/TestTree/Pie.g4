/** A simple dynamically-typed language that smacks of Python.
 *  This builds a tree, then we'll interpret it with a tree grammar
 *  Build a convential symbol table while parsing.  Save
 *  symbol ptrs in AST nodes.
 */
grammar Pie;

@parser::members {
    Interpreter interp;
    IScope currentScope;
    public PieParser(ITokenStream input, Interpreter interp) : this(input) {
        this.interp = interp;
        currentScope = interp.GlobalScope;
    }
}

program
	:	( functionDefinition | statement )+ EOF 
	;
	
structDefinition
    :   'struct' name=ID '{' vardef (',' vardef)* '}' NL
    ;

functionDefinition
	:	'def' ID '(' (vardef (',' vardef)* )? ')' slist
	;

slist
	:	':' NL statement+ '.' NL
	|	statement
	;

statement
	:	structDefinition						#structDefinitionStatement
	|	qid '=' expr NL							#assignementStatement
	|	'return' expr NL						#returnStatement
	|	'print' expr NL							#printStatement
	|	'if' expr c=slist ('else' el=slist)?	#ifStatement
	|	'while' expr slist						#whileStatement
	|	call NL									#callStatement
	|	NL										#nlStatement
	;

call
	:	name=ID '(' (expr (',' expr )*)? ')'
	;

expr
	:	addexpr (compOp addexpr)? 
	;

addexpr
	:	mulexpr (addOp mulexpr)*
	;

mulexpr 
	:	atom (multOp atom)*
	;

atom 
	:	INT					#intAtom
	|	CHAR				#charAtom
	|	FLOAT				#floatAtom
	|	STRING				#stringAtom
	|	qid					#qidAtom
	|	call				#callAtom
	|	instance			#instanceAtom
	|	'(' expr ')'		#parenthesizedAtom
	;

instance
	:	'new' sname=ID
	;

qid 
	:	ID ('.' ID)* 
	;  // CAN'T RESOLVE UNTIL RUNTIME!

vardef
	:	ID	
	;

multOp	: MUL | DIV ;
addOp	: ADD | SUB ;
compOp	: EQ | LT ;

// L e x i c a l  R u l e s

IF		: 'if' ;
ASSIGN	: '='  ;
PRINT	: 'print' ;
WHILE	: 'while'	;
RETURN	: 'return'	;
DEF		: 'def' ;
ADD		: '+' ;
SUB		: '-' ;
MUL		: '*' ;
DIV		: '/' ;
EQ		: '==' ;
LT		: '<' ;
STRUCT	: 'struct' ;
DOT		: '.' ;
NEW		: 'new' ;


NL	:	'\r'? '\n' ;

ID  :   LETTER (LETTER | '0'..'9')*  ;

fragment
LETTER
	:   ('a'..'z' | 'A'..'Z')
    ;

CHAR:	'\'' . '\'' ;

STRING:	'"' .*? '"' ;

INT :   '0'..'9'+ ;
    
FLOAT
	:	INT '.' INT*
	|	'.' INT+
	;

WS  :   (' '|'\t') -> channel(HIDDEN)
	;

SL_COMMENT
    :   '#' ~('\r'|'\n')* -> channel(HIDDEN)
    ;