# Slang Compilier
- FrontEnd
	- First Step (ANTLR Generated)
	- Second Step (ANTLR Visitors)
- CodeGenerator (to C++11/14)
- Work with console input/output

# Build
- Build solution
- Copy folders "Lib" and "CppLib" to the folder with executable file (bin\Release\netcoreapp{version of .NET Core SDK})

# Usage
- Console app needs 4 args:
  - Path to Slang project
  - Path to generated source
  - Language tag (default value - "cpp", another languages may be added in future)
  - Path to output executable file (may be not set)

Works with ANTLR4 / C#7 / .Net Core 3.1
Enjoy!