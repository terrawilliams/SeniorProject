SELECT
	occ.OccName,
	q.QualityDescription,
	i.ItemDescription,
	a.ActionDescription
FROM
	Occupation occ
LEFT JOIN Quality q ON q.QualityID == occ.OccQuality
INNER JOIN OccItems oi ON oi.OccID == occ.OccID
INNER JOIN Items i ON i.ItemID == oi.ItemID
INNER JOIN OccAction oa ON oa.OccID == occ.OccID
INNER JOIN Actions a ON a.ActionID == oa.ActionID

WHERE occ.OccName == 'Artisan'

ORDER BY
	RANDOM()
LIMIT 1;