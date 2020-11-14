IF OBJECT_ID('insert_patient') IS NULL
BEGIN	
	EXEC ('CREATE PROCEDURE insert_patient AS PRINT ''empty''')
END
GO

ALTER PROCEDURE insert_patient
	@name VARCHAR(300),
	@TAJ_nr VARCHAR(11),
	@address VARCHAR(1000),
	@phone VARCHAR(20)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM Patient WHERE TAJ_nr = @TAJ_nr)
		RETURN -1
	
	INSERT INTO Patient ([name], TAJ_nr, [address], phone) VALUES (@name, @TAJ_nr, @address, @phone)

END
GO