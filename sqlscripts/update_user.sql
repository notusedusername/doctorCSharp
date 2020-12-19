IF OBJECT_ID('update_patient') IS NULL
BEGIN	
	EXEC ('CREATE PROCEDURE update_patient AS PRINT ''empty''')
END
GO

ALTER PROCEDURE update_patient
	@id INT,
	@name VARCHAR(300),
	@TAJ_nr VARCHAR(11),
	@address VARCHAR(1000),
	@phone VARCHAR(20)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM Patient WHERE id = @id)
		RETURN -1
	IF EXISTS (SELECT 1 FROM Patient WHERE TAJ_nr = @TAJ_nr AND id <> @id)
		RETURN -2
	ELSE
		UPDATE Patient SET [name] = @name, [address] = @address,phone = @phone, TAJ_nr = @TAJ_nr WHERE id = @id

END
GO
