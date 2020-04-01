module Random

	[file "CppLib/Random_cpp.hpp" import "Random_cpp::rnd_"]
	public function(): real Rnd
	end

	[file "CppLib/Random_cpp.hpp" import "Random_cpp::rnd_real_"]
	public function(val real first, val real last): real RangeReal
	end

	[file "CppLib/Random_cpp.hpp" import "Random_cpp::rnd_int_"]
	public function(val integer first, val integer last): real RangeInteger
	end