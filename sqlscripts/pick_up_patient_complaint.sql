IF OBJECT_ID('pick_up_patient_complaint') IS NULL
BEGIN	
	EXEC ('CREATE PROCEDURE pick_up_patient_complaint AS PRINT ''empty''')
END
GO

ALTER PROCEDURE pick_up_patient_complaint
	@patient_id INT,
	@complaint VARCHAR(1000)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM Patient WHERE id = @patient_id)
		RETURN -1
	ELSE IF EXISTS (SELECT 1 FROM Treatment WHERE patient_id = @patient_id AND isClosed = 'F')
		RETURN -2 -- Can not pick up another complaint while there is an open complaint (the patient waiting for treatment)
	
	INSERT INTO Treatment (patient_id, complaint) VALUES (@patient_id, @complaint)

END
GO