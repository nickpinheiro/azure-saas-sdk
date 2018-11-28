CREATE PROCEDURE [dbo].[usp_AddTenant]
	@name nvarchar(128)
AS
	INSERT INTO [Tenant]([Name]) 
	VALUES (@name)
RETURN 0
