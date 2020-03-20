//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.5
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from SLangGrammar.g4 by ANTLR 4.6.5

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace SLangGrammar {
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.5")]
[System.CLSCompliant(false)]
public partial class SLangGrammarLexer : Lexer {
	public const int
		SimpleType=1, AddOp=2, SubOp=3, MulOp=4, DivOp=5, ModOp=6, BoolOr=7, BoolAnd=8, 
		BoolEq=9, BoolNeq=10, BoolG=11, BoolL=12, BoolGeq=13, BoolLeq=14, BoolNot=15, 
		Colon=16, Semicolon=17, Comma=18, Point=19, Variable=20, Const=21, Let=22, 
		Return=23, Input=24, Output=25, Call=26, Readonly=27, If=28, Then=29, 
		Else=30, While=31, Repeat=32, Elseif=33, Do=34, Module=35, Import=36, 
		Start=37, End=38, Function=39, Procedure=40, LBrace=41, RBrace=42, LSBrace=43, 
		RSBrace=44, Assign=45, New=46, Nil=47, Pointer=48, Array=49, FunctionArgModifier=50, 
		AccessModifier=51, Class=52, Empty=53, Inherit=54, Base=55, Abstract=56, 
		Override=57, File=58, IntValue=59, RealValue=60, BoolValue=61, Id=62, 
		StringLiteral=63, SingleCharacter=64, Comment=65, Ws=66;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"SimpleType", "AddOp", "SubOp", "MulOp", "DivOp", "ModOp", "BoolOr", "BoolAnd", 
		"BoolEq", "BoolNeq", "BoolG", "BoolL", "BoolGeq", "BoolLeq", "BoolNot", 
		"Colon", "Semicolon", "Comma", "Point", "Variable", "Const", "Let", "Return", 
		"Input", "Output", "Call", "Readonly", "If", "Then", "Else", "While", 
		"Repeat", "Elseif", "Do", "Module", "Import", "Start", "End", "Function", 
		"Procedure", "LBrace", "RBrace", "LSBrace", "RSBrace", "Assign", "Integer", 
		"Real", "Character", "Boolean", "String", "New", "Nil", "Pointer", "Array", 
		"FunctionArgModifier", "ArgValModifier", "ArgRefModifier", "AccessModifier", 
		"PublicModifier", "PrivateModifier", "Class", "Empty", "Inherit", "Base", 
		"Abstract", "Override", "File", "Digit", "IntValue", "RealValue", "BoolValue", 
		"Id", "StringLiteral", "SingleCharacter", "StringCharacter", "EscapeSequence", 
		"Comment", "Ws"
	};


	public SLangGrammarLexer(ICharStream input)
		: base(input)
	{
		_interp = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
		null, null, "'+'", "'-'", "'*'", "'/'", "'%'", "'||'", "'&&'", "'=='", 
		"'!='", "'>'", "'<'", "'>='", "'<='", "'!'", "':'", "';'", "','", "'.'", 
		"'variable'", "'const'", "'let'", "'return'", "'input'", "'output'", "'call'", 
		"'readonly'", "'if'", "'then'", "'else'", "'while'", "'repeat'", "'elseif'", 
		"'do'", "'module'", "'import'", "'start'", "'end'", "'function'", "'procedure'", 
		"'('", "')'", "'['", "']'", "':='", "'new'", "'nil'", "'pointer'", "'array'", 
		null, null, "'class'", "'empty'", "'inherit'", "'base'", "'abstract'", 
		"'override'", "'file'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "SimpleType", "AddOp", "SubOp", "MulOp", "DivOp", "ModOp", "BoolOr", 
		"BoolAnd", "BoolEq", "BoolNeq", "BoolG", "BoolL", "BoolGeq", "BoolLeq", 
		"BoolNot", "Colon", "Semicolon", "Comma", "Point", "Variable", "Const", 
		"Let", "Return", "Input", "Output", "Call", "Readonly", "If", "Then", 
		"Else", "While", "Repeat", "Elseif", "Do", "Module", "Import", "Start", 
		"End", "Function", "Procedure", "LBrace", "RBrace", "LSBrace", "RSBrace", 
		"Assign", "New", "Nil", "Pointer", "Array", "FunctionArgModifier", "AccessModifier", 
		"Class", "Empty", "Inherit", "Base", "Abstract", "Override", "File", "IntValue", 
		"RealValue", "BoolValue", "Id", "StringLiteral", "SingleCharacter", "Comment", 
		"Ws"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[System.Obsolete("Use Vocabulary instead.")]
	public static readonly string[] tokenNames = GenerateTokenNames(DefaultVocabulary, _SymbolicNames.Length);

	private static string[] GenerateTokenNames(IVocabulary vocabulary, int length) {
		string[] tokenNames = new string[length];
		for (int i = 0; i < tokenNames.Length; i++) {
			tokenNames[i] = vocabulary.GetLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = vocabulary.GetSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}

		return tokenNames;
	}

	[System.Obsolete("Use IRecognizer.Vocabulary instead.")]
	public override string[] TokenNames
	{
		get
		{
			return tokenNames;
		}
	}

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "SLangGrammar.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return _serializedATN; } }

	public static readonly string _serializedATN =
		"\x3\xAF6F\x8320\x479D\xB75C\x4880\x1605\x191C\xAB37\x2\x44\x24C\b\x1\x4"+
		"\x2\t\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x4\b\t\b"+
		"\x4\t\t\t\x4\n\t\n\x4\v\t\v\x4\f\t\f\x4\r\t\r\x4\xE\t\xE\x4\xF\t\xF\x4"+
		"\x10\t\x10\x4\x11\t\x11\x4\x12\t\x12\x4\x13\t\x13\x4\x14\t\x14\x4\x15"+
		"\t\x15\x4\x16\t\x16\x4\x17\t\x17\x4\x18\t\x18\x4\x19\t\x19\x4\x1A\t\x1A"+
		"\x4\x1B\t\x1B\x4\x1C\t\x1C\x4\x1D\t\x1D\x4\x1E\t\x1E\x4\x1F\t\x1F\x4 "+
		"\t \x4!\t!\x4\"\t\"\x4#\t#\x4$\t$\x4%\t%\x4&\t&\x4\'\t\'\x4(\t(\x4)\t"+
		")\x4*\t*\x4+\t+\x4,\t,\x4-\t-\x4.\t.\x4/\t/\x4\x30\t\x30\x4\x31\t\x31"+
		"\x4\x32\t\x32\x4\x33\t\x33\x4\x34\t\x34\x4\x35\t\x35\x4\x36\t\x36\x4\x37"+
		"\t\x37\x4\x38\t\x38\x4\x39\t\x39\x4:\t:\x4;\t;\x4<\t<\x4=\t=\x4>\t>\x4"+
		"?\t?\x4@\t@\x4\x41\t\x41\x4\x42\t\x42\x4\x43\t\x43\x4\x44\t\x44\x4\x45"+
		"\t\x45\x4\x46\t\x46\x4G\tG\x4H\tH\x4I\tI\x4J\tJ\x4K\tK\x4L\tL\x4M\tM\x4"+
		"N\tN\x4O\tO\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x5\x2\xA5\n\x2\x3\x3\x3\x3\x3"+
		"\x4\x3\x4\x3\x5\x3\x5\x3\x6\x3\x6\x3\a\x3\a\x3\b\x3\b\x3\b\x3\t\x3\t\x3"+
		"\t\x3\n\x3\n\x3\n\x3\v\x3\v\x3\v\x3\f\x3\f\x3\r\x3\r\x3\xE\x3\xE\x3\xE"+
		"\x3\xF\x3\xF\x3\xF\x3\x10\x3\x10\x3\x11\x3\x11\x3\x12\x3\x12\x3\x13\x3"+
		"\x13\x3\x14\x3\x14\x3\x15\x3\x15\x3\x15\x3\x15\x3\x15\x3\x15\x3\x15\x3"+
		"\x15\x3\x15\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16\x3\x17\x3\x17\x3"+
		"\x17\x3\x17\x3\x18\x3\x18\x3\x18\x3\x18\x3\x18\x3\x18\x3\x18\x3\x19\x3"+
		"\x19\x3\x19\x3\x19\x3\x19\x3\x19\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3"+
		"\x1A\x3\x1A\x3\x1B\x3\x1B\x3\x1B\x3\x1B\x3\x1B\x3\x1C\x3\x1C\x3\x1C\x3"+
		"\x1C\x3\x1C\x3\x1C\x3\x1C\x3\x1C\x3\x1C\x3\x1D\x3\x1D\x3\x1D\x3\x1E\x3"+
		"\x1E\x3\x1E\x3\x1E\x3\x1E\x3\x1F\x3\x1F\x3\x1F\x3\x1F\x3\x1F\x3 \x3 \x3"+
		" \x3 \x3 \x3 \x3!\x3!\x3!\x3!\x3!\x3!\x3!\x3\"\x3\"\x3\"\x3\"\x3\"\x3"+
		"\"\x3\"\x3#\x3#\x3#\x3$\x3$\x3$\x3$\x3$\x3$\x3$\x3%\x3%\x3%\x3%\x3%\x3"+
		"%\x3%\x3&\x3&\x3&\x3&\x3&\x3&\x3\'\x3\'\x3\'\x3\'\x3(\x3(\x3(\x3(\x3("+
		"\x3(\x3(\x3(\x3(\x3)\x3)\x3)\x3)\x3)\x3)\x3)\x3)\x3)\x3)\x3*\x3*\x3+\x3"+
		"+\x3,\x3,\x3-\x3-\x3.\x3.\x3.\x3/\x3/\x3/\x3/\x3/\x3/\x3/\x3/\x3\x30\x3"+
		"\x30\x3\x30\x3\x30\x3\x30\x3\x31\x3\x31\x3\x31\x3\x31\x3\x31\x3\x31\x3"+
		"\x31\x3\x31\x3\x31\x3\x31\x3\x32\x3\x32\x3\x32\x3\x32\x3\x32\x3\x32\x3"+
		"\x32\x3\x32\x3\x33\x3\x33\x3\x33\x3\x33\x3\x33\x3\x33\x3\x33\x3\x34\x3"+
		"\x34\x3\x34\x3\x34\x3\x35\x3\x35\x3\x35\x3\x35\x3\x36\x3\x36\x3\x36\x3"+
		"\x36\x3\x36\x3\x36\x3\x36\x3\x36\x3\x37\x3\x37\x3\x37\x3\x37\x3\x37\x3"+
		"\x37\x3\x38\x3\x38\x5\x38\x19E\n\x38\x3\x39\x3\x39\x3\x39\x3\x39\x3:\x3"+
		":\x3:\x3:\x3;\x3;\x5;\x1AA\n;\x3<\x3<\x3<\x3<\x3<\x3<\x3<\x3=\x3=\x3="+
		"\x3=\x3=\x3=\x3=\x3=\x3>\x3>\x3>\x3>\x3>\x3>\x3?\x3?\x3?\x3?\x3?\x3?\x3"+
		"@\x3@\x3@\x3@\x3@\x3@\x3@\x3@\x3\x41\x3\x41\x3\x41\x3\x41\x3\x41\x3\x42"+
		"\x3\x42\x3\x42\x3\x42\x3\x42\x3\x42\x3\x42\x3\x42\x3\x42\x3\x43\x3\x43"+
		"\x3\x43\x3\x43\x3\x43\x3\x43\x3\x43\x3\x43\x3\x43\x3\x44\x3\x44\x3\x44"+
		"\x3\x44\x3\x44\x3\x45\x3\x45\x3\x46\x6\x46\x1EE\n\x46\r\x46\xE\x46\x1EF"+
		"\x3G\aG\x1F3\nG\fG\xEG\x1F6\vG\x3G\x5G\x1F9\nG\x3G\x6G\x1FC\nG\rG\xEG"+
		"\x1FD\x3G\x3G\x5G\x202\nG\x3G\x6G\x205\nG\rG\xEG\x206\x5G\x209\nG\x3H"+
		"\x3H\x3H\x3H\x3H\x3H\x3H\x3H\x3H\x5H\x214\nH\x3I\x3I\aI\x218\nI\fI\xE"+
		"I\x21B\vI\x3J\x3J\aJ\x21F\nJ\fJ\xEJ\x222\vJ\x3J\x3J\x3K\x3K\x3K\x3K\x3"+
		"L\x3L\x5L\x22C\nL\x3M\x3M\x3M\x3N\x3N\x3N\x3N\aN\x235\nN\fN\xEN\x238\v"+
		"N\x3N\x3N\x3N\x3N\aN\x23E\nN\fN\xEN\x241\vN\x3N\x3N\x5N\x245\nN\x3N\x3"+
		"N\x3O\x3O\x3O\x3O\x3\x23F\x2\x2P\x3\x2\x3\x5\x2\x4\a\x2\x5\t\x2\x6\v\x2"+
		"\a\r\x2\b\xF\x2\t\x11\x2\n\x13\x2\v\x15\x2\f\x17\x2\r\x19\x2\xE\x1B\x2"+
		"\xF\x1D\x2\x10\x1F\x2\x11!\x2\x12#\x2\x13%\x2\x14\'\x2\x15)\x2\x16+\x2"+
		"\x17-\x2\x18/\x2\x19\x31\x2\x1A\x33\x2\x1B\x35\x2\x1C\x37\x2\x1D\x39\x2"+
		"\x1E;\x2\x1F=\x2 ?\x2!\x41\x2\"\x43\x2#\x45\x2$G\x2%I\x2&K\x2\'M\x2(O"+
		"\x2)Q\x2*S\x2+U\x2,W\x2-Y\x2.[\x2/]\x2\x2_\x2\x2\x61\x2\x2\x63\x2\x2\x65"+
		"\x2\x2g\x2\x30i\x2\x31k\x2\x32m\x2\x33o\x2\x34q\x2\x2s\x2\x2u\x2\x35w"+
		"\x2\x2y\x2\x2{\x2\x36}\x2\x37\x7F\x2\x38\x81\x2\x39\x83\x2:\x85\x2;\x87"+
		"\x2<\x89\x2\x2\x8B\x2=\x8D\x2>\x8F\x2?\x91\x2@\x93\x2\x41\x95\x2\x42\x97"+
		"\x2\x2\x99\x2\x2\x9B\x2\x43\x9D\x2\x44\x3\x2\v\x3\x2\x32;\x4\x2GGgg\x4"+
		"\x2--//\x5\x2\x43\\\x61\x61\x63|\x6\x2\x32;\x43\\\x61\x61\x63|\x3\x2$"+
		"$\n\x2$$))^^\x64\x64hhppttvv\x4\x2\f\f\xF\xF\x5\x2\v\f\xF\xF\"\"\x253"+
		"\x2\x3\x3\x2\x2\x2\x2\x5\x3\x2\x2\x2\x2\a\x3\x2\x2\x2\x2\t\x3\x2\x2\x2"+
		"\x2\v\x3\x2\x2\x2\x2\r\x3\x2\x2\x2\x2\xF\x3\x2\x2\x2\x2\x11\x3\x2\x2\x2"+
		"\x2\x13\x3\x2\x2\x2\x2\x15\x3\x2\x2\x2\x2\x17\x3\x2\x2\x2\x2\x19\x3\x2"+
		"\x2\x2\x2\x1B\x3\x2\x2\x2\x2\x1D\x3\x2\x2\x2\x2\x1F\x3\x2\x2\x2\x2!\x3"+
		"\x2\x2\x2\x2#\x3\x2\x2\x2\x2%\x3\x2\x2\x2\x2\'\x3\x2\x2\x2\x2)\x3\x2\x2"+
		"\x2\x2+\x3\x2\x2\x2\x2-\x3\x2\x2\x2\x2/\x3\x2\x2\x2\x2\x31\x3\x2\x2\x2"+
		"\x2\x33\x3\x2\x2\x2\x2\x35\x3\x2\x2\x2\x2\x37\x3\x2\x2\x2\x2\x39\x3\x2"+
		"\x2\x2\x2;\x3\x2\x2\x2\x2=\x3\x2\x2\x2\x2?\x3\x2\x2\x2\x2\x41\x3\x2\x2"+
		"\x2\x2\x43\x3\x2\x2\x2\x2\x45\x3\x2\x2\x2\x2G\x3\x2\x2\x2\x2I\x3\x2\x2"+
		"\x2\x2K\x3\x2\x2\x2\x2M\x3\x2\x2\x2\x2O\x3\x2\x2\x2\x2Q\x3\x2\x2\x2\x2"+
		"S\x3\x2\x2\x2\x2U\x3\x2\x2\x2\x2W\x3\x2\x2\x2\x2Y\x3\x2\x2\x2\x2[\x3\x2"+
		"\x2\x2\x2g\x3\x2\x2\x2\x2i\x3\x2\x2\x2\x2k\x3\x2\x2\x2\x2m\x3\x2\x2\x2"+
		"\x2o\x3\x2\x2\x2\x2u\x3\x2\x2\x2\x2{\x3\x2\x2\x2\x2}\x3\x2\x2\x2\x2\x7F"+
		"\x3\x2\x2\x2\x2\x81\x3\x2\x2\x2\x2\x83\x3\x2\x2\x2\x2\x85\x3\x2\x2\x2"+
		"\x2\x87\x3\x2\x2\x2\x2\x8B\x3\x2\x2\x2\x2\x8D\x3\x2\x2\x2\x2\x8F\x3\x2"+
		"\x2\x2\x2\x91\x3\x2\x2\x2\x2\x93\x3\x2\x2\x2\x2\x95\x3\x2\x2\x2\x2\x9B"+
		"\x3\x2\x2\x2\x2\x9D\x3\x2\x2\x2\x3\xA4\x3\x2\x2\x2\x5\xA6\x3\x2\x2\x2"+
		"\a\xA8\x3\x2\x2\x2\t\xAA\x3\x2\x2\x2\v\xAC\x3\x2\x2\x2\r\xAE\x3\x2\x2"+
		"\x2\xF\xB0\x3\x2\x2\x2\x11\xB3\x3\x2\x2\x2\x13\xB6\x3\x2\x2\x2\x15\xB9"+
		"\x3\x2\x2\x2\x17\xBC\x3\x2\x2\x2\x19\xBE\x3\x2\x2\x2\x1B\xC0\x3\x2\x2"+
		"\x2\x1D\xC3\x3\x2\x2\x2\x1F\xC6\x3\x2\x2\x2!\xC8\x3\x2\x2\x2#\xCA\x3\x2"+
		"\x2\x2%\xCC\x3\x2\x2\x2\'\xCE\x3\x2\x2\x2)\xD0\x3\x2\x2\x2+\xD9\x3\x2"+
		"\x2\x2-\xDF\x3\x2\x2\x2/\xE3\x3\x2\x2\x2\x31\xEA\x3\x2\x2\x2\x33\xF0\x3"+
		"\x2\x2\x2\x35\xF7\x3\x2\x2\x2\x37\xFC\x3\x2\x2\x2\x39\x105\x3\x2\x2\x2"+
		";\x108\x3\x2\x2\x2=\x10D\x3\x2\x2\x2?\x112\x3\x2\x2\x2\x41\x118\x3\x2"+
		"\x2\x2\x43\x11F\x3\x2\x2\x2\x45\x126\x3\x2\x2\x2G\x129\x3\x2\x2\x2I\x130"+
		"\x3\x2\x2\x2K\x137\x3\x2\x2\x2M\x13D\x3\x2\x2\x2O\x141\x3\x2\x2\x2Q\x14A"+
		"\x3\x2\x2\x2S\x154\x3\x2\x2\x2U\x156\x3\x2\x2\x2W\x158\x3\x2\x2\x2Y\x15A"+
		"\x3\x2\x2\x2[\x15C\x3\x2\x2\x2]\x15F\x3\x2\x2\x2_\x167\x3\x2\x2\x2\x61"+
		"\x16C\x3\x2\x2\x2\x63\x176\x3\x2\x2\x2\x65\x17E\x3\x2\x2\x2g\x185\x3\x2"+
		"\x2\x2i\x189\x3\x2\x2\x2k\x18D\x3\x2\x2\x2m\x195\x3\x2\x2\x2o\x19D\x3"+
		"\x2\x2\x2q\x19F\x3\x2\x2\x2s\x1A3\x3\x2\x2\x2u\x1A9\x3\x2\x2\x2w\x1AB"+
		"\x3\x2\x2\x2y\x1B2\x3\x2\x2\x2{\x1BA\x3\x2\x2\x2}\x1C0\x3\x2\x2\x2\x7F"+
		"\x1C6\x3\x2\x2\x2\x81\x1CE\x3\x2\x2\x2\x83\x1D3\x3\x2\x2\x2\x85\x1DC\x3"+
		"\x2\x2\x2\x87\x1E5\x3\x2\x2\x2\x89\x1EA\x3\x2\x2\x2\x8B\x1ED\x3\x2\x2"+
		"\x2\x8D\x1F4\x3\x2\x2\x2\x8F\x213\x3\x2\x2\x2\x91\x215\x3\x2\x2\x2\x93"+
		"\x21C\x3\x2\x2\x2\x95\x225\x3\x2\x2\x2\x97\x22B\x3\x2\x2\x2\x99\x22D\x3"+
		"\x2\x2\x2\x9B\x244\x3\x2\x2\x2\x9D\x248\x3\x2\x2\x2\x9F\xA5\x5_\x30\x2"+
		"\xA0\xA5\x5]/\x2\xA1\xA5\x5\x63\x32\x2\xA2\xA5\x5\x61\x31\x2\xA3\xA5\x5"+
		"\x65\x33\x2\xA4\x9F\x3\x2\x2\x2\xA4\xA0\x3\x2\x2\x2\xA4\xA1\x3\x2\x2\x2"+
		"\xA4\xA2\x3\x2\x2\x2\xA4\xA3\x3\x2\x2\x2\xA5\x4\x3\x2\x2\x2\xA6\xA7\a"+
		"-\x2\x2\xA7\x6\x3\x2\x2\x2\xA8\xA9\a/\x2\x2\xA9\b\x3\x2\x2\x2\xAA\xAB"+
		"\a,\x2\x2\xAB\n\x3\x2\x2\x2\xAC\xAD\a\x31\x2\x2\xAD\f\x3\x2\x2\x2\xAE"+
		"\xAF\a\'\x2\x2\xAF\xE\x3\x2\x2\x2\xB0\xB1\a~\x2\x2\xB1\xB2\a~\x2\x2\xB2"+
		"\x10\x3\x2\x2\x2\xB3\xB4\a(\x2\x2\xB4\xB5\a(\x2\x2\xB5\x12\x3\x2\x2\x2"+
		"\xB6\xB7\a?\x2\x2\xB7\xB8\a?\x2\x2\xB8\x14\x3\x2\x2\x2\xB9\xBA\a#\x2\x2"+
		"\xBA\xBB\a?\x2\x2\xBB\x16\x3\x2\x2\x2\xBC\xBD\a@\x2\x2\xBD\x18\x3\x2\x2"+
		"\x2\xBE\xBF\a>\x2\x2\xBF\x1A\x3\x2\x2\x2\xC0\xC1\a@\x2\x2\xC1\xC2\a?\x2"+
		"\x2\xC2\x1C\x3\x2\x2\x2\xC3\xC4\a>\x2\x2\xC4\xC5\a?\x2\x2\xC5\x1E\x3\x2"+
		"\x2\x2\xC6\xC7\a#\x2\x2\xC7 \x3\x2\x2\x2\xC8\xC9\a<\x2\x2\xC9\"\x3\x2"+
		"\x2\x2\xCA\xCB\a=\x2\x2\xCB$\x3\x2\x2\x2\xCC\xCD\a.\x2\x2\xCD&\x3\x2\x2"+
		"\x2\xCE\xCF\a\x30\x2\x2\xCF(\x3\x2\x2\x2\xD0\xD1\ax\x2\x2\xD1\xD2\a\x63"+
		"\x2\x2\xD2\xD3\at\x2\x2\xD3\xD4\ak\x2\x2\xD4\xD5\a\x63\x2\x2\xD5\xD6\a"+
		"\x64\x2\x2\xD6\xD7\an\x2\x2\xD7\xD8\ag\x2\x2\xD8*\x3\x2\x2\x2\xD9\xDA"+
		"\a\x65\x2\x2\xDA\xDB\aq\x2\x2\xDB\xDC\ap\x2\x2\xDC\xDD\au\x2\x2\xDD\xDE"+
		"\av\x2\x2\xDE,\x3\x2\x2\x2\xDF\xE0\an\x2\x2\xE0\xE1\ag\x2\x2\xE1\xE2\a"+
		"v\x2\x2\xE2.\x3\x2\x2\x2\xE3\xE4\at\x2\x2\xE4\xE5\ag\x2\x2\xE5\xE6\av"+
		"\x2\x2\xE6\xE7\aw\x2\x2\xE7\xE8\at\x2\x2\xE8\xE9\ap\x2\x2\xE9\x30\x3\x2"+
		"\x2\x2\xEA\xEB\ak\x2\x2\xEB\xEC\ap\x2\x2\xEC\xED\ar\x2\x2\xED\xEE\aw\x2"+
		"\x2\xEE\xEF\av\x2\x2\xEF\x32\x3\x2\x2\x2\xF0\xF1\aq\x2\x2\xF1\xF2\aw\x2"+
		"\x2\xF2\xF3\av\x2\x2\xF3\xF4\ar\x2\x2\xF4\xF5\aw\x2\x2\xF5\xF6\av\x2\x2"+
		"\xF6\x34\x3\x2\x2\x2\xF7\xF8\a\x65\x2\x2\xF8\xF9\a\x63\x2\x2\xF9\xFA\a"+
		"n\x2\x2\xFA\xFB\an\x2\x2\xFB\x36\x3\x2\x2\x2\xFC\xFD\at\x2\x2\xFD\xFE"+
		"\ag\x2\x2\xFE\xFF\a\x63\x2\x2\xFF\x100\a\x66\x2\x2\x100\x101\aq\x2\x2"+
		"\x101\x102\ap\x2\x2\x102\x103\an\x2\x2\x103\x104\a{\x2\x2\x104\x38\x3"+
		"\x2\x2\x2\x105\x106\ak\x2\x2\x106\x107\ah\x2\x2\x107:\x3\x2\x2\x2\x108"+
		"\x109\av\x2\x2\x109\x10A\aj\x2\x2\x10A\x10B\ag\x2\x2\x10B\x10C\ap\x2\x2"+
		"\x10C<\x3\x2\x2\x2\x10D\x10E\ag\x2\x2\x10E\x10F\an\x2\x2\x10F\x110\au"+
		"\x2\x2\x110\x111\ag\x2\x2\x111>\x3\x2\x2\x2\x112\x113\ay\x2\x2\x113\x114"+
		"\aj\x2\x2\x114\x115\ak\x2\x2\x115\x116\an\x2\x2\x116\x117\ag\x2\x2\x117"+
		"@\x3\x2\x2\x2\x118\x119\at\x2\x2\x119\x11A\ag\x2\x2\x11A\x11B\ar\x2\x2"+
		"\x11B\x11C\ag\x2\x2\x11C\x11D\a\x63\x2\x2\x11D\x11E\av\x2\x2\x11E\x42"+
		"\x3\x2\x2\x2\x11F\x120\ag\x2\x2\x120\x121\an\x2\x2\x121\x122\au\x2\x2"+
		"\x122\x123\ag\x2\x2\x123\x124\ak\x2\x2\x124\x125\ah\x2\x2\x125\x44\x3"+
		"\x2\x2\x2\x126\x127\a\x66\x2\x2\x127\x128\aq\x2\x2\x128\x46\x3\x2\x2\x2"+
		"\x129\x12A\ao\x2\x2\x12A\x12B\aq\x2\x2\x12B\x12C\a\x66\x2\x2\x12C\x12D"+
		"\aw\x2\x2\x12D\x12E\an\x2\x2\x12E\x12F\ag\x2\x2\x12FH\x3\x2\x2\x2\x130"+
		"\x131\ak\x2\x2\x131\x132\ao\x2\x2\x132\x133\ar\x2\x2\x133\x134\aq\x2\x2"+
		"\x134\x135\at\x2\x2\x135\x136\av\x2\x2\x136J\x3\x2\x2\x2\x137\x138\au"+
		"\x2\x2\x138\x139\av\x2\x2\x139\x13A\a\x63\x2\x2\x13A\x13B\at\x2\x2\x13B"+
		"\x13C\av\x2\x2\x13CL\x3\x2\x2\x2\x13D\x13E\ag\x2\x2\x13E\x13F\ap\x2\x2"+
		"\x13F\x140\a\x66\x2\x2\x140N\x3\x2\x2\x2\x141\x142\ah\x2\x2\x142\x143"+
		"\aw\x2\x2\x143\x144\ap\x2\x2\x144\x145\a\x65\x2\x2\x145\x146\av\x2\x2"+
		"\x146\x147\ak\x2\x2\x147\x148\aq\x2\x2\x148\x149\ap\x2\x2\x149P\x3\x2"+
		"\x2\x2\x14A\x14B\ar\x2\x2\x14B\x14C\at\x2\x2\x14C\x14D\aq\x2\x2\x14D\x14E"+
		"\a\x65\x2\x2\x14E\x14F\ag\x2\x2\x14F\x150\a\x66\x2\x2\x150\x151\aw\x2"+
		"\x2\x151\x152\at\x2\x2\x152\x153\ag\x2\x2\x153R\x3\x2\x2\x2\x154\x155"+
		"\a*\x2\x2\x155T\x3\x2\x2\x2\x156\x157\a+\x2\x2\x157V\x3\x2\x2\x2\x158"+
		"\x159\a]\x2\x2\x159X\x3\x2\x2\x2\x15A\x15B\a_\x2\x2\x15BZ\x3\x2\x2\x2"+
		"\x15C\x15D\a<\x2\x2\x15D\x15E\a?\x2\x2\x15E\\\x3\x2\x2\x2\x15F\x160\a"+
		"k\x2\x2\x160\x161\ap\x2\x2\x161\x162\av\x2\x2\x162\x163\ag\x2\x2\x163"+
		"\x164\ai\x2\x2\x164\x165\ag\x2\x2\x165\x166\at\x2\x2\x166^\x3\x2\x2\x2"+
		"\x167\x168\at\x2\x2\x168\x169\ag\x2\x2\x169\x16A\a\x63\x2\x2\x16A\x16B"+
		"\an\x2\x2\x16B`\x3\x2\x2\x2\x16C\x16D\a\x65\x2\x2\x16D\x16E\aj\x2\x2\x16E"+
		"\x16F\a\x63\x2\x2\x16F\x170\at\x2\x2\x170\x171\a\x63\x2\x2\x171\x172\a"+
		"\x65\x2\x2\x172\x173\av\x2\x2\x173\x174\ag\x2\x2\x174\x175\at\x2\x2\x175"+
		"\x62\x3\x2\x2\x2\x176\x177\a\x64\x2\x2\x177\x178\aq\x2\x2\x178\x179\a"+
		"q\x2\x2\x179\x17A\an\x2\x2\x17A\x17B\ag\x2\x2\x17B\x17C\a\x63\x2\x2\x17C"+
		"\x17D\ap\x2\x2\x17D\x64\x3\x2\x2\x2\x17E\x17F\au\x2\x2\x17F\x180\av\x2"+
		"\x2\x180\x181\at\x2\x2\x181\x182\ak\x2\x2\x182\x183\ap\x2\x2\x183\x184"+
		"\ai\x2\x2\x184\x66\x3\x2\x2\x2\x185\x186\ap\x2\x2\x186\x187\ag\x2\x2\x187"+
		"\x188\ay\x2\x2\x188h\x3\x2\x2\x2\x189\x18A\ap\x2\x2\x18A\x18B\ak\x2\x2"+
		"\x18B\x18C\an\x2\x2\x18Cj\x3\x2\x2\x2\x18D\x18E\ar\x2\x2\x18E\x18F\aq"+
		"\x2\x2\x18F\x190\ak\x2\x2\x190\x191\ap\x2\x2\x191\x192\av\x2\x2\x192\x193"+
		"\ag\x2\x2\x193\x194\at\x2\x2\x194l\x3\x2\x2\x2\x195\x196\a\x63\x2\x2\x196"+
		"\x197\at\x2\x2\x197\x198\at\x2\x2\x198\x199\a\x63\x2\x2\x199\x19A\a{\x2"+
		"\x2\x19An\x3\x2\x2\x2\x19B\x19E\x5q\x39\x2\x19C\x19E\x5s:\x2\x19D\x19B"+
		"\x3\x2\x2\x2\x19D\x19C\x3\x2\x2\x2\x19Ep\x3\x2\x2\x2\x19F\x1A0\ax\x2\x2"+
		"\x1A0\x1A1\a\x63\x2\x2\x1A1\x1A2\an\x2\x2\x1A2r\x3\x2\x2\x2\x1A3\x1A4"+
		"\at\x2\x2\x1A4\x1A5\ag\x2\x2\x1A5\x1A6\ah\x2\x2\x1A6t\x3\x2\x2\x2\x1A7"+
		"\x1AA\x5w<\x2\x1A8\x1AA\x5y=\x2\x1A9\x1A7\x3\x2\x2\x2\x1A9\x1A8\x3\x2"+
		"\x2\x2\x1AAv\x3\x2\x2\x2\x1AB\x1AC\ar\x2\x2\x1AC\x1AD\aw\x2\x2\x1AD\x1AE"+
		"\a\x64\x2\x2\x1AE\x1AF\an\x2\x2\x1AF\x1B0\ak\x2\x2\x1B0\x1B1\a\x65\x2"+
		"\x2\x1B1x\x3\x2\x2\x2\x1B2\x1B3\ar\x2\x2\x1B3\x1B4\at\x2\x2\x1B4\x1B5"+
		"\ak\x2\x2\x1B5\x1B6\ax\x2\x2\x1B6\x1B7\a\x63\x2\x2\x1B7\x1B8\av\x2\x2"+
		"\x1B8\x1B9\ag\x2\x2\x1B9z\x3\x2\x2\x2\x1BA\x1BB\a\x65\x2\x2\x1BB\x1BC"+
		"\an\x2\x2\x1BC\x1BD\a\x63\x2\x2\x1BD\x1BE\au\x2\x2\x1BE\x1BF\au\x2\x2"+
		"\x1BF|\x3\x2\x2\x2\x1C0\x1C1\ag\x2\x2\x1C1\x1C2\ao\x2\x2\x1C2\x1C3\ar"+
		"\x2\x2\x1C3\x1C4\av\x2\x2\x1C4\x1C5\a{\x2\x2\x1C5~\x3\x2\x2\x2\x1C6\x1C7"+
		"\ak\x2\x2\x1C7\x1C8\ap\x2\x2\x1C8\x1C9\aj\x2\x2\x1C9\x1CA\ag\x2\x2\x1CA"+
		"\x1CB\at\x2\x2\x1CB\x1CC\ak\x2\x2\x1CC\x1CD\av\x2\x2\x1CD\x80\x3\x2\x2"+
		"\x2\x1CE\x1CF\a\x64\x2\x2\x1CF\x1D0\a\x63\x2\x2\x1D0\x1D1\au\x2\x2\x1D1"+
		"\x1D2\ag\x2\x2\x1D2\x82\x3\x2\x2\x2\x1D3\x1D4\a\x63\x2\x2\x1D4\x1D5\a"+
		"\x64\x2\x2\x1D5\x1D6\au\x2\x2\x1D6\x1D7\av\x2\x2\x1D7\x1D8\at\x2\x2\x1D8"+
		"\x1D9\a\x63\x2\x2\x1D9\x1DA\a\x65\x2\x2\x1DA\x1DB\av\x2\x2\x1DB\x84\x3"+
		"\x2\x2\x2\x1DC\x1DD\aq\x2\x2\x1DD\x1DE\ax\x2\x2\x1DE\x1DF\ag\x2\x2\x1DF"+
		"\x1E0\at\x2\x2\x1E0\x1E1\at\x2\x2\x1E1\x1E2\ak\x2\x2\x1E2\x1E3\a\x66\x2"+
		"\x2\x1E3\x1E4\ag\x2\x2\x1E4\x86\x3\x2\x2\x2\x1E5\x1E6\ah\x2\x2\x1E6\x1E7"+
		"\ak\x2\x2\x1E7\x1E8\an\x2\x2\x1E8\x1E9\ag\x2\x2\x1E9\x88\x3\x2\x2\x2\x1EA"+
		"\x1EB\t\x2\x2\x2\x1EB\x8A\x3\x2\x2\x2\x1EC\x1EE\x5\x89\x45\x2\x1ED\x1EC"+
		"\x3\x2\x2\x2\x1EE\x1EF\x3\x2\x2\x2\x1EF\x1ED\x3\x2\x2\x2\x1EF\x1F0\x3"+
		"\x2\x2\x2\x1F0\x8C\x3\x2\x2\x2\x1F1\x1F3\x5\x89\x45\x2\x1F2\x1F1\x3\x2"+
		"\x2\x2\x1F3\x1F6\x3\x2\x2\x2\x1F4\x1F2\x3\x2\x2\x2\x1F4\x1F5\x3\x2\x2"+
		"\x2\x1F5\x1F8\x3\x2\x2\x2\x1F6\x1F4\x3\x2\x2\x2\x1F7\x1F9\x5\'\x14\x2"+
		"\x1F8\x1F7\x3\x2\x2\x2\x1F8\x1F9\x3\x2\x2\x2\x1F9\x1FB\x3\x2\x2\x2\x1FA"+
		"\x1FC\x5\x89\x45\x2\x1FB\x1FA\x3\x2\x2\x2\x1FC\x1FD\x3\x2\x2\x2\x1FD\x1FB"+
		"\x3\x2\x2\x2\x1FD\x1FE\x3\x2\x2\x2\x1FE\x208\x3\x2\x2\x2\x1FF\x201\t\x3"+
		"\x2\x2\x200\x202\t\x4\x2\x2\x201\x200\x3\x2\x2\x2\x201\x202\x3\x2\x2\x2"+
		"\x202\x204\x3\x2\x2\x2\x203\x205\x5\x89\x45\x2\x204\x203\x3\x2\x2\x2\x205"+
		"\x206\x3\x2\x2\x2\x206\x204\x3\x2\x2\x2\x206\x207\x3\x2\x2\x2\x207\x209"+
		"\x3\x2\x2\x2\x208\x1FF\x3\x2\x2\x2\x208\x209\x3\x2\x2\x2\x209\x8E\x3\x2"+
		"\x2\x2\x20A\x20B\av\x2\x2\x20B\x20C\at\x2\x2\x20C\x20D\aw\x2\x2\x20D\x214"+
		"\ag\x2\x2\x20E\x20F\ah\x2\x2\x20F\x210\a\x63\x2\x2\x210\x211\an\x2\x2"+
		"\x211\x212\au\x2\x2\x212\x214\ag\x2\x2\x213\x20A\x3\x2\x2\x2\x213\x20E"+
		"\x3\x2\x2\x2\x214\x90\x3\x2\x2\x2\x215\x219\t\x5\x2\x2\x216\x218\t\x6"+
		"\x2\x2\x217\x216\x3\x2\x2\x2\x218\x21B\x3\x2\x2\x2\x219\x217\x3\x2\x2"+
		"\x2\x219\x21A\x3\x2\x2\x2\x21A\x92\x3\x2\x2\x2\x21B\x219\x3\x2\x2\x2\x21C"+
		"\x220\a$\x2\x2\x21D\x21F\x5\x97L\x2\x21E\x21D\x3\x2\x2\x2\x21F\x222\x3"+
		"\x2\x2\x2\x220\x21E\x3\x2\x2\x2\x220\x221\x3\x2\x2\x2\x221\x223\x3\x2"+
		"\x2\x2\x222\x220\x3\x2\x2\x2\x223\x224\a$\x2\x2\x224\x94\x3\x2\x2\x2\x225"+
		"\x226\a)\x2\x2\x226\x227\x5\x97L\x2\x227\x228\a)\x2\x2\x228\x96\x3\x2"+
		"\x2\x2\x229\x22C\n\a\x2\x2\x22A\x22C\x5\x99M\x2\x22B\x229\x3\x2\x2\x2"+
		"\x22B\x22A\x3\x2\x2\x2\x22C\x98\x3\x2\x2\x2\x22D\x22E\a^\x2\x2\x22E\x22F"+
		"\t\b\x2\x2\x22F\x9A\x3\x2\x2\x2\x230\x231\a\x31\x2\x2\x231\x232\a\x31"+
		"\x2\x2\x232\x236\x3\x2\x2\x2\x233\x235\n\t\x2\x2\x234\x233\x3\x2\x2\x2"+
		"\x235\x238\x3\x2\x2\x2\x236\x234\x3\x2\x2\x2\x236\x237\x3\x2\x2\x2\x237"+
		"\x245\x3\x2\x2\x2\x238\x236\x3\x2\x2\x2\x239\x23A\a\x31\x2\x2\x23A\x23B"+
		"\a,\x2\x2\x23B\x23F\x3\x2\x2\x2\x23C\x23E\v\x2\x2\x2\x23D\x23C\x3\x2\x2"+
		"\x2\x23E\x241\x3\x2\x2\x2\x23F\x240\x3\x2\x2\x2\x23F\x23D\x3\x2\x2\x2"+
		"\x240\x242\x3\x2\x2\x2\x241\x23F\x3\x2\x2\x2\x242\x243\a,\x2\x2\x243\x245"+
		"\a\x31\x2\x2\x244\x230\x3\x2\x2\x2\x244\x239\x3\x2\x2\x2\x245\x246\x3"+
		"\x2\x2\x2\x246\x247\bN\x2\x2\x247\x9C\x3\x2\x2\x2\x248\x249\t\n\x2\x2"+
		"\x249\x24A\x3\x2\x2\x2\x24A\x24B\bO\x2\x2\x24B\x9E\x3\x2\x2\x2\x14\x2"+
		"\xA4\x19D\x1A9\x1EF\x1F4\x1F8\x1FD\x201\x206\x208\x213\x219\x220\x22B"+
		"\x236\x23F\x244\x3\b\x2\x2";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
} // namespace SLangGrammar
