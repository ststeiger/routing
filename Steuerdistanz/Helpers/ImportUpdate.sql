;WITH CTE AS 
(
	SELECT 
		 __Steuern_2015.kanton
		,__Steuern_2015.gemeindenummer
		,__Steuern_2015.gemeinde
		,__Steuern_2015.latitude
		,__Steuern_2015.longitude
		,t2.latitude AS sourceLat
		,t2.longitude AS sourceLong 
		,t2.gemeinde AS sourceGemeinde 
	FROM TestDb.dbo.__Steuern_2015 

	INNER JOIN TestDb.dbo.__Steuern_2016 AS t2 
		ON t2.gemeindenummer = __Steuern_2015.gemeindenummer
)
-- SELECT * FROM CTE 
UPDATE CTE SET latitude = sourceLat,longitude = sourceLong
