namespace City.Contract;

public record UpsertCityRequest(
    string name,
    double latitude,
    double longitude
);