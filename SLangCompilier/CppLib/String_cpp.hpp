#ifndef __STRING_CPP_H
#define __STRING_CPP_H

#include<string>

using namespace std;

namespace String_cpp
{
	int64_t str_len(string arg)
	{
		return arg.length();
	}

	bool str_eq_(string left, string right)
	{
	    return left == right;
	}

	bool str_neq_(string left, string right)
	{
	    return left != right;
	}

	bool str_l_(string left, string right)
	{
	    return left < right;
	}

	bool str_g_(string left, string right)
	{
	    return left > right;
	}

	bool str_leq_(string left, string right)
	{
	    return left <= right;
	}

	bool str_geq_(string left, string right)
	{
	    return left >= right;
	}

	string str_char_(string left, char right)
	{
		return left + right;
	}

	string str_concat_(string left, string right)
	{
		return left + right;
	}

	string str_itos_(int64_t value)
	{
		return to_string(value);
	}

	string str_rtos_(double value)
	{
		return to_string(value);
	}

}

#endif