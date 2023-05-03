namespace City.Contract;
using System.Text.Json;
public record CreateCityRequest(
    string name,
    double latitude,
    double longitude
);