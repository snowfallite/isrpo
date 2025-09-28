cmake_minimum_required(VERSION 3.10)
project(math)

set(NUM1 10 CACHE STRING "Первое число")
set(NUM2 5 CACHE STRING "Второе число")

math(EXPR A_int    "${NUM1}")
math(EXPR B_int    "${NUM2}")

math(EXPR SUM      "${A_int} + ${B_int}")
math(EXPR DIFF     "${A_int} - ${B_int}")
math(EXPR PRODUCT  "${A_int} * ${B_int}")

if (B_int STREQUAL "0")
  set(QUOTIENT "ДЕЛЕНИЕ НА 0")
else()
  math(EXPR QUOTIENT "${A_int} / ${B_int}")
endif()

message(STATUS "A = ${A_int}, B = ${B_int}")
message(STATUS "A + B = ${SUM}")
message(STATUS "A - B = ${DIFF}")
message(STATUS "A * B = ${PRODUCT}")
message(STATUS "A / B = ${QUOTIENT}")