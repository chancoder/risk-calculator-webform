# Risk Calculator - ASP.NET Web Forms Application

This project is a modernized conversion of a legacy Python 2.7 risk calculator application to ASP.NET Web Forms.

## Project Overview

The original Python application was a CGI-based financial risk management system that provided:
- Value at Risk (VaR) calculations
- Portfolio analysis
- Monte Carlo simulations
- Risk alert management

This ASP.NET Web Forms version provides the same functionality with a modern, responsive web interface.

## Features

### 1. VaR Calculation (`CalculateVar.aspx`)
- Calculate Value at Risk for individual stocks
- Credit risk scoring
- Risk level categorization (Low/Medium/High)
- Form validation and error handling

### 2. Portfolio View (`Portfolio.aspx`)
- Display current portfolio holdings
- Calculate individual and total VaR
- Portfolio risk analysis and summary

### 3. Monte Carlo Simulation (`MonteCarlo.aspx`)
- Run risk simulations with configurable iterations
- Calculate 95% and 99% VaR percentiles
- Expected return calculations
- Box-Muller transform for normal distribution

### 4. Risk Alerts (`RiskAlerts.aspx`)
- View and manage risk alerts
- Session-based alert storage
- Clear alerts functionality

## Technical Architecture

### Business Logic Classes (`App_Code/`)
- **`RiskCalculator.cs`**: Core risk calculation logic
- **`MonteCarloService.cs`**: Monte Carlo simulation engine
- **`RiskAlertService.cs`**: Alert management service

### Key Improvements over Python Version
1. **Modern Web Framework**: ASP.NET Web Forms with server controls
2. **Type Safety**: Strong typing with C#
3. **Better Error Handling**: Try-catch blocks and validation
4. **Responsive Design**: Modern CSS with mobile support
5. **Session Management**: Proper state management for alerts
6. **Code Organization**: Separation of concerns with code-behind files

## File Structure

```
├── Default.aspx                 # Main navigation page
├── CalculateVar.aspx           # VaR calculation page
├── Portfolio.aspx              # Portfolio view page
├── MonteCarlo.aspx             # Monte Carlo simulation page
├── RiskAlerts.aspx             # Risk alerts page
├── web.config                  # Application configuration
├── Global.asax                 # Global application events
├── Content/
│   └── Site.css               # Modern CSS styling
├── App_Code/
│   ├── RiskCalculator.cs      # Core business logic
│   ├── MonteCarloService.cs   # Simulation service
│   └── RiskAlertService.cs    # Alert management
├── Properties/
│   └── AssemblyInfo.cs        # Assembly information
└── *.designer.cs              # Auto-generated designer files
```

## Setup and Running

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.8
- IIS Express (included with Visual Studio)

### Running the Application
1. Open `RiskCalculatorWebForm.csproj` in Visual Studio
2. Restore NuGet packages if prompted
3. Press F5 or click "Start Debugging"
4. The application will open in your default browser

### Default Portfolio Data
The application includes sample portfolio data:
- AAPL: $150,000
- GOOGL: $200,000
- MSFT: $175,000
- AMZN: $180,000
- TSLA: $120,000

## Key Conversions from Python

### 1. CGI to Web Forms
- **Python**: CGI form handling with `cgi.FieldStorage()`
- **ASP.NET**: Server controls with postback events

### 2. String Formatting
- **Python**: Old-style `%` formatting and `.format()`
- **C#**: String interpolation and `String.Format()`

### 3. Random Number Generation
- **Python**: `random.gauss()` for normal distribution
- **C#**: Box-Muller transform implementation

### 4. Global Variables
- **Python**: Global dictionaries and lists
- **C#**: Session state and service classes

### 5. HTML Generation
- **Python**: String concatenation and print statements
- **ASP.NET**: Declarative markup with server controls

## Browser Compatibility
- Chrome (recommended)
- Firefox
- Edge
- Safari
- Mobile browsers (responsive design)

## Future Enhancements
- Database integration for persistent data
- Real-time market data integration
- Advanced charting with Chart.js or similar
- User authentication and authorization
- API endpoints for external integration
- Unit testing framework
- Logging and monitoring

## License
This project is provided as-is for educational and demonstration purposes.
