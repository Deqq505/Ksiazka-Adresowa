using SQLite;

namespace Ksiazka_Adresowa
{
    [System.ComponentModel.DataAnnotations.Schema.Table("customer")]
    public class Customer
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("customer_Name")]
        public string CustomerName { get; set; }
        
        [Column("mobile")]
        public string Mobile { get; set; }
        
        [Column("email")]
        public string Email { get; set; }
    } 
}
 