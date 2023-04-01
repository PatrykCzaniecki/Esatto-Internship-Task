using System.ComponentModel.DataAnnotations;

namespace EsattoApp;

public class Customer
{
    public Customer(int id, string name, string vatIdentificationNumber, DateTime creationDate, string address)
    {
        Id = id;
        Name = name;
        VatIdentificationNumber = vatIdentificationNumber;
        CreationDate = creationDate;
        Address = address;
    }

    [Key] public int Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string VatIdentificationNumber { get; set; }

    public DateTime CreationDate { get; set; }

    [Required] public string Address { get; set; }
}