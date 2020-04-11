empty module Files
	[from "CppLib/Files_cpp.hpp" import "Files_cpp::file_stream"]
	public empty class File;

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::open"]
	public function(val string path): File Open
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::close"]
	public procedure(ref File f) Close
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::w_int"]
	public procedure(ref File f, val integer value) WriteInteger
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::w_real"]
	public procedure(ref File f, val real value) WriteReal
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::w_str"]
	public procedure(ref File f, val string value) WriteString
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::w_chr"]
	public procedure(ref File f, val character value) WriteCharacter
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::w_bool"]
	public procedure(ref File f, val boolean value) WriteBoolean
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::r_int"]
	public function(ref File f): integer ReadInteger
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::r_real"]
	public function(ref File f): real ReadReal
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::r_str"]
	public function(ref File f): string ReadString
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::r_chr"]
	public function(ref File f): character ReadCharacter
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::r_bool"]
	public function(ref File f): boolean ReadBoolean
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::is_open"]
	public function(ref File f): boolean IsOpen
	end

	[from "CppLib/Files_cpp.hpp" import "Files_cpp::is_eof"]
	public function(ref File f): boolean IsEOF
	end
