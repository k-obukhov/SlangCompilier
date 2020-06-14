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

	int64_t rnd_int_(int64_t first, int64_t second)
	{
		assert(first < second);
		uniform_int_distribution<int64_t> dis(first, second);
		return dis(gen);
	}
}

#endif