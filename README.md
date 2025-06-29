# StockManagement

A website for managing stock and sales at various venues.

This project was initially set up for an artist to manage original artwork and the prints and cards created from them.

## User Interface - Blazor

The User interface is written as a Blazor Webassembly application hosted in a C# Web API project.

## Data access

The Web API uses service and repository layers to access a SQL Server database.

## Authentication

Internal using AspNet standard format.

## Release configuration

Released using Docker containers. One for the website and one for the SQL Server database.
