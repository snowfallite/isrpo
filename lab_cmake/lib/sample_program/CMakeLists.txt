add_executable(sample_program main.cpp)
include_directories(../sample_lib)
target_link_libraries(sample_program sample_lib)