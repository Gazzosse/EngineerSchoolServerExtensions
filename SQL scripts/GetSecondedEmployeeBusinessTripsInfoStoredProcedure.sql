CREATE OR REPLACE FUNCTION public."getSecondedEmployeeBusinessTripsInfo"(val_employeeid uuid)
	RETURNS TABLE(
		RowNumber BIGINT,
		BusinessTripStart DATE,
		CityName TEXT,
		BusinessTripReason TEXT,
		StateLocalName TEXT
	)
	LANGUAGE plpgsql
	AS $function$
	BEGIN
		RETURN QUERY
			SELECT 
				ROW_NUMBER() OVER (ORDER BY docMi."BusinessTripStart") as rowNumber,
				docMi."BusinessTripStart"::DATE, 
				cities."Name", 
				docMi."BusinessTripReason", 
				localStates."Name"
			FROM public."dvtable_{30eb9b87-822b-4753-9a50-a1825dca1b74}" docMi
			JOIN public."dvtable_{1b1a44fb-1fb1-4876-83aa-95ad38907e24}" cities 
				ON cities."RowID" = docMi."Cities"
			JOIN public."dvtable_{da37ca71-a977-48e9-a4fd-a2b30479e824}" localStates 
				ON localStates."ParentRowID" = docMi."State"
					AND localStates."LocaleID" = 1049
			WHERE docMi."Kind" = '6cf0521a-a2f2-4595-a65b-3c26f17f60a8'
				AND docMi."SecondedEmployee" = val_employeeid
			ORDER BY docMi."BusinessTripStart"::DATE;
	END;
	$function$;