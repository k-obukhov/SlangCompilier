module Files
	[file "CppLib/Files_cpp.hpp" import "Files_cpp::file_stream"]
	public empty class File

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::open"]
	public function(val string path): File Open
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::close"]
	public procedure(ref File file) Close
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::w_int"]
	public procedure(ref File file, val integer value) WriteInteger
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::w_real"]
	public procedure(ref File file, val real value) WriteReal
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::w_str"]
	public procedure(ref File file, val integer value) WriteString
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::w_chr"]
	public procedure(ref File file, val integer value) WriteCharacter
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::w_bool"]
	public procedure(ref File file, val boolean value) WriteBoolean
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::r_int"]
	public function(ref File file): integer ReadInteger
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::r_real"]
	public function(ref File file): integer ReadReal
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::r_str"]
	public function(ref File file): integer ReadString
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::r_chr"]
	public function(ref File file): integer ReadCharacter
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::r_bool"]
	public function(ref File file): boolean ReadBoolean
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::is_open"]
	public function(ref File file): boolean IsOpen
	end

	[file "CppLib/Files_cpp.hpp" import "Files_cpp::is_eof"]
	public function(ref File file): boolean IsEOF
	end
