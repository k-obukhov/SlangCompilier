#ifndef FILES_CPP_H
#define FILES_CPP_H

#include <fstream>
#include <string>

using namespace std;

namespace Files_cpp
{
	using file_stream = fstream;

	file_stream open(string path)
	{
		file_stream fs;
		fs.open(path, std::fstream::in | std::fstream::out | std::fstream::app);
		return fs;
	}

	void close(file_stream& stream)
	{
		stream.close();
	}

	void w_int(file_stream& stream, int64_t value)
	{
		stream << value;
	}

	void w_real(file_stream& stream, double value)
	{
		stream << value;
	}

	void w_str(file_stream& stream, string value)
	{
		stream << value;
	}

	void w_chr(file_stream& stream, char value)
	{
		stream << value;
	}

	void w_bool(file_stream& stream, bool value)
	{
		stream << value;
	}

	int64_t r_int(file_stream& stream)
	{
	        int64_t value;
		stream >> value;
		return value;
	}

	double r_real(file_stream& stream)
	{
		double value;
		stream >> value;
		return value;
	}

	string r_str(file_stream& stream)
	{
		string value;
		stream >> value;
		return value;
	}

	char r_chr(file_stream& stream)
	{
		char value;
		stream >> value;
		return value;
	}

	bool r_bool(file_stream& stream)
	{
		bool value;
		stream >> value;
		return value;
	}

	bool is_open(file_stream& stream)
	{
		return stream.is_open();
	}

	bool is_eof(file_stream& stream)
	{
		return stream.eof();
	}
}

#endif