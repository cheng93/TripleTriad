using System.Collections.Generic;
using System.Collections.ObjectModel;
using TripleTriad.Logic.Entities;
using TripleTriad.Logic.Enums;

namespace TripleTriad.Logic.Cards
{
    public static class AllCards
    {
        #region level1
        public static Card Geezard = new Card("Geezard", 1, new Rank(1, 4, 5, 1));
        public static Card Funguar = new Card("Funguar", 1, new Rank(5, 1, 1, 3));
        public static Card BiteBug = new Card("Bite Bug", 1, new Rank(1, 3, 3, 5));
        public static Card RedBat = new Card("Red Bat", 1, new Rank(6, 1, 1, 2));
        public static Card Blobra = new Card("Blobra", 1, new Rank(2, 3, 1, 5));
        public static Card Gayla = new Card("Gayla", 1, new Rank(2, 1, 4, 4), Element.Lightning);
        public static Card Gesper = new Card("Gesper", 1, new Rank(1, 5, 4, 1));
        public static Card FastitocalonF = new Card("Fastitocalon-F", 1, new Rank(3, 5, 2, 1), Element.Earth);
        public static Card BloodSoul = new Card("Blood Soul", 1, new Rank(2, 1, 6, 1));
        public static Card Caterchipillar = new Card("Caterchipillar", 1, new Rank(4, 2, 4, 3));
        public static Card Cockatrice = new Card("Cockatrice", 1, new Rank(2, 1, 2, 6), Element.Lightning);
        #endregion

        #region level2
        public static Card Grat = new Card("Grat", 2, new Rank(7, 1, 3, 1));
        public static Card Buel = new Card("Buel", 2, new Rank(6, 2, 2, 3));
        public static Card Mesmerize = new Card("Mesmerize", 2, new Rank(5, 3, 3, 4));
        public static Card GlacialEye = new Card("Glacial Eye", 2, new Rank(6, 1, 4, 3), Element.Ice);
        public static Card Belhelmel = new Card("Belhelmel", 2, new Rank(3, 4, 5, 3));
        public static Card Thrustaevis = new Card("Thrustaevis", 2, new Rank(5, 3, 2, 5), Element.Wind);
        public static Card Anacondaur = new Card("Anacondaur", 2, new Rank(5, 1, 3, 5), Element.Poison);
        public static Card Creeps = new Card("Creeps", 2, new Rank(5, 2, 5, 2), Element.Lightning);
        public static Card Grendel = new Card("Grendel", 2, new Rank(4, 4, 5, 2), Element.Lightning);
        public static Card Jelleye = new Card("Jelleye", 2, new Rank(3, 2, 1, 7));
        public static Card GrandMantis = new Card("Grand Mantis", 2, new Rank(5, 2, 5, 3));
        #endregion

        #region level3
        public static Card Forbidden = new Card("Forbidden", 3, new Rank(6, 6, 3, 2));
        public static Card Armadodo = new Card("Armadodo", 3, new Rank(6, 3, 1, 6), Element.Earth);
        public static Card TriFace = new Card("Tri-Face", 3, new Rank(3, 5, 5, 5), Element.Poison);
        public static Card Fastitocalon = new Card("Fastitocalon", 3, new Rank(7, 5, 1, 3), Element.Earth);
        public static Card SnowLion = new Card("Snow Lion", 3, new Rank(7, 1, 5, 3), Element.Ice);
        public static Card Ochu = new Card("Ochu", 3, new Rank(5, 6, 3, 3));
        public static Card Sam08G = new Card("SAM08G", 3, new Rank(5, 6, 2, 4), Element.Fire);
        public static Card DeathClaw = new Card("Death Claw", 3, new Rank(4, 4, 7, 2), Element.Fire);
        public static Card Cactuar = new Card("Cactuar", 3, new Rank(6, 2, 6, 3));
        public static Card Tonberry = new Card("Tonberry", 3, new Rank(3, 6, 4, 4));
        public static Card AbyssWorm = new Card("Abyss Worm", 3, new Rank(7, 2, 3, 5), Element.Earth);
        #endregion

        #region level4
        public static Card Turtapod = new Card("Turtapod", 4, new Rank(2, 3, 6, 7));
        public static Card Vysage = new Card("Vysage", 4, new Rank(6, 5, 4, 5));
        public static Card TRexaur = new Card("T-Rexaur", 4, new Rank(4, 6, 2, 7));
        public static Card Bomb = new Card("Bomb", 4, new Rank(2, 7, 6, 3), Element.Fire);
        public static Card Blitz = new Card("Blitz", 4, new Rank(1, 6, 4, 7), Element.Lightning);
        public static Card Wendigo = new Card("Wendigo", 4, new Rank(7, 3, 1, 6));
        public static Card Torama = new Card("Torama", 4, new Rank(7, 4, 4, 4));
        public static Card Imp = new Card("Imp", 4, new Rank(3, 7, 3, 6));
        public static Card BlueDragon = new Card("Blue Dragon", 4, new Rank(6, 2, 7, 3), Element.Poison);
        public static Card Adamantoise = new Card("Adamantoise", 4, new Rank(4, 5, 5, 6), Element.Earth);
        public static Card Hexadragon = new Card("Hexadragon", 4, new Rank(7, 5, 4, 3), Element.Fire);
        #endregion

        #region level5
        public static Card IronGiant = new Card("Iron Giant", 5, new Rank(6, 5, 6, 5));
        public static Card Behemoth = new Card("Behemoth", 5, new Rank(3, 6, 5, 7));
        public static Card Chimera = new Card("Chimera", 5, new Rank(7, 6, 5, 3), Element.Water);
        public static Card PuPu = new Card("PuPu", 5, new Rank(3, 10, 2, 1));
        public static Card Elastoid = new Card("Elastoid", 5, new Rank(6, 2, 6, 7));
        public static Card Gim47N = new Card("GIM47N", 5, new Rank(5, 5, 7, 4));
        public static Card Malboro = new Card("Malboro", 5, new Rank(7, 7, 4, 2), Element.Poison);
        public static Card RubyDragon = new Card("Ruby Dragon", 5, new Rank(7, 2, 7, 4), Element.Fire);
        public static Card Elnoyle = new Card("Elnoyle", 5, new Rank(5, 3, 7, 6));
        public static Card TonberryKing = new Card("Tonberry King", 5, new Rank(4, 6, 7, 4));
        public static Card BiggsWedge = new Card("Biggs, Wedge", 5, new Rank(6, 6, 2, 7));
        #endregion

        #region level6
        public static Card FujinRaijin = new Card("Rujin, Raijin", 6, new Rank(2, 8, 8, 4));
        public static Card Elvoret = new Card("Elvoret", 6, new Rank(7, 8, 3, 4), Element.Wind);
        public static Card XAtm092 = new Card("X-ATM092", 6, new Rank(4, 8, 7, 3));
        public static Card Granaldo = new Card("Granaldo", 6, new Rank(7, 2, 8, 5));
        public static Card Gerogero = new Card("Gerogero", 6, new Rank(1, 8, 8, 3), Element.Poison);
        public static Card Iguion = new Card("Iguion", 6, new Rank(8, 2, 8, 2));
        public static Card Abadon = new Card("Abadon", 6, new Rank(6, 8, 4, 5));
        public static Card Trauma = new Card("Trauma", 6, new Rank(4, 8, 5, 6));
        public static Card Oilboyle = new Card("Oilboyle", 6, new Rank(1, 8, 4, 8));
        public static Card ShumiTribe = new Card("Shumi Tribe", 6, new Rank(6, 5, 8, 4));
        public static Card Krysta = new Card("Krysta", 6, new Rank(7, 5, 8, 1));
        #endregion

        #region level7
        public static Card Propagator = new Card("Propagator", 7, new Rank(8, 4, 4, 8));
        public static Card JumboCactuar = new Card("Jumbo Cactuar", 7, new Rank(8, 8, 4, 4));
        public static Card TriPoint = new Card("Tri-Point", 7, new Rank(8, 5, 2, 8), Element.Lightning);
        public static Card Gargantua = new Card("Gargantua", 7, new Rank(5, 6, 6, 8));
        public static Card MobileType8 = new Card("Mobile Type 8", 7, new Rank(8, 6, 7, 3));
        public static Card Sphinxara = new Card("Sphinxara", 7, new Rank(8, 3, 5, 8));
        public static Card Tiamat = new Card("Tiamat", 7, new Rank(8, 8, 5, 4));
        public static Card Bgh251F2 = new Card("BGH251F2", 7, new Rank(5, 7, 8, 5));
        public static Card RedGiant = new Card("Red Giant", 7, new Rank(6, 8, 4, 7));
        public static Card Catoblepas = new Card("Catoblepas", 7, new Rank(1, 8, 7, 7));
        public static Card UltimaWeapon = new Card("Ultima Weapon", 7, new Rank(7, 7, 2, 8));
        #endregion

        #region level8
        public static Card ChubbyChocobo = new Card("Chubby Chocobo", 8, new Rank(4, 4, 8, 9));
        public static Card Angelo = new Card("Angelo", 8, new Rank(9, 6, 7, 3));
        public static Card Gilgamesh = new Card("Gilgamesh", 8, new Rank(3, 7, 9, 6));
        public static Card MiniMog = new Card("MiniMog", 8, new Rank(9, 3, 9, 2));
        public static Card Chicobo = new Card("Chicobo", 8, new Rank(9, 4, 8, 4));
        public static Card Quezacotl = new Card("Quezacotl", 8, new Rank(2, 9, 9, 4), Element.Lightning);
        public static Card Shiva = new Card("Shiva", 8, new Rank(6, 7, 4, 9), Element.Ice);
        public static Card Ifrit = new Card("Ifrit", 8, new Rank(9, 6, 2, 8), Element.Fire);
        public static Card Siren = new Card("Siren", 8, new Rank(8, 9, 6, 2));
        public static Card Sacred = new Card("Sacred", 8, new Rank(5, 1, 9, 9), Element.Earth);
        public static Card Minotaur = new Card("Minotaur", 8, new Rank(9, 5, 2, 9), Element.Earth);
        #endregion

        #region level9
        public static Card Carbuncle = new Card("Carbuncle", 9, new Rank(8, 4, 10, 4));
        public static Card Diablos = new Card("Diablos", 9, new Rank(5, 10, 8, 3));
        public static Card Leviathan = new Card("Leviathan", 9, new Rank(7, 10, 1, 7), Element.Water);
        public static Card Odin = new Card("Odin", 9, new Rank(8, 10, 3, 5));
        public static Card Pandemona = new Card("Pandemona", 9, new Rank(10, 1, 7, 7), Element.Wind);
        public static Card Cerberus = new Card("Cerberus", 9, new Rank(7, 4, 6, 10));
        public static Card Alexander = new Card("Alexander", 9, new Rank(9, 10, 4, 2), Element.Holy);
        public static Card Phoenix = new Card("Phoenix", 9, new Rank(7, 2, 7, 10), Element.Fire);
        public static Card Bahamut = new Card("Bahamut", 9, new Rank(10, 8, 2, 6));
        public static Card Doomtrain = new Card("Doomtrain", 9, new Rank(3, 1, 10, 10), Element.Poison);
        public static Card Eden = new Card("Eden", 9, new Rank(4, 4, 9, 10));
        #endregion

        #region level10
        public static Card Ward = new Card("Ward", 10, new Rank(10, 7, 2, 8));
        public static Card Kiros = new Card("Kiros", 10, new Rank(6, 7, 6, 10));
        public static Card Laguna = new Card("Laguna", 10, new Rank(5, 10, 3, 9));
        public static Card Selphie = new Card("Selphie", 10, new Rank(10, 8, 6, 4));
        public static Card Quistis = new Card("Quistis", 10, new Rank(9, 6, 10, 2));
        public static Card Irvine = new Card("Irvine", 10, new Rank(2, 6, 9, 10));
        public static Card Zell = new Card("Zell", 10, new Rank(8, 5, 10, 6));
        public static Card Rinoa = new Card("Rinoa", 10, new Rank(4, 10, 2, 10));
        public static Card Edea = new Card("Edea", 10, new Rank(10, 10, 3, 3));
        public static Card Seifer = new Card("Seifer", 10, new Rank(6, 9, 10, 4));
        public static Card Squall = new Card("Squall", 10, new Rank(10, 4, 6, 9));
        #endregion

        static AllCards()
        {
            var list = new List<Card>()
            {
                #region level 1
                Geezard,
                Funguar,
                BiteBug,
                RedBat,
                Blobra,
                Gayla,
                Gesper,
                FastitocalonF,
                BloodSoul,
                Caterchipillar,
                Cockatrice,
                #endregion
                #region level 2
                Grat,
                Buel,
                Mesmerize,
                GlacialEye,
                Belhelmel,
                Thrustaevis,
                Anacondaur,
                Creeps,
                Grendel,
                Jelleye,
                GrandMantis,
                #endregion
                #region level 3
                Forbidden,
                Armadodo,
                TriFace,
                Fastitocalon,
                SnowLion,
                Ochu,
                Sam08G,
                DeathClaw,
                Cactuar,
                Tonberry,
                AbyssWorm,
                #endregion
                #region level 4
                Turtapod,
                Vysage,
                TRexaur,
                Bomb,
                Blitz,
                Wendigo,
                Torama,
                Imp,
                BlueDragon,
                Adamantoise,
                Hexadragon,
                #endregion
                #region level 5
                IronGiant,
                Behemoth,
                Chimera,
                PuPu,
                Elastoid,
                Gim47N,
                Malboro,
                RubyDragon,
                Elnoyle,
                TonberryKing,
                BiggsWedge,
                #endregion
                #region level 6
                FujinRaijin,
                Elvoret,
                XAtm092,
                Granaldo,
                Gerogero,
                Iguion,
                Abadon,
                Trauma,
                Oilboyle,
                ShumiTribe,
                Krysta,
                #endregion
                #region level 7
                Propagator,
                JumboCactuar,
                TriPoint,
                Gargantua,
                MobileType8,
                Sphinxara,
                Tiamat,
                Bgh251F2,
                RedGiant,
                Catoblepas,
                UltimaWeapon,
                #endregion
                #region level 8
                ChubbyChocobo,
                Angelo,
                Gilgamesh,
                MiniMog,
                Chicobo,
                Quezacotl,
                Shiva,
                Ifrit,
                Siren,
                Sacred,
                Minotaur,
                #endregion
                #region level 9
                Carbuncle,
                Diablos,
                Leviathan,
                Odin,
                Pandemona,
                Cerberus,
                Alexander,
                Phoenix,
                Bahamut,
                Doomtrain,
                Eden,
                #endregion
                #region level 10
                Ward,
                Kiros,
                Laguna,
                Selphie,
                Quistis,
                Irvine,
                Zell,
                Rinoa,
                Edea,
                Seifer,
                Squall
                #endregion
            };

            List = new ReadOnlyCollection<Card>(list);
        }

        public static readonly IEnumerable<Card> List;
    }
}