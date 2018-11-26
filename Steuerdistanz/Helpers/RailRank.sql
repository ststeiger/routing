
SELECT 
	 kanton 
	,gemeindenummer 
	,gemeinde 
	,latitude 
	,longitude 
	--,from_50000/100.0e*6500*13 AS k50 
	--,from_80000/100.0e*6500*13 AS k80 
	--,from_100000/100.0e*6500*13 AS k100 
	--,from_125000/100.0e*6500*13 AS k125 
	--,from_150000/100.0e*6500*13 AS k150
	--,from_175000/100.0e*6500*13 AS k175 

	,AutoErlen.RD_Ort 
	,AutoErlen.RD_Entfernung 
	,ROUND(AutoErlen.RD_Dauer/3600.0, 2) AS RD_Dauer  

	--,AutoZurich.RD_Ort 
	--,AutoZurich.RD_Entfernung 
	--,AutoZurich.RD_Dauer 

	
	--,AutoSanktGallen.RD_Ort 
	--,AutoSanktGallen.RD_Entfernung 
	--,AutoSanktGallen.RD_Dauer 

		
	--,AutoBasel.RD_Ort 
	--,AutoBasel.RD_Entfernung 
	--,AutoBasel.RD_Dauer 

	--,AutoWinterthur.RD_Ort 
	--,AutoWinterthur.RD_Entfernung 
	--,AutoWinterthur.RD_Dauer 

	--,ZugErlen.RL_Ort 
	,ROUND(ZugErlen.RL_Dauer, 2) AS RL_Dauer  

	--,ZugZurich.RL_Ort 
	--,ZugZurich.RL_Dauer 

	--,ZugSanktGallen.RL_Ort 
	--,ZugSanktGallen.RL_Dauer 

	--,ZugBasel.RL_Ort 
	--,ZugBasel.RL_Dauer 

	--,ZugSanktGallen.RL_Ort 
	--,ZugSanktGallen.RL_Dauer 
	
	--,ZugWinterthur.RL_Ort 
	--,ZugWinterthur.RL_Dauer 


	,
	ROUND
	(
		(
		  ISNULL(AutoErlen.RD_Dauer, 0.0) 
		+ ISNULL(AutoZurich.RD_Dauer, 0.0) 
		+ ISNULL(AutoSanktGallen.RD_Dauer, 0.0) 
		+ ISNULL(AutoBasel.RD_Dauer, 0.0) 
		+ ISNULL(AutoSanktGallen.RD_Dauer, 0.0) 
		+ ISNULL(AutoWinterthur.RD_Dauer, 0.0) 
		) / (60*60)
		,2 
	) AS AutoDauer 

	, 
	ROUND
	(
		  ISNULL(ZugErlen.RL_Dauer, 0.0) 
		+ ISNULL(ZugZurich.RL_Dauer, 0.0) 
		+ ISNULL(ZugSanktGallen.RL_Dauer, 0.0) 
		+ ISNULL(ZugBasel.RL_Dauer, 0.0) 
		+ ISNULL(ZugSanktGallen.RL_Dauer, 0.0) 
		+ ISNULL(ZugWinterthur.RL_Dauer, 0.0) 
		,2
	) AS ZugDauer 

FROM __Steuern_2017 

LEFT JOIN __Steuern_2017_ZO_Road AS AutoErlen -- 3033 vs. 3022
	ON AutoErlen.RD_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND AutoErlen.RD_Ort = 'Erlen' 


LEFT JOIN __Steuern_2017_ZO_Road AS AutoZurich -- 3033 vs. 3022
	ON AutoZurich.RD_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND AutoZurich.RD_Ort = 'Zürich HB'

LEFT JOIN __Steuern_2017_ZO_Road AS AutoSanktGallen-- 3033 vs. 3022
	ON AutoSanktGallen.RD_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND AutoSanktGallen.RD_Ort = 'St. Gallen HB'

LEFT JOIN __Steuern_2017_ZO_Road AS AutoBasel -- 3033 vs. 3022
	ON AutoBasel.RD_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND AutoBasel.RD_Ort = 'Basel SBB'

LEFT JOIN __Steuern_2017_ZO_Road AS AutoWinterthur -- 3033 vs. 3022
	ON AutoWinterthur.RD_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND AutoWinterthur.RD_Ort = 'Winterthur'

LEFT JOIN __Steuern_2017_ZO_Rail AS ZugErlen
	ON ZugErlen.RL_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND ZugErlen.RL_Ort = 'Erlen' 

LEFT JOIN __Steuern_2017_ZO_Rail AS ZugZurich
	ON ZugZurich.RL_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND ZugZurich.RL_Ort = 'Zürich HB'

LEFT JOIN __Steuern_2017_ZO_Rail AS ZugSanktGallen
	ON ZugSanktGallen.RL_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND ZugSanktGallen.RL_Ort = 'St. Gallen HB'

LEFT JOIN __Steuern_2017_ZO_Rail AS ZugBasel
	ON ZugBasel.RL_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND ZugBasel.RL_Ort = 'Basel SBB'

LEFT JOIN __Steuern_2017_ZO_Rail AS ZugWinterthur 
	ON ZugWinterthur.RL_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND ZugWinterthur.RL_Ort = 'Winterthur'

-- WHERE __Steuern_2017_ZO_Road.RD_UID IS NULL 
WHERE ZugErlen.RL_UID IS NOT NULL 

ORDER BY ZugDauer ASC 
