IF OBJECT_ID('delete_patient') IS NULL
BEGIN	
	EXEC ('CREATE PROCEDURE delete_patient AS PRINT ''empty''')
END
GO

ALTER PROCEDURE delete_patient
	@id INT
AS
BEGIN
	IF EXISTS (SELECT 1 FROM Patient WHERE id = @id)
	BEGIN
		DELETE FROM Treatment WHERE id = @id        -- Delete all treatments of deleted patient first, because of the foreign key constraint on patient_id
		DELETE FROM Patient WHERE id = @id
	END
	ELSE
		return -1

END
GO