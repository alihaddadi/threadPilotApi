namespace threadpilot.models.vehicle;
public class Vehicle
{
    public string RegistrationNumber { get; set; }
    public string Model { get; set; }
    public string Make { get; set; }
    public string ChassiNumber { get; set; }

    public Vehicle(string registrationNumber, string model, string make, string chassiNumber)
    {
        RegistrationNumber = registrationNumber;
        Model = model;
        Make = make;
        ChassiNumber = chassiNumber;
    }
}