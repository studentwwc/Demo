using UnityEngine;

namespace DarkGod.Bag
{
    public abstract class BagItem:MonoBehaviour
    {
        public GoodsType GoodsType;
        

        public abstract void Receive(GoodsItem item);

    }
}