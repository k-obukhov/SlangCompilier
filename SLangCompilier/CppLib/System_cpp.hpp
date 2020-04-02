#ifndef SYSTEM_CPP_H
#define SYSTEM_CPP_H

#include <limits>
#include <iostream>

using namespace std;

namespace System_cpp
{
	void pause_()
	{
	    cin.ignore(numeric_limits<streamsize>::max());
		cin.get();
	}
	
	double toreal_(int arg)
	{
		return static_cast<double>(arg);
	}

	int toint_(double arg)
	{
		return static_cast<int>(arg);
	}

	int ord_(char arg)
	{
		return static_cast<int>(arg);
	}

	char chr_(int arg)
	{
		return static_cast<char>(arg);
	}
}

#endif