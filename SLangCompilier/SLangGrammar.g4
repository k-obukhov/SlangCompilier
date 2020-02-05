/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

grammar SLangGrammar;

start: (moduleImport)* module;
moduleImport: Import Id;
module: Module Id moduleDeclareSeq (moduleStatementsSeq)?;
moduleStatementsSeq: Start statementSeq End;
moduleDeclareSeq: (functionDecl | procedureDecl | typeDecl | moduleFieldDecl)*;

declare: variableDecl | constDecl;
variableDecl: (simpleDecl | arrayDecl | ptrDecl) (Assign exp)?;
simpleDecl: Variable (simpleType | customType) Id ;
arrayDecl: arrayDeclType Id ;
arrayDeclType: Array (LSBrace exp RSBrace)+ scalarType;
ptrDecl: ptrType Id ;
constDecl: Const typeName Id Assign exp;
moduleFieldDecl: AccessModifier (Readonly)? variableDecl Semicolon;

typeName: scalarType | arrayType;
ptrType: Pointer (LBrace customType RBrace)?; // id -- тип
customType: qualident;
scalarType: simpleType | customType | ptrType;
simpleType : SimpleType; // Встроенные типы 
SimpleType: Real | Integer | Boolean | Character | String;
arrayType: Array (arrayDimention)+ scalarType;
arrayDimention : LSBrace RSBrace;

typeDecl: AccessModifier (Base)? Class Id (typeInherit)? (typeFieldDecl)* End;
typeInherit: Inherit LBrace customType RBrace;
typeFieldDecl: AccessModifier variableDecl Semicolon;

functionDecl: (importHead)? AccessModifier (Abstract | Override)? (thisHeader)? Function LBrace routineArgList RBrace Colon typeName Id statementSeq End;
thisHeader: LBrace customType Id RBrace;
routineArgList: (routineArg (Comma routineArg)* | /* нет аргументов */ );
routineArg: FunctionArgModifier Id;
procedureDecl: (importHead)? AccessModifier (Abstract | Override)? (thisHeader)? Procedure LBrace routineArgList RBrace Id statementSeq End;
importHead: LSBrace File StringLiteral Import StringLiteral RSBrace;

statementSeq: (statement)*;
statement: (simpleStatement | complexStatement);
simpleStatement: (declare | let | input | output | returnC | call) Semicolon;
complexStatement: ifC | whileC | repeatC;

let: Let designator Assign exp;
ifC: If LBrace exp RBrace Then statementSeq (Elseif LBrace exp RBrace Then statementSeq)* (Else statementSeq)? End;
whileC: While LBrace exp RBrace Do statementSeq End;
repeatC: Repeat statementSeq While LBrace exp RBrace;
input: Input designator (Comma designator)*;
output: Output exp (Comma exp)*;
returnC: Return (exp)?;
call: Call designator;
exprList: (exp (Comma exp)* | /* nothing*/);

exp: simpleExpr (Relation exp)?; 

simpleExpr: term (AddictiveOp simpleExpr)?;

term: signedFactor (MultiplicativeOp term)?;
signedFactor: (AddOp | SubOp)? factor;

factor:  designator | ( IntValue | RealValue | BoolValue | StringLiteral | SingleCharacter ) | (BoolNot factor) | newC | (LBrace exp RBrace);
newC: New LBrace customType RBrace;
designator: Id (designatorStatement)*;
designatorStatement: (Point Id | LSBrace exp RSBrace | LBrace exprList RBrace);
qualident: (Id Point)? Id;

// Арифметика и булевы токены
AddOp: '+';
SubOp: '-';
MulOp: '*';
DivOp: '/';
ModOp: '%';

BoolOr: '||';
BoolAnd: '&&';

AddictiveOp: AddOp | SubOp | BoolOr;
MultiplicativeOp: MulOp | DivOp | BoolAnd;

BoolEq: '==';
BoolNeq: '!=';
BoolG: '>';
BoolL: '<';
BoolGeq: '>=';
BoolLeq: '<=';
BoolNot: '!';

Relation: BoolEq | BoolNeq | BoolG | BoolL | BoolLeq | BoolGeq;

// Базовые символы и имена
Colon: ':';
Semicolon: ';';
Comma: ',';
Point: '.';

Variable: 'variable';
Const: 'const';
Let: 'let';
Return: 'return';
Input: 'input';
Output: 'output';
Call: 'call';
Readonly: 'readonly';

If: 'if';
Then: 'then';
Else: 'else';
While: 'while';
Repeat: 'repeat';
Elseif: 'elseif';
Do: 'do';

Module: 'module';
Import: 'import';

Start: 'start';
End: 'end';

Function: 'function';
Procedure: 'procedure';

LBrace: '(';
RBrace: ')';

LSBrace: '[';
RSBrace: ']';

Assign: ':='; // Присваивание

// ТИПЫ
fragment Integer: 'integer'; // | 'int'
fragment Real: 'real';
fragment Character: 'character'; // | 'char'
fragment Boolean: 'boolean'; // | 'bool'
fragment String: 'string';

// Токены для указателей
New: 'new'; // выделение памяти
Nil: 'nil'; // значение null
Pointer: 'pointer'; // указатель

Array: 'array'; // Массивный тип

FunctionArgModifier : ArgValModifier | ArgRefModifier ; // Передача аргументов в функцию по значению и ссылке
fragment ArgValModifier: 'val';
fragment ArgRefModifier: 'ref';

AccessModifier: PublicModifier | PrivateModifier; // Модификаторы доступа
fragment PublicModifier: 'public';
fragment PrivateModifier: 'private';

Class: 'class';

Inherit: 'inherit'; // Класс наследует что-либо
Base: 'base'; // От класса можно наследоваться 

Abstract: 'abstract';
Override: 'override'; // Метод переопределяется

File: 'file';

fragment Digit: [0-9]; // цифра

IntValue: Digit+; // Целое число
RealValue: Digit*Point?Digit+([eE][-+]?Digit+)?; // Вещественное значение
BoolValue: 'true' | 'false'; // Булево значение

Id: [_a-zA-Z][_a-zA-Z0-9]*; // Идентификатор

StringLiteral:	'"' StringCharacter* '"'; // Строковый литерал
SingleCharacter: '\'' StringCharacter '\'';
fragment StringCharacter: ~["] | EscapeSequence; // Символ
fragment EscapeSequence : '\\' [btnfr"'\\]; // escape-символы типа \n \t ... либо группа 1 символа

Comment: ('//' ~[\r\n]* | '/*' .*? '*/') -> skip;
Ws: [ \t\r\n] -> skip;