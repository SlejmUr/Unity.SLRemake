namespace SLRemake.InventorySystem
{
    public static class ItemSerialGenerator
    {
        private static ushort _id;

        public static void Reset()
        {
            _id = 0;
        }

        public static ushort GenerateNext()
        {
            if (_id > 65000)
            {
                Reset();
            }
            _id++;
            return _id;
        }
    }

}