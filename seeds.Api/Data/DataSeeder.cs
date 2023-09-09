﻿using seeds.Dal.Model;

namespace seeds.Api.Data;

public class DataSeeder
{
    private readonly seedsApiContext context;
    public List<Category> Cats = new();
    public List<Family> Fams = new();
    public List<Topic> Topics = new();
    public List<User> Users = new();
    public List<UserPreference> Cups = new();
    public List<Idea> Ideas = new();

    public DataSeeder(seedsApiContext dbContext)
    {
        context = dbContext;
    }

    public void SeedData()
    {
        PopulateCatsFamsTopics();
        if (!context.Category.Any()) { context.Category.AddRange(Cats); }
        else { Cats = context.Category.ToList(); }
        if (!context.Family.Any()) { context.Family.AddRange(Fams); }
        else { Fams = context.Family.ToList(); }
        if (!context.Topic.Any()) { context.Topic.AddRange(Topics); }
        else { Topics = context.Topic.ToList(); }

        PopulateUsers();
        if (!context.User.Any()) { context.User.AddRange(Users); }
        else { Users = context.User.ToList(); }

        PopulateIdeas();
        if (!context.Idea.Any()) { context.Idea.AddRange(Ideas); }
        else { Ideas = context.Idea.ToList(); }

        context.SaveChanges();
    }

    /// <summary>
    /// This function is copied from seeds.Api.AutoGeneratedDataSeeder.cs,
    /// which in turn is generated from ../topics.md by MdConverter.Program.cs
    /// </summary>
    public void PopulateCatsFamsTopics()
    {
        List<string> famTopicsString;
        List<Topic> famTopics;

        #region FEA - Features
        Cats.Add(new() { Key = "FEA", Name = "Features" });

        famTopicsString = new() { "Spotify", "Apple Music", "Soundcloud", "YouTube Music", "Deezer", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "FEA", Name = $"music streamer: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "FEA", Name = "music streamer", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;


        famTopicsString = new() { "Netflix", "Amazon Prime", "Disney Plus", "HBO Max", "Sky", "YouTube TV", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "FEA", Name = $"video streamer: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "FEA", Name = "video streamer", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;


        famTopicsString = new() { "Instagram", "Facebook", "Snapchat", "Reddit", "TikTok", "LinkedIn", "X", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "FEA", Name = $"social media: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "FEA", Name = "social media", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;


        famTopicsString = new() { "Zoom", "Microsoft Teams", "Discord", "Slack", "Skype", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "FEA", Name = $"online meeting: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "FEA", Name = "online meeting", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;


        famTopicsString = new() { "Windows", "Linux", "OsX", "Android", "iOs", "Google Phone", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "FEA", Name = $"operating systems: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "FEA", Name = "operating systems", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;

        Topics.Add(new() { CategoryKey = "FEA", Name = "games" });

        famTopicsString = new() { "Tinder", "Bumble", "Grindr", "OKCupid", "Lovoo", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "FEA", Name = $"dating apps: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "FEA", Name = "dating apps", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;

        #endregion

        #region TRA - Transportation
        Cats.Add(new() { Key = "TRA", Name = "Transportation" });
        Topics.Add(new() { CategoryKey = "TRA", Name = "electro" });
        Topics.Add(new() { CategoryKey = "TRA", Name = "alternative fuels" });
        Topics.Add(new() { CategoryKey = "TRA", Name = "cars" });
        Topics.Add(new() { CategoryKey = "TRA", Name = "motorbikes" });
        Topics.Add(new() { CategoryKey = "TRA", Name = "bicycle" });
        Topics.Add(new() { CategoryKey = "TRA", Name = "new transportation" });
        Topics.Add(new() { CategoryKey = "TRA", Name = "public transport" });
        Topics.Add(new() { CategoryKey = "TRA", Name = "boats" });
        #endregion

        #region POW - Energy
        Cats.Add(new() { Key = "POW", Name = "Energy" });
        Topics.Add(new() { CategoryKey = "POW", Name = "alternatives" });
        Topics.Add(new() { CategoryKey = "POW", Name = "storage" });
        Topics.Add(new() { CategoryKey = "POW", Name = "reduction" });
        #endregion

        #region LIFE - Lifestyle
        Cats.Add(new() { Key = "LIFE", Name = "Lifestyle" });
        Topics.Add(new() { CategoryKey = "LIFE", Name = "gaming" });
        Topics.Add(new() { CategoryKey = "LIFE", Name = "traveling" });
        Topics.Add(new() { CategoryKey = "LIFE", Name = "beauty" });
        Topics.Add(new() { CategoryKey = "LIFE", Name = "dating" });
        Topics.Add(new() { CategoryKey = "LIFE", Name = "too much time" });
        Topics.Add(new() { CategoryKey = "LIFE", Name = "life hacks" });
        Topics.Add(new() { CategoryKey = "LIFE", Name = "series & movies" });
        Topics.Add(new() { CategoryKey = "LIFE", Name = "home plants" });
        Topics.Add(new() { CategoryKey = "LIFE", Name = "tinkering" });
        Topics.Add(new() { CategoryKey = "LIFE", Name = "photography" });
        Topics.Add(new() { CategoryKey = "LIFE", Name = "social media" });

        famTopicsString = new() { "Dogs", "Cats", "Fish", "Birds", "Hamsters", "Guinea Pigs", "Rabbits", "Turtles", "Snakes", "Lizards", "Ferrets", "Gerbils", "Hedgehogs", "Chinchillas", "Tarantulas", "Crabs", "Horses", "Sugar Gliders", "Mice", "Rats", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "LIFE", Name = $"pet: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "LIFE", Name = "pet", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;

        Topics.Add(new() { CategoryKey = "LIFE", Name = "shopping" });
        Topics.Add(new() { CategoryKey = "LIFE", Name = "childish fun" });
        #endregion

        #region WORK - Working
        Cats.Add(new() { Key = "WORK", Name = "Working" });
        Topics.Add(new() { CategoryKey = "WORK", Name = "productivity" });

        famTopicsString = new() { "farmer", "construction worker", "designer", "hair dresser", "garbage person", "barkeeper", "cook", "barista", "programmer", "doctor", "electronic engineer", "electric engineer", "scientist", "journalist", "film producer", "Influencer", "legal worker", "social worker", "merchant", "tourism worker", "trucker", "pilot", "carpenter", "electrician", "plumber", "welder", "politician", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "WORK", Name = $"for branch: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "WORK", Name = "for branch", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;

        Topics.Add(new() { CategoryKey = "WORK", Name = "finding passion" });
        #endregion

        #region $$$ - Money
        Cats.Add(new() { Key = "$$$", Name = "Money" });
        Topics.Add(new() { CategoryKey = "$$$", Name = "cryptocurrency" });
        Topics.Add(new() { CategoryKey = "$$$", Name = "trading" });
        Topics.Add(new() { CategoryKey = "$$$", Name = "banks" });
        #endregion

        #region HOME - Housing
        Cats.Add(new() { Key = "HOME", Name = "Housing" });
        Topics.Add(new() { CategoryKey = "HOME", Name = "living solutions" });
        Topics.Add(new() { CategoryKey = "HOME", Name = "garden" });
        Topics.Add(new() { CategoryKey = "HOME", Name = "keep it clean" });
        Topics.Add(new() { CategoryKey = "HOME", Name = "toiletty" });
        #endregion

        #region ENV - Environment
        Cats.Add(new() { Key = "ENV", Name = "Environment" });
        Topics.Add(new() { CategoryKey = "ENV", Name = "CO2 reduction" });
        Topics.Add(new() { CategoryKey = "ENV", Name = "climate change" });
        Topics.Add(new() { CategoryKey = "ENV", Name = "extreme weather" });
        Topics.Add(new() { CategoryKey = "ENV", Name = "save the bees" });
        Topics.Add(new() { CategoryKey = "ENV", Name = "reducle plastic" });
        Topics.Add(new() { CategoryKey = "ENV", Name = "greenify cities" });
        Topics.Add(new() { CategoryKey = "ENV", Name = "sustainability" });
        #endregion

        #region CPU - Software
        Cats.Add(new() { Key = "CPU", Name = "Software" });
        Topics.Add(new() { CategoryKey = "CPU", Name = "games" });
        Topics.Add(new() { CategoryKey = "CPU", Name = "augmented reality" });

        famTopicsString = new() { "Python", "Java", "JavaScript", "C++", "C#", "PHP", "Ruby", "Swift", "Kotlin", "TypeScript", "Go", "Rust", "Perl", "MATLAB", "R", "Scala", "Dart", "Lua", "Julia", "Objective-C", "COBOL", "Fortran", "Haskell", "Groovy", "Shell Scripting (Bash)", "Assembly Language", "PL/SQL", "VB", "(La)TeX", "Lisp", "Prolog", "Ada", "SQL", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "CPU", Name = $"for language: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "CPU", Name = "for language", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;

        Topics.Add(new() { CategoryKey = "CPU", Name = "mobile app dev." });
        Topics.Add(new() { CategoryKey = "CPU", Name = "databases" });
        Topics.Add(new() { CategoryKey = "CPU", Name = "server" });
        Topics.Add(new() { CategoryKey = "CPU", Name = "new language" });
        Topics.Add(new() { CategoryKey = "CPU", Name = "hacking / security" });
        Topics.Add(new() { CategoryKey = "CPU", Name = "AI" });
        Topics.Add(new() { CategoryKey = "CPU", Name = "quantum computing" });
        #endregion

        #region H4H - Human for Human
        Cats.Add(new() { Key = "H4H", Name = "Human for Human" });
        Topics.Add(new() { CategoryKey = "H4H", Name = "diminishing inequality" });
        Topics.Add(new() { CategoryKey = "H4H", Name = "nutrition supply" });
        Topics.Add(new() { CategoryKey = "H4H", Name = "power supply" });
        Topics.Add(new() { CategoryKey = "H4H", Name = "contra unemployment" });
        Topics.Add(new() { CategoryKey = "H4H", Name = "contra homelessness" });
        #endregion

        #region EDU - Education
        Cats.Add(new() { Key = "EDU", Name = "Education" });
        Topics.Add(new() { CategoryKey = "EDU", Name = "teaching style" });
        Topics.Add(new() { CategoryKey = "EDU", Name = "educational system" });
        Topics.Add(new() { CategoryKey = "EDU", Name = "early development" });
        #endregion

        #region FOOD - Food & Drinks
        Cats.Add(new() { Key = "FOOD", Name = "Food & Drinks" });
        Topics.Add(new() { CategoryKey = "FOOD", Name = "coffee" });
        Topics.Add(new() { CategoryKey = "FOOD", Name = "tea" });
        Topics.Add(new() { CategoryKey = "FOOD", Name = "cocktails" });
        Topics.Add(new() { CategoryKey = "FOOD", Name = "secret recipes" });
        Topics.Add(new() { CategoryKey = "FOOD", Name = "new restaurants" });
        Topics.Add(new() { CategoryKey = "FOOD", Name = "substitutes" });
        #endregion

        #region HEAL - Health
        Cats.Add(new() { Key = "HEAL", Name = "Health" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "healing movements" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "nutrition" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "meditational" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "mental health" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "minimal workout" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "addictive workout" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "prophylactic lifestyle" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "contra addiction" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "grandma's medicine" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "exercises vs. pain" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "energize me" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "get happy!" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "posture" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "hygiene" });
        Topics.Add(new() { CategoryKey = "HEAL", Name = "allergies" });

        famTopicsString = new() { "eyes", "head", "shoulder", "back", "stomach", "hands", "heart", "knees", "foot", "sex", "hip", "neck", "legs", "ears", "skin", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "HEAL", Name = $"body region: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "HEAL", Name = "body region", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;

        #endregion

        #region NEER - Engineering
        Cats.Add(new() { Key = "NEER", Name = "Engineering" });
        Topics.Add(new() { CategoryKey = "NEER", Name = "space exploration" });
        #endregion

        #region GOV - Governmental
        Cats.Add(new() { Key = "GOV", Name = "Governmental" });
        Topics.Add(new() { CategoryKey = "GOV", Name = "new law" });
        Topics.Add(new() { CategoryKey = "GOV", Name = "adapting law" });
        Topics.Add(new() { CategoryKey = "GOV", Name = "reducing law" });

        famTopicsString = new() { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua and Barbuda", "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Brazil", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cabo Verde", "Cambodia", "Cameroon", "Canada", "Central African Republic", "Chad", "Chile", "China", "Colombia", "Comoros", "Congo", "Costa Rica", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Timor-Leste", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Eswatini", "Ethiopia", "Fiji", "Finland", "France", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Greece", "Grenada", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Honduras", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Israel", "Italy", "Ivory Coast", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Kosovo", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco", "Mozambique", "Myanmar (formerly Burma)", "Namibia", "Nauru", "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger", "Nigeria", "North Macedonia", "Norway", "Oman", "Pakistan", "Palau", "Palestine State", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Qatar", "Romania", "Russia", "Rwanda", "Saint Kitts and Nevis", "Saint Lucia", "Saint Vincent and the Grenadines", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Korea", "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname", "Sweden", "Switzerland", "Syria", "Tajikistan", "Tanzania", "Thailand", "Togo", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "UK", "USA", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City", "Venezuela", "Vietnam", "Yemen", "Zambia", "Zimbabwe", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "GOV", Name = $"for country: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "GOV", Name = "for country", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;


        famTopicsString = new() { "Hamburg", "Salzburg", "München", "Paris", "Wien", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "GOV", Name = $"for city: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "GOV", Name = "for city", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Topics.Add(new() { CategoryKey = "GOV", Name = "city design" });
        #endregion

        #region DSGN - Design
        Cats.Add(new() { Key = "DSGN", Name = "Design" });
        Topics.Add(new() { CategoryKey = "DSGN", Name = "furniture" });
        Topics.Add(new() { CategoryKey = "DSGN", Name = "architectural" });
        Topics.Add(new() { CategoryKey = "DSGN", Name = "stationery" });
        Topics.Add(new() { CategoryKey = "DSGN", Name = "electronic devices" });
        Topics.Add(new() { CategoryKey = "DSGN", Name = "haut couture" });
        Topics.Add(new() { CategoryKey = "DSGN", Name = "jewelry" });
        Topics.Add(new() { CategoryKey = "DSGN", Name = "autarkic solutions" });
        Topics.Add(new() { CategoryKey = "DSGN", Name = "shoes" });
        Topics.Add(new() { CategoryKey = "DSGN", Name = "luggage" });
        Topics.Add(new() { CategoryKey = "DSGN", Name = "watches" });
        #endregion

        #region OUT - Outdoors
        Cats.Add(new() { Key = "OUT", Name = "Outdoors" });
        Topics.Add(new() { CategoryKey = "OUT", Name = "surviving" });
        Topics.Add(new() { CategoryKey = "OUT", Name = "survival gear" });
        Topics.Add(new() { CategoryKey = "OUT", Name = "camping" });
        Topics.Add(new() { CategoryKey = "OUT", Name = "backpacking" });
        Topics.Add(new() { CategoryKey = "OUT", Name = "tents" });
        Topics.Add(new() { CategoryKey = "OUT", Name = "functional wear" });
        #endregion

        #region ART - Arts
        Cats.Add(new() { Key = "ART", Name = "Arts" });
        Topics.Add(new() { CategoryKey = "ART", Name = "painting" });
        Topics.Add(new() { CategoryKey = "ART", Name = "sculpting" });

        famTopicsString = new() { "Impressionism", "Cubism", "Surrealism", "Abstract Expressionism", "Pop Art", "Minimalism", "Fauvism", "Dada", "Constructivism", "Futurism", "Art Nouveau", "Suprematism", "Expressionism", "Post-Impressionism", "Symbolism", "Precisionism", "De Stijl", "Neo-Expressionism", "Abstract Art", "Op Art", "Conceptual Art", "Land Art", "Photorealism", "Video Art", "Contemporary Art", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "ART", Name = $"movement: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "ART", Name = "movement", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;

        #endregion

        #region MP3 - Music
        Cats.Add(new() { Key = "MP3", Name = "Music" });

        famTopicsString = new() { "electro", "party", "dancing", "heroic", "classic", "deep", "tense", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "MP3", Name = $"music mood: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "MP3", Name = "music mood", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }

        famTopicsString = new() { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua and Barbuda", "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Brazil", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cabo Verde", "Cambodia", "Cameroon", "Canada", "Central African Republic", "Chad", "Chile", "China", "Colombia", "Comoros", "Congo", "Costa Rica", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Timor-Leste", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Eswatini", "Ethiopia", "Fiji", "Finland", "France", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Greece", "Grenada", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Honduras", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Israel", "Italy", "Ivory Coast", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Kosovo", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco", "Mozambique", "Myanmar (formerly Burma)", "Namibia", "Nauru", "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger", "Nigeria", "North Macedonia", "Norway", "Oman", "Pakistan", "Palau", "Palestine State", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Qatar", "Romania", "Russia", "Rwanda", "Saint Kitts and Nevis", "Saint Lucia", "Saint Vincent and the Grenadines", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Korea", "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname", "Sweden", "Switzerland", "Syria", "Tajikistan", "Tanzania", "Thailand", "Togo", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "UK", "USA", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City", "Venezuela", "Vietnam", "Yemen", "Zambia", "Zimbabwe", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "MP3", Name = $"local music: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "MP3", Name = "local music", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;

        #endregion

        #region LIT - Literature
        Cats.Add(new() { Key = "LIT", Name = "Literature" });

        famTopicsString = new() { "English", "Mandarin Chinese", "Spanish", "Hindi", "Arabic", "Bengali", "Portuguese", "Russian", "Japanese", "Punjabi", "German", "Javanese", "Korean", "French", "Telugu", "Marathi", "Tamil", "Urdu", "Turkish", "Italian", "Vietnamese", "Gujarati", "Polish", "Ukrainian", "Malayalam", "Kannada", "Oriya", "Burmese", "Kurdish", "Sindhi", "Azerbaijani", "Uzbek", "Dutch", "Yoruba", "Amharic", "Oromo", "Thai", "Malay", "Romanian", "Hungarian", "Czech", "Greek", "Swedish", "Bulgarian", "Danish", "Finnish", "Norwegian", "Icelandic", "Hebrew", "Yiddish", "Georgian", "Armenian", "Tigrinya", "Pashto", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "LIT", Name = $"poetry: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "LIT", Name = "poetry", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = -1;


        famTopicsString = new() { "English", "Mandarin Chinese", "Spanish", "Hindi", "Arabic", "Bengali", "Portuguese", "Russian", "Japanese", "Punjabi", "German", "Javanese", "Korean", "French", "Telugu", "Marathi", "Tamil", "Urdu", "Turkish", "Italian", "Vietnamese", "Gujarati", "Polish", "Ukrainian", "Malayalam", "Kannada", "Oriya", "Burmese", "Kurdish", "Sindhi", "Azerbaijani", "Uzbek", "Dutch", "Yoruba", "Amharic", "Oromo", "Thai", "Malay", "Romanian", "Hungarian", "Czech", "Greek", "Swedish", "Bulgarian", "Danish", "Finnish", "Norwegian", "Icelandic", "Hebrew", "Yiddish", "Georgian", "Armenian", "Tigrinya", "Pashto", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "LIT", Name = $"short: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "LIT", Name = "short", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = -1;

        Topics.Add(new() { CategoryKey = "LIT", Name = "novel topics" });
        Topics.Add(new() { CategoryKey = "LIT", Name = "sciFi topics" });
        Topics.Add(new() { CategoryKey = "LIT", Name = "sayings" });
        Topics.Add(new() { CategoryKey = "LIT", Name = "one-liners" });
        Topics.Add(new() { CategoryKey = "LIT", Name = "showerthoughts" });

        famTopicsString = new() { "English", "Mandarin Chinese", "Spanish", "Hindi", "Arabic", "Bengali", "Portuguese", "Russian", "Japanese", "Punjabi", "German", "Javanese", "Korean", "French", "Telugu", "Marathi", "Tamil", "Urdu", "Turkish", "Italian", "Vietnamese", "Gujarati", "Polish", "Ukrainian", "Malayalam", "Kannada", "Oriya", "Burmese", "Kurdish", "Sindhi", "Azerbaijani", "Uzbek", "Dutch", "Yoruba", "Amharic", "Oromo", "Thai", "Malay", "Romanian", "Hungarian", "Czech", "Greek", "Swedish", "Bulgarian", "Danish", "Finnish", "Norwegian", "Icelandic", "Hebrew", "Yiddish", "Georgian", "Armenian", "Tigrinya", "Pashto", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "LIT", Name = $"for language: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "LIT", Name = "for language", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = -1;

        #endregion

        #region SOC - Society
        Cats.Add(new() { Key = "SOC", Name = "Society" });
        Topics.Add(new() { CategoryKey = "SOC", Name = "grand movement" });
        Topics.Add(new() { CategoryKey = "SOC", Name = "global crisis" });
        Topics.Add(new() { CategoryKey = "SOC", Name = "communities" });
        Topics.Add(new() { CategoryKey = "SOC", Name = "new political systems" });
        Topics.Add(new() { CategoryKey = "SOC", Name = "consumerism" });
        Topics.Add(new() { CategoryKey = "SOC", Name = "privacy" });
        Topics.Add(new() { CategoryKey = "SOC", Name = "genders" });
        Topics.Add(new() { CategoryKey = "SOC", Name = "distribute expertise" });
        #endregion

        #region GAD - Gadgets
        Cats.Add(new() { Key = "GAD", Name = "Gadgets" });
        Topics.Add(new() { CategoryKey = "GAD", Name = "w/o electro" });
        Topics.Add(new() { CategoryKey = "GAD", Name = "diy" });
        Topics.Add(new() { CategoryKey = "GAD", Name = "3d prints" });
        #endregion

        #region SPO - Sport
        Cats.Add(new() { Key = "SPO", Name = "Sport" });

        famTopicsString = new() { "Soccer", "American Football", "Basketball", "Cricket", "Tennis", "Golf", "Rugby", "Baseball", "Ice Hockey", "Volleyball", "Table Tennis", "Badminton", "Swimming", "Athletics", "Formula 1", "Boxing", "Wrestling", "Gymnastics", "Skiing", "Snowboarding", "Cycling", "Horse Racing", "Sailing", "Surfing", "Skateboarding", "MMA", "Archery", "Shooting", "Weightlifting", "Canoeing", "Rowing", "Triathlon", "Equestrian", "Track and Field", "Fencing", "Taekwondo", "Judo", "Karate", "Climbing Sports", "Darts", "Snooker", "Bowling", "Squash", "Hiking", "Ultimate Frisbee", "CrossFit", "Pole Vaulting", "Bobsleigh", "Water Polo", "Handball", "Lacrosse", "Softball", "Roller Skating", "Kiteboarding", "Beach Volleyball", "Inline Hockey", "Wakeboarding", "Canyoning", "Parkour", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "SPO", Name = $"sports gear: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "SPO", Name = "sports gear", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;


        famTopicsString = new() { "Soccer", "American Football", "Basketball", "Cricket", "Tennis", "Golf", "Rugby", "Baseball", "Ice Hockey", "Volleyball", "Table Tennis", "Badminton", "Swimming", "Athletics", "Formula 1", "Boxing", "Wrestling", "Gymnastics", "Skiing", "Snowboarding", "Cycling", "Horse Racing", "Sailing", "Surfing", "Skateboarding", "MMA", "Archery", "Shooting", "Weightlifting", "Canoeing", "Rowing", "Triathlon", "Equestrian", "Track and Field", "Fencing", "Taekwondo", "Judo", "Karate", "Climbing Sports", "Darts", "Snooker", "Bowling", "Squash", "Hiking", "Ultimate Frisbee", "CrossFit", "Pole Vaulting", "Bobsleigh", "Water Polo", "Handball", "Lacrosse", "Softball", "Roller Skating", "Kiteboarding", "Beach Volleyball", "Inline Hockey", "Wakeboarding", "Canyoning", "Parkour", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "SPO", Name = $"new moves: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "SPO", Name = "new moves", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;

        Topics.Add(new() { CategoryKey = "SPO", Name = "olympiad" });

        famTopicsString = new() { "Soccer", "American Football", "Basketball", "Cricket", "Tennis", "Golf", "Rugby", "Baseball", "Ice Hockey", "Volleyball", "Table Tennis", "Badminton", "Swimming", "Athletics", "Formula 1", "Boxing", "Wrestling", "Gymnastics", "Skiing", "Snowboarding", "Cycling", "Horse Racing", "Sailing", "Surfing", "Skateboarding", "MMA", "Archery", "Shooting", "Weightlifting", "Canoeing", "Rowing", "Triathlon", "Equestrian", "Track and Field", "Fencing", "Taekwondo", "Judo", "Karate", "Climbing Sports", "Darts", "Snooker", "Bowling", "Squash", "Hiking", "Ultimate Frisbee", "CrossFit", "Pole Vaulting", "Bobsleigh", "Water Polo", "Handball", "Lacrosse", "Softball", "Roller Skating", "Kiteboarding", "Beach Volleyball", "Inline Hockey", "Wakeboarding", "Canyoning", "Parkour", };
        famTopics = famTopicsString.Select(name => new Topic()
        { CategoryKey = "SPO", Name = $"train for: {name.Trim()}" }).ToList();
        Fams.Add(new() { CategoryKey = "SPO", Name = "train for", Topics = famTopics });
        foreach (var topic in famTopics) { Topics.Add(topic); }
        Fams[^1].ProbablePreference = 0;

        #endregion
    }
    public void PopulateUsers()
    {
        Users.Add(new User { Username = "tobi" });
        Users.Add(new User { Username = "Tobi" });
        Users.Add(new User { Username = "theDad" });
        Users.Add(new User { Username = "thePro" });
        Users.Add(new User { Username = "theNiceOne" });
        Users.Add(new User { Username = "theCriticalOne" });
        Users.Add(new User { Username = "theInspiredOne" });
        Users.Add(new User { Username = "prefa" });
    }
    public void PopulateIdeas()
    {
        string tWorld = "w/orld method";
        string tEasyW = "EasyWipe";
        Ideas.Add(new()
        {
            Title = tWorld,
            Slogan = "Apply Capitalism Against Global Warming",
            CreatorName = "tobi",
            Topics = new()
                {
                    Topics.First(t => t.CategoryKey=="ENV" && t.Name=="CO2 reduction"),
                    Topics.First(t => t.CategoryKey=="SOC" && t.Name=="grand movement"),
                }
        });
        Ideas.Add(new()
        {
            Title = tEasyW,
            Slogan = "No More Wiping Pain (for men)",
            CreatorName = "tobi",
            Topics = new()
                {
                    Topics.First(t => t.CategoryKey=="DSGN" && t.Name=="autarkic solutions"),
                }
        });
        Ideas.Add(new()
        {
            Title = "Integrated Fridge",
            Slogan = "Use the Fridge's Heat For Warm Water",
            CreatorName = "theDad",
            Topics = new List<Topic>
                {
                    Topics.First(t => t.CategoryKey=="POW" && t.Name=="reduction"),
                }
        });
        Ideas.Add(new()
        {
            Title = "LookDown Mirror",
            Slogan = "IR Sensor in Car Side Mirror Checks Ice",
            CreatorName = "theDad",
            Topics = new()
                {
                    Topics.First(t => t.CategoryKey == "TRA" && t.Name == "cars"),
                }
        });
        Ideas.Add(new()
        {
            Title = "Contra Soleil",
            Slogan = "Simple Styrofoam w/ Sucker on Window to Block Sun",
            CreatorName = "theDad",
            Topics = new()
                {
                    Topics.First(t => t.CategoryKey=="POW" && t.Name=="reduction"),
                    Topics.First(t => t.CategoryKey=="HOME" && t.Name=="living solutions"),
                }
        });
        Ideas.Add(new()
        {
            Title = "IntegratedPV",
            Slogan = "Roof Tile that is Also a Solar Collector",
            CreatorName = "prefa",
            Topics = new()
                {
                    Topics.First(t => t.CategoryKey == "POW" && t.Name == "alternatives"),
                    Topics.First(t => t.CategoryKey == "DSGN" && t.Name == "electronic devices"),
                }
        });
        //for (int i = 1; i <= 50; i++)
        //{
        //    Ideas.Add(new()
        //    {
        //        Title = "DummyIdea" + i,
        //        Slogan = "Some slogan.",
        //        CreatorName = "tobi",
        //    });
        //}

    }
}
