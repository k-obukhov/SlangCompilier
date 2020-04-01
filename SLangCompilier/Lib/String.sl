module String

	[file "CppLib/String_cpp.hpp" import "String_cpp::str_eq_"]
	public function(val string left, val string right): boolean Equals
	end

	[file "CppLib/String_cpp.hpp" import "String_cpp::str_l_"]
	public function(val string left, val string right): boolean Less
	end

	[file "CppLib/String_cpp.hpp" import "String_cpp::str_g_"]
	public function(val string left, val string right): boolean Greater
	end

	[file "CppLib/String_cpp.hpp" import "String_cpp::str_leq_"]
	public function(val string left, val string right): boolean LessOrEquals
	end

	[file "CppLib/String_cpp.hpp" import "String_cpp::str_geq_"]
	public function(val string left, val string right): boolean GreaterOrEquals
	end

	[file "CppLib/String_cpp.hpp" import "String_cpp::str_neq_"]
	public function(val string left, val string right): boolean NotEquals
	end

	[file "CppLib/String_cpp.hpp" import "String_cpp::str_char_"]
	public function(val string str, val character chr): string AddChar
	end

	[file "CppLib/String_cpp.hpp" import "String_cpp::str_concat_"]
	public function(val string lhs, val string rhs): string Concat
	end

	[file "CppLib/String_cpp.hpp" import "String_cpp::str_itos_"]
	public function(val integer arg): string IntToString
	end
	
	[file "CppLib/String_cpp.hpp" import "String_cpp::str_rtos_"]
	public function(val integer arg): string RealToString
	end