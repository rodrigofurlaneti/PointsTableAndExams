-- ============================================================
-- SEED: ExamCategory + Exam
-- Source: CS. Muniz Endocrinology request form
-- ============================================================

USE PointsTableAndExams;
GO

SET NOCOUNT ON;

-- ============================================================
-- EXAM CATEGORIES
-- ============================================================
INSERT INTO dbo.ExamCategory (Name, SortOrder) VALUES
('Biochemistry (Blood)',        1),
('Immunology',                  2),
('Hepatitis Serology',          3),
('HIV Serology',                4),
('Antinuclear Antibodies',      5),
('C-Reactive Protein (CRP)',    6),
('Tumor Markers',               7),
('Stool',                       8),
('Urine',                       9),
('Hematology',                  10),
('Hormones',                    11),
('Thyroid',                     12);
GO

-- ============================================================
-- CATEGORY ID VARIABLES
-- ============================================================
DECLARE
    @catBioch   UNIQUEIDENTIFIER = (SELECT Id FROM dbo.ExamCategory WHERE Name = 'Biochemistry (Blood)'),
    @catImuno   UNIQUEIDENTIFIER = (SELECT Id FROM dbo.ExamCategory WHERE Name = 'Immunology'),
    @catHepat   UNIQUEIDENTIFIER = (SELECT Id FROM dbo.ExamCategory WHERE Name = 'Hepatitis Serology'),
    @catHIV     UNIQUEIDENTIFIER = (SELECT Id FROM dbo.ExamCategory WHERE Name = 'HIV Serology'),
    @catANA     UNIQUEIDENTIFIER = (SELECT Id FROM dbo.ExamCategory WHERE Name = 'Antinuclear Antibodies'),
    @catCRP     UNIQUEIDENTIFIER = (SELECT Id FROM dbo.ExamCategory WHERE Name = 'C-Reactive Protein (CRP)'),
    @catTumor   UNIQUEIDENTIFIER = (SELECT Id FROM dbo.ExamCategory WHERE Name = 'Tumor Markers'),
    @catStool   UNIQUEIDENTIFIER = (SELECT Id FROM dbo.ExamCategory WHERE Name = 'Stool'),
    @catUrine   UNIQUEIDENTIFIER = (SELECT Id FROM dbo.ExamCategory WHERE Name = 'Urine'),
    @catHemat   UNIQUEIDENTIFIER = (SELECT Id FROM dbo.ExamCategory WHERE Name = 'Hematology'),
    @catHorm    UNIQUEIDENTIFIER = (SELECT Id FROM dbo.ExamCategory WHERE Name = 'Hormones'),
    @catThyroid UNIQUEIDENTIFIER = (SELECT Id FROM dbo.ExamCategory WHERE Name = 'Thyroid');

-- ============================================================
-- 1. BIOCHEMISTRY (BLOOD)
-- ============================================================
INSERT INTO dbo.Exam (ExamCategoryId, Name, Abbreviation, Description) VALUES
(@catBioch, '1,25 Vitamin D',                   NULL,       'Active form of vitamin D'),
(@catBioch, '25 Vitamin D',                     NULL,       'Circulating vitamin D — main measurement form'),
(@catBioch, 'Folic Acid',                       NULL,       'Vitamin B9'),
(@catBioch, 'Uric Acid',                        NULL,       'Purine metabolism byproduct'),
(@catBioch, 'Albumin',                          NULL,       'Most abundant plasma protein'),
(@catBioch, 'Amylase',                          NULL,       'Pancreatic and salivary digestive enzyme'),
(@catBioch, 'Apolipoprotein A',                 'Apo A',    NULL),
(@catBioch, 'Apolipoprotein B',                 'Apo B',    NULL),
(@catBioch, 'Bilirubin',                        NULL,       'Total, direct and indirect'),
(@catBioch, 'Ionized Calcium',                  NULL,       NULL),
(@catBioch, 'Total Calcium',                    NULL,       NULL),
(@catBioch, 'Copper',                           NULL,       NULL),
(@catBioch, 'Total Cholesterol and Fractions',  NULL,       'LDL, HDL, VLDL'),
(@catBioch, 'CPK',                              'CPK',      'Creatine phosphokinase'),
(@catBioch, 'Creatinine',                       NULL,       NULL),
(@catBioch, 'CTX',                              'CTX',      'C-terminal telopeptide — bone resorption marker'),
(@catBioch, 'Protein Electrophoresis',          NULL,       NULL),
(@catBioch, 'Alkaline Phosphatase',             'ALP',      NULL),
(@catBioch, 'Phosphorus',                       NULL,       NULL),
(@catBioch, 'Fructosamine',                     NULL,       'Short-term glycemic control'),
(@catBioch, 'Gamma GT',                         'GGT',      'Gamma-glutamyltransferase'),
(@catBioch, 'Fasting Glucose',                  NULL,       'Blood glucose — fasting'),
(@catBioch, 'Glycated Hemoglobin',              'HbA1c',    '2-3 month glycemic control'),
(@catBioch, 'Lipase',                           NULL,       NULL),
(@catBioch, 'Magnesium',                        NULL,       NULL),
(@catBioch, 'P1NP',                             'P1NP',     'Procollagen type I N-terminal propeptide — bone formation marker'),
(@catBioch, 'Potassium',                        NULL,       NULL),
(@catBioch, 'Total Protein and Fractions',      NULL,       NULL),
(@catBioch, 'Sodium',                           NULL,       NULL),
(@catBioch, 'AST',                              'AST',      'Aspartate aminotransferase (TGO)'),
(@catBioch, 'ALT',                              'ALT',      'Alanine aminotransferase (TGP)'),
(@catBioch, 'Triglycerides',                    NULL,       NULL),
(@catBioch, 'Blood Urea Nitrogen',              'BUN',      NULL),
(@catBioch, 'Vitamin B12',                      NULL,       'Cobalamin'),
(@catBioch, 'Zinc',                             NULL,       NULL);

-- ============================================================
-- 2. IMMUNOLOGY
-- ============================================================
INSERT INTO dbo.Exam (ExamCategoryId, Name, Abbreviation, Description) VALUES
(@catImuno, 'Anti-ANCA',                                'ANCA',     'Anti-neutrophil cytoplasmic antibody'),
(@catImuno, 'Anti-ASCA',                                'ASCA',     'Anti-Saccharomyces cerevisiae antibody'),
(@catImuno, 'Anti-Cardiolipin (IgG + IgM + IgA)',       NULL,       NULL),
(@catImuno, 'Anti-Parietal Cell Antibody',              NULL,       NULL),
(@catImuno, 'Anti-CCP',                                 'CCP',      'Anti-cyclic citrullinated peptide'),
(@catImuno, 'Anti-Endomysial Antibody',                 NULL,       'IgA — celiac disease screening'),
(@catImuno, 'Anti-Phospholipid',                        NULL,       NULL),
(@catImuno, 'Anti-GAD',                                 'GAD',      'Glutamic acid decarboxylase antibody'),
(@catImuno, 'Anti-Gliadin',                             NULL,       NULL),
(@catImuno, 'Anti-Islet Cell / ICA',                    'ICA',      'Islet cell antibody'),
(@catImuno, 'Anti-Insulin / IAA',                       'IAA',      'Insulin autoantibody'),
(@catImuno, 'Anti-Mitochondrial Antibody',              'AMA',      NULL),
(@catImuno, 'Anti-Smooth Muscle Antibody',              'ASMA',     NULL),
(@catImuno, 'Anti-Transglutaminase (IgA)',              'tTG',      'Celiac disease screening'),
(@catImuno, 'HLA-B27',                                  'HLA-B27',  'Human leukocyte antigen B27'),
(@catImuno, 'LKM1',                                     'LKM1',     'Liver-kidney microsomal antibody type 1'),
(@catImuno, 'Cytomegalovirus Serology',                 'CMV',      NULL),
(@catImuno, 'Mononucleosis Serology',                   NULL,       NULL),
(@catImuno, 'Syphilis Serology',                        NULL,       'VDRL / FTA-Abs'),
(@catImuno, 'Toxoplasmosis Serology',                   NULL,       NULL);

-- ============================================================
-- 3. HEPATITIS SEROLOGY
-- ============================================================
INSERT INTO dbo.Exam (ExamCategoryId, Name, Abbreviation, Description) VALUES
(@catHepat, 'Hepatitis A',  'HAV',  'Anti-HAV IgM and IgG'),
(@catHepat, 'Hepatitis B',  'HBV',  'HBsAg, Anti-HBs, Anti-HBc'),
(@catHepat, 'Hepatitis C',  'HCV',  'Anti-HCV');

-- ============================================================
-- 4. HIV SEROLOGY
-- ============================================================
INSERT INTO dbo.Exam (ExamCategoryId, Name, Abbreviation, Description) VALUES
(@catHIV, 'HIV (HIV1 - HIV2)', 'HIV', '4th generation — p24 antigen + antibodies');

-- ============================================================
-- 5. ANTINUCLEAR ANTIBODIES
-- ============================================================
INSERT INTO dbo.Exam (ExamCategoryId, Name, Abbreviation, Description) VALUES
(@catANA, 'ASLO',                   'ASLO',     'Anti-streptolysin O'),
(@catANA, 'Native DNA Antibody',    'Anti-dsDNA','Double-stranded DNA antibody'),
(@catANA, 'ENA Panel',              'ENA',      'Extractable nuclear antigen antibodies'),
(@catANA, 'Antinuclear Antibody',   'ANA',      'Fluorescent antinuclear antibody'),
(@catANA, 'Rheumatoid Factor',      'RF',       NULL),
(@catANA, 'Anti-SSA (Ro)',          'SSA',      'Anti-Ro antibody'),
(@catANA, 'Anti-SSB (La)',          'SSB',      'Anti-La antibody');

-- ============================================================
-- 6. C-REACTIVE PROTEIN (CRP)
-- ============================================================
INSERT INTO dbo.Exam (ExamCategoryId, Name, Abbreviation, Description) VALUES
(@catCRP, 'CRP — Inflammatory Process Assessment',  'CRP',      'Quantitative C-reactive protein'),
(@catCRP, 'CRP — Cardiovascular Risk Assessment',   'hs-CRP',   'High-sensitivity C-reactive protein');

-- ============================================================
-- 7. TUMOR MARKERS
-- ============================================================
INSERT INTO dbo.Exam (ExamCategoryId, Name, Abbreviation, Description) VALUES
(@catTumor, 'Alpha-fetoprotein',    'AFP',      'Hepatocellular and germ cell marker'),
(@catTumor, 'CA 15-3',              'CA 15-3',  'Breast cancer marker'),
(@catTumor, 'CA 19-9',              'CA 19-9',  'Pancreatic / GI tract cancer marker'),
(@catTumor, 'CA 72-4',              'CA 72-4',  'Gastric cancer marker'),
(@catTumor, 'CA 125',               'CA 125',   'Ovarian cancer marker'),
(@catTumor, 'Calcitonin',           NULL,       'Medullary thyroid cancer marker'),
(@catTumor, 'CEA',                  'CEA',      'Carcinoembryonic antigen — colorectal'),
(@catTumor, 'Total PSA',            'PSA',      'Prostate-specific antigen');

-- ============================================================
-- 8. STOOL
-- ============================================================
INSERT INTO dbo.Exam (ExamCategoryId, Name, Abbreviation, Description) VALUES
(@catStool, 'Stool Ova and Parasite Test',  NULL, 'Parasite and egg screening'),
(@catStool, 'Fecal Occult Blood Test',      'FOBT', 'Occult blood screening');

-- ============================================================
-- 9. URINE
-- ============================================================
INSERT INTO dbo.Exam (ExamCategoryId, Name, Abbreviation, Description) VALUES
(@catUrine, 'Spot Urine Albumin',           NULL,   'Albumin-to-creatinine ratio'),
(@catUrine, '24-hour Urine Calcium',        NULL,   'Calcium in 24-hour urine collection'),
(@catUrine, '24-hour Urine Cortisol',       NULL,   'Free urinary cortisol'),
(@catUrine, 'Urine Culture and Sensitivity','UC&S', 'Urine culture with antibiogram'),
(@catUrine, 'Urinalysis (Type I)',           'UA',   'Complete urinalysis');

-- ============================================================
-- 10. HEMATOLOGY
-- ============================================================
INSERT INTO dbo.Exam (ExamCategoryId, Name, Abbreviation, Description) VALUES
(@catHemat, 'Complete Coagulation Panel',   NULL,   'PT, aPTT, fibrinogen'),
(@catHemat, 'Hemoglobin Electrophoresis',   NULL,   NULL),
(@catHemat, 'Sickle Cell Test',             NULL,   'Sickle cell screening'),
(@catHemat, 'Ferritin',                     NULL,   'Iron storage protein'),
(@catHemat, 'Serum Iron',                   NULL,   NULL),
(@catHemat, 'Blood Type and Rh Factor',     'ABO',  NULL),
(@catHemat, 'Complete Blood Count',         'CBC',  'Red cells, white cells and platelets'),
(@catHemat, 'Platelet Count',               NULL,   NULL),
(@catHemat, 'Transferrin Saturation',       NULL,   NULL),
(@catHemat, 'Erythrocyte Sedimentation Rate','ESR', NULL),
(@catHemat, 'Reticulocyte Count',           NULL,   NULL);

-- ============================================================
-- 11. HORMONES
-- ============================================================
INSERT INTO dbo.Exam (ExamCategoryId, Name, Abbreviation, Description) VALUES
(@catHorm, '17-OH Progesterone',                '17-OHP',   NULL),
(@catHorm, 'ACTH',                              'ACTH',     'Adrenocorticotropic hormone'),
(@catHorm, 'Aldosterone',                       NULL,       NULL),
(@catHorm, 'Androstenedione',                   NULL,       NULL),
(@catHorm, 'Plasma Renin Activity',             'PRA',      NULL),
(@catHorm, 'Beta-hCG',                          'β-hCG',    'Human chorionic gonadotropin — beta'),
(@catHorm, 'Basal Cortisol',                    NULL,       'Morning serum cortisol'),
(@catHorm, 'DHEA',                              'DHEA',     'Dehydroepiandrosterone'),
(@catHorm, 'Estradiol',                         'E2',       NULL),
(@catHorm, 'FSH',                               'FSH',      'Follicle-stimulating hormone'),
(@catHorm, 'Growth Hormone',                    'GH',       NULL),
(@catHorm, 'IGF-1',                             'IGF-1',    'Insulin-like growth factor 1 (Somatomedin C)'),
(@catHorm, 'IGFBP-3',                           'IGFBP-3',  'Insulin-like growth factor binding protein 3'),
(@catHorm, 'Insulin',                           NULL,       NULL),
(@catHorm, 'LH',                                'LH',       'Luteinizing hormone'),
(@catHorm, 'Osteocalcin',                       NULL,       'Bone formation marker'),
(@catHorm, 'Parathyroid Hormone (PTH)',         'PTH',      'Intact parathyroid hormone'),
(@catHorm, 'C-Peptide',                         'C-pep',    'Beta-cell residual function'),
(@catHorm, 'Progesterone',                      NULL,       NULL),
(@catHorm, 'Prolactin',                         'PRL',      NULL),
(@catHorm, 'DHEA-Sulfate',                      'DHEA-S',   'Dehydroepiandrosterone sulfate'),
(@catHorm, 'SHBG',                              'SHBG',     'Sex hormone-binding globulin'),
(@catHorm, 'Total Testosterone',                NULL,       NULL),
(@catHorm, 'Anti-Müllerian Hormone',            'AMH',      'Ovarian reserve marker');

-- ============================================================
-- 12. THYROID
-- ============================================================
INSERT INTO dbo.Exam (ExamCategoryId, Name, Abbreviation, Description) VALUES
(@catThyroid, 'Anti-Thyroglobulin Antibody',    'Anti-Tg',  'Thyroglobulin antibody'),
(@catThyroid, 'Anti-Thyroid Peroxidase',         'Anti-TPO', 'Thyroid peroxidase antibody'),
(@catThyroid, 'T3',                              'T3',       'Total triiodothyronine'),
(@catThyroid, 'Free T4',                         'fT4',      'Free thyroxine'),
(@catThyroid, 'Total T4',                        'T4',       'Total thyroxine'),
(@catThyroid, 'Thyroglobulin',                   'Tg',       'Thyroid tissue marker'),
(@catThyroid, 'TSH Receptor Antibody',           'TRAb',     'Anti-TSH receptor antibody'),
(@catThyroid, 'TSH',                             'TSH',      'Thyroid-stimulating hormone');

GO

PRINT 'Exams seeded successfully.';
GO
