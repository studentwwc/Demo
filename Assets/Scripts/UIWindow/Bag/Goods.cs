namespace DarkGod.Bag
{
    public class Goods
    {
        public GoodsItem item;
        public int count;
        public int bagIndex;

        public Goods(GoodsItem item,int count,int bagIndex)
        {
            this.item = item;
            this.count = count;
            this.bagIndex = bagIndex;
        }
    }
}