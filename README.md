# What is this?

A project seed for a C# dotnet API ("PaylocityBenefitsCalculator").  It is meant to get you started on the Paylocity BackEnd Coding Challenge by taking some initial setup decisions away.

The goal is to respect your time, avoid live coding, and get a sense for how you work.

# Coding Challenge

**Show us how you work.**

Each of our Paylocity product teams operates like a small startup, empowered to deliver business value in
whatever way they see fit. Because our teams are close knit and fast moving it is imperative that you are able
to work collaboratively with your fellow developers. 

This coding challenge is designed to allow you to demonstrate your abilities and discuss your approach to
design and implementation with your potential colleagues. You are free to use whatever technologies you
prefer but please be prepared to discuss the choices you’ve made. We encourage you to focus on creating a
logical and functional solution rather than one that is completely polished and ready for production.

The challenge can be used as a canvas to capture your strengths in addition to reflecting your overall coding
standards and approach. There’s no right or wrong answer. It’s more about how you think through the
problem. We’re looking to see your skills in all three tiers so the solution can be used as a conversation piece
to show our teams your abilities across the board.

Requirements will be given separately.

# Solution by Jiri Buchlovsky

** Salary calculator **
Application calculates employees bi-weekly or monthly net salary from their anual gross salary and provides information about employees and their dependents.

Data are stored in memory and results are provided through REST API.

** Usage **
Option 1: Swagger
Swagger is smart tool documenting and providing easy acces to test API endpoints directly in your solution.
Endpoints can be tested by clicking "Try it out" at each endpoint.

1.1 Start API solution using the Visual Studio "start" button.
1.2 Select Get bi-weekly salary endpoint from Payroll Container and click "Try it out".
1.3 Enter id between 1 and 3. If you select IDs outside of the range the solution will notify you about an error.
1.4 Click "Execute" button.
1.5 Field "Response body" under the Parameters setup will contain the solution JSON. Property data contains the calculated value.

Endpoint Get monthly salary works exactly the same.


Option 2: Postman or simillar tools
Instead of Swagger you can use tools like Postman.
This allows you to test the solution outside so you are not biased with using the single solution.

2.1 Start API solution using the Visual Studio "Api" button.
2.2 Open Postman desktop app (not available in web app). 
2.3 Create new request where url is https://localhost:7124/api/v1/employees/{id} where id is between 1 and 3 for valid result, outside for error notification.
2.4 Click "Send" button.
2.5 Bottom section of postman will again contain JSON file with property called data containing the result salary value.
