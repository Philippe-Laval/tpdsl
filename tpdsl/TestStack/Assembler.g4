/** A generic bytecode assembler whose instructions take 0..3 operands.
 *  Instruction set is dictated externally with a String[].  Implement
 *  specifics by subclassing and defining gen() methods. Comments start
 *  with ';' and all instructions end with '\n'.  Handles both register (rN)
 *  and stack-based assembly instructions.  Labels are "ID:".  "main:" label
 *  is where we start execution.  Use .globals and .def for global data
 *  and function definitions, respectively.
 */
grammar Assembler;

// START: members
@parser::members {
    // Define the functionality required by the parser for code generation
    protected virtual void Gen(IToken instrToken) {;}
    protected virtual void Gen(IToken instrToken, IToken operandToken) {;}
    protected virtual void Gen(IToken instrToken, IToken oToken1, IToken oToken2) {;}
    protected virtual void Gen(IToken instrToken, IToken oToken1, IToken oToken2, IToken oToken3) {;}
    protected virtual void CheckForUnresolvedReferences() {;}
    protected virtual void DefineFunction(IToken idToken, int nargs, int nlocals) {;}
    protected virtual void DefineDataSize(int n) {;}
    protected virtual void DefineLabel(IToken idToken) {;}
}
// END: members

program
    :   globals?
        ( functionDeclaration | instr | label | NEWLINE )+
        {CheckForUnresolvedReferences();}
    ;
   
// how much data space
// START: data
globals : NEWLINE* '.globals' INT NEWLINE {DefineDataSize($INT.int);} ;
// END: data

//  .def fact: args=1, locals=0
// START: func
functionDeclaration
    :   '.def' name=ID ':' 'args' '=' a=INT ',' 'locals' '=' lo=INT NEWLINE
        {DefineFunction($name, $a.int, $lo.int);}
    ;
// END: func

// START: instr
instr
    :   ID NEWLINE                         {Gen($ID);}
    |   ID operand NEWLINE                 {Gen($ID,$operand.start);}
    |   ID a=operand ',' b=operand NEWLINE {Gen($ID,$a.start,$b.start);}
    |   ID a=operand ',' b=operand ',' c=operand NEWLINE
        {Gen($ID,$a.start,$b.start,$c.start);}
    ;
// END: instr

// START: operand
operand
    :   ID   // basic code label; E.g., "loop"
    |   REG  // register name; E.g., "r0"
    |   FUNC // function label; E.g., "f()"
    |   INT
// ...
// END: operand
    |   CHAR
    |   STRING
    |   FLOAT
    ;

label
    :   ID ':' {DefineLabel($ID);}
    ;

REG :   'r' INT ;

ID  :   LETTER (LETTER | '0'..'9')* ;

FUNC:   ID '()' {Text = Text.Substring(0, Text.Length-2);} ;

fragment
LETTER
    :   ('a'..'z' | 'A'..'Z')
    ;
    
INT :   '-'? '0'..'9'+ ;

CHAR:   '\'' . '\'' ;

STRING: '"' STR_CHARS '"' {Text = Text.Substring(1, Text.Length-2);} ;

fragment STR_CHARS : ~'"'* ;

FLOAT
    :   INT '.' INT*
    |   '.' INT+
    ;

WS  :   (' '|'\t')+ -> skip ;

NEWLINE
    :   (';' .*)? '\r'? '\n'  // optional comment followed by newline
    ;
