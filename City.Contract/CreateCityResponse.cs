namespace City.Contract;

public record CreateCityResponse(
    Guid id,
    String latitude,
    String longitude,
    DateTime lastModified
);