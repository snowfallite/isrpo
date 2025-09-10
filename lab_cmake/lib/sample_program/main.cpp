#include <iostream>
#include "sample_module.h"
#include <windows.h>
using namespace std;

int main() {
    SetConsoleOutputCP(1251);

    
    cout << "HEllo world. СУмма : " << sum(4,5) << endl;
    system("pause");
    return 0;
}