// START: header
grammar Cymbol; // my grammar is called Cymbol

// define a SymbolTable field in generated parser
@parser::members {SymbolTable symtab;}

compilationUnit[SymbolTable symtab] // pass symbol table to start rule
@init {this.symtab = symtab;}       // set the parser's field
    :   varDeclaration+ // recognize at least one variable declaration
    ;
// END: header

// START: type   
type returns [IType tsym]
@after { // $start is the first tree node matched by this rule
    Console.WriteLine($"line {$start.Line}: ref {$tsym.Name}");
}
    :   'float' {$tsym = (IType)symtab.Resolve("float");}
    |   'int'   {$tsym = (IType)symtab.Resolve("int");}
    ;
// END: type   

// START: decl
varDeclaration
    :   type ID ('=' expression)? ';' // E.g., "int i = 2;", "int i;"
    	{
        Console.WriteLine($"line {$ID.line}: def {$ID.text}");
    	VariableSymbol vs = new VariableSymbol($ID.text,$type.tsym);
    	symtab.Define(vs);
    	}
    ;
// END: decl

expression
    :   primary ('+' primary)*
    ;

// START: primary
primary
    :   ID // reference variable in an expression
    	{Console.WriteLine($"line {$ID.line}: ref to {symtab.Resolve($ID.text)}");}
    |   INT
    |   '(' expression ')'
    ;
// END: primary

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
