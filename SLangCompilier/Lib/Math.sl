module Math
	
	public const real E = 2,7182818284;
	public const real PI = 3.14159265359;

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::sin_"]
	public function(real arg): real Sin
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::cos_"]
	public function(real arg): real Cos
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::tan_"]
	public function(real arg): real Tan
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::atan_"]
	public function(real arg): real ATan
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::acos_"]
	public function(real arg): real ACos
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::asin_"]
	public function(real arg): real ASin
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::atan2_"]
	public function(real first, real second): real ATan2
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::exp_"]
	public function(real arg): real Exp
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::log_"]
	public function(real arg): real Log
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::log10_"]
	public function(real arg): real Log10
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::log2_"]
	public function(real arg): real Log2
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::exp2_"]
	public function(real arg): real Exp2
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::pow_"]
	public function(real first, real second): real Pow
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::sqrt_"]
	public function(real arg): real Sqrt
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::ceil_"]
	public function(real arg): integer Ceil
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::floor_"]
	public function(real arg): integer Floor
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::round_"]
	public function(real arg): integer Round
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::fabs_"]
	public function(real arg): real Fabs
	end

	[file "CppLib/Math_cpp.hpp" import "Math_cpp::abs_"]
	public function(integer arg): integer Abs
	end
