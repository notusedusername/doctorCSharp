IF OBJECT_ID('set_patient_diagnosis') IS NULL
BEGIN	
	EXEC ('CREATE PROCEDURE set_patient_diagnosis AS PRINT ''empty''')
END
GO

ALTER PROCEDURE set_patient_diagnosis
	@patient_id INT,
	@diagnosis VARCHAR(4000)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM Patient WHERE id = @patient_id)
		return -1

	IF NOT EXISTS (SELECT 1 FROM Treatment WHERE patient_id = @patient_id AND isClosed = 'F')
		RETURN -2	-- There isn't any not diagnosed compliant of the patient

	UPDATE Treatment SET diagnosis = @diagnosis, isClosed = 'T' WHERE patient_id = @patient_id AND isClosed = 'F'

END
GO