using SQLite;

namespace Ksiazka_Adresowa
{
    [Table("customer")]
    public class Customer
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }
        
        [Column("customer_name"), MaxLength(50)]
        public string CustomerName { get; set; }
        
        [Column("mobile"), MaxLength(20)]
        public string Mobile { get; set; }
        
        [Column("email"), MaxLength(50)]
        public string Email { get; set; }
        
        [Column("address"), MaxLength(100)]
        public string Address { get; set; }
        
        [Column("city"), MaxLength(50)]
        public string City { get; set; }
        
        [Column("postal_code"), MaxLength(10)]
        public string PostalCode { get; set; }
        
        [Column("notes")]
        public string Notes { get; set; }
    }
}