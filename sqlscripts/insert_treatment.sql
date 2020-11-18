IF OBJECT_ID('insert_treatment') IS NULL
BEGIN	
	EXEC ('CREATE PROCEDURE insert_treatment AS PRINT ''empty''')
END
GO

ALTER PROCEDURE insert_treatment
	@patient_id INT,
	@arrival DATETIME,
	@complaint VARCHAR(1000),
	@diagnosis VARCHAR(4000),
	@isClosed CHAR(1)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM TREATMENT WHERE patient_id = @patient_id)
		RETURN -1
	
	ELSE INSERT INTO Treatment (patient_id, arrival,complaint,diagnosis,isClosed) VALUES (@patient_id, @arrival,@complaint,@diagnosis,@isClosed)

END
GO