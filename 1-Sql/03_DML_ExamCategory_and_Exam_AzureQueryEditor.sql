-- ============================================================
-- SEED: ExamCategory + Exam
-- VERSION: Azure SQL Query Editor (no USE, no GO)
-- ExamCategory.IsActive has NO DB default — explicit 1 required
-- Exam.IsActive has NO DB default — explicit 1 required
-- Id is client-generated (no DB default) — use NEWID()
-- ============================================================

SET NOCOUNT ON;

-- ============================================================
-- EXAM CATEGORIES
-- ============================================================
INSERT INTO dbo.ExamCategory (Id, Name, SortOrder, IsActive)
VALUES
(NEWID(), 'Biochemistry (Blood)',       1,  1),
(NEWID(), 'Immunology',                 2,  1),
(NEWID(), 'Hepatitis Serology',         3,  1),
(NEWID(), 'HIV Serology',               4,  1),
(NEWID(), 'Antinuclear Antibodies',     5,  1),
(NEWID(), 'C-Reactive Protein (CRP)',   6,  1),
(NEWID(), 'Tumor Markers',              7,  1),
(NEWID(), 'Stool',                      8,  1),
(NEWID(), 'Urine',                      9,  1),
(NEWID(), 'Hematology',                 10, 1),
(NEWID(), 'Hormones',                   11, 1),
(NEWID(), 'Thyroid',                    12, 1);

-- ============================================================
-- 1. BIOCHEMISTRY (BLOOD)
-- ============================================================
INSERT INTO dbo.Exam (Id, ExamCategoryId, Name, Abbreviation, Description, IsActive)
SELECT NEWID(), c.Id, v.Name, v.Abbreviation, v.Description, 1
FROM dbo.ExamCategory c
CROSS JOIN (VALUES
    ('1,25 Vitamin D',                  NULL,       'Active form of vitamin D'),
    ('25 Vitamin D',                    NULL,       'Circulating vitamin D — main measurement form'),
    ('Folic Acid',                      NULL,       'Vitamin B9'),
    ('Uric Acid',                       NULL,       'Purine metabolism byproduct'),
    ('Albumin',                         NULL,       'Most abundant plasma protein'),
    ('Amylase',                         NULL,       'Pancreatic and salivary digestive enzyme'),
    ('Apolipoprotein A',                'Apo A',    NULL),
    ('Apolipoprotein B',                'Apo B',    NULL),
    ('Bilirubin',                       NULL,       'Total, direct and indirect'),
    ('Ionized Calcium',                 NULL,       NULL),
    ('Total Calcium',                   NULL,       NULL),
    ('Copper',                          NULL,       NULL),
    ('Total Cholesterol and Fractions', NULL,       'LDL, HDL, VLDL'),
    ('CPK',                             'CPK',      'Creatine phosphokinase'),
    ('Creatinine',                      NULL,       NULL),
    ('CTX',                             'CTX',      'C-terminal telopeptide — bone resorption marker'),
    ('Protein Electrophoresis',         NULL,       NULL),
    ('Alkaline Phosphatase',            'ALP',      NULL),
    ('Phosphorus',                      NULL,       NULL),
    ('Fructosamine',                    NULL,       'Short-term glycemic control'),
    ('Gamma GT',                        'GGT',      'Gamma-glutamyltransferase'),
    ('Fasting Glucose',                 NULL,       'Blood glucose — fasting'),
    ('Glycated Hemoglobin',             'HbA1c',    '2-3 month glycemic control'),
    ('Lipase',                          NULL,       NULL),
    ('Magnesium',                       NULL,       NULL),
    ('P1NP',                            'P1NP',     'Procollagen type I N-terminal propeptide — bone formation marker'),
    ('Potassium',                       NULL,       NULL),
    ('Total Protein and Fractions',     NULL,       NULL),
    ('Sodium',                          NULL,       NULL),
    ('AST',                             'AST',      'Aspartate aminotransferase (TGO)'),
    ('ALT',                             'ALT',      'Alanine aminotransferase (TGP)'),
    ('Triglycerides',                   NULL,       NULL),
    ('Blood Urea Nitrogen',             'BUN',      NULL),
    ('Vitamin B12',                     NULL,       'Cobalamin'),
    ('Zinc',                            NULL,       NULL)
) v(Name, Abbreviation, Description)
WHERE c.Name = 'Biochemistry (Blood)';

-- ============================================================
-- 2. IMMUNOLOGY
-- ============================================================
INSERT INTO dbo.Exam (Id, ExamCategoryId, Name, Abbreviation, Description, IsActive)
SELECT NEWID(), c.Id, v.Name, v.Abbreviation, v.Description, 1
FROM dbo.ExamCategory c
CROSS JOIN (VALUES
    ('Anti-ANCA',                               'ANCA',     'Anti-neutrophil cytoplasmic antibody'),
    ('Anti-ASCA',                               'ASCA',     'Anti-Saccharomyces cerevisiae antibody'),
    ('Anti-Cardiolipin (IgG + IgM + IgA)',      NULL,       NULL),
    ('Anti-Parietal Cell Antibody',             NULL,       NULL),
    ('Anti-CCP',                                'CCP',      'Anti-cyclic citrullinated peptide'),
    ('Anti-Endomysial Antibody',                NULL,       'IgA — celiac disease screening'),
    ('Anti-Phospholipid',                       NULL,       NULL),
    ('Anti-GAD',                                'GAD',      'Glutamic acid decarboxylase antibody'),
    ('Anti-Gliadin',                            NULL,       NULL),
    ('Anti-Islet Cell / ICA',                   'ICA',      'Islet cell antibody'),
    ('Anti-Insulin / IAA',                      'IAA',      'Insulin autoantibody'),
    ('Anti-Mitochondrial Antibody',             'AMA',      NULL),
    ('Anti-Smooth Muscle Antibody',             'ASMA',     NULL),
    ('Anti-Transglutaminase (IgA)',             'tTG',      'Celiac disease screening'),
    ('HLA-B27',                                 'HLA-B27',  'Human leukocyte antigen B27'),
    ('LKM1',                                    'LKM1',     'Liver-kidney microsomal antibody type 1'),
    ('Cytomegalovirus Serology',                'CMV',      NULL),
    ('Mononucleosis Serology',                  NULL,       NULL),
    ('Syphilis Serology',                       NULL,       'VDRL / FTA-Abs'),
    ('Toxoplasmosis Serology',                  NULL,       NULL)
) v(Name, Abbreviation, Description)
WHERE c.Name = 'Immunology';

-- ============================================================
-- 3. HEPATITIS SEROLOGY
-- ============================================================
INSERT INTO dbo.Exam (Id, ExamCategoryId, Name, Abbreviation, Description, IsActive)
SELECT NEWID(), c.Id, v.Name, v.Abbreviation, v.Description, 1
FROM dbo.ExamCategory c
CROSS JOIN (VALUES
    ('Hepatitis A', 'HAV', 'Anti-HAV IgM and IgG'),
    ('Hepatitis B', 'HBV', 'HBsAg, Anti-HBs, Anti-HBc'),
    ('Hepatitis C', 'HCV', 'Anti-HCV')
) v(Name, Abbreviation, Description)
WHERE c.Name = 'Hepatitis Serology';

-- ============================================================
-- 4. HIV SEROLOGY
-- ============================================================
INSERT INTO dbo.Exam (Id, ExamCategoryId, Name, Abbreviation, Description, IsActive)
SELECT NEWID(), c.Id, v.Name, v.Abbreviation, v.Description, 1
FROM dbo.ExamCategory c
CROSS JOIN (VALUES
    ('HIV (HIV1 - HIV2)', 'HIV', '4th generation — p24 antigen + antibodies')
) v(Name, Abbreviation, Description)
WHERE c.Name = 'HIV Serology';

-- ============================================================
-- 5. ANTINUCLEAR ANTIBODIES
-- ============================================================
INSERT INTO dbo.Exam (Id, ExamCategoryId, Name, Abbreviation, Description, IsActive)
SELECT NEWID(), c.Id, v.Name, v.Abbreviation, v.Description, 1
FROM dbo.ExamCategory c
CROSS JOIN (VALUES
    ('ASLO',                'ASLO',         'Anti-streptolysin O'),
    ('Native DNA Antibody', 'Anti-dsDNA',   'Double-stranded DNA antibody'),
    ('ENA Panel',           'ENA',          'Extractable nuclear antigen antibodies'),
    ('Antinuclear Antibody','ANA',          'Fluorescent antinuclear antibody'),
    ('Rheumatoid Factor',   'RF',           NULL),
    ('Anti-SSA (Ro)',       'SSA',          'Anti-Ro antibody'),
    ('Anti-SSB (La)',       'SSB',          'Anti-La antibody')
) v(Name, Abbreviation, Description)
WHERE c.Name = 'Antinuclear Antibodies';

-- ============================================================
-- 6. C-REACTIVE PROTEIN (CRP)
-- ============================================================
INSERT INTO dbo.Exam (Id, ExamCategoryId, Name, Abbreviation, Description, IsActive)
SELECT NEWID(), c.Id, v.Name, v.Abbreviation, v.Description, 1
FROM dbo.ExamCategory c
CROSS JOIN (VALUES
    ('CRP — Inflammatory Process Assessment',   'CRP',    'Quantitative C-reactive protein'),
    ('CRP — Cardiovascular Risk Assessment',    'hs-CRP', 'High-sensitivity C-reactive protein')
) v(Name, Abbreviation, Description)
WHERE c.Name = 'C-Reactive Protein (CRP)';

-- ============================================================
-- 7. TUMOR MARKERS
-- ============================================================
INSERT INTO dbo.Exam (Id, ExamCategoryId, Name, Abbreviation, Description, IsActive)
SELECT NEWID(), c.Id, v.Name, v.Abbreviation, v.Description, 1
FROM dbo.ExamCategory c
CROSS JOIN (VALUES
    ('Alpha-fetoprotein',   'AFP',      'Hepatocellular and germ cell marker'),
    ('CA 15-3',             'CA 15-3',  'Breast cancer marker'),
    ('CA 19-9',             'CA 19-9',  'Pancreatic / GI tract cancer marker'),
    ('CA 72-4',             'CA 72-4',  'Gastric cancer marker'),
    ('CA 125',              'CA 125',   'Ovarian cancer marker'),
    ('Calcitonin',          NULL,       'Medullary thyroid cancer marker'),
    ('CEA',                 'CEA',      'Carcinoembryonic antigen — colorectal'),
    ('Total PSA',           'PSA',      'Prostate-specific antigen')
) v(Name, Abbreviation, Description)
WHERE c.Name = 'Tumor Markers';

-- ============================================================
-- 8. STOOL
-- ============================================================
INSERT INTO dbo.Exam (Id, ExamCategoryId, Name, Abbreviation, Description, IsActive)
SELECT NEWID(), c.Id, v.Name, v.Abbreviation, v.Description, 1
FROM dbo.ExamCategory c
CROSS JOIN (VALUES
    ('Stool Ova and Parasite Test',  NULL,   'Parasite and egg screening'),
    ('Fecal Occult Blood Test',      'FOBT', 'Occult blood screening')
) v(Name, Abbreviation, Description)
WHERE c.Name = 'Stool';

-- ============================================================
-- 9. URINE
-- ============================================================
INSERT INTO dbo.Exam (Id, ExamCategoryId, Name, Abbreviation, Description, IsActive)
SELECT NEWID(), c.Id, v.Name, v.Abbreviation, v.Description, 1
FROM dbo.ExamCategory c
CROSS JOIN (VALUES
    ('Spot Urine Albumin',              NULL,   'Albumin-to-creatinine ratio'),
    ('24-hour Urine Calcium',           NULL,   'Calcium in 24-hour urine collection'),
    ('24-hour Urine Cortisol',          NULL,   'Free urinary cortisol'),
    ('Urine Culture and Sensitivity',   'UC&S', 'Urine culture with antibiogram'),
    ('Urinalysis (Type I)',             'UA',   'Complete urinalysis')
) v(Name, Abbreviation, Description)
WHERE c.Name = 'Urine';

-- ============================================================
-- 10. HEMATOLOGY
-- ============================================================
INSERT INTO dbo.Exam (Id, ExamCategoryId, Name, Abbreviation, Description, IsActive)
SELECT NEWID(), c.Id, v.Name, v.Abbreviation, v.Description, 1
FROM dbo.ExamCategory c
CROSS JOIN (VALUES
    ('Complete Coagulation Panel',      NULL,   'PT, aPTT, fibrinogen'),
    ('Hemoglobin Electrophoresis',      NULL,   NULL),
    ('Sickle Cell Test',                NULL,   'Sickle cell screening'),
    ('Ferritin',                        NULL,   'Iron storage protein'),
    ('Serum Iron',                      NULL,   NULL),
    ('Blood Type and Rh Factor',        'ABO',  NULL),
    ('Complete Blood Count',            'CBC',  'Red cells, white cells and platelets'),
    ('Platelet Count',                  NULL,   NULL),
    ('Transferrin Saturation',          NULL,   NULL),
    ('Erythrocyte Sedimentation Rate',  'ESR',  NULL),
    ('Reticulocyte Count',              NULL,   NULL)
) v(Name, Abbreviation, Description)
WHERE c.Name = 'Hematology';

-- ============================================================
-- 11. HORMONES
-- ============================================================
INSERT INTO dbo.Exam (Id, ExamCategoryId, Name, Abbreviation, Description, IsActive)
SELECT NEWID(), c.Id, v.Name, v.Abbreviation, v.Description, 1
FROM dbo.ExamCategory c
CROSS JOIN (VALUES
    ('17-OH Progesterone',          '17-OHP',   NULL),
    ('ACTH',                        'ACTH',     'Adrenocorticotropic hormone'),
    ('Aldosterone',                 NULL,       NULL),
    ('Androstenedione',             NULL,       NULL),
    ('Plasma Renin Activity',       'PRA',      NULL),
    ('Beta-hCG',                    'b-hCG',    'Human chorionic gonadotropin — beta'),
    ('Basal Cortisol',              NULL,       'Morning serum cortisol'),
    ('DHEA',                        'DHEA',     'Dehydroepiandrosterone'),
    ('Estradiol',                   'E2',       NULL),
    ('FSH',                         'FSH',      'Follicle-stimulating hormone'),
    ('Growth Hormone',              'GH',       NULL),
    ('IGF-1',                       'IGF-1',    'Insulin-like growth factor 1 (Somatomedin C)'),
    ('IGFBP-3',                     'IGFBP-3',  'Insulin-like growth factor binding protein 3'),
    ('Insulin',                     NULL,       NULL),
    ('LH',                          'LH',       'Luteinizing hormone'),
    ('Osteocalcin',                 NULL,       'Bone formation marker'),
    ('Parathyroid Hormone (PTH)',   'PTH',      'Intact parathyroid hormone'),
    ('C-Peptide',                   'C-pep',    'Beta-cell residual function'),
    ('Progesterone',                NULL,       NULL),
    ('Prolactin',                   'PRL',      NULL),
    ('DHEA-Sulfate',                'DHEA-S',   'Dehydroepiandrosterone sulfate'),
    ('SHBG',                        'SHBG',     'Sex hormone-binding globulin'),
    ('Total Testosterone',          NULL,       NULL),
    ('Anti-Mullerian Hormone',      'AMH',      'Ovarian reserve marker')
) v(Name, Abbreviation, Description)
WHERE c.Name = 'Hormones';

-- ============================================================
-- 12. THYROID
-- ============================================================
INSERT INTO dbo.Exam (Id, ExamCategoryId, Name, Abbreviation, Description, IsActive)
SELECT NEWID(), c.Id, v.Name, v.Abbreviation, v.Description, 1
FROM dbo.ExamCategory c
CROSS JOIN (VALUES
    ('Anti-Thyroglobulin Antibody', 'Anti-Tg',  'Thyroglobulin antibody'),
    ('Anti-Thyroid Peroxidase',     'Anti-TPO', 'Thyroid peroxidase antibody'),
    ('T3',                          'T3',       'Total triiodothyronine'),
    ('Free T4',                     'fT4',      'Free thyroxine'),
    ('Total T4',                    'T4',       'Total thyroxine'),
    ('Thyroglobulin',               'Tg',       'Thyroid tissue marker'),
    ('TSH Receptor Antibody',       'TRAb',     'Anti-TSH receptor antibody'),
    ('TSH',                         'TSH',      'Thyroid-stimulating hormone')
) v(Name, Abbreviation, Description)
WHERE c.Name = 'Thyroid';

-- ============================================================
-- VERIFY
-- ============================================================
SELECT 'ExamCategory' AS TableName, COUNT(*) AS Rows FROM dbo.ExamCategory
UNION ALL
SELECT 'Exam', COUNT(*) FROM dbo.Exam;
