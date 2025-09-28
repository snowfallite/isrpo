#include <iostream>
#include "config.h"

#ifdef USE_LIB1
#include "libOne.h"
#endif
#ifdef USE_LIB2
#include "libTwo.h"
#endif
#ifdef USE_LIB3
#include "libThree.h"
#endif

int main() {
    setlocale(LC_ALL, "RU");
#ifdef USE_LIB1
    std::cout << "libOne active : " << fooOne(5,5) << std::endl;
#endif

#ifdef USE_LIB2 
    std::cout << "libTwo active : " << fooTwo(5,5) << std::endl;
#endif

#ifdef USE_LIB3
    std::cout << "libThree active : " << fooThree(5,5) << std::endl;
#endif

#ifndef USE_LIB1
#ifndef USE_LIB2
#ifndef USE_LIB3
    std::cout << "Ни одна библиотека не выбрана" << std::endl;
#endif
#endif
#endif

    system("pause");
    return 0;
}