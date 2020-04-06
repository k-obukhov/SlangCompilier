#ifndef __MATH_CPP_H
#define __MATH_CPP_H
#include <cmath>

namespace Math_cpp 
{
	static double PI_ = 3.14159265359;
	static double E_ = 2.7182818284;
	// todo additional checks in future for args
	double sin_(double arg)
	{
		return sin(arg);
	}

	double cos_(double arg)
	{
		return cos(arg);
	}

	double tan_(double arg)
	{
		return tan(arg);
	}

	double atan_(double arg)
	{
		return atan(arg);
	}

	double acos_(double arg)
	{
		return acos(arg);
	}

	double asin_(double arg)
	{
		return asin(arg);
	}

	double atan2_(double first, double second)
	{
		return atan2(first, second);
	}

	double exp_(double arg)
	{
		return exp(arg);
	}

	double log_(double arg)
	{
		return log(arg);
	}

	double log10_(double arg)
	{
		return log10(arg);
	}

	double log2_(double arg)
	{
		return log2(arg);
	}

	double exp2_(double arg)
	{
		return exp2(arg);
	}

	double pow_(double first, double second)
	{
		return pow(first, second);
	}

	double sqrt_(double arg)
	{
		return sqrt(arg);
	}

	int ceil_(double arg)
	{
		return ceil(arg);
	}

	int floor_(double arg)
	{
		return floor(arg);
	}

	int round_(double arg)
	{
		return round(arg);
	}

	int abs_(int arg)
	{
		return abs(arg);
	}

	double fabs_(double arg)
	{
		return fabs(arg);
	}
}

#endif