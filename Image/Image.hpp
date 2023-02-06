#include <string>
#include <vector>
namespace Tahsin
{
    struct Color
    {
    public:
        unsigned char r, g, b, a;
        std::string GetHex();
        Color GetNegative();
        Color GetGreyScale();
        Color RedMask();
        Color GreenMask();
        Color BlueMask();
        Color AlphaMask();
        Color();
        Color(unsigned char r, unsigned char g, unsigned char b);
        Color(unsigned char r, unsigned char g, unsigned char b, unsigned char a);
        Color(std::string hexValue);
        Color GetMidColor(Color color);
        Color operator*(float value);
        Color operator+(char value);
    };
    struct CustomColorSpace
    {
        public:
            Color from, to;
            CustomColorSpace();
            CustomColorSpace(Color from, Color to);
            int RedRange();
            int BlueRange();
            int GreenRange();
            int AlphaRange();
            Color MoveThrough(float value = 0.5f);
    };
    
    std::vector<std::vector<Color>> GenerateColorVariants(Color source, int width, int height);
    std::vector<std::vector<Color>> GenerateMidColorTable(std::vector<Color> a, std::vector<Color> b);
    std::vector<std::vector<Color>> ScaleColorArray(std::vector<std::vector<Color>> resource, int scaleX = 10, int scaleY = 10);
    std::vector<std::vector<Color>> GenerateColorTransition(std::vector<Color> a, std::vector<Color> b, int step);
    std::vector<std::vector<Color>> MirrorVertical(std::vector<std::vector<Color>> source);
    std::vector<std::vector<Color>> Subversion(std::vector<std::vector<Color>> source);
    std::vector<std::vector<Color>> Contrast(std::vector<std::vector<Color>> source, float value);
    void ApplyContrast(std::vector<std::vector<Color>> &source, float value);
    std::vector<std::vector<Color>> Brightness(std::vector<std::vector<Color>> source, int value);
    void ApplyBrightness(std::vector<std::vector<Color>> &source, int value);
    std::vector<char> GetBMPHeader(std::vector<std::vector<Color>> source);
    std::vector<char> ToBMPColorCode(std::vector<std::vector<Color>> source);
    std::vector<char> ToBMP(std::vector<std::vector<Color>> source);
}