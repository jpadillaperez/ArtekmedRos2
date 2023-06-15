# Install script for directory: C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/install/string_msgs_hololens_dll")
endif()
string(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
if(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  if(BUILD_TYPE)
    string(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  else()
    set(CMAKE_INSTALL_CONFIG_NAME "Release")
  endif()
  message(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
endif()

# Set the component getting installed.
if(NOT CMAKE_INSTALL_COMPONENT)
  if(COMPONENT)
    message(STATUS "Install component: \"${COMPONENT}\"")
    set(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  else()
    set(CMAKE_INSTALL_COMPONENT)
  endif()
endif()

# Is this installation the result of a crosscompile?
if(NOT DEFINED CMAKE_CROSSCOMPILING)
  set(CMAKE_CROSSCOMPILING "FALSE")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  if("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/string_msgs_hololens_dll/dotnet" TYPE DIRECTORY FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/Debug/netcoreapp2.0/win-x64/publish//")
  elseif("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/string_msgs_hololens_dll/dotnet" TYPE DIRECTORY FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/Release/netcoreapp2.0/win-x64/publish//")
  elseif("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/string_msgs_hololens_dll/dotnet" TYPE DIRECTORY FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/MinSizeRel/netcoreapp2.0/win-x64/publish//")
  elseif("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/string_msgs_hololens_dll/dotnet" TYPE DIRECTORY FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/RelWithDebInfo/netcoreapp2.0/win-x64/publish//")
  endif()
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/string_msgs_hololens_dll" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/lib/string_msgs_hololens_dll.bat")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/ament_index/resource_index/package_run_dependencies" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/ament_cmake_index/share/ament_index/resource_index/package_run_dependencies/string_msgs_hololens_dll")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/ament_index/resource_index/parent_prefix_path" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/ament_cmake_index/share/ament_index/resource_index/parent_prefix_path/string_msgs_hololens_dll")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_msgs_hololens_dll/environment" TYPE FILE FILES "C:/opt/ros/foxy/x64/share/ament_cmake_core/cmake/environment_hooks/environment/ament_prefix_path.bat")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_msgs_hololens_dll/environment" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/ament_cmake_environment_hooks/ament_prefix_path.dsv")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_msgs_hololens_dll/environment" TYPE FILE FILES "C:/opt/ros/foxy/x64/share/ament_cmake_core/cmake/environment_hooks/environment/path.bat")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_msgs_hololens_dll/environment" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/ament_cmake_environment_hooks/path.dsv")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_msgs_hololens_dll" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/ament_cmake_environment_hooks/local_setup.bat")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_msgs_hololens_dll" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/ament_cmake_environment_hooks/local_setup.dsv")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_msgs_hololens_dll" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/ament_cmake_environment_hooks/package.dsv")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/ament_index/resource_index/packages" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/ament_cmake_index/share/ament_index/resource_index/packages/string_msgs_hololens_dll")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_msgs_hololens_dll/cmake" TYPE FILE FILES
    "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/ament_cmake_core/string_msgs_hololens_dllConfig.cmake"
    "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/ament_cmake_core/string_msgs_hololens_dllConfig-version.cmake"
    )
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_msgs_hololens_dll" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/package.xml")
endif()

if(CMAKE_INSTALL_COMPONENT)
  set(CMAKE_INSTALL_MANIFEST "install_manifest_${CMAKE_INSTALL_COMPONENT}.txt")
else()
  set(CMAKE_INSTALL_MANIFEST "install_manifest.txt")
endif()

string(REPLACE ";" "\n" CMAKE_INSTALL_MANIFEST_CONTENT
       "${CMAKE_INSTALL_MANIFEST_FILES}")
file(WRITE "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/string_msgs_hololens_dll/build/string_msgs_hololens_dll/${CMAKE_INSTALL_MANIFEST}"
     "${CMAKE_INSTALL_MANIFEST_CONTENT}")
