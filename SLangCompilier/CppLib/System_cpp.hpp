#ifndef SYSTEM_CPP_H
#define SYSTEM_CPP_H

#include <limits>
#include <iostream>

using namespace std;

namespace System_cpp
{
	void pause_()
	{
		cin.sync();
		cin.get();
	}
	
	double toreal_(int64_t arg)
	{
		return static_cast<double>(arg);
	}

	int64_t toint_(double arg)
	{
		return static_cast<int64_t>(arg);
	}

	int64_t ord_(char arg)
	{
		return static_cast<int64_t>(arg);
	}

	char chr_(int64_t arg)
	{
		return static_cast<char>(arg);
	}
}

#endif