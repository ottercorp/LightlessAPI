namespace LightlessSync.API.Data.Enum;

public enum Region
{
    NorthAmerica,
    Europe,
    Japan,
    Oceania,
    China,
}

public enum Datacenter
{
    Aether,
    Crystal,
    Primal,
    Dynamis,
    Chaos,
    Light,
    Elemental,
    Gaia,
    Mana,
    Meteor,
    Materia,
    陆行鸟,
    莫古力,
    猫小胖,
    豆豆柴,
}

public enum World
{
    // North America - Aether
    Adamantoise = 73,
    Cactuar = 79,
    Faerie = 54,
    Gilgamesh = 63,
    Jenova = 40,
    Midgardsormr = 65,
    Sargatanas = 99,
    Siren = 57,

    // North America - Crystal
    Balmung = 91,
    Brynhildr = 34,
    Coeurl = 74,
    Diabolos = 62,
    Goblin = 81,
    Malboro = 75,
    Mateus = 37,
    Zalera = 41,

    // North America - Primal
    Behemoth = 78,
    Excalibur = 93,
    Exodus = 53,
    Famfrit = 35,
    Hyperion = 95,
    Lamia = 55,
    Leviathan = 64,
    Ultros = 77,

    // North America - Dynamis
    Cuchulainn = 408,
    Golem = 411,
    Halicarnassus = 406,
    Kraken = 409,
    Maduin = 407,
    Marilith = 404,
    Rafflesia = 410,
    Seraph = 405,

    // Europe - Chaos
    Cerberus = 80,
    Louisoix = 83,
    Moogle = 71,
    Omega = 39,
    Phantom = 401,
    Ragnarok = 97,
    Sagittarius = 400,
    Spriggan = 85,

    // Europe - Light
    Alpha = 402,
    Lich = 36,
    Odin = 66,
    Phoenix = 56,
    Raiden = 403,
    Shiva = 67,
    Twintania = 33,
    Zodiark = 42,

    // Japan - Elemental
    Aegis = 90,
    Atomos = 68,
    Carbuncle = 45,
    Garuda = 58,
    Gungnir = 94,
    Kujata = 49,
    Tonberry = 72,
    Typhon = 50,

    // Japan - Gaia
    Alexander = 43,
    Bahamut = 69,
    Durandal = 92,
    Fenrir = 46,
    Ifrit = 59,
    Ridill = 98,
    Tiamat = 76,
    Ultima = 51,

    // Japan - Mana
    Anima = 44,
    Asura = 23,
    Chocobo = 70,
    Hades = 47,
    Ixion = 48,
    Masamune = 96,
    Pandaemonium = 28,
    Titan = 61,

    // Japan - Meteor
    Belias = 24,
    Mandragora = 82,
    Ramuh = 60,
    Shinryu = 29,
    Unicorn = 30,
    Valefor = 52,
    Yojimbo = 31,
    Zeromus = 32,

    // Oceania - Materia
    Bismarck = 22,
    Ravana = 21,
    Sephirot = 86,
    Sophia = 87,
    Zurvan = 88,
    
    // China - LuXingNiao
    拉诺西亚 = 1042,
    幻影群岛 = 1044,
    神意之地 = 1081,
    萌芽池 = 1060,
    红玉海 = 1167,
    宇宙和音 = 1173,
    沃仙曦染 = 1174,
    晨曦王座 = 1175,
    
    
    // China - MoGuLi
    潮风亭 = 1170,
    神拳痕 = 1171,
    白银乡 = 1172,
    白金幻象 = 1076,
    旅人栈桥 = 1113,
    拂晓之间 = 1121,
    龙巢神殿 = 1166,
    梦羽宝境 = 1176,
    
    // China - MaoXiaoPang
    紫水栈桥 = 1043,
    延夏 = 1169,
    静语庄园 = 1106,
    摩杜纳 = 1045,
    海猫茶屋 = 1177,
    柔风海湾 = 1178,
    琥珀原 = 1179,
    
    // China - DouDouChai
    水晶塔 = 1192,
    银泪湖 = 1183,
    太阳海岸 = 1180,
    伊修加德 = 1186,
    红茶川 = 1201,
}

public static class FfxivTravelMap
{
    public static readonly IReadOnlyDictionary<Region, IReadOnlyList<Datacenter>> DatacentersByRegion =
        new Dictionary<Region, IReadOnlyList<Datacenter>>
        {
            [Region.NorthAmerica] = new[] { Datacenter.Aether, Datacenter.Crystal, Datacenter.Primal, Datacenter.Dynamis },
            [Region.Europe] = new[] { Datacenter.Chaos, Datacenter.Light },
            [Region.Japan] = new[] { Datacenter.Elemental, Datacenter.Gaia, Datacenter.Mana, Datacenter.Meteor },
            [Region.Oceania] = new[] { Datacenter.Materia },
            [Region.China] = new[] { Datacenter.陆行鸟, Datacenter.莫古力, Datacenter.猫小胖, Datacenter.豆豆柴  },
        };

    public static readonly IReadOnlyDictionary<Datacenter, IReadOnlyList<World>> WorldsByDatacenter =
        new Dictionary<Datacenter, IReadOnlyList<World>>
        {
            [Datacenter.Aether] = new[]
            {
                World.Adamantoise, World.Cactuar, World.Faerie, World.Gilgamesh,
                World.Jenova, World.Midgardsormr, World.Sargatanas, World.Siren,
            },
            [Datacenter.Crystal] = new[]
            {
                World.Balmung, World.Brynhildr, World.Coeurl, World.Diabolos,
                World.Goblin, World.Malboro, World.Mateus, World.Zalera,
            },
            [Datacenter.Primal] = new[]
            {
                World.Behemoth, World.Excalibur, World.Exodus, World.Famfrit,
                World.Hyperion, World.Lamia, World.Leviathan, World.Ultros,
            },
            [Datacenter.Dynamis] = new[]
            {
                World.Cuchulainn, World.Golem, World.Halicarnassus, World.Kraken,
                World.Maduin, World.Marilith, World.Rafflesia, World.Seraph,
            },
            [Datacenter.Chaos] = new[]
            {
                World.Cerberus, World.Louisoix, World.Moogle, World.Omega,
                World.Phantom, World.Ragnarok, World.Sagittarius, World.Spriggan,
            },
            [Datacenter.Light] = new[]
            {
                World.Alpha, World.Lich, World.Odin, World.Phoenix,
                World.Raiden, World.Shiva, World.Twintania, World.Zodiark,
            },
            [Datacenter.Elemental] = new[]
            {
                World.Aegis, World.Atomos, World.Carbuncle, World.Garuda,
                World.Gungnir, World.Kujata, World.Tonberry, World.Typhon,
            },
            [Datacenter.Gaia] = new[]
            {
                World.Alexander, World.Bahamut, World.Durandal, World.Fenrir,
                World.Ifrit, World.Ridill, World.Tiamat, World.Ultima,
            },
            [Datacenter.Mana] = new[]
            {
                World.Anima, World.Asura, World.Chocobo, World.Hades,
                World.Ixion, World.Masamune, World.Pandaemonium, World.Titan,
            },
            [Datacenter.Meteor] = new[]
            {
                World.Belias, World.Mandragora, World.Ramuh, World.Shinryu,
                World.Unicorn, World.Valefor, World.Yojimbo, World.Zeromus,
            },
            [Datacenter.Materia] = new[]
            {
                World.Bismarck, World.Ravana, World.Sephirot, World.Sophia, World.Zurvan,
            },
            [Datacenter.陆行鸟] = new[]
            {
                World.拉诺西亚, World.幻影群岛, World.神意之地, World.萌芽池, 
                World.红玉海, World.宇宙和音, World.沃仙曦染, World.晨曦王座,
            },
            [Datacenter.莫古力] = new[]
            {
                World.潮风亭, World.神拳痕, World.白银乡, World.白金幻象,
                World.旅人栈桥, World.拂晓之间, World.龙巢神殿, World.梦羽宝境,
            },
            [Datacenter.猫小胖] = new[]
            {
                World.紫水栈桥, World.延夏, World.静语庄园, World.摩杜纳,
                World.海猫茶屋, World.柔风海湾, World.琥珀原,
            },
            [Datacenter.豆豆柴] = new[]
            {
                World.水晶塔, World.银泪湖, World.太阳海岸, World.伊修加德, World.红茶川,
            },
        };

    public static readonly IReadOnlyDictionary<World, Region> RegionByWorld;
    public static readonly IReadOnlyDictionary<Region, IReadOnlyList<World>> WorldsByRegion;
    public static readonly IReadOnlyDictionary<Region, IReadOnlyList<World>> VisibleWorldsByRegion;

    static FfxivTravelMap()
    {
        var regionByWorld = new Dictionary<World, Region>();
        var worldsByRegion = new Dictionary<Region, IReadOnlyList<World>>();

        foreach (var (region, datacenters) in DatacentersByRegion)
        {
            var worlds = datacenters
                .SelectMany(dc => WorldsByDatacenter.TryGetValue(dc, out var list) ? list : [])
                .ToList();

            worldsByRegion[region] = worlds;
            foreach (var world in worlds)
                regionByWorld[world] = region;
        }

        RegionByWorld = regionByWorld;
        WorldsByRegion = worldsByRegion;

        VisibleWorldsByRegion = new Dictionary<Region, IReadOnlyList<World>>
        {
            [Region.NorthAmerica] = [.. worldsByRegion[Region.NorthAmerica], .. worldsByRegion[Region.Oceania]],
            [Region.Europe] = [.. worldsByRegion[Region.Europe], .. worldsByRegion[Region.Oceania]],
            [Region.Japan] = [.. worldsByRegion[Region.Japan], .. worldsByRegion[Region.Oceania]],
            [Region.Oceania] = worldsByRegion[Region.Oceania],
            [Region.China] = worldsByRegion[Region.China],
        };
    }

    public static IReadOnlyList<World> GetVisibleWorlds(World callerWorld)
    {
        return RegionByWorld.TryGetValue(callerWorld, out var region)
            && VisibleWorldsByRegion.TryGetValue(region, out var visible)
            ? visible
            : [];
    }
}
