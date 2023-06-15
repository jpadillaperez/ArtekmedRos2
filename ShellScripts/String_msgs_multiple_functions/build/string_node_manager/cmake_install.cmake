# Install script for directory: C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/install/string_node_manager")
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
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/string_node_manager/dotnet" TYPE DIRECTORY FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/Debug/netcoreapp2.0/win-x64/publish//")
  elseif("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/string_node_manager/dotnet" TYPE DIRECTORY FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/Release/netcoreapp2.0/win-x64/publish//")
  elseif("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/string_node_manager/dotnet" TYPE DIRECTORY FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/MinSizeRel/netcoreapp2.0/win-x64/publish//")
  elseif("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/string_node_manager/dotnet" TYPE DIRECTORY FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/RelWithDebInfo/netcoreapp2.0/win-x64/publish//")
  endif()
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/string_node_manager" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/lib/string_node_manager.bat")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/ament_index/resource_index/package_run_dependencies" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/ament_cmake_index/share/ament_index/resource_index/package_run_dependencies/string_node_manager")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/ament_index/resource_index/parent_prefix_path" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/ament_cmake_index/share/ament_index/resource_index/parent_prefix_path/string_node_manager")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_node_manager/environment" TYPE FILE FILES "C:/opt/ros/foxy/x64/share/ament_cmake_core/cmake/environment_hooks/environment/ament_prefix_path.bat")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_node_manager/environment" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/ament_cmake_environment_hooks/ament_prefix_path.dsv")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_node_manager/environment" TYPE FILE FILES "C:/opt/ros/foxy/x64/share/ament_cmake_core/cmake/environment_hooks/environment/path.bat")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_node_manager/environment" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/ament_cmake_environment_hooks/path.dsv")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_node_manager" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/ament_cmake_environment_hooks/local_setup.bat")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_node_manager" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/ament_cmake_environment_hooks/local_setup.dsv")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_node_manager" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/ament_cmake_environment_hooks/package.dsv")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/ament_index/resource_index/packages" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/ament_cmake_index/share/ament_index/resource_index/packages/string_node_manager")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_node_manager/cmake" TYPE FILE FILES
    "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/ament_cmake_core/string_node_managerConfig.cmake"
    "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/ament_cmake_core/string_node_managerConfig-version.cmake"
    )
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/string_node_manager" TYPE FILE FILES "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/package.xml")
endif()

if(CMAKE_INSTALL_COMPONENT)
  set(CMAKE_INSTALL_MANIFEST "install_manifest_${CMAKE_INSTALL_COMPONENT}.txt")
else()
  set(CMAKE_INSTALL_MANIFEST "install_manifest.txt")
endif()

string(REPLACE ";" "\n" CMAKE_INSTALL_MANIFEST_CONTENT
       "${CMAKE_INSTALL_MANIFEST_FILES}")
file(WRITE "C:/Users/Jorge/Documents/GitHub/ArtekmedRos2/ShellScripts/String_msgs_multiple_functions/build/string_node_manager/${CMAKE_INSTALL_MANIFEST}"
     "${CMAKE_INSTALL_MANIFEST_CONTENT}")
