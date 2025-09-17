#!/usr/bin/env python2.7
# -*- coding: utf-8 -*-

"""
Legacy Risk Calculator Application - Python 2.7
Financial Risk Management System

This is intentionally legacy code with old practices:
- Python 2.7 syntax
- print statements instead of functions
- No async operations
- Basic CGI-style web interface
- Global variables and state
- Old string formatting
"""

import cgi
import cgitb
import datetime
import math
import random
import sys
import os

# Enable CGI error reporting
cgitb.enable()

# Legacy global variables - not thread safe!
portfolio_data = {
    'AAPL': 150000.00,
    'GOOGL': 200000.00,
    'MSFT': 175000.00,
    'AMZN': 180000.00,
    'TSLA': 120000.00
}

risk_alerts = []

def print_header():
    """Print HTTP headers - old CGI style"""
    print "Content-Type: text/html"
    print ""

def print_html_start(title):
    """Print HTML document start"""
    print "<html><head><title>%s</title></head>" % title
    print "<body bgcolor='#f5f5f5'>"

def print_html_end():
    """Print HTML document end"""
    print "</body></html>"

def show_main_page():
    """Display main navigation page"""
    print_html_start("Legacy Risk Calculator")
    print "<h1>Financial Risk Management System v1.0</h1>"
    print "<h3>Legacy Python 2.7 Application - Needs Modernization!</h3>"
    print "<table border='1' cellpadding='10'>"
    print "<tr><td><a href='?action=calculate'>Calculate VaR</a></td></tr>"
    print "<tr><td><a href='?action=portfolio'>View Portfolio</a></td></tr>"
    print "<tr><td><a href='?action=monte_carlo'>Monte Carlo Simulation</a></td></tr>"
    print "<tr><td><a href='?action=alerts'>Risk Alerts</a></td></tr>"
    print "</table>"
    current_time = datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")
    print "<p><i>Current Time: %s</i></p>" % current_time
    print_html_end()

def calculate_var_page(form):
    """VaR calculation page"""
    print_html_start("VaR Calculation")
    print "<h2>Value at Risk (VaR) Calculator</h2>"
    
    symbol = form.getvalue('symbol')
    amount = form.getvalue('amount')
    
    if symbol and amount:
        try:
            amount_float = float(amount)
            var = calculate_var(symbol, amount_float)
            credit_risk = calculate_credit_risk(symbol)
            
            print "<h3>Risk Analysis for %s</h3>" % symbol
            print "<table border='1' cellpadding='5'>"
            print "<tr><td>Investment Amount</td><td>$%.2f</td></tr>" % amount_float
            print "<tr><td>Daily VaR (95%%)</td><td>$%.2f</td></tr>" % var
            print "<tr><td>Credit Risk Score</td><td>%.1f/10</td></tr>" % credit_risk
            print "<tr><td>Risk Level</td><td>%s</td></tr>" % get_risk_level(var/amount_float)
            print "</table>"
            
            # Add to alerts if high risk - using old string formatting
            if var/amount_float > 0.05:
                alert_msg = "HIGH RISK: %s VaR exceeds 5%% threshold at %s" % (
                    symbol, datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")
                )
                risk_alerts.append(alert_msg)
                
        except ValueError:
            print "<p style='color: red'>Error: Invalid amount format</p>"
    else:
        print "<form method='get'>"
        print "<input type='hidden' name='action' value='calculate'>"
        print "Stock Symbol: <input type='text' name='symbol' value='AAPL'><br><br>"
        print "Amount ($): <input type='text' name='amount' value='100000'><br><br>"
        print "<input type='submit' value='Calculate Risk'>"
        print "</form>"
    
    print "<p><a href='?'>Back to Main</a></p>"
    print_html_end()

def show_portfolio():
    """Display portfolio holdings"""
    print_html_start("Portfolio View")
    print "<h2>Current Portfolio Holdings</h2>"
    print "<table border='1' cellpadding='5'>"
    print "<tr><th>Symbol</th><th>Value ($)</th><th>Daily VaR</th><th>Risk %</th></tr>"
    
    total_value = 0.0
    total_var = 0.0
    
    # Legacy iteration using items() - Python 2.7 style
    for symbol, value in portfolio_data.items():
        var = calculate_var(symbol, value)
        risk_pct = (var / value) * 100
        
        print "<tr>"
        print "<td>%s</td>" % symbol
        print "<td>$%.2f</td>" % value
        print "<td>$%.2f</td>" % var
        print "<td>%.2f%%</td>" % risk_pct
        print "</tr>"
        
        total_value += value
        total_var += var
    
    total_risk_pct = (total_var / total_value) * 100
    print "<tr><th>TOTAL</th><th>$%.2f</th><th>$%.2f</th><th>%.2f%%</th></tr>" % (
        total_value, total_var, total_risk_pct
    )
    print "</table>"
    
    print "<p><strong>Portfolio Summary:</strong></p>"
    print "<ul>"
    print "<li>Total Portfolio Value: $%.2f</li>" % total_value
    print "<li>Total Daily VaR: $%.2f</li>" % total_var
    print "<li>Portfolio Risk Ratio: %.2f%%</li>" % total_risk_pct
    print "</ul>"
    
    print "<p><a href='?'>Back to Main</a></p>"
    print_html_end()

def monte_carlo_simulation():
    """Simple Monte Carlo risk simulation"""
    print_html_start("Monte Carlo Simulation")
    print "<h2>Monte Carlo Risk Simulation</h2>"
    
    print "<p>Running 1000 simulations for portfolio risk scenarios...</p>"
    
    # Simple Monte Carlo - legacy approach
    simulations = 1000
    results = []
    
    for i in range(simulations):
        portfolio_return = 0.0
        for symbol, value in portfolio_data.items():
            volatility = get_historical_volatility(symbol)
            # Simple random walk
            daily_return = random.gauss(0.001, volatility)  # Small positive drift
            portfolio_return += (value / sum(portfolio_data.values())) * daily_return
        
        results.append(portfolio_return)
    
    # Calculate percentiles - old way without numpy
    results.sort()
    var_95 = results[int(0.05 * len(results))]
    var_99 = results[int(0.01 * len(results))]
    
    print "<table border='1' cellpadding='5'>"
    print "<tr><td>Simulations Run</td><td>%d</td></tr>" % simulations
    print "<tr><td>95%% VaR (Daily)</td><td>%.4f%%</td></tr>" % (var_95 * 100)
    print "<tr><td>99%% VaR (Daily)</td><td>%.4f%%</td></tr>" % (var_99 * 100)
    print "<tr><td>Expected Return</td><td>%.4f%%</td></tr>" % (sum(results)/len(results) * 100)
    print "</table>"
    
    print "<p><a href='?'>Back to Main</a></p>"
    print_html_end()

def show_risk_alerts():
    """Display risk alerts"""
    print_html_start("Risk Alerts")
    print "<h2>Risk Alert System</h2>"
    
    if not risk_alerts:
        print "<p>No risk alerts at this time.</p>"
    else:
        print "<ul>"
        for alert in risk_alerts:
            print "<li>%s</li>" % alert
        print "</ul>"
    
    print "<p><a href='?'>Back to Main</a></p>"
    print_html_end()

def calculate_var(symbol, amount):
    """Calculate Value at Risk using historical simulation"""
    volatility = get_historical_volatility(symbol)
    confidence_level = 1.645  # 95% confidence Z-score
    return amount * volatility * confidence_level

def get_historical_volatility(symbol):
    """Get mock historical volatility data"""
    volatilities = {
        'AAPL': 0.025,
        'GOOGL': 0.030,
        'MSFT': 0.022,
        'AMZN': 0.035,
        'TSLA': 0.055
    }
    return volatilities.get(symbol, 0.025)

def calculate_credit_risk(symbol):
    """Simple credit risk scoring (1-10 scale)"""
    # Use hash for consistent but varied scores
    symbol_hash = hash(symbol)
    return 1 + (abs(symbol_hash) % 90) / 10.0

def get_risk_level(risk_ratio):
    """Categorize risk level"""
    if risk_ratio < 0.02:
        return "LOW"
    elif risk_ratio < 0.05:
        return "MEDIUM"
    else:
        return "HIGH"

def main():
    """Main CGI handler"""
    print_header()
    
    # Parse form data - old CGI way
    form = cgi.FieldStorage()
    action = form.getvalue('action')
    
    if action == 'calculate':
        calculate_var_page(form)
    elif action == 'portfolio':
        show_portfolio()
    elif action == 'monte_carlo':
        monte_carlo_simulation()
    elif action == 'alerts':
        show_risk_alerts()
    else:
        show_main_page()

if __name__ == "__main__":
    main()