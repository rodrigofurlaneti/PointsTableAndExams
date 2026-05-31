-- ============================================================
-- SEED: FoodCategory + FoodItem
-- Source: Points Table — 300 points is your daily total
-- ============================================================

USE PointsTableAndExams;
GO

SET NOCOUNT ON;

-- ============================================================
-- FOOD CATEGORIES
-- ============================================================
INSERT INTO dbo.FoodCategory (Name, Description, DefaultQuotaPoints, ServingUnit, SortOrder) VALUES
('Vegetables',          'Free consumption — no points counted',                         0,    'Unlimited',                1),
('Legumes',             'Cooked or raw legumes',                                        10,   '2 full tablespoons',       2),
('Meats',               'Meats, poultry, fish and seafood',                             25,   '1 serving quota',          3),
('Cheeses',             'All types of cheese',                                          25,   '1 serving quota',          4),
('Grains and Starches', 'Rice, bread, pasta, flours and cereals',                       20,   '1 serving quota',          5),
('Fruits',              'Fresh and dried fruits',                                       15,   '1 serving quota',          6),
('Fast Food / Snacks',  'Snacks and fast food — points per individual serving',         NULL, 'Serving',                  7),
('Fat-Free Sweets',     'Desserts and sweets with low fat content',                     NULL, 'Serving',                  8),
('High-Fat Sweets',     'Desserts and sweets with fat — consume in moderation',         NULL, 'Serving',                  9),
('Condiments',          'Seasonings and condiments',                                    NULL, 'Serving',                  10),
('Others',              'Food items that do not fit other categories',                  NULL, 'Serving',                  11),
('Beverages',           'Alcoholic and non-alcoholic beverages',                        NULL, 'Serving',                  12),
('Soups',               'Soups and broths',                                             NULL, 'Serving / Ladle',          13);
GO

-- ============================================================
-- CATEGORY ID VARIABLES
-- ============================================================
DECLARE
    @catVeg         INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'Vegetables'),
    @catLeg         INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'Legumes'),
    @catMeat        INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'Meats'),
    @catCheese      INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'Cheeses'),
    @catGrains      INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'Grains and Starches'),
    @catFruits      INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'Fruits'),
    @catSnacks      INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'Fast Food / Snacks'),
    @catSweetLow    INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'Fat-Free Sweets'),
    @catSweetHigh   INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'High-Fat Sweets'),
    @catCond        INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'Condiments'),
    @catOther       INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'Others'),
    @catBev         INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'Beverages'),
    @catSoup        INT = (SELECT Id FROM dbo.FoodCategory WHERE Name = 'Soups');

-- ============================================================
-- 1. VEGETABLES (0 points — free)
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catVeg, 'Swiss Chard',           'Unlimited', 0),
(@catVeg, 'Watercress',            'Unlimited', 0),
(@catVeg, 'Celery',                'Unlimited', 0),
(@catVeg, 'Lettuce',               'Unlimited', 0),
(@catVeg, 'Onion',                 'Unlimited', 0),
(@catVeg, 'Brussels Sprouts',      'Unlimited', 0),
(@catVeg, 'Fennel',                'Unlimited', 0),
(@catVeg, 'Endive',                'Unlimited', 0),
(@catVeg, 'Spinach',               'Unlimited', 0),
(@catVeg, 'Beet Leaf',             'Unlimited', 0),
(@catVeg, 'Jilo (African Eggplant)','Unlimited',0),
(@catVeg, 'West Indian Gherkin',   'Unlimited', 0),
(@catVeg, 'Turnip',                'Unlimited', 0),
(@catVeg, 'Cucumber',              'Unlimited', 0),
(@catVeg, 'Bell Pepper',           'Unlimited', 0),
(@catVeg, 'Radish',                'Unlimited', 0),
(@catVeg, 'Cabbage',               'Unlimited', 0),
(@catVeg, 'Arugula',               'Unlimited', 0),
(@catVeg, 'Celery Stalk',          'Unlimited', 0),
(@catVeg, 'Tomato',                'Unlimited', 0);

-- ============================================================
-- 2. LEGUMES (2 full tablespoons = 10 pts)
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catLeg, 'Zucchini',              '2 tablespoons', 10),
(@catLeg, 'Artichoke',             '2 tablespoons', 10),
(@catLeg, 'Asparagus',             '2 tablespoons', 10),
(@catLeg, 'Eggplant',              '2 tablespoons', 10),
(@catLeg, 'Broccoli',              '2 tablespoons', 10),
(@catLeg, 'Bamboo or Bean Sprouts','2 tablespoons', 10),
(@catLeg, 'Green Onion',           '2 tablespoons', 10),
(@catLeg, 'Carrot',                '2 tablespoons', 10),
(@catLeg, 'Mushroom',              '2 tablespoons', 10),
(@catLeg, 'Cauliflower',           '2 tablespoons', 10),
(@catLeg, 'Green Peas',            '2 tablespoons', 10),
(@catLeg, 'Okra',                  '2 tablespoons', 10),
(@catLeg, 'Green Beans',           '2 tablespoons', 10);

-- ============================================================
-- 3. MEATS (1 quota = 25 pts)
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catMeat, 'Canned Tuna',                  '1 medium unit',            25),
(@catMeat, 'Lean Steak (no fat/skin)',      '1 tea-saucer',             25),
(@catMeat, 'Salted Codfish (Bacalhau)',     '1 tea-saucer',             25),
(@catMeat, 'Shrimp',                        '1 tea-saucer',             25),
(@catMeat, 'Dried Shrimp',                  '1 tea-saucer',             25),
(@catMeat, 'Chicken',                       '1 small unit',             25),
(@catMeat, 'Beef',                          '1 small unit',             25),
(@catMeat, 'Pork',                          '1 hamburger patty',        25),
(@catMeat, 'Rabbit',                        '1 small slice or thigh',   25),
(@catMeat, 'Quail',                         '2 units',                  25),
(@catMeat, 'Tenderloin / Sirloin',          '1 fillet',                 25),
(@catMeat, 'Beef Liver',                    '1 tea-saucer',             25),
(@catMeat, 'Haddock',                       '1 small unit',             25),
(@catMeat, 'Lobster',                       '1 unit',                   25),
(@catMeat, 'Smoked Sausage (Linguiça)',     '1 unit',                   25),
(@catMeat, 'Squid',                         '1 small unit',             25),
(@catMeat, 'Shellfish',                     '1 small unit',             25),
(@catMeat, 'Mussel',                        '1 unit',                   25),
(@catMeat, 'Oyster',                        '1 unit',                   25),
(@catMeat, 'Egg',                           '1 unit',                   25),
(@catMeat, 'Quail Legs',                    '1 small portion',          25),
(@catMeat, 'Smoked Turkey Breast',          '1 thin slice',             25),
(@catMeat, 'Fresh Ham',                     '1 tea-saucer',             25),
(@catMeat, 'Octopus',                       '1 tea-saucer',             25),
(@catMeat, 'Ham / Cold Cuts',               '2 thin slices',            25),
(@catMeat, 'Grilled Cheese',                '1 tea-saucer',             25),
(@catMeat, 'Salmon',                        '1 tea-saucer',             25),
(@catMeat, 'Smoked Salmon',                 '2 thin slices',            25),
(@catMeat, 'Hot Dog Sausage',               '1 unit',                   25),
(@catMeat, 'Fresh Sardine',                 '1 small unit',             25),
(@catMeat, 'Sardine in Oil',                '1 small unit',             25),
(@catMeat, 'Sardine in Tomato Sauce',       '1 small unit',             25),
(@catMeat, 'Veal',                          '1 tea-saucer',             25);

-- ============================================================
-- 4. CHEESES (1 quota = 25 pts)
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catCheese, 'Blue Cheese (Natural)',   '1 thin slice',         25),
(@catCheese, 'Camembert',               '1 thin slice',         25),
(@catCheese, 'Gorgonzola',              '1 thin slice',         25),
(@catCheese, 'Gruyère',                 '1 thin slice',         25),
(@catCheese, 'Fresh White Cheese',      '1 thin slice',         25),
(@catCheese, 'Mozzarella (block)',       '1/2 unit',             25),
(@catCheese, 'Sliced Mozzarella',        '1 slice',              25),
(@catCheese, 'Parmesan',                '1 tablespoon (level)', 25),
(@catCheese, 'Provolone',               '1 thin slice',         25),
(@catCheese, 'Soy Cheese (Tofu)',       '1 thin slice',         25),
(@catCheese, 'Cream Cheese (Requeijão)','2 dessert spoons',     25),
(@catCheese, 'Ricotta',                 '1 thin slice',         25),
(@catCheese, 'Roquefort',               '1 thin slice',         25);

-- ============================================================
-- 5. GRAINS AND STARCHES (1 quota = 20 pts)
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catGrains, 'Cooked White Rice',               '2 tablespoons',    20),
(@catGrains, 'Greek-style Rice',                '1 tablespoon',     20),
(@catGrains, 'Vegetable Rice',                  '1 tablespoon',     20),
(@catGrains, 'Baked Rice',                      '1 tablespoon',     20),
(@catGrains, 'Cream Cracker Biscuit',           '3 units',          20),
(@catGrains, 'Cooked Cannelloni',               '1 medium unit',    20),
(@catGrains, 'Cooked Cappelletti / Ravioli',    '1 tablespoon',     20),
(@catGrains, 'Creamed Corn',                    '1 tablespoon',     20),
(@catGrains, 'Couscous',                        '1 tablespoon',     20),
(@catGrains, 'Oat Bran',                        '1 tablespoon',     20),
(@catGrains, 'Oat Flour',                       '1 tablespoon',     20),
(@catGrains, 'Rice Flour',                      '1 tablespoon',     20),
(@catGrains, 'Toasted Cassava Flour (Farofa)',  '1 tablespoon',     20),
(@catGrains, 'Cooked Black Beans',              '4 tablespoons',    20),
(@catGrains, 'Beans / Peas / Lentils',          '4 tablespoons',    20),
(@catGrains, 'Rice Starch',                     '1 tablespoon',     20),
(@catGrains, 'Cornmeal (Fubá)',                 '1 tablespoon',     20),
(@catGrains, 'Wheat Germ',                      '1 tablespoon',     20),
(@catGrains, 'Cooked Lasagna',                  '1 tablespoon',     20),
(@catGrains, 'Cooked Pasta',                    '2 tablespoons',    20),
(@catGrains, 'Cooked Cassava',                  '1 small piece',    20),
(@catGrains, 'Cooked Corn',                     '2 tablespoons',    20),
(@catGrains, 'Corn on the Cob',                 '4 tablespoons',    20),
(@catGrains, 'Hominy (Canjica)',                '4 tablespoons',    25),
(@catGrains, 'Sliced Toasted Bread',            '1 slice',          20),
(@catGrains, 'Sandwich Bread Slice',            '1 slice',          20),
(@catGrains, 'Gluten Bread Toast',              '1 slice',          20),
(@catGrains, 'Hamburger Bun',                   '1/2 unit',         20),
(@catGrains, 'Whole Grain Bread',               '1 slice',          20),
(@catGrains, 'Whole Grain Toast',               '1 slice',          20),
(@catGrains, 'Dinner Roll',                     '1 small unit',     20),
(@catGrains, 'Cheese Bread (Pão de Queijo)',    '1 small unit',     20),
(@catGrains, 'Pita Bread',                      '1 small unit',     20),
(@catGrains, 'Water and Salt Cracker',          '3 units',          20);

-- ============================================================
-- 6. FRUITS (1 quota = 15 pts)
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catFruits, 'Avocado',             '1 small unit',         15),
(@catFruits, 'Pineapple',           '1 small slice',        15),
(@catFruits, 'Yellow Plum',         '2 small units',        15),
(@catFruits, 'Red Plum',            '2 small units',        15),
(@catFruits, 'Dried Plum',          '2 units',              15),
(@catFruits, 'Small Banana',        '1/2 unit',             15),
(@catFruits, 'Banana',              '1 unit',               15),
(@catFruits, 'Persimmon',           '1 small unit',         15),
(@catFruits, 'Coconut (no water)',  '1 small slice',        15),
(@catFruits, 'Fig',                 '1 unit',               15),
(@catFruits, 'Raspberry',           '1/2 tea cup',          15),
(@catFruits, 'Sugar Apple',         '1/2 unit',             15),
(@catFruits, 'Grapefruit',          '1/2 unit',             15),
(@catFruits, 'Guava',               '1 small unit',         15),
(@catFruits, 'Jackfruit',           '2 small pieces',       15),
(@catFruits, 'Kiwi',                '1 unit',               15),
(@catFruits, 'Orange',              '1 unit',               15),
(@catFruits, 'Pera Orange',         '1 unit',               15),
(@catFruits, 'Apple',               '1 unit',               15),
(@catFruits, 'Papaya',              '1 medium slice',       15),
(@catFruits, 'Mango',               '1 small unit',         15),
(@catFruits, 'Passion Fruit',       '1 unit',               15),
(@catFruits, 'Watermelon',          '1 slice',              15),
(@catFruits, 'Melon',               '1 slice',              15),
(@catFruits, 'Blueberry',           '1/2 tea cup',          15),
(@catFruits, 'Nectarine',           '1 unit',               15),
(@catFruits, 'Papaya (small)',      '1 unit',               15),
(@catFruits, 'Peach',               '1 unit',               15),
(@catFruits, 'Custard Apple',       '1/2 unit',             15),
(@catFruits, 'Fruit Salad',         '1 tea cup',            15),
(@catFruits, 'Tangerine',           '1 unit',               15),
(@catFruits, 'Grapes',              '12 units',             15);

-- ============================================================
-- 7. FAST FOOD / SNACKS (individual points)
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catSnacks, 'Small French Fries',          '1 serving',     62),
(@catSnacks, 'Medium French Fries',         '1 serving',     80),
(@catSnacks, 'Big Mac',                     '1 unit',        164),
(@catSnacks, 'Cheeseburger',                '1 unit',        100),
(@catSnacks, 'Plain Hamburger',             '1 unit',        82),
(@catSnacks, 'Mc Chicken',                  '1 unit',        120),
(@catSnacks, 'Happy Meal (Chicken)',         '1 unit',        175),
(@catSnacks, 'Mc Chicken Crispy',           '1 unit',        140),
(@catSnacks, 'Mc Nuggets',                  '6 units',       60),
(@catSnacks, 'Medium Fries',                '1 serving',     80),
(@catSnacks, 'Quarter Pounder (Large)',     '1 unit',        190),
(@catSnacks, 'Diet / Light Line — Type 1', '1 unit',        2),
(@catSnacks, 'Diet Chocolate Powder',       '1 teaspoon',    10),
(@catSnacks, 'Diet Gelatin',                '1 tablespoon',  3),
(@catSnacks, 'Diet Jam',                    '1 tablespoon',  5),
(@catSnacks, 'Diet Ice Cream',              '1 scoop',       12),
(@catSnacks, 'Diet Fruit Popsicle',         '1 unit',        12),
(@catSnacks, 'Diet Soda',                   '1 glass',       0);

-- ============================================================
-- 8. FAT-FREE SWEETS
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catSweetLow, 'Brown Sugar',           '1 tablespoon',  20),
(@catSweetLow, 'Hard Candy',            '1 unit',        25),
(@catSweetLow, 'Chewing Gum',           '1 unit',        10),
(@catSweetLow, 'Pumpkin Preserve',      '2 tablespoons', 35),
(@catSweetLow, 'Candied Fruits',        '1/2 serving',   44),
(@catSweetLow, 'Fruit Jam',             '1 tablespoon',  20),
(@catSweetLow, 'Gelatin',               '1 tablespoon',  12),
(@catSweetLow, 'Honey',                 '1 tablespoon',  17),
(@catSweetLow, 'Fruit Popsicle',        '1 unit',        17),
(@catSweetLow, 'Natural Fruit Juice',   '1 small glass', 63);

-- ============================================================
-- 9. HIGH-FAT SWEETS
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catSweetHigh, 'Chocolate Milk Powder',    '1 tablespoon',     25),
(@catSweetHigh, 'Alfajor',                  '1 unit',           60),
(@catSweetHigh, 'Chocolate Bar',            '1 small bar',      85),
(@catSweetHigh, 'Cream-filled Cookie',      '1 unit',           22),
(@catSweetHigh, 'Baked Cream Puff',         '1 unit',           40),
(@catSweetHigh, 'Cream Puff',               '1 unit',           80),
(@catSweetHigh, 'Brigadeiro (Truffle)',     '1 small unit',     30),
(@catSweetHigh, 'Brownie',                  '1 small unit',     70),
(@catSweetHigh, 'Plain Cake (no filling)',  '1 medium slice',   75),
(@catSweetHigh, 'Chocolate',                '1 small unit',     170),
(@catSweetHigh, 'Dried Chocolate',          '1 unit',           35),
(@catSweetHigh, 'Frosting / Topping',       '1 unit',           40),
(@catSweetHigh, 'Croissant',                '1 unit',           60),
(@catSweetHigh, 'Yogurt (Danone-style)',    '1 unit',           39),
(@catSweetHigh, 'Dulce de Leche',           '1 tablespoon',     22),
(@catSweetHigh, 'Egg Flan',                 '1 serving',        40),
(@catSweetHigh, 'Yogurt',                   '1 cup',            40),
(@catSweetHigh, 'Condensed Milk',           '3 tablespoons',    60),
(@catSweetHigh, 'Meringue',                 '1 unit',           20),
(@catSweetHigh, 'Mousse',                   '1 unit',           50),
(@catSweetHigh, 'Peanut Candy (Paçoca)',   '1 unit',           30),
(@catSweetHigh, 'Panettone',                '1 thin slice',     50),
(@catSweetHigh, 'Peanut Brittle',           '1 small unit',     30),
(@catSweetHigh, 'Pudding',                  '1 medium slice',   35),
(@catSweetHigh, 'Coconut Sweet (Quindim)', '1 unit',           40),
(@catSweetHigh, 'Filled Donut',             '1 unit',           60),
(@catSweetHigh, 'Milk Ice Cream',           '1 scoop',          60),
(@catSweetHigh, 'Apple Pie',                '1 small slice',    60),
(@catSweetHigh, 'Strawberry Pie',           '1 small slice',    60),
(@catSweetHigh, 'Wafer',                    '1 unit',           25),
(@catSweetHigh, 'Heavy Cream',              '1 tablespoon',     25),
(@catSweetHigh, 'Margarine',                '1 teaspoon',       25),
(@catSweetHigh, 'Butter',                   '1 teaspoon',       25),
(@catSweetHigh, 'Oil, Olive Oil, Lard, Bacon','1 teaspoon',    25);

-- ============================================================
-- 10. CONDIMENTS
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catCond, 'Ketchup',           '1 tablespoon',      10),
(@catCond, 'Mayonnaise',        '1 level tablespoon',40),
(@catCond, 'Mustard',           '1 tablespoon',      5),
(@catCond, 'White Sauce',       '2 tablespoons',     50),
(@catCond, 'Worcestershire Sauce','4 tablespoons',   40),
(@catCond, 'Hominy Seasoning',  '4 tablespoons',     25);

-- ============================================================
-- 11. OTHERS
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catOther, 'Fresh Chocolate Milk',         '200ml',                55),
(@catOther, 'Almond',                       '1 unit',               10),
(@catOther, 'Salted Roasted Peanut',        '1/2 tea cup',          60),
(@catOther, 'Olive',                        '2 units',              5),
(@catOther, 'Granola Bar',                  '1 unit',               55),
(@catOther, 'Plain Cracker',                '1 unit',               10),
(@catOther, 'Cracker Pack',                 '1 pack',               70),
(@catOther, 'Beef Parmigiana',              '1 serving',            155),
(@catOther, 'Toasted Cream Cracker',        '1 unit',               8),
(@catOther, 'Toasted Cashew',               '1 unit',               10),
(@catOther, 'Brazil Nut',                   '1 unit',               12),
(@catOther, 'Coconut Candy',                '1 unit',               65),
(@catOther, 'Preserved Coconut in Milk',    '1 tablespoon',         15),
(@catOther, 'Croquette / Kibbe / Pastry',   '1 small unit',         25),
(@catOther, 'Chicken Dumpling / Coxinha',   '1 small unit',         45),
(@catOther, 'Party Cassava Flour (Farofa)', '2 tablespoons',        25),
(@catOther, 'Feijoada (Black Bean Stew)',   '1/2 tea cup',          35),
(@catOther, 'Cheese Fondue',                '1 tablespoon',         25),
(@catOther, 'Granola',                      '1 tablespoon',         20),
(@catOther, 'Lasagna',                      '1 tablespoon',         25),
(@catOther, 'Coconut Milk',                 '1/2 glass',            70),
(@catOther, 'Passion Fruit Mousse',         '1/2 cup',              70),
(@catOther, 'Walnut',                       '1 unit',               10),
(@catOther, 'Stuffed Pastry (Paineira)',    '1 unit',               70),
(@catOther, 'Potato Salad',                 '1 small unit',         45),
(@catOther, 'Pine Nut',                     '100g',                 65),
(@catOther, 'Pistachio',                    '100g',                 77),
(@catOther, 'Pizza',                        '1 slice',              75),
(@catOther, 'Polenta with Sauce',           '1 tablespoon',         12),
(@catOther, 'Chips / Snack',                '1 tablespoon',         25),
(@catOther, 'Vegetable Salad',              '1 serving',            40),
(@catOther, 'Salad or Yogurt or Mousse',    '1/2 cup',              45),
(@catOther, 'Smoked Salmon',                '1 unit',               25),
(@catOther, 'Tapioca Crepe',                '1 unit',               25),
(@catOther, 'Cherry Tomato',                '2 units',              25),
(@catOther, 'Vatapá (Brazilian Stew)',      '2 tablespoons',        25),
(@catOther, 'Yakult Probiotic',             '1 unit',               15);

-- ============================================================
-- 12. BEVERAGES
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catBev, 'Coconut Water',              '1 glass',          10),
(@catBev, 'Fruit Batida (cocktail)',    '1/2 glass',        45),
(@catBev, 'Caipirinha',                 '1 glass',          45),
(@catBev, 'Beer',                       '1 can',            45),
(@catBev, 'Whole Milk or Yogurt',       '200ml',            30),
(@catBev, 'Skim Milk or Yogurt',        '200ml',            20),
(@catBev, 'Semi-skim Milk',             '200ml',            25),
(@catBev, 'Soy Milk Powder',            '1 tablespoon',     15),
(@catBev, 'Soda',                       '1 can (350ml)',    22),
(@catBev, 'Fresh Fruit Juice',          '1 glass',          25),
(@catBev, 'Tucupi (indigenous broth)',  '100ml',            10),
(@catBev, 'Wine',                       '1 glass (150ml)',  25),
(@catBev, 'Whisky / Dry White Wine',    '1 shot (50ml)',    40),
(@catBev, 'Vodka or Campari',           '1 shot',           40);

-- ============================================================
-- 13. SOUPS
-- ============================================================
INSERT INTO dbo.FoodItem (FoodCategoryId, Name, ServingSize, Points) VALUES
(@catSoup, 'Beef Broth',            '1 ladle',  10),
(@catSoup, 'Chicken Broth',         '1 ladle',  10),
(@catSoup, 'Chicken Stock',         '1 ladle',  10),
(@catSoup, 'Vegetable Broth',       '1 ladle',  5),
(@catSoup, 'Beef Consommé',         '6 ladles', 25),
(@catSoup, 'Asparagus Soup',        '4 ladles', 25),
(@catSoup, 'French Onion Soup',     '6 ladles', 25),
(@catSoup, 'Mushroom Soup',         '1 ladle',  30),
(@catSoup, 'Split Pea Soup',        '1 ladle',  60),
(@catSoup, 'Black Bean Soup',       '1 ladle',  35),
(@catSoup, 'Packaged Soup',         '1 ladle',  30);

GO

PRINT 'Food items seeded successfully.';
GO
