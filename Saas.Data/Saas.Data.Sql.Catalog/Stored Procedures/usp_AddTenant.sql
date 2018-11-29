CREATE PROCEDURE [dbo].[usp_AddTenant]
	@name nvarchar(128),
	@productId int
AS
	INSERT INTO [Tenant]([Name],[ProductId]) 
	VALUES (@name, @productId)
RETURN 0
