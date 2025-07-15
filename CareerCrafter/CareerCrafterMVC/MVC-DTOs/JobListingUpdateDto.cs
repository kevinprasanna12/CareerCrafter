namespace CareerCrafterMVC.Models;

public class JobListingUpdateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public decimal ? Salary { get; set; }
    public string Qualifications { get; set; }
}
