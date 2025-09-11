cmake_minimum_required(VERSION 4.1)
find_package(OpenCV REQUIRED)

project(main)
set(BUILD_SHARED_LIBS FALSE)
add_subdirectory(lib/sample_lib)
add_subdirectory(lib/sample_program)
add_subdirectory(opencv-4.12.0)