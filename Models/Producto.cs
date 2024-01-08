namespace Proyecto_API.Models
{
    public class Producto
    {
        public int? _idProduct { get; set; }
        public string _productName {  get; set; }
        public string _category { get; set; }
        public int _stock {  get; set; }
        public decimal _price { get; set; }
        public Boolean _state {  get; set; }
        public string _urlImage { get; set; }
    }
}
