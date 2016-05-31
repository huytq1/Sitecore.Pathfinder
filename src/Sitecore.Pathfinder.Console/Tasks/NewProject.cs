// � 2015-2016 Sitecore Corporation A/S. All rights reserved.

using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.Extensions;
using Sitecore.Pathfinder.IO;
using Sitecore.Pathfinder.Tasks.Building;

namespace Sitecore.Pathfinder.Tasks
{
    public class NewProject : BuildTaskBase
    {
        // attribution: https://en.wikipedia.org/wiki/List_of_computer_technology_code_names
        [NotNull,ItemNotNull]
        private readonly string[] _codeNames = {                               
            "Apollo",
            "Astro",
            "Baikal",
            "Bali",
            "Bamboo",
            "Banias",
            "Banister",
            "Bantam",
            "Barak",
            "Barcelona",
            "Barney",
            "Barracuda",
            "Bart",
            "Barton",
            "Batman",
            "Batphone",
            "Beagle",
            "Becks",
            "Beetle",
            "Beef Miracle",
            "Bender",
            "Big Electric Cat",
            "Big Foot",
            "Big Sur",
            "Bigfish",
            "Bigmac",
            "BigTop",
            "Biltmore",
            "Bimini",
            "Birch",
            "Bismillah",
            "Blackbeard",
            "Blackbird",
            "Blackcomb",
            "Blackford",
            "Blackjack",
            "Bladerunner",
            "Blaze",
            "Blue",
            "Blue Box",
            "Bluebird",
            "Bluedog",
            "Blueringer",
            "Blugu",
            "Bo",
            "Bon Echo",
            "Bongo",
            "Bonjour",
            "Bordeaux",
            "Borealis",
            "Borrow",
            "Braun",
            "BraveHawk",
            "Brazil",
            "BreadboxA",
            "Breezy Badger",
            "Brick",
            "Bride of Buster",
            "Brimstone",
            "Broadwater",
            "Broadway",
            "Brokenboring",
            "Brookdale",
            "Brooklyn",
            "Brooks",
            "Brugge",
            "Bulldozer",
            "Bullfrog",
            "Bullwinkle",
            "Bulverde",
            "Burnaby",
            "Buzz",
            "Cairo",
            "Calais",
            "Calexico",
            "Calistoga",
            "Calvin",
            "Camaro",
            "Cambridge",
            "Camelot",
            "Camino",
            "Campfire",
            "Campus",
            "Canary",
            "Candy",
            "Canterwood",
            "Capone",
            "Captain Rayno",
            "Carl Sagan",
            "Carmel",
            "Carrera",
            "Cartman",
            "Casanova",
            "Cascade",
            "Cheetah",
            "Cheeze Whiz",
            "Chels",
            "Cherrystone",
            "Chicago",
            "Chilliwack ",
            "Chimera",
            "Chinook",
            "Chivano",
            "Chrichton",
            "Chrysalis Client",
            "Cinnamon",
            "Classic",
            "ClawHammer",
            "ClickOnce",
            "Cloud",
            "Clovertown",
            "Cobalt",
            "Cobra",
            "Cocoa",
            "Cold Fusion",
            "Colgate",
            "Colleen",
            "Colorado",
            "Colossus",
            "Colt",
            "Columbia",
            "Columbus",
            "Colusa",
            "Combo",
            "Comet",
            "Companion",
            "Condor",
            "Conroe",
            "Constantine",
            "Converse",
            "Cooker",
            "Copenhagen",
            "Copland",
            "Coppermine",
            "Copperriver",
            "Cornbread",
            "Corona",
            "Cortet",
            "Cortland",
            "Corvette",
            "Covington",
            "Coyote",
            "Crane",
            "Crescendo",
            "Crestine",
            "Crestline",
            "Crusader",
            "Crusoe",
            "Crypto",
            "Crystal",
            "Cuba",
            "Cupcake",
            "Curley",
            "Cyclone",
            "Dagwood",
            "Daktari",
            "Dapper Drake",
            "Dark Matter",
            "Dart",
            "Dartanian",
            "Darth Vader",
            "Darwin",
            "Daybreak",
            "Dayton",
            "Daytona",
            "DBLite",
            "Decaf",
            "Deepcove",
            "Deepmind",
            "Deerfield",
            "dejaVu",
            "Delorean",
            "Derringer",
            "Deschutes",
            "Detroit",
            "Deuterium",
            "Devel",
            "Diamond",
            "Diana",
            "Diesel",
            "Dilbert",
            "Dinnerbox",
            "Dirty Bird",
            "Discovery",
            "Discus",
            "Dixon",
            "Dockyard",
            "Dollhouse",
            "Dolphin",
            "Donut",
            "Dothan",
            "Dove",
            "Dover",
            "Dobra Voda",
            "Dragon",
            "DragonHawk",
            "Dublin",
            "Duo",
            "Duracell",
            "Duraflame",
            "Eagle",
            "Eagle Ridge",
            "East Fork",
            "Echo Lake",
            "Eclair",
            "Eclipse",
            "Edgy Eft",
            "Edison",
            "Egret",
            "Eierspass",
            "Eiger",
            "Eight Ball",
            "Einstein",
            "ELB",
            "Electron",
            "Elf",
            "Elite I",
            "Elixir",
            "Elmer",
            "Elsie",
            "Emerald",
            "Emerald Bay",
            "Emily",
            "Emma",
            "Enchilada",
            "Encompass",
            "Endever",
            "Energizer",
            "Enigma",
            "Envici",
            "Epic",
            "Equinox",
            "Erickson",
            "Escher",
            "Espresso",
            "Esprit",
            "Esther",
            "Eszter",
            "ET",
            "Etch",
            "Europa",
            "Eveready",
            "Everest",
            "Evo",
            "Excalibur",
            "Ezra",
            "Fafnir",
            "Fairbanks",
            "Falcon",
            "Fanwood",
            "Fast Eddy",
            "Fat Mac",
            "Fat Timba",
            "Feint",
            "Feisty Dunnart",
            "Feisty Fawn",
            "Fernie",
            "Ferrari",
            "[Festen",
            "Fester",
            "Fiji",
            "Finestra",
            "Fire",
            "Fireball",
            "Firefly",
            "Firestorm",
            "Firetruck",
            "Fisher",
            "Five Star",
            "Flagship",
            "Flamingo",
            "Flapjack",
            "Flare",
            "Flipflop",
            "Flyweight",
            "Foster",
            "Foster Farms",
            "Four Square",
            "Fred",
            "Freeport",
            "Freestyle",
            "Freeze",
            "Freon",
            "Freshchoice",
            "Fridge",
            "Frogger",
            "FroYo",
            "Fuji",
            "Full Monty",
            "Fullmoon",
            "Fusion",
            "Future",
            "Gaga",
            "Galactica",
            "Galaxy",
            "Galiano",
            "Galibaldi",
            "Galileo",
            "Gallatin",
            "Garfield",
            "Gaston",
            "Gemini",
            "Genesis",
            "Genesis",
            "Genie",
            "Georgia]",
            "Gershwin",
            "Geyserville",
            "Gideon",
            "Gingerbread",
            "Glenwood",
            "Gobi",
            "Gobi",
            "Goddard",
            "Godzilla",
            "Golden Gate",
            "Goldfish",
            "Golem",
            "Gossamer",
            "Granite Bay",
            "Grantsdale",
            "Green Jade",
            "Greencreek",
            "Greenwich",
            "Grendelsbane",
            "Grimoire",
            "Grizzly",
            "Grover",
            "Gryphon",
            "Guava",
            "Guinness",
            "Gumbi",
            "Gumby",
            "Gutsy Gibbon",
            "Gypsy",
            "Haarlem",
            "Hades",
            "Hadjaha",
            "Hailstorm",
            "Hakone",
            "Halfdome",
            "Halibut",
            "Halloween",
            "Halo",
            "Hamlet",
            "Hamm",
            "Hammer",
            "Hammerhead",
            "Happy Meal",
            "Hardy Heron",
            "Harpertown",
            "Hastings",
            "Hawaii",
            "Hawk",
            "Heckel",
            "Hedwig",
            "Heidelberg",
            "Hekk",
            "Helios",
            "Helium",
            "Hercules",
            "Hermes",
            "Hoary Hedgehog",
            "Hobbes",
            "Hobo",
            "Hokusai",
            "Homer",
            "Honeycomb",
            "Hook",
            "Hooper",
            "Horizon",
            "Hornet",
            "HotJava",
            "Houdini",
            "Hulk Hogan",
            "Hummingbird",
            "Hurricane",
            "Hustenstopper",
            "Hydra",
            "Hyperbolic",
            "Ibis",
            "Ice Cream",
            "Ice Cream Sandwich",
            "Iceberg",
            "Igen",
            "Ikki",
            "Indigo",
            "Indium",
            "Infinite Improbability Drive",
            "Instatower",
            "Interface Manager",
            "Intrepid Ibex",
            "Irongate",
            "Ironsides",
            "Irwindale",
            "Italy",
            "Itanium",
            "Ivory",
            "Ivy",
            "Jackson",
            "Jackson Pollock",
            "Jade",
            "Jaguar",
            "Jaguar",
            "Jalapeno",
            "Janus",
            "Jasmine",
            "Jason",
            "Jasper",
            "Jaunty Jackalope",
            "Javelin",
            "Jayhawk",
            "Jeckle",
            "Jedi",
            "Jedy",
            "Jelly Bean",
            "Jeeves",
            "Jet",
            "Jiro",
            "John",
            "Jonah",
            "Joshua",
            "Juhhu",
            "Jumanji",
            "Juneau",
            "Junior",
            "Jupiter",
            "Kaede",
            "Kahlua",
            "Kamion",
            "Kanga",
            "Kangaroo",
            "Kansas",
            "Karatu",
            "Karelia",
            "Karmic Koala",
            "Katana",
            "Katmai",
            "Kauai",
            "Kelowna",
            "Ketchup",
            "Keystone",
            "Khepri",
            "Kitkat",
            "Kkachi",
            "Klamath",
            "Klingon",
            "Kodiak",
            "Kollege",
            "Kona",
            "Kong",
            "Kootenay",
            "Kopernicus",
            "Krakatoa",
            "Krans",
            "Krum",
            "Krups",
            "Kryptonite",
            "Kyoto",
            "Kyrene",
            "Lady Kenmore",
            "Ladner",
            "LaGrande",
            "Laguna",
            "Lakeport",
            "Landshark",
            "Laughlin",
            "Leadville",
            "Leary",
            "Legend",
            "Lego",
            "Leeloo",
            "Lenny",
            "Leo",
            "Leonidas",
            "Leopard",
            "Liberation",
            "Liberty",
            "Lightweight",
            "Lilac",
            "Limbo",
            "Lindenhurst",
            "Liquid Sky",
            "Lisa",
            "Little Big Mac",
            "Littleneck",
            "Logo",
            "Lokar",
            "Lollie",
            "Lonestar",
            "Longhorn",
            "Lorax",
            "Lorraine",
            "Lovelock",
            "Love Shack",
            "Lucid Lynx",
            "Lumumba",
            "Luna",
            "Lunchbox",
            "Lutra sumatrana",
            "Luzon",
            "Lyra",
            "Macallan",
            "Maccabbee",
            "Mach5",
            "Mad Hatter",
            "Madison",
            "Mafalda",
            "Magneto",
            "Magnolia",
            "Magnum",
            "Mai Tai",
            "Main Street",
            "Maipo",
            "Makalu",
            "Mako",
            "Makrolab",
            "Malibu",
            "Mamba",
            "Mammoth",
            "Manchester",
            "Mango",
            "Mango",
            "Manhattan",
            "Manifest",
            "Manila",
            "Manitoba",
            "Mantaray",
            "Marathon",
            "Marblehead",
            "Mark Twain",
            "Marcato",
            "Mars",
            "Marshmallow",
            "Mascarpone",
            "Master",
            "Matterhorn",
            "Matthew",
            "Maui",
            "Maui",
            "Maverick Meerkat",
            "Maxcat",
            "McKinley",
            "Meerkat",
            "Memphis",
            "Menagine",
            "Mending",
            "Mendocino",
            "Merced",
            "Mercury",
            "Merl",
            "Merlin",
            "Merom",
            "Metro",
            "Midas",
            "Midnight Run",
            "Midway",
            "Mighty Cat",
            "Mikey",
            "Millennium",
            "Millington",
            "Milwaukee",
            "Minnow",
            "Mira",
            "Mishteh",
            "Mistral",
            "Mobile Triton",
            "Mohawk",
            "Mojave",
            "Monad",
            "Monet",
            "Montana",
            "Monte Carlo",
            "Montecito",
            "Montera",
            "Monterey",
            "Montvale",
            "Monza",
            "Mooch",
            "Moonbase",
            "Moonshine",
            "Moosehead",
            "Morgan",
            "Moriarty",
            "Moses",
            "Mother's Day",
            "Mount Prospect",
            "Mousex",
            "Moxie",
            "Mozilla",
            "Mr. Coffee",
            "Mr. T",
            "Mriya",
            "Mucho Grande",
            "Mulligan",
            "Mustang",
            "Nachos",
            "Nahant",
            "Nano",
            "Napa",
            "Nashville",
            "Natoma",
            "Natty Narwhal",
            "Nautilus",
            "Navigator",
            "Nehalem",
            "Nehemiah",
            "Nell",
            "Neptune",
            "Neutron",
            "Nevada",
            "Newcastle",
            "Newton",
            "Niagara",
            "Nighthawk",
            "Ninevah",
            "Nitro",
            "Nocona",
            "Nodewarrior",
            "NoDo",
            "Noodle",
            "Nordica",
            "North",
            "Northbridge",
            "Northwood",
            "Nova",
            "Nova96",
            "Oak",
            "Oberon",
            "Obi Wan",
            "Oceanic",
            "Odem",
            "Odin",
            "Odyssey",
            "Offcampus",
            "Okinawa",
            "Omega",
            "Oneiric Ocelot",
            "Optimus",
            "Orbit",
            "Orca",
            "Oreo",
            "Orion",
            "Orleans",
            "Oslo",
            "Osmium",
            "Osprey",
            "Othello",
            "Otter",
            "Owens",
            "Pacific",
            "Pacifica",
            "Palermo",
            "Palladium",
            "Palomi",
            "Panama",
            "Panda",
            "Panther",
            "Paramount",
            "Paran",
            "Parhelia",
            "Paris",
            "Paul",
            "Peanuts",
            "Pegasus",
            "Pendolino",
            "Penguin",
            "Penryn",
            "Pensacola",
            "Perestroika",
            "Perigree",
            "Persistence",
            "Peter Pan",
            "Phantasmal",
            "Pharaoh",
            "Phiphi",
            "Phenom",
            "Phoebe",
            "Phoenix",
            "Photon",
            "Pico",
            "Piglet",
            "Pike Peak",
            "Piltdown Man",
            "Pinball",
            "Pineapple",
            "Ping Pong",
            "Pinnacle",
            "Pinstripe",
            "Pippin",
            "Pipeline",
            "Pismo",
            "Pizza",
            "Placer",
            "Plano",
            "Platinum",
            "Platte",
            "Pliska",
            "Plumas",
            "Plus",
            "PlusPlus",
            "Pluto",
            "Polaris",
            "Pommes",
            "Pomona",
            "Pooh",
            "Poppy",
            "Portland",
            "Portola",
            "Potato",
            "Potomac",
            "Powderhorn",
            "Power Express",
            "Power Surge",
            "Powerware",
            "Prelude",
            "Premise",
            "Prescott",
            "Presler",
            "Prestonia",
            "Primus",
            "Prism",
            "Profusion",
            "Project Atlantis",
            "Project Caf�",
            "Project Chess",
            "Project Ganges",
            "Project Needlemouse",
            "Project Pipeline",
            "Project Reality",
            "Propeller",
            "Psyche",
            "Puffin",
            "Pulsar",
            "Puma",
            "Puppy",
            "Purple",
            "Python",
            "Quadra",
            "Quadro",
            "Quahog",
            "Quark",
            "Quasar",
            "Quattro",
            "Quesnel",
            "Quicksilver",
            "Racer",
            "Radiance",
            "Raffica",
            "Raffika",
            "Rage",
            "Raisin",
            "Rajt",
            "Rambo",
            "Rampage",
            "Raphsody",
            "Rapier",
            "Raptor",
            "Raven",
            "Rawhide",
            "RayBan",
            "Razor",
            "Rebound",
            "Redstone",
            "Red Pill",
            "Regatta",
            "Rembrandt",
            "Renault",
            "Reno",
            "Replacements",
            "Revolution",
            "Rex",
            "Rhapsody",
            "Rialto",
            "Richmond",
            "Rincewind",
            "Ringo",
            "Rio",
            "Rio de Janeiro",
            "Riviera",
            "Road Warrior",
            "Roadracer",
            "Roadrunner",
            "Roam",
            "Rochester",
            "Rocky",
            "Rockchip",
            "Rome",
            "Romeo",
            "Rosebud",
            "Rost",
            "Roswell",
            "Round One Inc.",
            "Royale",
            "Rubicon",
            "Rudi",
            "Sabin",
            "Sabre",
            "Safari",
            "Sagres",
            "Sahara",
            "Salem",
            "Samila",
            "Samuel",
            "Sandy Bridge",
            "San Diego",
            "Santa Fe",
            "Santa Rosa",
            "Sapphire",
            "Satura",
            "Sawtooth",
            "Scimitar",
            "Scorpion",
            "Scud",
            "Sea Lion",
            "Seam",
            "Seattle",
            "Seawolf",
            "Serengeti",
            "Sevar",
            "Severn",
            "Shadow",
            "ShakaZulu",
            "Shame",
            "Shark",
            "Sharptooth",
            "Shasta",
            "Sheffield",
            "Sheffield Tesla",
            "Shelton",
            "Sherlock",
            "Sherman",
            "Sherry",
            "Shillelagh",
            "Shiloh",
            "Shiner",
            "Shitake",
            "Shogun",
            "Show Biz",
            "Shrike",
            "Siberia",
            "Sibyl",
            "Sidewinder",
            "Silence",
            "Silverdale",
            "Silverstone",
            "Sirius",
            "Sisyphus",
            "Skipjack",
            "Skyhawk",
            "SledgeHammer",
            "Slice",
            "Slipstream",
            "Smithfield",
            "Snapshot",
            "Snark",
            "Snowball",
            "Snow Leopard",
            "Sobeck",
            "Solano",
            "Solarnet",
            "Solstizio",
            "Sonata",
            "Sonoma",
            "Space Cadet",
            "Space Monkey",
            "Space Mountain",
            "Spam",
            "Sparkle",
            "Sparkler",
            "Speedbump",
            "Speedracer",
            "Speedway",
            "Spike",
            "Spitfire",
            "Spitfire",
            "Splash Mountain",
            "Spock",
            "Sponge",
            "Sport",
            "Springdale",
            "Spruce Goose",
            "Spud",
            "Sputnik",
            "Sputnik Bluesky",
            "Sputnik Orion",
            "Spuzzum",
            "Squeaky",
            "Staccato",
            "Star Trek",
            "Starbuck",
            "Starbucks",
            "Starburst",
            "Starcat",
            "Starfire",
            "Starter",
            "Stealth",
            "Steam",
            "Stenz",
            "Stinger",
            "Stingrack",
            "Stingray",
            "Stingrock",
            "Stoned Beaver",
            "Stonehenge",
            "Storm",
            "Strange Cargo",
            "Stratos",
            "Sumatra",
            "Summit",
            "Sunbox",
            "Sunchild",
            "SunDials",
            "SunDragon",
            "Sunergy",
            "SunFiler",
            "Sunfire",
            "Sunflower",
            "Sunlight",
            "SunLink",
            "Sunrack",
            "SunRay",
            "Sunrise",
            "SunScreen",
            "SunSwift",
            "SuperFetch",
            "SuperGun",
            "Superior",
            "Supernami",
            "Susan",
            "Suzuka",
            "Swift",
            "Swing",
            "Sysyphus",
            "Strations",
            "T-Bird",
            "T-Rex",
            "Tabasco",
            "Tadpole",
            "Talisker",
            "Talon",
            "Tanglewood",
            "Tanner",
            "Tantalus",
            "Tanzania",
            "Taroon",
            "Tarzan",
            "Tattle",
            "Taylor3",
            "Tazmax",
            "Tazmo",
            "Teddy",
            "Tehama",
            "Tejas",
            "Tempest",
            "Tempo",
            "Teragrid",
            "Terminator",
            "Tervel",
            "Tettnang",
            "Thor",
            "Thoroughbred",
            "Thunder Mountain",
            "Thunderbird",
            "Tiger",
            "Tiger Eye",
            "Tiger Mountain",
            "Tillamook",
            "Tim",
            "Timba",
            "Today",
            "Toddy",
            "Tofino",
            "Togo",
            "Togo Tall",
            "Tonga",
            "Topaz",
            "Topcat",
            "Topdog",
            "Toro",
            "Tornado",
            "Toucan",
            "Trail",
            "Trailblazer",
            "Traktopel",
            "Transformer",
            "Threshold",
            "TRex",
            "Tribble",
            "Trike",
            "Trinity",
            "Triton",
            "Troy",
            "Tsunami",
            "Tualatin",
            "Tukwila",
            "Tulip",
            "Tulloch",
            "Tulsa",
            "Tumwater",
            "TurboGX",
            "Turion",
            "Twiggy",
            "Twin Castle",
            "TwinPeaks",
            "Twister",
            "Ucluelet",
            "Ulysses",
            "UltraLight",
            "UltraPenguin",
            "Underdog",
            "Underground",
            "Union Bay",
            "Unisun",
            "Ural",
            "Uzi",
            "Vail",
            "Valandraud",
            "Valhalla",
            "Valkyrie",
            "Vancouver",
            "Vanderbilt",
            "Vanderpool",
            "Vega",
            "Venice",
            "Vegas",
            "Venus",
            "Vernon",
            "Verne",
            "Vertex",
            "Viking",
            "VineSeed",
            "Viper",
            "Viros",
            "Virtue",
            "Visa",
            "Vishera",
            "Vitamin",
            "Vostok",
            "Voyager",
            "Vulcan",
            "Vienna",
            "Waghor",
            "Walkaround",
            "Wall Street",
            "Wallop",
            "Wally",
            "Walrus",
            "Warm Springs",
            "Warty Warthog",
            "Water",
            "Watney",
            "Wave",
            "Weed-Whacker",
            "Werewolf",
            "Whidbey",
            "Whistler",
            "Whitefield",
            "White Rabbit",
            "Whitney",
            "Whopper",
            "Wide Stinger",
            "Widget",
            "Wilbur",
            "Willamette",
            "Wildcat",
            "Wildfire",
            "Wildlife",
            "Wiley",
            "Winchester",
            "Wind",
            "Windermere",
            "Windsor",
            "Wolfack",
            "Wolfdale",
            "Wolverine",
            "Wombat",
            "Wonderboy",
            "Woodcrest",
            "Woody",
            "Wren",
            "Wyoming",
            "Wyvern",
            "Xbox",
            "Xena",
            "Xenon",
            "Yaeger",
            "Yamhill",
            "Yami",
            "Yarrow",
            "Yarrow",
            "Yellow Box",
            "Yoda",
            "Yonah",
            "Yosemite",
            "Yosemite",
            "Yorkfield",
            "Yosemite",
            "Yukon",
            "Zambezi",
            "Zappa",
            "Zebra",
            "Zelda",
            "Zephyr",
            "Zeus",
            "Zippy",
            "Zircon",
            "Zeotrope",
            "Zod",
            "Zoltrix",
            "Zone",
            "Zoot",
            "Zulu",
            "Zydeco"
        };

        [NotNull]
        private string _dataFolderDirectory = string.Empty;

        [NotNull]
        private string _editorFileName = string.Empty;

        [NotNull]
        private string _hostName = string.Empty;

        [NotNull]
        private string _projectUniqueId = string.Empty;

        [NotNull]
        private string _starterKitFileName = string.Empty;

        [NotNull]
        private string _taskRunnerFileName = string.Empty;

        [NotNull]
        private string _websiteDirectory = string.Empty;

        [ImportingConstructor]
        public NewProject([NotNull] IConsoleService console, [NotNull] IFileSystemService fileSystem) : base("new-project")
        {
            Console = console;
            FileSystem = fileSystem;
        }

        [NotNull]
        protected IConsoleService Console { get; }

        [NotNull]
        protected IFileSystemService FileSystem { get; }

        public override void Run(IBuildContext context)
        {
            var projectDirectory = context.ProjectDirectory;
            if (!FileSystem.DirectoryExists(projectDirectory))
            {
                FileSystem.CreateDirectory(projectDirectory);
            }

            Console.WriteLine("Welcome to Sitecore Pathfinder.");

            if (Directory.GetFiles(projectDirectory).Length > 0 || Directory.GetDirectories(projectDirectory).Length > 0)
            {
                Console.WriteLine();
                Console.WriteLine("The current directory is not empty. It is recommended to create a new project in an empty directory.");
                Console.WriteLine();
                if (Console.YesNo("Are you sure you want to create the project in this directory [N]: ", false, "overwrite") != true)
                {
                    return;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Pathfinder needs 4 pieces of information to create a new project; a unique Id for the project, the Sitecore website and data folder directories to deploy to, and the hostname of the website. If you have not yet created a Sitecore website, use a tool like Sitecore Instance Manager to create it for you.");
            Console.WriteLine();
            Console.WriteLine("The project�s unique ID can be a unique string (like MyCompany.MyProject) or a Guid. If you do not specify a unique ID, Pathfinder will generate a Guid for you.");
            Console.WriteLine();
            Console.WriteLine("You should *not* change the project unique ID at a later point, since Sitecore item IDs are dependent on it.");
            Console.WriteLine();

            var rnd = new Random();
            _projectUniqueId = _codeNames[rnd.Next(_codeNames.Length)] + " " + Guid.NewGuid().ToString("P");
            _projectUniqueId = Console.ReadLine("Enter the project unique ID [" + _projectUniqueId + "]: ", _projectUniqueId, "projectid");

            Console.WriteLine();
            Console.WriteLine("Pathfinder requires physical access to both the Website and DataFolder directories to deploy packages.");
            Console.WriteLine();

            var projectName = "Sitecore";
            Guid guid;
            if (!Guid.TryParse(_projectUniqueId, out guid))
            {
                projectName = _projectUniqueId;
            }

            var defaultWebsiteDirectory = context.Configuration.GetString(Constants.Configuration.NewProject.DefaultWebsiteDirectory).TrimEnd('\\');
            if (string.IsNullOrEmpty(defaultWebsiteDirectory))
            {
                var wwwrootDirectory = context.Configuration.GetString(Constants.Configuration.NewProject.WwwrootDirectory, "c:\\inetpub\\wwwroot").TrimEnd('\\');
                defaultWebsiteDirectory = $"{wwwrootDirectory}\\{projectName}\\Website";
            }

            do
            {
                var website = Console.ReadLine($"Enter the directory of the Website [{defaultWebsiteDirectory}]: ", defaultWebsiteDirectory, "website");
                _websiteDirectory = PathHelper.Combine(defaultWebsiteDirectory, website);
            }
            while (!ValidateWebsiteDirectory(context));

            Console.WriteLine();

            var defaultDataFolderDirectory = context.Configuration.GetString(Constants.Configuration.NewProject.DefaultDataFolderDirectory).TrimEnd('\\');
            if (string.IsNullOrEmpty(defaultDataFolderDirectory))
            {
                defaultDataFolderDirectory = Path.Combine(Path.GetDirectoryName(_websiteDirectory) ?? string.Empty, "Data");
            }

            do
            {
                _dataFolderDirectory = Console.ReadLine("Enter the directory of the DataFolder [" + defaultDataFolderDirectory + "]: ", defaultDataFolderDirectory, "datafolder");
            }
            while (!ValidateDataFolderDirectory(context));

            Console.WriteLine();
            Console.WriteLine("Finally Pathfinder requires the hostname of the Sitecore website.");
            Console.WriteLine();

            var defaultHostName = context.Configuration.GetString(Constants.Configuration.NewProject.DefaultHostName);
            if (string.IsNullOrEmpty(defaultHostName))
            {
                defaultHostName = $"http://{projectName.ToLowerInvariant()}";
            }

            _hostName = Console.ReadLine($"Enter the hostname of the website [{defaultHostName}]: ", defaultHostName, "host");
            if (!_hostName.Contains(Uri.SchemeDelimiter))
            {
                _hostName = Uri.UriSchemeHttp + Uri.SchemeDelimiter + _hostName.TrimStart('/');
            }

            Console.WriteLine();
            if (Console.YesNo("Do you want to install an editor configuration [Y]: ", true) == true)
            {
                var editorsDirectory = Path.Combine(context.ToolsDirectory, "files\\editors");
                var editors = Directory.GetFiles(editorsDirectory, "*.zip", SearchOption.AllDirectories).ToDictionary(Path.GetFileNameWithoutExtension, e => e);

                _editorFileName = Console.Pick("Select editor [1]: ", editors, "editor");
            }

            Console.WriteLine();
            if (Console.YesNo("Do you want to install a task runner [N]: ", false) == true)
            {
                var taskRunnerDirectory = Path.Combine(context.ToolsDirectory, "files\\taskrunners");
                var taskRunners = Directory.GetFiles(taskRunnerDirectory, "*.zip", SearchOption.AllDirectories).ToDictionary(Path.GetFileNameWithoutExtension, e => e);

                _taskRunnerFileName = Console.Pick("Select task runner [1]: ", taskRunners, "taskrunner");
            }

            Console.WriteLine();
            if (Console.YesNo("Do you want to install a starter kit [Y]: ", true) == true)
            {
                var starterKitDirectory = Path.Combine(context.ToolsDirectory, "files\\starterkits");
                var starterKits = Directory.GetFiles(starterKitDirectory, "*.zip", SearchOption.AllDirectories).ToDictionary(Path.GetFileNameWithoutExtension, e => e);

                _starterKitFileName = Console.Pick("Select starter kit [1]: ", starterKits, "starterkit");
            }

            Console.WriteLine();
            Console.WriteLine("Creating project...");

            CopyProjectTemplate(context, projectDirectory);
            CopyStarterKit(context, projectDirectory);
            CopyEditor(context, projectDirectory);
            CopyTaskRunner(context, projectDirectory);
            UpdateConfigFile(context, projectDirectory);
        }

        protected virtual void CopyEditor([NotNull] IBuildContext context, [NotNull] string projectDirectory)
        {
            if (!string.IsNullOrEmpty(_editorFileName))
            {
                FileSystem.Unzip(_editorFileName, projectDirectory);
            }
        }

        protected virtual void CopyProjectTemplate([NotNull] IBuildContext context, [NotNull] string projectDirectory)
        {
            var sourceDirectory = Path.Combine(context.ToolsDirectory, "files\\project\\*");
            FileSystem.XCopy(sourceDirectory, projectDirectory);
        }

        protected virtual void CopyStarterKit([NotNull] IBuildContext context, [NotNull] string projectDirectory)
        {
            if (!string.IsNullOrEmpty(_starterKitFileName))
            {
                FileSystem.Unzip(_starterKitFileName, projectDirectory);
            }
        }

        protected virtual void CopyTaskRunner([NotNull] IBuildContext context, [NotNull] string projectDirectory)
        {
            if (!string.IsNullOrEmpty(_taskRunnerFileName))
            {
                FileSystem.Unzip(_taskRunnerFileName, projectDirectory);
            }
        }

        protected virtual void UpdateConfigFile([NotNull] IBuildContext context, [NotNull] string projectDirectory)
        {
            var projectConfigFileName = Path.Combine(projectDirectory, context.Configuration.GetString(Constants.Configuration.ProjectConfigFileName));
            var config = FileSystem.ReadAllText(projectConfigFileName);

            config = config.Replace("{project-unique-id}", _projectUniqueId);
            config = config.Replace("c:\\\\inetpub\\\\wwwroot\\\\Sitecore.Default\\\\Website", _websiteDirectory.Replace("\\", "\\\\"));
            config = config.Replace("c:\\\\inetpub\\\\wwwroot\\\\Sitecore.Default\\\\Data", _dataFolderDirectory.Replace("\\", "\\\\"));
            config = config.Replace("http://sitecore.default", _hostName);

            FileSystem.WriteAllText(projectConfigFileName, config);
        }

        protected virtual bool ValidateDataFolderDirectory([NotNull] IBuildContext context)
        {
            var kernelFileName = Path.Combine(_dataFolderDirectory, "indexes");
            if (!FileSystem.DirectoryExists(kernelFileName))
            {
                Console.WriteLine("This does not appear to be a valid Sitecore data folder as /indexes does not exist.");
                return Console.YesNo("Do you want to continue anyway? [N]", false) == true;
            }

            return true;
        }

        protected virtual bool ValidateWebsiteDirectory([NotNull] IBuildContext context)
        {
            var kernelFileName = Path.Combine(_websiteDirectory, "bin\\Sitecore.Kernel.dll");
            if (!FileSystem.FileExists(kernelFileName))
            {
                Console.WriteLine("This does not appear to be a valid Sitecore website as /bin/Sitecore.Kernel.dll does not exist.");
                return Console.YesNo("Do you want to continue anyway? [N]", false) == true;
            }

            return true;
        }
    }
}
