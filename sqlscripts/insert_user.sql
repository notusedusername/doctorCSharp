IF OBJECT_ID('insert_user') IS NULL
BEGIN	
	EXEC ('CREATE PROCEDURE insert_user AS PRINT ''empty''')
END
GO

ALTER PROCEDURE insert_user
	@name VARCHAR(300),
	@email VARCHAR(30),
	@password VARCHAR(100)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM Users WHERE email = @email)
		RETURN -1
	
	INSERT INTO Users ([name], email, [password]) VALUES (@name, @email, @password)

END
GO