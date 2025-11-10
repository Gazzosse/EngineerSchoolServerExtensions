CREATE OR REPLACE FUNCTION public."InsertBulkGroups"(
    group_names TEXT[]
)
	RETURNS BIGINT
	LANGUAGE plpgsql
	AS $function$
	DECLARE
	    inserted_count BIGINT;
	BEGIN
	    INSERT INTO public."dvtable_{7473f07f-11ed-4762-9f1e-7ff10808ddd1}"(
	        "ChangeServerID",
	        "ParentTreeRowID", 
	        "SDID",
	        "Name",
	        "Type"
	    )
	    SELECT 
	        '00000000-0000-0000-0000-000000000000'::UUID,
	        'bbf0ec19-c659-4cb6-b4d7-81dac5399991'::UUID,
	        '4fbfcd90-945e-4497-ac3d-61104410e6a9'::UUID,
	        group_name,
	        '1'
	    FROM UNNEST(group_names) AS group_name;
	    
	    GET DIAGNOSTICS inserted_count = ROW_COUNT;
	    
	    UPDATE dvsys_instances_date
	    SET "ChangeDateTime" = NOW()
	    WHERE "InstanceID" = '6710b92a-e148-4363-8a6f-1aa0eb18936c'::uuid;
	    
	    RETURN inserted_count;
	END;
	$function$;