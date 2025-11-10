CREATE VIEW employee_root_organization AS
WITH RECURSIVE department_hierarchy AS (
	SELECT 
		e."RowID" as employee_id,
		d."RowID" as department_id,
		d."ParentTreeRowID",
		d."Name"
	FROM public."dvtable_{dbc8ae9d-c1d2-4d5e-978b-339d22b32482}" e
	JOIN public."dvtable_{7473f07f-11ed-4762-9f1e-7ff10808ddd1}" d 
		ON d."RowID" = e."ParentRowID"

	UNION ALL

	SELECT
		dh.employee_id,
		d."RowID" as department_id,
		d."ParentTreeRowID",
		d."Name"
	FROM public."dvtable_{7473f07f-11ed-4762-9f1e-7ff10808ddd1}" d
	INNER JOIN department_hierarchy dh ON d."RowID" = dh."ParentTreeRowID"
	WHERE dh."ParentTreeRowID" IS NOT NULL
		AND dh."ParentTreeRowID" != '00000000-0000-0000-0000-000000000000'
)
SELECT 
	employee_id as "RowID",
    "Name"
FROM department_hierarchy 
WHERE "ParentTreeRowID" = '00000000-0000-0000-0000-000000000000';