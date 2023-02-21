cmake_minimum_required(VERSION 3.5)
project(string_msgs_multiple_functions_ddl)

# Default to C99
if(NOT CMAKE_C_STANDARD)
  set(CMAKE_C_STANDARD 99)
endif()

# Default to C++14
if(NOT CMAKE_CXX_STANDARD)
  set(CMAKE_CXX_STANDARD 14)
endif()

if(CMAKE_COMPILER_IS_GNUCXX OR CMAKE_CXX_COMPILER_ID MATCHES "Clang")
  add_compile_options(-Wall -Wextra -Wpedantic)
endif()

set(CSHARP_TARGET_FRAMEWORK "netcoreapp2.0")

find_package(ament_cmake REQUIRED)
find_package(rcldotnet REQUIRED)
find_package(rcldotnet_common REQUIRED)
find_package(dotnet_cmake_module REQUIRED)
find_package(DotNETExtra REQUIRED)

find_package(std_msgs REQUIRED)

set(_assemblies_dep_dlls
  ${rcldotnet_ASSEMBLIES_DLL}
  ${rcldotnet_common_ASSEMBLIES_DLL}
  ${std_msgs_ASSEMBLIES_DLL}
  "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/Plugins/Json130r2/Bin/netstandard2.0/newtonsoft.json.dll"
)

add_dotnet_executable(string_msgs_multiple_functions_ddl
  src/string_msgs_multiple_functions_ddl.cs
  INCLUDE_DLLS
  ${_assemblies_dep_dlls}
)

install_dotnet(string_msgs_multiple_functions_ddl DESTINATION lib/${PROJECT_NAME}/dotnet)

ament_package()
