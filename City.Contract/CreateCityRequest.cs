namespace City.Contract;

public record CreateCityRequest(
    string name,
    double latitude,
    double longitude
);