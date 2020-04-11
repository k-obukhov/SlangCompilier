#ifndef RANDOM_CPP_H
#define RANDOM_CPP_H

#include <random>
#include <limits>
#include <cmath>
#include <cassert>
#include <ctime>

using namespace std;

namespace Random_cpp
{
	static default_random_engine gen(time(0));

	double rnd_()
	{
		return generate_canonical<double, 10>(gen);
	}

	double rnd_real_(double first, double second)
	{
		assert(first < second);
		uniform_real_distribution<> dis(first, second);
		return dis(gen);
	}

	double rnd_int_(int first, int second)
	{
		assert(first < second);
		uniform_int_distribution<> dis(first, second);
		return dis(gen);
	}
}

#endif