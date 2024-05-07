namespace GlassECommerce.Models
{
    public class CollectionProduct
    {
        public int CollectionId { get; set; }
        public int ProductId { get; set; }
        //
        public virtual Collection Collection { get; set; }
        public virtual Product Product { get; set; }
    }
}
