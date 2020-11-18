IF OBJECT_ID('delete_patient') IS NULL
BEGIN	
	EXEC ('CREATE PROCEDURE delete_patient AS PRINT ''empty''')
END
GO

ALTER PROCEDURE delete_patient
	@TAJ_nr VARCHAR(11)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM Patient WHERE TAJ_nr = @TAJ_nr)
		DELETE FROM Patient WHERE TAJ_nr = @TAJ_nr ;
	ELSE
		return -1

END
GO