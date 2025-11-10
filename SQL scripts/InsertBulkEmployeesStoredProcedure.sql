CREATE OR REPLACE FUNCTION public."InsertBulkEmployees"(
    parent_row_ids UUID[],
    first_names TEXT[],
    middle_names TEXT[],
    last_names TEXT[]
)
	RETURNS BIGINT
	LANGUAGE plpgsql
	AS $function$
	DECLARE
	    inserted_count BIGINT;
	BEGIN
	    IF array_length(parent_row_ids, 1) != array_length(first_names, 1)
	       OR array_length(parent_row_ids, 1) != array_length(middle_names, 1)
	       OR array_length(parent_row_ids, 1) != array_length(last_names, 1) THEN
	        RAISE EXCEPTION 'Все массивы должны быть одинаковой длины';
	    END IF;
	    
	    INSERT INTO public."dvtable_{dbc8ae9d-c1d2-4d5e-978b-339d22b32482}"(
	        "ChangeServerID",
	        "ParentRowID",
	        "SDID",
	        "FirstName", 
	        "MiddleName",
	        "LastName",
	        "Position",
	        "Importance",
	        "CardEmployeeKindSpecified",
	        "InactiveStatus",
	        "UseThinClient",
	        "AskForKeyContainerPassword"
	    )
	    SELECT 
	        '00000000-0000-0000-0000-000000000000'::UUID,
	        parent_row_id,
	        '4fbfcd90-945e-4497-ac3d-61104410e6a9'::UUID,
	        first_name,
	        middle_name, 
	        last_name,
	        'c4f438e9-86d7-4409-8e77-85ee6f8e14eb'::UUID,
	        '0',
	        'false',
	        '0',
	        'false',
	        '0'
	    FROM UNNEST(parent_row_ids, first_names, middle_names, last_names) 
	         AS t(parent_row_id, first_name, middle_name, last_name);
	    
	    GET DIAGNOSTICS inserted_count = ROW_COUNT;
	    
	    UPDATE dvsys_instances_date
	    SET "ChangeDateTime" = NOW()
	    WHERE "InstanceID" = '6710b92a-e148-4363-8a6f-1aa0eb18936c'::UUID;
	    
	    RETURN inserted_count;
	END;
	$function$;
