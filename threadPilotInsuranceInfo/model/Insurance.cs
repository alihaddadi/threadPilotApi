namespace threadpilot.models.insurance;

public class Insurance
{
    public string SSN { get; set; }
    public string InsuranceHolderName { get; set; }
    public double InsurancePrice { get; set; }
    public string InsuranceType { get; set; }
    public string VehicleRegistrationNumber{ get; set; }

    public Insurance(string ssn, string insuranceHolderName, double insurancePrice, string insuranceType, string vehicleRegistrationNumber)
    {
        SSN = ssn;
        InsuranceHolderName = insuranceHolderName;
        InsurancePrice = insurancePrice;
        InsuranceType = insuranceType;
        VehicleRegistrationNumber = vehicleRegistrationNumber;
    }
}