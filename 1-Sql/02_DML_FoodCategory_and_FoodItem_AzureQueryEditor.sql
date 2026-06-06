-- ============================================================
-- SEED: FoodCategory + FoodItem
-- VERSION: Azure SQL Query Editor (no USE, no GO)
-- FoodCategory.IsActive has DB default true — omitted
-- FoodItem.IsActive has NO default — explicit 1 required
-- Id is client-generated (no DB default) — use NEWID()
-- ============================================================

SET NOCOUNT ON;

-- ============================================================
-- FOOD CATEGORIES (Id must be explicit — no DB default)
-- ============================================================
INSERT INTO dbo.FoodCategory (Id, Name, Description, DefaultQuotaPoints, ServingUnit, SortOrder)
VALUES
(NEWID(), 'Vegetables',          'Free consumption — no points counted',                         0,    'Unlimited',         1),
(NEWID(), 'Legumes',             'Cooked or raw legumes',                                        10,   '2 full tablespoons',2),
(NEWID(), 'Meats',               'Meats, poultry, fish and seafood',                             25,   '1 serving quota',   3),
(NEWID(), 'Cheeses',             'All types of cheese',                                          25,   '1 serving quota',   4),
(NEWID(), 'Grains and Starches', 'Rice, bread, pasta, flours and cereals',                       20,   '1 serving quota',   5),
(NEWID(), 'Fruits',              'Fresh and dried fruits',                                       15,   '1 serving quota',   6),
(NEWID(), 'Fast Food / Snacks',  'Snacks and fast food — points per individual serving',         NULL, 'Serving',           7),
(NEWID(), 'Fat-Free Sweets',     'Desserts and sweets with low fat content',                     NULL, 'Serving',           8),
(NEWID(), 'High-Fat Sweets',     'Desserts and sweets with fat — consume in moderation',         NULL, 'Serving',           9),
(NEWID(), 'Condiments',          'Seasonings and condiments',                                    NULL, 'Serving',           10),
(NEWID(), 'Others',              'Food items that do not fit other categories',                  NULL, 'Serving',           11),
(NEWID(), 'Beverages',           'Alcoholic and non-alcoholic beverages',                        NULL, 'Serving',           12),
(NEWID(), 'Soups',               'Soups and broths',                                             NULL, 'Serving / Ladle',   13);

-- ============================================================
-- 1. VEGETABLES (0 points — free)
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Swiss Chard',             'Unlimited', 0),
    ('Watercress',              'Unlimited', 0),
    ('Celery',                  'Unlimited', 0),
    ('Lettuce',                 'Unlimited', 0),
    ('Onion',                   'Unlimited', 0),
    ('Brussels Sprouts',        'Unlimited', 0),
    ('Fennel',                  'Unlimited', 0),
    ('Endive',                  'Unlimited', 0),
    ('Spinach',                 'Unlimited', 0),
    ('Beet Leaf',               'Unlimited', 0),
    ('Jilo (African Eggplant)', 'Unlimited', 0),
    ('West Indian Gherkin',     'Unlimited', 0),
    ('Turnip',                  'Unlimited', 0),
    ('Cucumber',                'Unlimited', 0),
    ('Bell Pepper',             'Unlimited', 0),
    ('Radish',                  'Unlimited', 0),
    ('Cabbage',                 'Unlimited', 0),
    ('Arugula',                 'Unlimited', 0),
    ('Celery Stalk',            'Unlimited', 0),
    ('Tomato',                  'Unlimited', 0)
) v(Name, ServingSize, Points)
WHERE c.Name = 'Vegetables';

-- ============================================================
-- 2. LEGUMES (2 full tablespoons = 10 pts)
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Zucchini',                '2 tablespoons', 10),
    ('Artichoke',               '2 tablespoons', 10),
    ('Asparagus',               '2 tablespoons', 10),
    ('Eggplant',                '2 tablespoons', 10),
    ('Broccoli',                '2 tablespoons', 10),
    ('Bamboo or Bean Sprouts',  '2 tablespoons', 10),
    ('Green Onion',             '2 tablespoons', 10),
    ('Carrot',                  '2 tablespoons', 10),
    ('Mushroom',                '2 tablespoons', 10),
    ('Cauliflower',             '2 tablespoons', 10),
    ('Green Peas',              '2 tablespoons', 10),
    ('Okra',                    '2 tablespoons', 10),
    ('Green Beans',             '2 tablespoons', 10)
) v(Name, ServingSize, Points)
WHERE c.Name = 'Legumes';

-- ============================================================
-- 3. MEATS (1 quota = 25 pts)
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Canned Tuna',                 '1 medium unit',            25),
    ('Lean Steak (no fat/skin)',    '1 tea-saucer',             25),
    ('Salted Codfish (Bacalhau)',    '1 tea-saucer',             25),
    ('Shrimp',                      '1 tea-saucer',             25),
    ('Dried Shrimp',                '1 tea-saucer',             25),
    ('Chicken',                     '1 small unit',             25),
    ('Beef',                        '1 small unit',             25),
    ('Pork',                        '1 hamburger patty',        25),
    ('Rabbit',                      '1 small slice or thigh',   25),
    ('Quail',                       '2 units',                  25),
    ('Tenderloin / Sirloin',        '1 fillet',                 25),
    ('Beef Liver',                  '1 tea-saucer',             25),
    ('Haddock',                     '1 small unit',             25),
    ('Lobster',                     '1 unit',                   25),
    ('Smoked Sausage (Linguica)',    '1 unit',                   25),
    ('Squid',                       '1 small unit',             25),
    ('Shellfish',                   '1 small unit',             25),
    ('Mussel',                      '1 unit',                   25),
    ('Oyster',                      '1 unit',                   25),
    ('Egg',                         '1 unit',                   25),
    ('Quail Legs',                  '1 small portion',          25),
    ('Smoked Turkey Breast',        '1 thin slice',             25),
    ('Fresh Ham',                   '1 tea-saucer',             25),
    ('Octopus',                     '1 tea-saucer',             25),
    ('Ham / Cold Cuts',             '2 thin slices',            25),
    ('Grilled Cheese',              '1 tea-saucer',             25),
    ('Salmon',                      '1 tea-saucer',             25),
    ('Smoked Salmon',               '2 thin slices',            25),
    ('Hot Dog Sausage',             '1 unit',                   25),
    ('Fresh Sardine',               '1 small unit',             25),
    ('Sardine in Oil',              '1 small unit',             25),
    ('Sardine in Tomato Sauce',     '1 small unit',             25),
    ('Veal',                        '1 tea-saucer',             25)
) v(Name, ServingSize, Points)
WHERE c.Name = 'Meats';

-- ============================================================
-- 4. CHEESES (1 quota = 25 pts)
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Blue Cheese (Natural)',   '1 thin slice',         25),
    ('Camembert',               '1 thin slice',         25),
    ('Gorgonzola',              '1 thin slice',         25),
    ('Gruyere',                 '1 thin slice',         25),
    ('Fresh White Cheese',      '1 thin slice',         25),
    ('Mozzarella (block)',       '1/2 unit',             25),
    ('Sliced Mozzarella',        '1 slice',              25),
    ('Parmesan',                '1 tablespoon (level)', 25),
    ('Provolone',               '1 thin slice',         25),
    ('Soy Cheese (Tofu)',       '1 thin slice',         25),
    ('Cream Cheese (Requeijao)','2 dessert spoons',     25),
    ('Ricotta',                 '1 thin slice',         25),
    ('Roquefort',               '1 thin slice',         25)
) v(Name, ServingSize, Points)
WHERE c.Name = 'Cheeses';

-- ============================================================
-- 5. GRAINS AND STARCHES (1 quota = 20 pts)
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Cooked White Rice',               '2 tablespoons',    20),
    ('Greek-style Rice',                '1 tablespoon',     20),
    ('Vegetable Rice',                  '1 tablespoon',     20),
    ('Baked Rice',                      '1 tablespoon',     20),
    ('Cream Cracker Biscuit',           '3 units',          20),
    ('Cooked Cannelloni',               '1 medium unit',    20),
    ('Cooked Cappelletti / Ravioli',    '1 tablespoon',     20),
    ('Creamed Corn',                    '1 tablespoon',     20),
    ('Couscous',                        '1 tablespoon',     20),
    ('Oat Bran',                        '1 tablespoon',     20),
    ('Oat Flour',                       '1 tablespoon',     20),
    ('Rice Flour',                      '1 tablespoon',     20),
    ('Toasted Cassava Flour (Farofa)',  '1 tablespoon',     20),
    ('Cooked Black Beans',              '4 tablespoons',    20),
    ('Beans / Peas / Lentils',          '4 tablespoons',    20),
    ('Rice Starch',                     '1 tablespoon',     20),
    ('Cornmeal (Fuba)',                 '1 tablespoon',     20),
    ('Wheat Germ',                      '1 tablespoon',     20),
    ('Cooked Lasagna',                  '1 tablespoon',     20),
    ('Cooked Pasta',                    '2 tablespoons',    20),
    ('Cooked Cassava',                  '1 small piece',    20),
    ('Cooked Corn',                     '2 tablespoons',    20),
    ('Corn on the Cob',                 '4 tablespoons',    20),
    ('Hominy (Canjica)',                '4 tablespoons',    25),
    ('Sliced Toasted Bread',            '1 slice',          20),
    ('Sandwich Bread Slice',            '1 slice',          20),
    ('Gluten Bread Toast',              '1 slice',          20),
    ('Hamburger Bun',                   '1/2 unit',         20),
    ('Whole Grain Bread',               '1 slice',          20),
    ('Whole Grain Toast',               '1 slice',          20),
    ('Dinner Roll',                     '1 small unit',     20),
    ('Cheese Bread (Pao de Queijo)',    '1 small unit',     20),
    ('Pita Bread',                      '1 small unit',     20),
    ('Water and Salt Cracker',          '3 units',          20)
) v(Name, ServingSize, Points)
WHERE c.Name = 'Grains and Starches';

-- ============================================================
-- 6. FRUITS (1 quota = 15 pts)
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Avocado',             '1 small unit',         15),
    ('Pineapple',           '1 small slice',        15),
    ('Yellow Plum',         '2 small units',        15),
    ('Red Plum',            '2 small units',        15),
    ('Dried Plum',          '2 units',              15),
    ('Small Banana',        '1/2 unit',             15),
    ('Banana',              '1 unit',               15),
    ('Persimmon',           '1 small unit',         15),
    ('Coconut (no water)',  '1 small slice',        15),
    ('Fig',                 '1 unit',               15),
    ('Raspberry',           '1/2 tea cup',          15),
    ('Sugar Apple',         '1/2 unit',             15),
    ('Grapefruit',          '1/2 unit',             15),
    ('Guava',               '1 small unit',         15),
    ('Jackfruit',           '2 small pieces',       15),
    ('Kiwi',                '1 unit',               15),
    ('Orange',              '1 unit',               15),
    ('Pera Orange',         '1 unit',               15),
    ('Apple',               '1 unit',               15),
    ('Papaya',              '1 medium slice',       15),
    ('Mango',               '1 small unit',         15),
    ('Passion Fruit',       '1 unit',               15),
    ('Watermelon',          '1 slice',              15),
    ('Melon',               '1 slice',              15),
    ('Blueberry',           '1/2 tea cup',          15),
    ('Nectarine',           '1 unit',               15),
    ('Papaya (small)',      '1 unit',               15),
    ('Peach',               '1 unit',               15),
    ('Custard Apple',       '1/2 unit',             15),
    ('Fruit Salad',         '1 tea cup',            15),
    ('Tangerine',           '1 unit',               15),
    ('Grapes',              '12 units',             15)
) v(Name, ServingSize, Points)
WHERE c.Name = 'Fruits';

-- ============================================================
-- 7. FAST FOOD / SNACKS (individual points)
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Small French Fries',          '1 serving',     62),
    ('Medium French Fries',         '1 serving',     80),
    ('Big Mac',                     '1 unit',        164),
    ('Cheeseburger',                '1 unit',        100),
    ('Plain Hamburger',             '1 unit',        82),
    ('Mc Chicken',                  '1 unit',        120),
    ('Happy Meal (Chicken)',         '1 unit',        175),
    ('Mc Chicken Crispy',           '1 unit',        140),
    ('Mc Nuggets',                  '6 units',       60),
    ('Medium Fries',                '1 serving',     80),
    ('Quarter Pounder (Large)',     '1 unit',        190),
    ('Diet / Light Line - Type 1', '1 unit',        2),
    ('Diet Chocolate Powder',       '1 teaspoon',    10),
    ('Diet Gelatin',                '1 tablespoon',  3),
    ('Diet Jam',                    '1 tablespoon',  5),
    ('Diet Ice Cream',              '1 scoop',       12),
    ('Diet Fruit Popsicle',         '1 unit',        12),
    ('Diet Soda',                   '1 glass',       0)
) v(Name, ServingSize, Points)
WHERE c.Name = 'Fast Food / Snacks';

-- ============================================================
-- 8. FAT-FREE SWEETS
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Brown Sugar',         '1 tablespoon',  20),
    ('Hard Candy',          '1 unit',        25),
    ('Chewing Gum',         '1 unit',        10),
    ('Pumpkin Preserve',    '2 tablespoons', 35),
    ('Candied Fruits',      '1/2 serving',   44),
    ('Fruit Jam',           '1 tablespoon',  20),
    ('Gelatin',             '1 tablespoon',  12),
    ('Honey',               '1 tablespoon',  17),
    ('Fruit Popsicle',      '1 unit',        17),
    ('Natural Fruit Juice', '1 small glass', 63)
) v(Name, ServingSize, Points)
WHERE c.Name = 'Fat-Free Sweets';

-- ============================================================
-- 9. HIGH-FAT SWEETS
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Chocolate Milk Powder',       '1 tablespoon',     25),
    ('Alfajor',                     '1 unit',           60),
    ('Chocolate Bar',               '1 small bar',      85),
    ('Cream-filled Cookie',         '1 unit',           22),
    ('Baked Cream Puff',            '1 unit',           40),
    ('Cream Puff',                  '1 unit',           80),
    ('Brigadeiro (Truffle)',        '1 small unit',     30),
    ('Brownie',                     '1 small unit',     70),
    ('Plain Cake (no filling)',     '1 medium slice',   75),
    ('Chocolate',                   '1 small unit',     170),
    ('Dried Chocolate',             '1 unit',           35),
    ('Frosting / Topping',          '1 unit',           40),
    ('Croissant',                   '1 unit',           60),
    ('Yogurt (Danone-style)',       '1 unit',           39),
    ('Dulce de Leche',              '1 tablespoon',     22),
    ('Egg Flan',                    '1 serving',        40),
    ('Yogurt',                      '1 cup',            40),
    ('Condensed Milk',              '3 tablespoons',    60),
    ('Meringue',                    '1 unit',           20),
    ('Mousse',                      '1 unit',           50),
    ('Peanut Candy (Pacoca)',       '1 unit',           30),
    ('Panettone',                   '1 thin slice',     50),
    ('Peanut Brittle',              '1 small unit',     30),
    ('Pudding',                     '1 medium slice',   35),
    ('Coconut Sweet (Quindim)',     '1 unit',           40),
    ('Filled Donut',                '1 unit',           60),
    ('Milk Ice Cream',              '1 scoop',          60),
    ('Apple Pie',                   '1 small slice',    60),
    ('Strawberry Pie',              '1 small slice',    60),
    ('Wafer',                       '1 unit',           25),
    ('Heavy Cream',                 '1 tablespoon',     25),
    ('Margarine',                   '1 teaspoon',       25),
    ('Butter',                      '1 teaspoon',       25),
    ('Oil, Olive Oil, Lard, Bacon', '1 teaspoon',       25)
) v(Name, ServingSize, Points)
WHERE c.Name = 'High-Fat Sweets';

-- ============================================================
-- 10. CONDIMENTS
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Ketchup',             '1 tablespoon',       10),
    ('Mayonnaise',          '1 level tablespoon', 40),
    ('Mustard',             '1 tablespoon',       5),
    ('White Sauce',         '2 tablespoons',      50),
    ('Worcestershire Sauce','4 tablespoons',      40),
    ('Hominy Seasoning',    '4 tablespoons',      25)
) v(Name, ServingSize, Points)
WHERE c.Name = 'Condiments';

-- ============================================================
-- 11. OTHERS
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Fresh Chocolate Milk',        '200ml',             55),
    ('Almond',                      '1 unit',            10),
    ('Salted Roasted Peanut',       '1/2 tea cup',       60),
    ('Olive',                       '2 units',           5),
    ('Granola Bar',                 '1 unit',            55),
    ('Plain Cracker',               '1 unit',            10),
    ('Cracker Pack',                '1 pack',            70),
    ('Beef Parmigiana',             '1 serving',         155),
    ('Toasted Cream Cracker',       '1 unit',            8),
    ('Toasted Cashew',              '1 unit',            10),
    ('Brazil Nut',                  '1 unit',            12),
    ('Coconut Candy',               '1 unit',            65),
    ('Preserved Coconut in Milk',   '1 tablespoon',      15),
    ('Croquette / Kibbe / Pastry',  '1 small unit',      25),
    ('Chicken Dumpling / Coxinha',  '1 small unit',      45),
    ('Party Cassava Flour (Farofa)','2 tablespoons',     25),
    ('Feijoada (Black Bean Stew)',  '1/2 tea cup',       35),
    ('Cheese Fondue',               '1 tablespoon',      25),
    ('Granola',                     '1 tablespoon',      20),
    ('Lasagna',                     '1 tablespoon',      25),
    ('Coconut Milk',                '1/2 glass',         70),
    ('Passion Fruit Mousse',        '1/2 cup',           70),
    ('Walnut',                      '1 unit',            10),
    ('Stuffed Pastry (Paineira)',   '1 unit',            70),
    ('Potato Salad',                '1 small unit',      45),
    ('Pine Nut',                    '100g',              65),
    ('Pistachio',                   '100g',              77),
    ('Pizza',                       '1 slice',           75),
    ('Polenta with Sauce',          '1 tablespoon',      12),
    ('Chips / Snack',               '1 tablespoon',      25),
    ('Vegetable Salad',             '1 serving',         40),
    ('Salad or Yogurt or Mousse',   '1/2 cup',           45),
    ('Smoked Salmon',               '1 unit',            25),
    ('Tapioca Crepe',               '1 unit',            25),
    ('Cherry Tomato',               '2 units',           25),
    ('Vatapa (Brazilian Stew)',     '2 tablespoons',     25),
    ('Yakult Probiotic',            '1 unit',            15)
) v(Name, ServingSize, Points)
WHERE c.Name = 'Others';

-- ============================================================
-- 12. BEVERAGES
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Coconut Water',           '1 glass',         10),
    ('Fruit Batida (cocktail)', '1/2 glass',       45),
    ('Caipirinha',              '1 glass',         45),
    ('Beer',                    '1 can',           45),
    ('Whole Milk or Yogurt',    '200ml',           30),
    ('Skim Milk or Yogurt',     '200ml',           20),
    ('Semi-skim Milk',          '200ml',           25),
    ('Soy Milk Powder',         '1 tablespoon',    15),
    ('Soda',                    '1 can (350ml)',   22),
    ('Fresh Fruit Juice',       '1 glass',         25),
    ('Tucupi (indigenous broth)','100ml',          10),
    ('Wine',                    '1 glass (150ml)', 25),
    ('Whisky / Dry White Wine', '1 shot (50ml)',   40),
    ('Vodka or Campari',        '1 shot',          40)
) v(Name, ServingSize, Points)
WHERE c.Name = 'Beverages';

-- ============================================================
-- 13. SOUPS
-- ============================================================
INSERT INTO dbo.FoodItem (Id, FoodCategoryId, Name, ServingSize, Points, IsActive)
SELECT NEWID(), c.Id, v.Name, v.ServingSize, v.Points, 1
FROM dbo.FoodCategory c
CROSS JOIN (VALUES
    ('Beef Broth',      '1 ladle',  10),
    ('Chicken Broth',   '1 ladle',  10),
    ('Chicken Stock',   '1 ladle',  10),
    ('Vegetable Broth', '1 ladle',  5),
    ('Beef Consomme',   '6 ladles', 25),
    ('Asparagus Soup',  '4 ladles', 25),
    ('French Onion Soup','6 ladles',25),
    ('Mushroom Soup',   '1 ladle',  30),
    ('Split Pea Soup',  '1 ladle',  60),
    ('Black Bean Soup', '1 ladle',  35),
    ('Packaged Soup',   '1 ladle',  30)
) v(Name, ServingSize, Points)
WHERE c.Name = 'Soups';

-- ============================================================
-- VERIFY
-- ============================================================
SELECT 'FoodCategory' AS TableName, COUNT(*) AS Rows FROM dbo.FoodCategory
UNION ALL
SELECT 'FoodItem', COUNT(*) FROM dbo.FoodItem;
