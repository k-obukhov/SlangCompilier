empty module String

	[from "CppLib/String_cpp.hpp" import "String_cpp::str_len"]
	public function(val string arg): integer Length
	end

	[from "CppLib/String_cpp.hpp" import "String_cpp::str_eq_"]
	public function(val string left, val string right): boolean Equals
	end

	[from "CppLib/String_cpp.hpp" import "String_cpp::str_l_"]
	public function(val string left, val string right): boolean Less
	end

	[from "CppLib/String_cpp.hpp" import "String_cpp::str_g_"]
	public function(val string left, val string right): boolean Greater
	end

	[from "CppLib/String_cpp.hpp" import "String_cpp::str_leq_"]
	public function(val string left, val string right): boolean LessOrEquals
	end

	[from "CppLib/String_cpp.hpp" import "String_cpp::str_geq_"]
	public function(val string left, val string right): boolean GreaterOrEquals
	end

	[from "CppLib/String_cpp.hpp" import "String_cpp::str_neq_"]
	public function(val string left, val string right): boolean NotEquals
	end

	[from "CppLib/String_cpp.hpp" import "String_cpp::str_char_"]
	public function(val string str, val character chr): string AddChar
	end

	[from "CppLib/String_cpp.hpp" import "String_cpp::str_concat_"]
	public function(val string lhs, val string rhs): string Concat
	end

	[from "CppLib/String_cpp.hpp" import "String_cpp::str_itos_"]
	public function(val integer arg): string IntToString
	end
	
	[from "CppLib/String_cpp.hpp" import "String_cpp::str_rtos_"]
	public function(val real arg): string RealToString
	end