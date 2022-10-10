namespace TahsinsLibrary.Layers
{
    public abstract class Layer<T>
    {
        public T[] dataVarierty;
        protected int[,] permission;
        protected T[,] data;
        public Layer(int width, int height, int permissionCount)
        {
            data = new T[width, height];
            permission = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    permission[i, j] = permissionCount;
                }
            }
        }
        public abstract void GenerateData();
        public abstract T[,] GetData();
    }
}