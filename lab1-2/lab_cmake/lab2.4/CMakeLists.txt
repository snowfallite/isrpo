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