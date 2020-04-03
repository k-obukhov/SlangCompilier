module Math
	
	public const real E := 2.7182818284;
	public const real PI := 3.14159265359;

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::sin_"]
	public function(val real arg): real Sin
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::cos_"]
	public function(val real arg): real Cos
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::tan_"]
	public function(val real arg): real Tan
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::atan_"]
	public function(val real arg): real ATan
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::acos_"]
	public function(val real arg): real ACos
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::asin_"]
	public function(val real arg): real ASin
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::atan2_"]
	public function(val real first, val real second): real ATan2
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::exp_"]
	public function(val real arg): real Exp
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::log_"]
	public function(val real arg): real Log
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::log10_"]
	public function(val real arg): real Log10
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::log2_"]
	public function(val real arg): real Log2
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::exp2_"]
	public function(val real arg): real Exp2
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::pow_"]
	public function(val real first, val real second): real Pow
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::sqrt_"]
	public function(val real arg): real Sqrt
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::ceil_"]
	public function(val real arg): integer Ceil
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::floor_"]
	public function(val real arg): integer Floor
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::round_"]
	public function(val real arg): integer Round
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::fabs_"]
	public function(val real arg): real Fabs
	end

	[from "CppLib/Math_cpp.hpp" import "Math_cpp::abs_"]
	public function(val integer arg): integer Abs
	end
