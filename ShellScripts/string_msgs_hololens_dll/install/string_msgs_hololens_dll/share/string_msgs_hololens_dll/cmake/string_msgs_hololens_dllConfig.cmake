# generated from ament/cmake/core/templates/nameConfig.cmake.in

# prevent multiple inclusion
if(_string_msgs_hololens_dll_CONFIG_INCLUDED)
  # ensure to keep the found flag the same
  if(NOT DEFINED string_msgs_hololens_dll_FOUND)
    # explicitly set it to FALSE, otherwise CMake will set it to TRUE
    set(string_msgs_hololens_dll_FOUND FALSE)
  elseif(NOT string_msgs_hololens_dll_FOUND)
    # use separate condition to avoid uninitialized variable warning
    set(string_msgs_hololens_dll_FOUND FALSE)
  endif()
  return()
endif()
set(_string_msgs_hololens_dll_CONFIG_INCLUDED TRUE)

# output package information
if(NOT string_msgs_hololens_dll_FIND_QUIETLY)
  message(STATUS "Found string_msgs_hololens_dll: 0.0.0 (${string_msgs_hololens_dll_DIR})")
endif()

# warn when using a deprecated package
if(NOT "" STREQUAL "")
  set(_msg "Package 'string_msgs_hololens_dll' is deprecated")
  # append custom deprecation text if available
  if(NOT "" STREQUAL "TRUE")
    set(_msg "${_msg} ()")
  endif()
  # optionally quiet the deprecation message
  if(NOT ${string_msgs_hololens_dll_DEPRECATED_QUIET})
    message(DEPRECATION "${_msg}")
  endif()
endif()

# flag package as ament-based to distinguish it after being find_package()-ed
set(string_msgs_hololens_dll_FOUND_AMENT_PACKAGE TRUE)

# include all config extra files
set(_extras "")
foreach(_extra ${_extras})
  include("${string_msgs_hololens_dll_DIR}/${_extra}")
endforeach()
