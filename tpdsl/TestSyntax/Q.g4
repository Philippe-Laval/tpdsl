grammar Q;

@parser::members {
    Interpreter interp;
    public QParser(ITokenStream input, Interpreter interp) 
        : this(input)
    {
        this.interp = interp;
    }
}

program
    :   stat+
    ;

stat:   table
    |   insert
    |   assign
    |   query
    |   print
    ;

print:  'print' expr ';' {interp.Print($expr.value);}
    ;

// START: table
table
    :   'create' 'table' tbl=ID
        '(' 'primary' 'key' key=ID (',' columns+=ID)+ ')' ';'
        {interp.CreateTable($tbl.text, $key.text, $columns);}
    ;
// END: table

// START: assign
assign : ID '=' expr ';'  {interp.Store($ID.text, $expr.value);} ;
// END: assign

// START: insert
insert
    : 'insert' 'into' ID 'set' setFields[interp.Tables[$ID.text]] ';'
      {interp.InsertInto($ID.text, $setFields.row);}
    ;
// END: insert
    
// START: fields
setFields[Table t] returns [Row row]
@init { $row = new Row(t.Columns); } // set return value
    :   set[$row] (',' set[$row])*
    ;
set[Row row] // pass in Row we're filling in
    :   ID '=' expr {row.Set($ID.text, $expr.value);}
    ;
// END: fields
    
query returns [object value]
    :   'select' columns+=ID (',' columns+=ID)* 'from' tbl=ID
        (   'where' key=ID '=' expr
            {$value = interp.Select($tbl.text, $columns, $key.text, $expr.value);}
        |   {$value = interp.Select($tbl.text, $columns);}
        )
    ;

// START: expr
// Match a simple value or do a query
expr returns [object value] // access as $expr.value in other rules
    :   ID      {$value = interp.Load($ID.text);}
    |   INT     {$value = $INT.int;}
    |   STRING  {$value = $STRING.text;}
    |   query   {$value = $query.value;}
    ;
// END: expr

ID  :   ('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|'0'..'9'|'_')* ;

INT :   '0'..'9'+ ;

STRING
    :   '\'' ~'\''* '\''
        {Text = Text.Substring(1, Text.Length-2);}
    ;

COMMENT
    :   '//' ~('\n'|'\r')* '\r'? '\n' -> channel(HIDDEN)
    ;

WS  :   ( ' '
        | '\t'
        | '\r'
        | '\n'
        ) -> channel(HIDDEN)
    ;
