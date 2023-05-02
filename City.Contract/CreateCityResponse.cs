namespace City.Contract;

public record CreateCityResponse(
    Guid id,
    string name,
    double latitude,
    double longitude,
    DateTime lastModified
);