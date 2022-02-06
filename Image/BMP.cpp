#include <array>
#include <vector>
#include <iostream>
#include "ValueConversation.cpp"
#include "ArrayOperations.cpp"

using namespace std;
string *CreateBitmapInfoHeader(int width, int height)
{
    vector<string> temp;
    Tahsin::length = 2;
    temp.push_back(Tahsin::GetHexValue('B'));
    temp.push_back(Tahsin::GetHexValue('M'));
    
    Tahsin::length = 8;
    int paddingCount = width * 3 % 4;
    string tempS = Tahsin::GetHexValue(54 + width * height * 3 + height * paddingCount);
    string* tempSA = Tahsin::GroupAndReverse(tempS,2);
    for (int i = 0; i < tempS.size()/2; i++)
    {
        temp.push_back(tempSA[i]);
    }

    Tahsin::length = 4;
    string tempArr[] = {Tahsin::GetHexValue(0),Tahsin::GetHexValue(0)};
    

    return temp.data();
}
int main()
{
    string test;
    cin >> test;
    int groupSize;
    cin >> groupSize;
    string *temp = Tahsin::GroupAndReverse(test,groupSize);
    for (int i = 0; i < test.size()/groupSize; i++)
    {
        cout << temp[i] << endl;
    }
    
}
