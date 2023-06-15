cmake_minimum_required(VERSION 3.5)
project(string_node_manager)

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

)

add_dotnet_executable(string_node_manager
  src/string_node_manager_function.cs
  INCLUDE_DLLS
  ${_assemblies_dep_dlls}
)

install_dotnet(string_node_manager DESTINATION lib/${PROJECT_NAME}/dotnet)
ament_package()