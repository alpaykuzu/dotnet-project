namespace API.DTOs.Product
{
    public class ProductRequest
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
    }
}
