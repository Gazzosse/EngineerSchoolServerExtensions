CREATE OR REPLACE VIEW employee_root_organization_via_ltree AS
SELECT 
    e."RowID",
    rd."Name"
FROM public."dvtable_{dbc8ae9d-c1d2-4d5e-978b-339d22b32482}" e
JOIN public."dvtable_{7473f07f-11ed-4762-9f1e-7ff10808ddd1}" d 
    ON d."RowID" = e."ParentRowID"
JOIN public."dvtable_{7473f07f-11ed-4762-9f1e-7ff10808ddd1}" rd 
    ON rd."SystemTreeID" = (
        CAST(SPLIT_PART(d."SectionTreeKey"::text, '.', 2) AS BIGINT)
    )