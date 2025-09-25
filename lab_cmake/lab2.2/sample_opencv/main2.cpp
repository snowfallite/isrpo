#include "opencv2/objdetect/objdetect.hpp"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"

#include <iostream>
#include <stdio.h>

using namespace std;
using namespace cv;

int main()
{
    Mat image;
    image = imread("C:\\Users\\sunny\\Desktop\\college\\isrpo\\lab_cmake\\lab2.2\\app\\x64\\Debug\\pic.jpg");
    namedWindow("window1", 1);   imshow("window1", image);

    // Загружаем xml файл (.xml file)
    CascadeClassifier face_cascade;
    face_cascade.load("C:\\Users\\sunny\\Desktop\\college\\isrpo\\lab_cmake\\lab2.2\\_build\\install\\etc\\haarcascades\\haarcascade_frontalface_alt2.xml");

    // Находим лица на фотографии
    std::vector<Rect> faces;
    face_cascade.detectMultiScale(image, faces, 1.1, 2, 0, Size(30, 30));

    // Рисуем круги вокруг лиц
    for (int i = 0; i < faces.size(); i++)
    {
        Point center(faces[i].x + faces[i].width * 0.5, faces[i].y + faces[i].height * 0.5);
        ellipse(image, center, Size(faces[i].width * 0.5, faces[i].height * 0.5), 0, 0, 360, Scalar(255, 0, 255), 4, 8, 0);
    }

    imshow("Detected Face", image);

    waitKey(0);
    return 0;
}
