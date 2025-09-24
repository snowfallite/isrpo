#include <opencv2/core.hpp>
#include <opencv2/imgcodecs.hpp>
#include <opencv2/highgui.hpp>
#include <iostream>

using namespace cv;
using namespace std;
int main(int argc, char** argv)
{
    Mat image;
    image = imread("pic.jpg", IMREAD_COLOR); // чтение файла с картинкой
    if (image.empty()) // проверка корректен ли был ввод
    {
        cout << "Could not open or find the image" << std::endl;
        return -1;
    }
    namedWindow("Display window", WINDOW_AUTOSIZE); // создание окна для отображения.
    imshow("Display window", image); // показываем картинку внутри окна.
    waitKey(0); // ждем нажатия клавиши
    return 0;
}
