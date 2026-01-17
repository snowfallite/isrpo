<<<<<<< HEAD:lab1-2/lab_cmake/lab2.4/CMakeLists.txt
cmake_minimum_required(VERSION 3.10)
project(Lab24 CXX)


option(USE_LIB1 "Использовать библиотеку lib1" ON)
option(USE_LIB2 "Использовать библиотеку lib2" ON)
option(USE_LIB3 "Использовать библиотеку lib3" ON)

add_executable(main program/main.cpp)

if (USE_LIB1)
    add_library(lib1 libs/libOne/libOne.cpp)
    target_include_directories(lib1 PUBLIC libs/libOne)
    target_link_libraries(main PRIVATE lib1)
endif()


if (USE_LIB2)
    add_library(lib2 libs/libTwo/libTwo.cpp)
    target_include_directories(lib2 PUBLIC libs/libTwo)
    target_link_libraries(main PRIVATE lib2)
endif()


if (USE_LIB3)
    add_library(lib3 libs/libThree/libThree.cpp)
    target_include_directories(lib3 PUBLIC libs/libThree)
    target_link_libraries(main PRIVATE lib3)
endif()


configure_file(config.h.in config.h)
target_include_directories(main PRIVATE ${CMAKE_BINARY_DIR})
=======
cmake_minimum_required(VERSION 4.1)
project(lastTwo)
set(BUILD_SHARED_LIBS FALSE)

option(USE_LIB1 "Использовать библиотеку lib1" ON)
option(USE_LIB2 "Использовать библиотеку lib2" OFF)
option(USE_LIB3 "Использовать библиотеку lib3" OFF)

configure_file(config.h.in config.h)

# --- lib1 ---
if (USE_LIB1)
    add_library(libOne libs/libOne/libOne.cpp)
    target_include_directories(libOne PUBLIC libs/libOne) 
endif()

# --- lib2 ---
if (USE_LIB2)
    add_library(libTwo libs/libTwo/libTwo.cpp)
    target_include_directories(libTwo PUBLIC libs/libTwo)
endif()

# --- lib3 ---
if (USE_LIB3)
    add_library(libThree libs/libThree/libThree.cpp)
    target_include_directories(libThree PUBLIC libs/libThree)
endif()


add_executable(main program/main.cpp)
target_include_directories(main PRIVATE "${CMAKE_BINARY_DIR}")


if (USE_LIB1)
    target_link_libraries(main PRIVATE libOne)
endif()
if (USE_LIB2)
    target_link_libraries(main PRIVATE libTwo)
endif()
if (USE_LIB3)
    target_link_libraries(main PRIVATE libThree)
endif()
>>>>>>> 04498137eb724510bf4f5f9f714dcfd458dbe926:lab_cmake/lab2.4/CMakeLists.txt
