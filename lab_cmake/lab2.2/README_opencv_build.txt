# Сборка OpenCV с target INSTALL
# 1. Генерируем решение OpenCV (один раз):
cmake -S opencv-4.12.0 -B _build/opencv -DCMAKE_BUILD_TYPE=Release -DBUILD_SHARED_LIBS=ON -DCMAKE_INSTALL_PREFIX="${CMAKE_SOURCE_DIR}/_build/install"

# 2. Собираем и устанавливаем OpenCV:
cmake --build _build/opencv --config Release --target INSTALL

# После этого используйте такой CMakeLists.txt для своего приложения:

cmake_minimum_required(VERSION 3.10)
project(lab2.2)

# Пути к установленным include, lib, bin OpenCV
set(OpenCV_INCLUDE_DIRS "${CMAKE_SOURCE_DIR}/_build/install/include")
set(OpenCV_LIB_DIR "${CMAKE_SOURCE_DIR}/_build/install/x64/vc17/lib")
set(OpenCV_BIN_DIR "${CMAKE_SOURCE_DIR}/_build/install/x64/vc17/bin")

set(OpenCV_LIBS
    opencv_core4120.lib
    opencv_imgproc4120.lib
    opencv_highgui4120.lib
    opencv_imgcodecs4120.lib
)

add_executable(main sample_opencv/main_opencv.cpp)

target_include_directories(main PRIVATE ${OpenCV_INCLUDE_DIRS})

foreach(lib ${OpenCV_LIBS})
    target_link_libraries(main PRIVATE ${OpenCV_LIB_DIR}/${lib})
endforeach()

add_custom_command(TARGET main POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
        ${OpenCV_BIN_DIR}/opencv_core4120.dll
        ${OpenCV_BIN_DIR}/opencv_imgproc4120.dll
        ${OpenCV_BIN_DIR}/opencv_highgui4120.dll
        ${OpenCV_BIN_DIR}/opencv_imgcodecs4120.dll
        $<TARGET_FILE_DIR:main>
)
