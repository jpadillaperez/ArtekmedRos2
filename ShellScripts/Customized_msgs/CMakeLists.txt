cmake_minimum_required(VERSION 3.5)
project(node_manager C)

if(CMAKE_COMPILER_IS_GNUCXX OR CMAKE_CXX_COMPILER_ID MATCHES "Clang")
  add_compile_options(-Wall -Wextra -Wpedantic)
endif()

set(CSHARP_TARGET_FRAMEWORK "netcoreapp2.0")

find_package(ament_cmake REQUIRED)
find_package(rcldotnet REQUIRED)
find_package(rcldotnet_common REQUIRED)
find_package(dotnet_cmake_module REQUIRED)
find_package(DotNETExtra REQUIRED)

find_package(customized_msgs REQUIRED)
find_package(geometry_msgs REQUIRED)

set(_assemblies_dep_dlls
  ${rcldotnet_ASSEMBLIES_DLL}
  ${rcldotnet_common_ASSEMBLIES_DLL}
  ${customized_msgs_ASSEMBLIES_DLL}
)

add_dotnet_executable(node_manager
  src/node_manager_function.cs
  INCLUDE_DLLS
  ${_assemblies_dep_dlls}
)

install_dotnet(node_manager DESTINATION lib/${PROJECT_NAME}/dotnet)

ament_package()
