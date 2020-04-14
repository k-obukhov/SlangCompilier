#ifndef FILES_CPP_H
#define FILES_CPP_H

#include <fstream>
#include <string>
#include <exception>

using namespace std;

namespace Files_cpp
{
	using file_stream = fstream;

	file_stream open(string path, char f_options)
	{
		/*
		options
		r, w, a, +
		r -- read 
		w - write + trunc
		a - write + app
		+ - read + write
		*/

		file_stream fs;
		auto options = std::fstream::in;
		switch (f_options)
		{
			case 'r':
				options = std::fstream::in;
				break;
			case 'w':
				options = std::fstream::out | std::fstream::trunc;
				break;
			case 'a':
				options = std::fstream::out | std::fstream::app;
				break;
			case '+':
				options = std::fstream::in | std::fstream::out;
			default:
				throw std::invalid_argument("invalid options for file open");
		}
        fs.open(path, options);
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