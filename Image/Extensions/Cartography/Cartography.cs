namespace TahsinsLibrary.Cartography
{
    public abstract class Marking
    {
        public abstract void EffectOnMap(ref Color[,] map);
    }
}