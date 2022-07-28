// START: header
grammar Fig;

@header {
using System.Reflection;
}
// END: header

// START: members
@parser::members {
    /** Map object names to their instances */
    public Dictionary<string,object> Instances = new Dictionary<string,object>();

    // Define the functionality required by the parser
        protected virtual void SetObjectProperty(object o, string propertyName, object value) {;}
	    protected virtual object NewInstance(string name) { return null; }
}
// END: members


// START: file
file returns [Dictionary<string,object> Instances]
    :   object+  {$Instances = this.Instances;} // return instances table
    ;
// END: file

// START: object
object returns [object o] // return object that we create
    :   type v=ID
        {
            $o = NewInstance($type.text); // simulate "new type()"
            Instances.Add($v.text, $o); // track in instances dictionary
        }
        '{' property[$o]* '}'       // match properties
    ;
// END: object

type:   ID ('.' ID)* ; // qualified ID like java.util.List
    
// START: property
property[Object o]     // accept instance parameter from object rule
    :   ID '=' expr ';'
        // set o's ID property with expr result
        {SetObjectProperty(o,$ID.text,$expr.value);}
    ;
// END: property

// START: expr
expr returns [object value]
    :   STRING  {string s = $STRING.text; // get string value
                 $value = s.Substring(1, s.Length-1);} // strip quotes
    |   INT     {$value = int.Parse($INT.text);}
    |   '$' ID  {$value = Instances[$ID.text];} // object reference
    |   list    {$value = $list.elements;} // return list's return value
    ;
// END: expr

// START: list
list returns [List<object> elements]
@init {
    $elements = new List<object>(); // init return value
}
    :   '[' e=expr {$elements.Add($e.value);}
            (',' e=expr {$elements.Add($e.value);})*
        ']'
    |   '[' ']' // empty list
    ;
// END: list

INT :   '0'..'9'+ ;
ID  :   ('_'|'a'..'z'|'A'..'Z') ('_'|'a'..'z'|'A'..'Z'|'0'..'9')* ;
STRING : '"' .*? '"' ;
WS  :   (' '|'\n'|'\t')+   -> skip ;
SLCMT:  '//' .* '\r'? '\n' -> skip ;
CMT :   '/*' .* '*/'       -> skip ;
