using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Infrastructure.Data.Seeds;

/// <summary>
/// Seeds reference data (FoodCategory, FoodItem, ExamCategory, Exam)
/// on first startup. Runs only when the tables are empty.
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext db, ILogger logger)
    {
        await SeedFoodDataAsync(db, logger);
        await SeedExamDataAsync(db, logger);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // FOOD DATA
    // ─────────────────────────────────────────────────────────────────────────

    private static async Task SeedFoodDataAsync(AppDbContext db, ILogger logger)
    {
        if (await db.FoodCategories.AnyAsync()) return;

        logger.LogInformation("Seeding food categories and items…");

        // ── Categories ────────────────────────────────────────────────────────
        var vegetables  = FoodCategory.Create("Vegetables",          "Free consumption — no points counted",                  0,    "Unlimited",         1).Value;
        var legumes     = FoodCategory.Create("Legumes",             "Cooked or raw legumes",                                 10,   "2 full tablespoons", 2).Value;
        var meats       = FoodCategory.Create("Meats",               "Meats, poultry, fish and seafood",                      25,   "1 serving quota",   3).Value;
        var cheeses     = FoodCategory.Create("Cheeses",             "All types of cheese",                                   25,   "1 serving quota",   4).Value;
        var grains      = FoodCategory.Create("Grains and Starches", "Rice, bread, pasta, flours and cereals",                20,   "1 serving quota",   5).Value;
        var fruits      = FoodCategory.Create("Fruits",              "Fresh and dried fruits",                                15,   "1 serving quota",   6).Value;
        var snacks      = FoodCategory.Create("Fast Food / Snacks",  "Snacks and fast food — points per individual serving",  null, "Serving",           7).Value;
        var sweetLow    = FoodCategory.Create("Fat-Free Sweets",     "Desserts and sweets with low fat content",              null, "Serving",           8).Value;
        var sweetHigh   = FoodCategory.Create("High-Fat Sweets",     "Desserts and sweets with fat — consume in moderation",  null, "Serving",           9).Value;
        var condiments  = FoodCategory.Create("Condiments",          "Seasonings and condiments",                             null, "Serving",           10).Value;
        var others      = FoodCategory.Create("Others",              "Food items that do not fit other categories",           null, "Serving",           11).Value;
        var beverages   = FoodCategory.Create("Beverages",           "Alcoholic and non-alcoholic beverages",                 null, "Serving",           12).Value;
        var soups       = FoodCategory.Create("Soups",               "Soups and broths",                                      null, "Serving / Ladle",   13).Value;

        db.FoodCategories.AddRange(vegetables, legumes, meats, cheeses, grains, fruits,
            snacks, sweetLow, sweetHigh, condiments, others, beverages, soups);
        await db.SaveChangesAsync();

        // ── Items — helper ────────────────────────────────────────────────────
        FoodItem Item(FoodCategory cat, string name, string serving, int pts) =>
            FoodItem.Create(cat.Id, name, serving, pts).Value;

        var items = new List<FoodItem>();

        // 1. VEGETABLES (0 pts)
        foreach (var name in new[]
        {
            "Swiss Chard","Watercress","Celery","Lettuce","Onion","Brussels Sprouts",
            "Fennel","Endive","Spinach","Beet Leaf","Jilo (African Eggplant)",
            "West Indian Gherkin","Turnip","Cucumber","Bell Pepper","Radish",
            "Cabbage","Arugula","Celery Stalk","Tomato"
        })
            items.Add(Item(vegetables, name, "Unlimited", 0));

        // 2. LEGUMES (10 pts)
        foreach (var name in new[]
        {
            "Zucchini","Artichoke","Asparagus","Eggplant","Broccoli",
            "Bamboo or Bean Sprouts","Green Onion","Carrot","Mushroom",
            "Cauliflower","Green Peas","Okra","Green Beans"
        })
            items.Add(Item(legumes, name, "2 tablespoons", 10));

        // 3. MEATS (25 pts)
        items.AddRange(new[]
        {
            Item(meats, "Canned Tuna",                "1 medium unit",          25),
            Item(meats, "Lean Steak (no fat/skin)",   "1 tea-saucer",           25),
            Item(meats, "Salted Codfish (Bacalhau)",  "1 tea-saucer",           25),
            Item(meats, "Shrimp",                      "1 tea-saucer",           25),
            Item(meats, "Dried Shrimp",                "1 tea-saucer",           25),
            Item(meats, "Chicken",                     "1 small unit",           25),
            Item(meats, "Beef",                        "1 small unit",           25),
            Item(meats, "Pork",                        "1 hamburger patty",      25),
            Item(meats, "Rabbit",                      "1 small slice or thigh", 25),
            Item(meats, "Quail",                       "2 units",                25),
            Item(meats, "Tenderloin / Sirloin",        "1 fillet",               25),
            Item(meats, "Beef Liver",                  "1 tea-saucer",           25),
            Item(meats, "Haddock",                     "1 small unit",           25),
            Item(meats, "Lobster",                     "1 unit",                 25),
            Item(meats, "Smoked Sausage (Linguiça)",   "1 unit",                 25),
            Item(meats, "Squid",                       "1 small unit",           25),
            Item(meats, "Shellfish",                   "1 small unit",           25),
            Item(meats, "Mussel",                      "1 unit",                 25),
            Item(meats, "Oyster",                      "1 unit",                 25),
            Item(meats, "Egg",                         "1 unit",                 25),
            Item(meats, "Quail Legs",                  "1 small portion",        25),
            Item(meats, "Smoked Turkey Breast",        "1 thin slice",           25),
            Item(meats, "Fresh Ham",                   "1 tea-saucer",           25),
            Item(meats, "Octopus",                     "1 tea-saucer",           25),
            Item(meats, "Ham / Cold Cuts",             "2 thin slices",          25),
            Item(meats, "Grilled Cheese",              "1 tea-saucer",           25),
            Item(meats, "Salmon",                      "1 tea-saucer",           25),
            Item(meats, "Smoked Salmon",               "2 thin slices",          25),
            Item(meats, "Hot Dog Sausage",             "1 unit",                 25),
            Item(meats, "Fresh Sardine",               "1 small unit",           25),
            Item(meats, "Sardine in Oil",              "1 small unit",           25),
            Item(meats, "Sardine in Tomato Sauce",     "1 small unit",           25),
            Item(meats, "Veal",                        "1 tea-saucer",           25),
        });

        // 4. CHEESES (25 pts)
        items.AddRange(new[]
        {
            Item(cheeses, "Blue Cheese (Natural)",  "1 thin slice",         25),
            Item(cheeses, "Camembert",               "1 thin slice",         25),
            Item(cheeses, "Gorgonzola",              "1 thin slice",         25),
            Item(cheeses, "Gruyère",                 "1 thin slice",         25),
            Item(cheeses, "Fresh White Cheese",      "1 thin slice",         25),
            Item(cheeses, "Mozzarella (block)",      "1/2 unit",             25),
            Item(cheeses, "Sliced Mozzarella",       "1 slice",              25),
            Item(cheeses, "Parmesan",                "1 tablespoon (level)", 25),
            Item(cheeses, "Provolone",               "1 thin slice",         25),
            Item(cheeses, "Soy Cheese (Tofu)",      "1 thin slice",         25),
            Item(cheeses, "Cream Cheese (Requeijão)","2 dessert spoons",    25),
            Item(cheeses, "Ricotta",                 "1 thin slice",         25),
            Item(cheeses, "Roquefort",               "1 thin slice",         25),
        });

        // 5. GRAINS AND STARCHES (20 pts)
        items.AddRange(new[]
        {
            Item(grains, "Cooked White Rice",              "2 tablespoons", 20),
            Item(grains, "Greek-style Rice",               "1 tablespoon",  20),
            Item(grains, "Vegetable Rice",                 "1 tablespoon",  20),
            Item(grains, "Baked Rice",                     "1 tablespoon",  20),
            Item(grains, "Cream Cracker Biscuit",          "3 units",       20),
            Item(grains, "Cooked Cannelloni",              "1 medium unit", 20),
            Item(grains, "Cooked Cappelletti / Ravioli",   "1 tablespoon",  20),
            Item(grains, "Creamed Corn",                   "1 tablespoon",  20),
            Item(grains, "Couscous",                       "1 tablespoon",  20),
            Item(grains, "Oat Bran",                       "1 tablespoon",  20),
            Item(grains, "Oat Flour",                      "1 tablespoon",  20),
            Item(grains, "Rice Flour",                     "1 tablespoon",  20),
            Item(grains, "Toasted Cassava Flour (Farofa)", "1 tablespoon",  20),
            Item(grains, "Cooked Black Beans",             "4 tablespoons", 20),
            Item(grains, "Beans / Peas / Lentils",         "4 tablespoons", 20),
            Item(grains, "Rice Starch",                    "1 tablespoon",  20),
            Item(grains, "Cornmeal (Fubá)",                "1 tablespoon",  20),
            Item(grains, "Wheat Germ",                     "1 tablespoon",  20),
            Item(grains, "Cooked Lasagna",                 "1 tablespoon",  20),
            Item(grains, "Cooked Pasta",                   "2 tablespoons", 20),
            Item(grains, "Cooked Cassava",                 "1 small piece", 20),
            Item(grains, "Cooked Corn",                    "2 tablespoons", 20),
            Item(grains, "Corn on the Cob",                "4 tablespoons", 20),
            Item(grains, "Hominy (Canjica)",               "4 tablespoons", 25),
            Item(grains, "Sliced Toasted Bread",           "1 slice",       20),
            Item(grains, "Sandwich Bread Slice",           "1 slice",       20),
            Item(grains, "Gluten Bread Toast",             "1 slice",       20),
            Item(grains, "Hamburger Bun",                  "1/2 unit",      20),
            Item(grains, "Whole Grain Bread",              "1 slice",       20),
            Item(grains, "Whole Grain Toast",              "1 slice",       20),
            Item(grains, "Dinner Roll",                    "1 small unit",  20),
            Item(grains, "Cheese Bread (Pão de Queijo)",   "1 small unit",  20),
            Item(grains, "Pita Bread",                     "1 small unit",  20),
            Item(grains, "Water and Salt Cracker",         "3 units",       20),
        });

        // 6. FRUITS (15 pts)
        items.AddRange(new[]
        {
            Item(fruits, "Avocado",            "1 small unit",  15),
            Item(fruits, "Pineapple",           "1 small slice", 15),
            Item(fruits, "Yellow Plum",         "2 small units", 15),
            Item(fruits, "Red Plum",            "2 small units", 15),
            Item(fruits, "Dried Plum",          "2 units",       15),
            Item(fruits, "Small Banana",        "1/2 unit",      15),
            Item(fruits, "Banana",              "1 unit",        15),
            Item(fruits, "Persimmon",           "1 small unit",  15),
            Item(fruits, "Coconut (no water)", "1 small slice", 15),
            Item(fruits, "Fig",                 "1 unit",        15),
            Item(fruits, "Raspberry",           "1/2 tea cup",   15),
            Item(fruits, "Sugar Apple",         "1/2 unit",      15),
            Item(fruits, "Grapefruit",          "1/2 unit",      15),
            Item(fruits, "Guava",               "1 small unit",  15),
            Item(fruits, "Jackfruit",           "2 small pieces",15),
            Item(fruits, "Kiwi",                "1 unit",        15),
            Item(fruits, "Orange",              "1 unit",        15),
            Item(fruits, "Pera Orange",         "1 unit",        15),
            Item(fruits, "Apple",               "1 unit",        15),
            Item(fruits, "Papaya",              "1 medium slice",15),
            Item(fruits, "Mango",               "1 small unit",  15),
            Item(fruits, "Passion Fruit",       "1 unit",        15),
            Item(fruits, "Watermelon",          "1 slice",       15),
            Item(fruits, "Melon",               "1 slice",       15),
            Item(fruits, "Blueberry",           "1/2 tea cup",   15),
            Item(fruits, "Nectarine",           "1 unit",        15),
            Item(fruits, "Papaya (small)",     "1 unit",        15),
            Item(fruits, "Peach",               "1 unit",        15),
            Item(fruits, "Custard Apple",       "1/2 unit",      15),
            Item(fruits, "Fruit Salad",         "1 tea cup",     15),
            Item(fruits, "Tangerine",           "1 unit",        15),
            Item(fruits, "Grapes",              "12 units",      15),
        });

        // 7. FAST FOOD / SNACKS
        items.AddRange(new[]
        {
            Item(snacks, "Small French Fries",         "1 serving", 62),
            Item(snacks, "Medium French Fries",        "1 serving", 80),
            Item(snacks, "Big Mac",                    "1 unit",    164),
            Item(snacks, "Cheeseburger",               "1 unit",    100),
            Item(snacks, "Plain Hamburger",            "1 unit",    82),
            Item(snacks, "Mc Chicken",                 "1 unit",    120),
            Item(snacks, "Happy Meal (Chicken)",       "1 unit",    175),
            Item(snacks, "Mc Chicken Crispy",          "1 unit",    140),
            Item(snacks, "Mc Nuggets",                 "6 units",   60),
            Item(snacks, "Quarter Pounder (Large)",    "1 unit",    190),
            Item(snacks, "Diet / Light Line — Type 1","1 unit",    2),
            Item(snacks, "Diet Chocolate Powder",      "1 teaspoon",10),
            Item(snacks, "Diet Gelatin",               "1 tablespoon",3),
            Item(snacks, "Diet Jam",                   "1 tablespoon",5),
            Item(snacks, "Diet Ice Cream",             "1 scoop",   12),
            Item(snacks, "Diet Fruit Popsicle",        "1 unit",    12),
            Item(snacks, "Diet Soda",                  "1 glass",   0),
        });

        // 8. FAT-FREE SWEETS
        items.AddRange(new[]
        {
            Item(sweetLow, "Brown Sugar",        "1 tablespoon",  20),
            Item(sweetLow, "Hard Candy",         "1 unit",        25),
            Item(sweetLow, "Chewing Gum",        "1 unit",        10),
            Item(sweetLow, "Pumpkin Preserve",   "2 tablespoons", 35),
            Item(sweetLow, "Candied Fruits",     "1/2 serving",   44),
            Item(sweetLow, "Fruit Jam",          "1 tablespoon",  20),
            Item(sweetLow, "Gelatin",            "1 tablespoon",  12),
            Item(sweetLow, "Honey",              "1 tablespoon",  17),
            Item(sweetLow, "Fruit Popsicle",     "1 unit",        17),
            Item(sweetLow, "Natural Fruit Juice","1 small glass", 63),
        });

        // 9. HIGH-FAT SWEETS
        items.AddRange(new[]
        {
            Item(sweetHigh, "Chocolate Milk Powder",    "1 tablespoon",  25),
            Item(sweetHigh, "Alfajor",                  "1 unit",        60),
            Item(sweetHigh, "Chocolate Bar",            "1 small bar",   85),
            Item(sweetHigh, "Cream-filled Cookie",      "1 unit",        22),
            Item(sweetHigh, "Baked Cream Puff",         "1 unit",        40),
            Item(sweetHigh, "Cream Puff",               "1 unit",        80),
            Item(sweetHigh, "Brigadeiro (Truffle)",     "1 small unit",  30),
            Item(sweetHigh, "Brownie",                  "1 small unit",  70),
            Item(sweetHigh, "Plain Cake (no filling)",  "1 medium slice",75),
            Item(sweetHigh, "Chocolate",                "1 small unit",  170),
            Item(sweetHigh, "Dried Chocolate",          "1 unit",        35),
            Item(sweetHigh, "Frosting / Topping",       "1 unit",        40),
            Item(sweetHigh, "Croissant",                "1 unit",        60),
            Item(sweetHigh, "Yogurt (Danone-style)",    "1 unit",        39),
            Item(sweetHigh, "Dulce de Leche",           "1 tablespoon",  22),
            Item(sweetHigh, "Egg Flan",                 "1 serving",     40),
            Item(sweetHigh, "Yogurt",                   "1 cup",         40),
            Item(sweetHigh, "Condensed Milk",           "3 tablespoons", 60),
            Item(sweetHigh, "Meringue",                 "1 unit",        20),
            Item(sweetHigh, "Mousse",                   "1 unit",        50),
            Item(sweetHigh, "Peanut Candy (Paçoca)",   "1 unit",        30),
            Item(sweetHigh, "Panettone",                "1 thin slice",  50),
            Item(sweetHigh, "Peanut Brittle",           "1 small unit",  30),
            Item(sweetHigh, "Pudding",                  "1 medium slice",35),
            Item(sweetHigh, "Coconut Sweet (Quindim)", "1 unit",        40),
            Item(sweetHigh, "Filled Donut",             "1 unit",        60),
            Item(sweetHigh, "Milk Ice Cream",           "1 scoop",       60),
            Item(sweetHigh, "Apple Pie",                "1 small slice", 60),
            Item(sweetHigh, "Strawberry Pie",           "1 small slice", 60),
            Item(sweetHigh, "Wafer",                    "1 unit",        25),
            Item(sweetHigh, "Heavy Cream",              "1 tablespoon",  25),
            Item(sweetHigh, "Margarine",                "1 teaspoon",    25),
            Item(sweetHigh, "Butter",                   "1 teaspoon",    25),
            Item(sweetHigh, "Oil, Olive Oil, Lard, Bacon","1 teaspoon", 25),
        });

        // 10. CONDIMENTS
        items.AddRange(new[]
        {
            Item(condiments, "Ketchup",              "1 tablespoon",       10),
            Item(condiments, "Mayonnaise",           "1 level tablespoon", 40),
            Item(condiments, "Mustard",              "1 tablespoon",       5),
            Item(condiments, "White Sauce",          "2 tablespoons",      50),
            Item(condiments, "Worcestershire Sauce", "4 tablespoons",      40),
            Item(condiments, "Hominy Seasoning",     "4 tablespoons",      25),
        });

        // 11. OTHERS
        items.AddRange(new[]
        {
            Item(others, "Fresh Chocolate Milk",       "200ml",            55),
            Item(others, "Almond",                     "1 unit",           10),
            Item(others, "Salted Roasted Peanut",      "1/2 tea cup",      60),
            Item(others, "Olive",                      "2 units",          5),
            Item(others, "Granola Bar",                "1 unit",           55),
            Item(others, "Plain Cracker",              "1 unit",           10),
            Item(others, "Cracker Pack",               "1 pack",           70),
            Item(others, "Beef Parmigiana",            "1 serving",        155),
            Item(others, "Toasted Cream Cracker",      "1 unit",           8),
            Item(others, "Toasted Cashew",             "1 unit",           10),
            Item(others, "Brazil Nut",                 "1 unit",           12),
            Item(others, "Coconut Candy",              "1 unit",           65),
            Item(others, "Preserved Coconut in Milk",  "1 tablespoon",     15),
            Item(others, "Croquette / Kibbe / Pastry", "1 small unit",     25),
            Item(others, "Chicken Dumpling / Coxinha", "1 small unit",     45),
            Item(others, "Party Cassava Flour (Farofa)","2 tablespoons",   25),
            Item(others, "Feijoada (Black Bean Stew)", "1/2 tea cup",      35),
            Item(others, "Cheese Fondue",              "1 tablespoon",     25),
            Item(others, "Granola",                    "1 tablespoon",     20),
            Item(others, "Lasagna",                    "1 tablespoon",     25),
            Item(others, "Coconut Milk",               "1/2 glass",        70),
            Item(others, "Passion Fruit Mousse",       "1/2 cup",          70),
            Item(others, "Walnut",                     "1 unit",           10),
            Item(others, "Stuffed Pastry (Paineira)",  "1 unit",           70),
            Item(others, "Potato Salad",               "1 small unit",     45),
            Item(others, "Pine Nut",                   "100g",             65),
            Item(others, "Pistachio",                  "100g",             77),
            Item(others, "Pizza",                      "1 slice",          75),
            Item(others, "Polenta with Sauce",         "1 tablespoon",     12),
            Item(others, "Chips / Snack",              "1 tablespoon",     25),
            Item(others, "Vegetable Salad",            "1 serving",        40),
            Item(others, "Salad or Yogurt or Mousse",  "1/2 cup",          45),
            Item(others, "Smoked Salmon",              "1 unit",           25),
            Item(others, "Tapioca Crepe",              "1 unit",           25),
            Item(others, "Cherry Tomato",              "2 units",          25),
            Item(others, "Vatapá (Brazilian Stew)",   "2 tablespoons",    25),
            Item(others, "Yakult Probiotic",           "1 unit",           15),
        });

        // 12. BEVERAGES
        items.AddRange(new[]
        {
            Item(beverages, "Coconut Water",           "1 glass",      10),
            Item(beverages, "Fruit Batida (cocktail)", "1/2 glass",    45),
            Item(beverages, "Caipirinha",              "1 glass",      45),
            Item(beverages, "Beer",                    "1 can",        45),
            Item(beverages, "Whole Milk or Yogurt",    "200ml",        30),
            Item(beverages, "Skim Milk or Yogurt",     "200ml",        20),
            Item(beverages, "Semi-skim Milk",          "200ml",        25),
            Item(beverages, "Soy Milk Powder",         "1 tablespoon", 15),
            Item(beverages, "Soda",                    "1 can (350ml)",22),
            Item(beverages, "Fresh Fruit Juice",       "1 glass",      25),
            Item(beverages, "Tucupi (indigenous broth)","100ml",       10),
            Item(beverages, "Wine",                    "1 glass (150ml)",25),
            Item(beverages, "Whisky / Dry White Wine", "1 shot (50ml)",40),
            Item(beverages, "Vodka or Campari",        "1 shot",       40),
        });

        // 13. SOUPS
        items.AddRange(new[]
        {
            Item(soups, "Beef Broth",         "1 ladle",  10),
            Item(soups, "Chicken Broth",      "1 ladle",  10),
            Item(soups, "Chicken Stock",      "1 ladle",  10),
            Item(soups, "Vegetable Broth",    "1 ladle",  5),
            Item(soups, "Beef Consommé",      "6 ladles", 25),
            Item(soups, "Asparagus Soup",     "4 ladles", 25),
            Item(soups, "French Onion Soup",  "6 ladles", 25),
            Item(soups, "Mushroom Soup",      "1 ladle",  30),
            Item(soups, "Split Pea Soup",     "1 ladle",  60),
            Item(soups, "Black Bean Soup",    "1 ladle",  35),
            Item(soups, "Packaged Soup",      "1 ladle",  30),
        });

        db.FoodItems.AddRange(items);
        await db.SaveChangesAsync();

        logger.LogInformation("Seeded {CatCount} food categories and {ItemCount} food items.",
            13, items.Count);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // EXAM DATA
    // ─────────────────────────────────────────────────────────────────────────

    private static async Task SeedExamDataAsync(AppDbContext db, ILogger logger)
    {
        if (await db.ExamCategories.AnyAsync()) return;

        logger.LogInformation("Seeding exam categories and exams…");

        // ── Categories ────────────────────────────────────────────────────────
        var bioch   = ExamCategory.Create("Biochemistry (Blood)",     1);
        var imuno   = ExamCategory.Create("Immunology",               2);
        var hepat   = ExamCategory.Create("Hepatitis Serology",       3);
        var hiv     = ExamCategory.Create("HIV Serology",             4);
        var ana     = ExamCategory.Create("Antinuclear Antibodies",   5);
        var crp     = ExamCategory.Create("C-Reactive Protein (CRP)", 6);
        var tumor   = ExamCategory.Create("Tumor Markers",            7);
        var stool   = ExamCategory.Create("Stool",                    8);
        var urine   = ExamCategory.Create("Urine",                    9);
        var hemat   = ExamCategory.Create("Hematology",               10);
        var horm    = ExamCategory.Create("Hormones",                 11);
        var thyroid = ExamCategory.Create("Thyroid",                  12);

        db.ExamCategories.AddRange(bioch, imuno, hepat, hiv, ana, crp, tumor, stool, urine, hemat, horm, thyroid);
        await db.SaveChangesAsync();

        // ── Exams — helper ────────────────────────────────────────────────────
        Exam E(ExamCategory cat, string name, string? abbr = null, string? desc = null) =>
            Exam.Create(cat.Id, name, abbr, desc);

        var exams = new List<Exam>
        {
            // 1. BIOCHEMISTRY (BLOOD)
            E(bioch, "1,25 Vitamin D",                  null,     "Active form of vitamin D"),
            E(bioch, "25 Vitamin D",                    null,     "Circulating vitamin D — main measurement form"),
            E(bioch, "Folic Acid",                      null,     "Vitamin B9"),
            E(bioch, "Uric Acid",                       null,     "Purine metabolism byproduct"),
            E(bioch, "Albumin",                         null,     "Most abundant plasma protein"),
            E(bioch, "Amylase",                         null,     "Pancreatic and salivary digestive enzyme"),
            E(bioch, "Apolipoprotein A",                "Apo A",  null),
            E(bioch, "Apolipoprotein B",                "Apo B",  null),
            E(bioch, "Bilirubin",                       null,     "Total, direct and indirect"),
            E(bioch, "Ionized Calcium",                 null,     null),
            E(bioch, "Total Calcium",                   null,     null),
            E(bioch, "Copper",                          null,     null),
            E(bioch, "Total Cholesterol and Fractions", null,     "LDL, HDL, VLDL"),
            E(bioch, "CPK",                             "CPK",    "Creatine phosphokinase"),
            E(bioch, "Creatinine",                      null,     null),
            E(bioch, "CTX",                             "CTX",    "C-terminal telopeptide — bone resorption marker"),
            E(bioch, "Protein Electrophoresis",         null,     null),
            E(bioch, "Alkaline Phosphatase",            "ALP",    null),
            E(bioch, "Phosphorus",                      null,     null),
            E(bioch, "Fructosamine",                    null,     "Short-term glycemic control"),
            E(bioch, "Gamma GT",                        "GGT",    "Gamma-glutamyltransferase"),
            E(bioch, "Fasting Glucose",                 null,     "Blood glucose — fasting"),
            E(bioch, "Glycated Hemoglobin",             "HbA1c",  "2-3 month glycemic control"),
            E(bioch, "Lipase",                          null,     null),
            E(bioch, "Magnesium",                       null,     null),
            E(bioch, "P1NP",                            "P1NP",   "Procollagen type I N-terminal propeptide — bone formation marker"),
            E(bioch, "Potassium",                       null,     null),
            E(bioch, "Total Protein and Fractions",     null,     null),
            E(bioch, "Sodium",                          null,     null),
            E(bioch, "AST",                             "AST",    "Aspartate aminotransferase (TGO)"),
            E(bioch, "ALT",                             "ALT",    "Alanine aminotransferase (TGP)"),
            E(bioch, "Triglycerides",                   null,     null),
            E(bioch, "Blood Urea Nitrogen",             "BUN",    null),
            E(bioch, "Vitamin B12",                     null,     "Cobalamin"),
            E(bioch, "Zinc",                            null,     null),

            // 2. IMMUNOLOGY
            E(imuno, "Anti-ANCA",                              "ANCA",    "Anti-neutrophil cytoplasmic antibody"),
            E(imuno, "Anti-ASCA",                              "ASCA",    "Anti-Saccharomyces cerevisiae antibody"),
            E(imuno, "Anti-Cardiolipin (IgG + IgM + IgA)",    null,      null),
            E(imuno, "Anti-Parietal Cell Antibody",            null,      null),
            E(imuno, "Anti-CCP",                               "CCP",     "Anti-cyclic citrullinated peptide"),
            E(imuno, "Anti-Endomysial Antibody",               null,      "IgA — celiac disease screening"),
            E(imuno, "Anti-Phospholipid",                      null,      null),
            E(imuno, "Anti-GAD",                               "GAD",     "Glutamic acid decarboxylase antibody"),
            E(imuno, "Anti-Gliadin",                           null,      null),
            E(imuno, "Anti-Islet Cell / ICA",                  "ICA",     "Islet cell antibody"),
            E(imuno, "Anti-Insulin / IAA",                     "IAA",     "Insulin autoantibody"),
            E(imuno, "Anti-Mitochondrial Antibody",            "AMA",     null),
            E(imuno, "Anti-Smooth Muscle Antibody",            "ASMA",    null),
            E(imuno, "Anti-Transglutaminase (IgA)",            "tTG",     "Celiac disease screening"),
            E(imuno, "HLA-B27",                                "HLA-B27", "Human leukocyte antigen B27"),
            E(imuno, "LKM1",                                   "LKM1",    "Liver-kidney microsomal antibody type 1"),
            E(imuno, "Cytomegalovirus Serology",               "CMV",     null),
            E(imuno, "Mononucleosis Serology",                 null,      null),
            E(imuno, "Syphilis Serology",                      null,      "VDRL / FTA-Abs"),
            E(imuno, "Toxoplasmosis Serology",                 null,      null),

            // 3. HEPATITIS SEROLOGY
            E(hepat, "Hepatitis A", "HAV", "Anti-HAV IgM and IgG"),
            E(hepat, "Hepatitis B", "HBV", "HBsAg, Anti-HBs, Anti-HBc"),
            E(hepat, "Hepatitis C", "HCV", "Anti-HCV"),

            // 4. HIV SEROLOGY
            E(hiv, "HIV (HIV1 - HIV2)", "HIV", "4th generation — p24 antigen + antibodies"),

            // 5. ANTINUCLEAR ANTIBODIES
            E(ana, "ASLO",                 "ASLO",      "Anti-streptolysin O"),
            E(ana, "Native DNA Antibody",  "Anti-dsDNA","Double-stranded DNA antibody"),
            E(ana, "ENA Panel",            "ENA",       "Extractable nuclear antigen antibodies"),
            E(ana, "Antinuclear Antibody", "ANA",       "Fluorescent antinuclear antibody"),
            E(ana, "Rheumatoid Factor",    "RF",        null),
            E(ana, "Anti-SSA (Ro)",        "SSA",       "Anti-Ro antibody"),
            E(ana, "Anti-SSB (La)",        "SSB",       "Anti-La antibody"),

            // 6. C-REACTIVE PROTEIN
            E(crp, "CRP — Inflammatory Process Assessment", "CRP",    "Quantitative C-reactive protein"),
            E(crp, "CRP — Cardiovascular Risk Assessment",  "hs-CRP", "High-sensitivity C-reactive protein"),

            // 7. TUMOR MARKERS
            E(tumor, "Alpha-fetoprotein", "AFP",    "Hepatocellular and germ cell marker"),
            E(tumor, "CA 15-3",           "CA 15-3","Breast cancer marker"),
            E(tumor, "CA 19-9",           "CA 19-9","Pancreatic / GI tract cancer marker"),
            E(tumor, "CA 72-4",           "CA 72-4","Gastric cancer marker"),
            E(tumor, "CA 125",            "CA 125", "Ovarian cancer marker"),
            E(tumor, "Calcitonin",        null,     "Medullary thyroid cancer marker"),
            E(tumor, "CEA",               "CEA",    "Carcinoembryonic antigen — colorectal"),
            E(tumor, "Total PSA",         "PSA",    "Prostate-specific antigen"),

            // 8. STOOL
            E(stool, "Stool Ova and Parasite Test", null,   "Parasite and egg screening"),
            E(stool, "Fecal Occult Blood Test",     "FOBT", "Occult blood screening"),

            // 9. URINE
            E(urine, "Spot Urine Albumin",           null,   "Albumin-to-creatinine ratio"),
            E(urine, "24-hour Urine Calcium",        null,   "Calcium in 24-hour urine collection"),
            E(urine, "24-hour Urine Cortisol",       null,   "Free urinary cortisol"),
            E(urine, "Urine Culture and Sensitivity","UC&S", "Urine culture with antibiogram"),
            E(urine, "Urinalysis (Type I)",          "UA",   "Complete urinalysis"),

            // 10. HEMATOLOGY
            E(hemat, "Complete Coagulation Panel",  null,  "PT, aPTT, fibrinogen"),
            E(hemat, "Hemoglobin Electrophoresis",  null,  null),
            E(hemat, "Sickle Cell Test",            null,  "Sickle cell screening"),
            E(hemat, "Ferritin",                    null,  "Iron storage protein"),
            E(hemat, "Serum Iron",                  null,  null),
            E(hemat, "Blood Type and Rh Factor",    "ABO", null),
            E(hemat, "Complete Blood Count",        "CBC", "Red cells, white cells and platelets"),
            E(hemat, "Platelet Count",              null,  null),
            E(hemat, "Transferrin Saturation",      null,  null),
            E(hemat, "Erythrocyte Sedimentation Rate","ESR",null),
            E(hemat, "Reticulocyte Count",          null,  null),

            // 11. HORMONES
            E(horm, "17-OH Progesterone",           "17-OHP",  null),
            E(horm, "ACTH",                         "ACTH",    "Adrenocorticotropic hormone"),
            E(horm, "Aldosterone",                  null,      null),
            E(horm, "Androstenedione",              null,      null),
            E(horm, "Plasma Renin Activity",        "PRA",     null),
            E(horm, "Beta-hCG",                     "β-hCG",   "Human chorionic gonadotropin — beta"),
            E(horm, "Basal Cortisol",               null,      "Morning serum cortisol"),
            E(horm, "DHEA",                         "DHEA",    "Dehydroepiandrosterone"),
            E(horm, "Estradiol",                    "E2",      null),
            E(horm, "FSH",                          "FSH",     "Follicle-stimulating hormone"),
            E(horm, "Growth Hormone",               "GH",      null),
            E(horm, "IGF-1",                        "IGF-1",   "Insulin-like growth factor 1 (Somatomedin C)"),
            E(horm, "IGFBP-3",                      "IGFBP-3", "Insulin-like growth factor binding protein 3"),
            E(horm, "Insulin",                      null,      null),
            E(horm, "LH",                           "LH",      "Luteinizing hormone"),
            E(horm, "Osteocalcin",                  null,      "Bone formation marker"),
            E(horm, "Parathyroid Hormone (PTH)",    "PTH",     "Intact parathyroid hormone"),
            E(horm, "C-Peptide",                    "C-pep",   "Beta-cell residual function"),
            E(horm, "Progesterone",                 null,      null),
            E(horm, "Prolactin",                    "PRL",     null),
            E(horm, "DHEA-Sulfate",                 "DHEA-S",  "Dehydroepiandrosterone sulfate"),
            E(horm, "SHBG",                         "SHBG",    "Sex hormone-binding globulin"),
            E(horm, "Total Testosterone",           null,      null),
            E(horm, "Anti-Müllerian Hormone",       "AMH",     "Ovarian reserve marker"),

            // 12. THYROID
            E(thyroid, "Anti-Thyroglobulin Antibody", "Anti-Tg",  "Thyroglobulin antibody"),
            E(thyroid, "Anti-Thyroid Peroxidase",      "Anti-TPO", "Thyroid peroxidase antibody"),
            E(thyroid, "T3",                           "T3",       "Total triiodothyronine"),
            E(thyroid, "Free T4",                      "fT4",      "Free thyroxine"),
            E(thyroid, "Total T4",                     "T4",       "Total thyroxine"),
            E(thyroid, "Thyroglobulin",                "Tg",       "Thyroid tissue marker"),
            E(thyroid, "TSH Receptor Antibody",        "TRAb",     "Anti-TSH receptor antibody"),
            E(thyroid, "TSH",                          "TSH",      "Thyroid-stimulating hormone"),
        };

        db.Exams.AddRange(exams);
        await db.SaveChangesAsync();

        logger.LogInformation("Seeded {CatCount} exam categories and {ExamCount} exams.",
            12, exams.Count);
    }
}
