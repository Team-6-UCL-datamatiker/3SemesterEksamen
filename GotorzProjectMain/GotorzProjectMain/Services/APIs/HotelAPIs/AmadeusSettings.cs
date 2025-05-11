namespace GotorzProjectMain.Services.APIs.HotelAPIs;

// Singleton record for runtime storing of environment variables for use of Amadeus API's
public sealed record AmadeusSettings
(
    string? ApiKey,
    string? ApiSecret
);
