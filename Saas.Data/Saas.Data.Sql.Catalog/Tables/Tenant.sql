﻿CREATE TABLE [dbo].[Tenant]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NewId(), 
    [Name] NVARCHAR(50) NOT NULL, 
	[CreatedOn] DATETIME NOT NULL DEFAULT GetDate(), 
    [ProductId] INT NOT NULL
)
