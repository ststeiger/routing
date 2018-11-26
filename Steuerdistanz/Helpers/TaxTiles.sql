
; WITH CTE AS 
(
	SELECT 
		 kanton
		,gemeindenummer
		,gemeinde
		,latitude
		,longitude
		,from_50000 
		,100 * PERCENT_RANK() OVER (ORDER BY from_50000) AS PercentBetter    
		,100 - 100* PERCENT_RANK() OVER (ORDER BY from_50000) AS PercentWorse     
		
	FROM __Steuern_2017
) 
SELECT * FROM CTE 
WHERE (1=2) 
OR gemeinde LIKE '%altstätten%'
OR gemeinde LIKE '%münsin%'
OR gemeinde LIKE '%oensingen%'
OR gemeinde LIKE '%gallen%'
OR gemeinde LIKE '%baar%'
OR gemeinde LIKE '%staufen%'



;WITH CTE AS 
(
	SELECT 
		 kanton
		,gemeindenummer
		,gemeinde
		,latitude
		,longitude
		,from_50000 
		,NTILE(10) OVER (ORDER BY from_50000) AS Decile   
	FROM __Steuern_2017 
)
SELECT 
	 Decile 
	,MIN(from_50000) AS decileLeast 
	,MAX(from_50000) AS decileGreatest 
FROM CTE 
GROUP BY Decile 
