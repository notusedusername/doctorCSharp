IF OBJECT_ID('update_patient') IS NULL
BEGIN	
	EXEC ('CREATE PROCEDURE update_patient AS PRINT ''empty''')
END
GO

ALTER PROCEDURE update_patient
	@name VARCHAR(300),
	@TAJ_nr VARCHAR(11),
	@address VARCHAR(1000),
	@phone VARCHAR(20)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM Patient WHERE TAJ_nr = @TAJ_nr)
		UPDATE Patient SET [name] = @name, [address] = @address,phone = @phone WHERE TAJ_nr = @TAJ_nr;
	ELSE
		return -1

END
GO
