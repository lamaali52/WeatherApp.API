# WeatherApp.API

ASP.NET Core Web API - handles all database operations for WeatherApp.

## Endpoints
- `POST /api/Auth/login` - Validate user login
- `POST /api/Weather/save` - Save weather search result
- `GET /api/Weather/searches` - Get all weather searches
- `PUT /api/Weather/update` - Update weather record

## Stored Procedures
- `sp_ValidateUser` - Validates login credentials
- `sp_SaveWeatherSearch` - Saves weather data
- `sp_GetWeatherSearches` - Retrieves search history
- `sp_UpdateWeatherSearch` - Updates record and logs audit

## MVC Repository
https://github.com/lamaali52/WeatherApp.MVC
