﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var counrties = HRCountry.FromJson(jsonString);

namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    /// <summary>
    /// HR Country model
    /// </summary>
    public partial class HRCountry
    {
        /// <summary>
        /// MongoDB ID
        /// </summary>
        [JsonProperty("_id")]
        public MongoDB.Bson.ObjectId _id;

        /// <summary>
        /// Country name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// topLevelDomain
        /// </summary>
        [JsonProperty("topLevelDomain")]
        public string[] TopLevelDomain { get; set; }
        /// <summary>
        /// ISO code (2char)
        /// </summary>
        [JsonProperty("alpha2Code")]
        public string Alpha2Code { get; set; }
        /// <summary>
        /// Iso Code (3 char)
        /// </summary>
        [JsonProperty("alpha3Code")]
        public string Alpha3Code { get; set; }
        /// <summary>
        /// Prefix phne number.
        /// </summary>
        [JsonProperty("callingCodes")]
        public string[] CallingCodes { get; set; }
        /// <summary>
        /// Capital
        /// </summary>
        [JsonProperty("capital")]
        public string Capital { get; set; }
        /// <summary>
        /// altSpellings
        /// </summary>
        [JsonProperty("altSpellings")]
        public string[] AltSpellings { get; set; }
        /// <summary>
        /// Region (as continent)
        /// </summary>
        [JsonProperty("region")]
        public Region Region { get; set; }
        /// <summary>
        /// SubRegion
        /// </summary>
        [JsonProperty("subregion")]
        public string Subregion { get; set; }
        /// <summary>
        /// Number of inhabitant
        /// </summary>
        [JsonProperty("population")]
        public long Population { get; set; }
        /// <summary>
        /// Lat / Lon probably WGS 84
        /// </summary>
        [JsonProperty("latlng")]
        public double[] Latlng { get; set; }
        /// <summary>
        /// demonym
        /// </summary>
        [JsonProperty("demonym")]
        public string Demonym { get; set; }
        /// <summary>
        /// Area
        /// </summary>
        [JsonProperty("area")]
        public double? Area { get; set; }
        /// <summary>
        /// gini
        /// </summary>
        [JsonProperty("gini")]
        public double? Gini { get; set; }
        /// <summary>
        /// Lit of time zones
        /// </summary>
        [JsonProperty("timezones")]
        public string[] Timezones { get; set; }
        /// <summary>
        /// Countries in borders
        /// </summary>
        [JsonProperty("borders")]
        public string[] Borders { get; set; }
        /// <summary>
        /// Name in native langage
        /// </summary>
        [JsonProperty("nativeName")]
        public string NativeName { get; set; }
        /// <summary>
        /// numeric code
        /// </summary>
        [JsonProperty("numericCode")]
        public string NumericCode { get; set; }
        /// <summary>
        /// List of curencies available
        /// </summary>
        [JsonProperty("currencies")]
        public Currency[] Currencies { get; set; }
        /// <summary>
        /// List of langages spoken
        /// </summary>
        [JsonProperty("languages")]
        public Language[] Languages { get; set; }
        /// <summary>
        /// List of translated native name
        /// </summary>
        [JsonProperty("translations")]
        public Translations Translations { get; set; }
        /// <summary>
        /// Url of svg flag
        /// </summary>
        [JsonProperty("flag")]
        public Uri Flag { get; set; }
        /// <summary>
        /// List of regionalBlocs
        /// </summary>
        [JsonProperty("regionalBlocs")]
        public RegionalBloc[] RegionalBlocs { get; set; }
    }
    /// <summary>
    /// Class for currency
    /// </summary>
    public partial class Currency
    {
        /// <summary>
        /// Currency code
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// Currency name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Currency Symbol
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
    /// <summary>
    /// Language class.
    /// </summary>
    public partial class Language
    {
        /// <summary>
        /// Name of langage in iso639 1
        /// </summary>
        [JsonProperty("iso639_1")]
        public string Iso6391 { get; set; }
        /// <summary>
        /// Name of langage in iso639 2
        /// </summary>

        [JsonProperty("iso639_2")]
        public string Iso6392 { get; set; }
        /// <summary>
        /// Name of langage
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Native name of langage
        /// </summary>

        [JsonProperty("nativeName")]
        public string NativeName { get; set; }
    }
    /// <summary>
    /// Regional bloc class
    /// </summary>
    public partial class RegionalBloc
    {
        /// <summary>
        /// acronym
        /// </summary>
        [JsonProperty("acronym")]
        public Acronym Acronym { get; set; }
        /// <summary>
        /// name
        /// </summary>
        [JsonProperty("name")]
        public Name Name { get; set; }
        /// <summary>
        /// otherAcronyms
        /// </summary>
        [JsonProperty("otherAcronyms")]
        public OtherAcronym[] OtherAcronyms { get; set; }
        /// <summary>
        /// otherNames
        /// </summary>
        [JsonProperty("otherNames")]
        public OtherName[] OtherNames { get; set; }
    }
    /// <summary>
    /// Class for Translations
    /// </summary>
    public partial class Translations
    {
        /// <summary>
        /// Deutshland
        /// </summary>
        [JsonProperty("de")]
        public string De { get; set; }
        /// <summary>
        /// Spain
        /// </summary>
        [JsonProperty("es")]
        public string Es { get; set; }
        /// <summary>
        /// France
        /// </summary>
        [JsonProperty("fr")]
        public string Fr { get; set; }
        /// <summary>
        /// Japan
        /// </summary>
        [JsonProperty("ja")]
        public string Ja { get; set; }
        /// <summary>
        /// Italy
        /// </summary>
        [JsonProperty("it")]
        public string It { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("br")]
        public string Br { get; set; }
        /// <summary>
        /// Br
        /// </summary>
        [JsonProperty("pt")]
        public string Pt { get; set; }
        /// <summary>
        /// Nederlands
        /// </summary>
        [JsonProperty("nl")]
        public string Nl { get; set; }
        /// <summary>
        /// hr, promise it's not me :-)
        /// </summary>
        [JsonProperty("hr")]
        public string Hr { get; set; }
    }
    /// <summary>
    /// Region enum
    /// </summary>
    public enum Region {
        /// <summary>
        /// Africa
        /// </summary>
        Africa,
        /// <summary>
        /// South, central and north america
        /// </summary>
        Americas,
        /// <summary>
        /// Asia
        /// </summary>
        Asia,
        /// <summary>
        /// Empty (does not mean All)
        /// </summary>
        Empty,
        /// <summary>
        /// Europe
        /// </summary>
        Europe,
        /// <summary>
        /// Oceania
        /// </summary>
        Oceania,
        /// <summary>
        /// Polar
        /// </summary>
        Polar,
        /// <summary>
        /// All regions including Empty.
        /// </summary>
        All
    };
    /// <summary>
    /// Acronym enum
    /// </summary>
    public enum Acronym {
        /// <summary>
        /// African League
        /// </summary>
        Al,
        /// <summary>
        /// Association Of South East Asian Nations
        /// </summary>
        Asean,
        /// <summary>
        /// African Union
        /// </summary>
        Au,
        /// <summary>
        /// Central American Integration System
        /// </summary>
        Cais,
        /// <summary>
        /// Caribena Community
        /// </summary>
        Caricom,
        /// <summary>
        /// Central European Free Trade Agreement
        /// </summary>
        Cefta,
        /// <summary>
        /// Eurasian Economic Union
        /// </summary>
        Eeu,
        /// <summary>
        /// European Free Trade Association
        /// </summary>
        Efta,
        /// <summary>
        /// European Union
        /// </summary>
        Eu,
        /// <summary>
        /// North American Free Trade Agreement
        /// </summary>
        Nafta,
        /// <summary>
        /// Pacific Alliance
        /// </summary>
        Pa,
        /// <summary>
        /// South Asian Association For Regional Cooperation
        /// </summary>
        Saarc,
        /// <summary>
        /// Union Of South American Nations
        /// </summary>
        Usan
    };
    /// <summary>
    /// Name enum
    /// </summary>
    public enum Name {
        /// <summary>
        /// AfricanUnion
        /// </summary>
        AfricanUnion,
        /// <summary>
        /// ArabLeague
        /// </summary>
        ArabLeague,
        /// <summary>
        /// AssociationOfSoutheastAsianNations
        /// </summary>
        AssociationOfSoutheastAsianNations,
        /// <summary>
        /// CaribbeanCommunity
        /// </summary>
        CaribbeanCommunity,
        /// <summary>
        /// CentralAmericanIntegrationSystem
        /// </summary>
        CentralAmericanIntegrationSystem,
        /// <summary>
        /// CentralEuropeanFreeTradeAgreement
        /// </summary>
        CentralEuropeanFreeTradeAgreement,
        /// <summary>
        /// EurasianEconomicUnion
        /// </summary>
        EurasianEconomicUnion,
        /// <summary>
        /// EuropeanFreeTradeAssociation
        /// </summary>
        EuropeanFreeTradeAssociation,
        /// <summary>
        /// EuropeanUnion
        /// </summary>
        EuropeanUnion,
        /// <summary>
        /// NorthAmericanFreeTradeAgreement
        /// </summary>
        NorthAmericanFreeTradeAgreement,
        /// <summary>
        /// PacificAlliance
        /// </summary>
        PacificAlliance,
        /// <summary>
        /// SouthAsianAssociationForRegionalCooperation
        /// </summary>
        SouthAsianAssociationForRegionalCooperation,
        /// <summary>
        /// UnionOfSouthAmericanNations
        /// </summary>
        UnionOfSouthAmericanNations
    };
    /// <summary>
    /// Other acronym enum
    /// </summary>
    public enum OtherAcronym {
        /// <summary>
        /// Eurasian Economic Union
        /// </summary>
        Eaeu,
        /// <summary>
        /// Unknown acronym.
        /// </summary>
        Sica,
        /// <summary>
        /// Union of South American Nations
        /// </summary>
        Unasul,
        /// <summary>
        /// Union of South American Nations
        /// </summary>
        Unasur,
        /// <summary>
        /// Unknown acronym.
        /// </summary>
        Uzan
    };
    /// <summary>
    /// Othername enum
    /// </summary>
    public enum OtherName {
        /// <summary>
        /// AccordDeLibreÉchangeNordAméricain
        /// </summary>
        AccordDeLibreÉchangeNordAméricain,
        /// <summary>
        /// AlianzaDelPacífico
        /// </summary>
        AlianzaDelPacífico,
        /// <summary>
        /// CaribischeGemeenschap
        /// </summary>
        CaribischeGemeenschap,
        /// <summary>
        /// CommunautéCaribéenne
        /// </summary>
        CommunautéCaribéenne,
        /// <summary>
        /// ComunidadDelCaribe
        /// </summary>
        ComunidadDelCaribe,
        /// <summary>
        /// JāmiAtAdDuwalAlArabīyah
        /// </summary>
        JāmiAtAdDuwalAlArabīyah,
        /// <summary>
        /// LeagueOfArabStates
        /// </summary>
        LeagueOfArabStates,
        /// <summary>
        /// SistemaDeLaIntegraciónCentroamericana
        /// </summary>
        SistemaDeLaIntegraciónCentroamericana,
        /// <summary>
        /// SouthAmericanUnion
        /// </summary>
        SouthAmericanUnion,
        /// <summary>
        /// TratadoDeLibreComercioDeAméricaDelNorte
        /// </summary>
        TratadoDeLibreComercioDeAméricaDelNorte,
        /// <summary>
        /// UmojaWaAfrika
        /// </summary>
        UmojaWaAfrika,
        /// <summary>
        /// UnieVanZuidAmerikaanseNaties
        /// </summary>
        UnieVanZuidAmerikaanseNaties,
        /// <summary>
        /// UnionAfricaine
        /// </summary>
        UnionAfricaine,
        /// <summary>
        /// UniãoAfricana
        /// </summary>
        UniãoAfricana,
        /// <summary>
        /// UniãoDeNaçõesSulAmericanas
        /// </summary>
        UniãoDeNaçõesSulAmericanas,
        /// <summary>
        /// UniónAfricana
        /// </summary>
        UniónAfricana,
        /// <summary>
        /// UniónDeNacionesSuramericanas
        /// </summary>
        UniónDeNacionesSuramericanas,
        /// <summary>
        /// الاتحادالأفريقي
        /// </summary>
        الاتحادالأفريقي,
        /// <summary>
        /// جامعةالدولالعربية
        /// </summary>
        جامعةالدولالعربية
    };
    /// <summary>
    /// Class HRCountry
    /// </summary>
    public partial class HRCountry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static HRCountry[] FromJson(string json) => JsonConvert.DeserializeObject<HRCountry[]>(json, QuickType.Converter.Settings);
    }
    /// <summary>
    /// Class serialze
    /// </summary>
    public static class Serialize
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ToJson(this HRCountry[] self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                RegionConverter.Singleton,
                AcronymConverter.Singleton,
                NameConverter.Singleton,
                OtherAcronymConverter.Singleton,
                OtherNameConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class RegionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Region) || t == typeof(Region?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Region.Empty;
                case "Africa":
                    return Region.Africa;
                case "Americas":
                    return Region.Americas;
                case "Asia":
                    return Region.Asia;
                case "Europe":
                    return Region.Europe;
                case "Oceania":
                    return Region.Oceania;
                case "Polar":
                    return Region.Polar;
            }
            throw new Exception("Cannot unmarshal type Region");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Region)untypedValue;
            switch (value)
            {
                case Region.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Region.Africa:
                    serializer.Serialize(writer, "Africa");
                    return;
                case Region.Americas:
                    serializer.Serialize(writer, "Americas");
                    return;
                case Region.Asia:
                    serializer.Serialize(writer, "Asia");
                    return;
                case Region.Europe:
                    serializer.Serialize(writer, "Europe");
                    return;
                case Region.Oceania:
                    serializer.Serialize(writer, "Oceania");
                    return;
                case Region.Polar:
                    serializer.Serialize(writer, "Polar");
                    return;
            }
            throw new Exception("Cannot marshal type Region");
        }

        public static readonly RegionConverter Singleton = new RegionConverter();
    }

    internal class AcronymConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Acronym) || t == typeof(Acronym?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "AL":
                    return Acronym.Al;
                case "ASEAN":
                    return Acronym.Asean;
                case "AU":
                    return Acronym.Au;
                case "CAIS":
                    return Acronym.Cais;
                case "CARICOM":
                    return Acronym.Caricom;
                case "CEFTA":
                    return Acronym.Cefta;
                case "EEU":
                    return Acronym.Eeu;
                case "EFTA":
                    return Acronym.Efta;
                case "EU":
                    return Acronym.Eu;
                case "NAFTA":
                    return Acronym.Nafta;
                case "PA":
                    return Acronym.Pa;
                case "SAARC":
                    return Acronym.Saarc;
                case "USAN":
                    return Acronym.Usan;
            }
            throw new Exception("Cannot unmarshal type Acronym");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Acronym)untypedValue;
            switch (value)
            {
                case Acronym.Al:
                    serializer.Serialize(writer, "AL");
                    return;
                case Acronym.Asean:
                    serializer.Serialize(writer, "ASEAN");
                    return;
                case Acronym.Au:
                    serializer.Serialize(writer, "AU");
                    return;
                case Acronym.Cais:
                    serializer.Serialize(writer, "CAIS");
                    return;
                case Acronym.Caricom:
                    serializer.Serialize(writer, "CARICOM");
                    return;
                case Acronym.Cefta:
                    serializer.Serialize(writer, "CEFTA");
                    return;
                case Acronym.Eeu:
                    serializer.Serialize(writer, "EEU");
                    return;
                case Acronym.Efta:
                    serializer.Serialize(writer, "EFTA");
                    return;
                case Acronym.Eu:
                    serializer.Serialize(writer, "EU");
                    return;
                case Acronym.Nafta:
                    serializer.Serialize(writer, "NAFTA");
                    return;
                case Acronym.Pa:
                    serializer.Serialize(writer, "PA");
                    return;
                case Acronym.Saarc:
                    serializer.Serialize(writer, "SAARC");
                    return;
                case Acronym.Usan:
                    serializer.Serialize(writer, "USAN");
                    return;
            }
            throw new Exception("Cannot marshal type Acronym");
        }

        public static readonly AcronymConverter Singleton = new AcronymConverter();
    }

    internal class NameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Name) || t == typeof(Name?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "African Union":
                    return Name.AfricanUnion;
                case "Arab League":
                    return Name.ArabLeague;
                case "Association of Southeast Asian Nations":
                    return Name.AssociationOfSoutheastAsianNations;
                case "Caribbean Community":
                    return Name.CaribbeanCommunity;
                case "Central American Integration System":
                    return Name.CentralAmericanIntegrationSystem;
                case "Central European Free Trade Agreement":
                    return Name.CentralEuropeanFreeTradeAgreement;
                case "Eurasian Economic Union":
                    return Name.EurasianEconomicUnion;
                case "European Free Trade Association":
                    return Name.EuropeanFreeTradeAssociation;
                case "European Union":
                    return Name.EuropeanUnion;
                case "North American Free Trade Agreement":
                    return Name.NorthAmericanFreeTradeAgreement;
                case "Pacific Alliance":
                    return Name.PacificAlliance;
                case "South Asian Association for Regional Cooperation":
                    return Name.SouthAsianAssociationForRegionalCooperation;
                case "Union of South American Nations":
                    return Name.UnionOfSouthAmericanNations;
            }
            throw new Exception("Cannot unmarshal type Name");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Name)untypedValue;
            switch (value)
            {
                case Name.AfricanUnion:
                    serializer.Serialize(writer, "African Union");
                    return;
                case Name.ArabLeague:
                    serializer.Serialize(writer, "Arab League");
                    return;
                case Name.AssociationOfSoutheastAsianNations:
                    serializer.Serialize(writer, "Association of Southeast Asian Nations");
                    return;
                case Name.CaribbeanCommunity:
                    serializer.Serialize(writer, "Caribbean Community");
                    return;
                case Name.CentralAmericanIntegrationSystem:
                    serializer.Serialize(writer, "Central American Integration System");
                    return;
                case Name.CentralEuropeanFreeTradeAgreement:
                    serializer.Serialize(writer, "Central European Free Trade Agreement");
                    return;
                case Name.EurasianEconomicUnion:
                    serializer.Serialize(writer, "Eurasian Economic Union");
                    return;
                case Name.EuropeanFreeTradeAssociation:
                    serializer.Serialize(writer, "European Free Trade Association");
                    return;
                case Name.EuropeanUnion:
                    serializer.Serialize(writer, "European Union");
                    return;
                case Name.NorthAmericanFreeTradeAgreement:
                    serializer.Serialize(writer, "North American Free Trade Agreement");
                    return;
                case Name.PacificAlliance:
                    serializer.Serialize(writer, "Pacific Alliance");
                    return;
                case Name.SouthAsianAssociationForRegionalCooperation:
                    serializer.Serialize(writer, "South Asian Association for Regional Cooperation");
                    return;
                case Name.UnionOfSouthAmericanNations:
                    serializer.Serialize(writer, "Union of South American Nations");
                    return;
            }
            throw new Exception("Cannot marshal type Name");
        }

        public static readonly NameConverter Singleton = new NameConverter();
    }

    internal class OtherAcronymConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(OtherAcronym) || t == typeof(OtherAcronym?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "EAEU":
                    return OtherAcronym.Eaeu;
                case "SICA":
                    return OtherAcronym.Sica;
                case "UNASUL":
                    return OtherAcronym.Unasul;
                case "UNASUR":
                    return OtherAcronym.Unasur;
                case "UZAN":
                    return OtherAcronym.Uzan;
            }
            throw new Exception("Cannot unmarshal type OtherAcronym");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (OtherAcronym)untypedValue;
            switch (value)
            {
                case OtherAcronym.Eaeu:
                    serializer.Serialize(writer, "EAEU");
                    return;
                case OtherAcronym.Sica:
                    serializer.Serialize(writer, "SICA");
                    return;
                case OtherAcronym.Unasul:
                    serializer.Serialize(writer, "UNASUL");
                    return;
                case OtherAcronym.Unasur:
                    serializer.Serialize(writer, "UNASUR");
                    return;
                case OtherAcronym.Uzan:
                    serializer.Serialize(writer, "UZAN");
                    return;
            }
            throw new Exception("Cannot marshal type OtherAcronym");
        }

        public static readonly OtherAcronymConverter Singleton = new OtherAcronymConverter();
    }

    internal class OtherNameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(OtherName) || t == typeof(OtherName?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Accord de Libre-échange Nord-Américain":
                    return OtherName.AccordDeLibreÉchangeNordAméricain;
                case "Alianza del Pacífico":
                    return OtherName.AlianzaDelPacífico;
                case "Caribische Gemeenschap":
                    return OtherName.CaribischeGemeenschap;
                case "Communauté Caribéenne":
                    return OtherName.CommunautéCaribéenne;
                case "Comunidad del Caribe":
                    return OtherName.ComunidadDelCaribe;
                case "Jāmiʻat ad-Duwal al-ʻArabīyah":
                    return OtherName.JāmiAtAdDuwalAlArabīyah;
                case "League of Arab States":
                    return OtherName.LeagueOfArabStates;
                case "Sistema de la Integración Centroamericana,":
                    return OtherName.SistemaDeLaIntegraciónCentroamericana;
                case "South American Union":
                    return OtherName.SouthAmericanUnion;
                case "Tratado de Libre Comercio de América del Norte":
                    return OtherName.TratadoDeLibreComercioDeAméricaDelNorte;
                case "Umoja wa Afrika":
                    return OtherName.UmojaWaAfrika;
                case "Unie van Zuid-Amerikaanse Naties":
                    return OtherName.UnieVanZuidAmerikaanseNaties;
                case "Union africaine":
                    return OtherName.UnionAfricaine;
                case "União Africana":
                    return OtherName.UniãoAfricana;
                case "União de Nações Sul-Americanas":
                    return OtherName.UniãoDeNaçõesSulAmericanas;
                case "Unión Africana":
                    return OtherName.UniónAfricana;
                case "Unión de Naciones Suramericanas":
                    return OtherName.UniónDeNacionesSuramericanas;
                case "الاتحاد الأفريقي":
                    return OtherName.الاتحادالأفريقي;
                case "جامعة الدول العربية":
                    return OtherName.جامعةالدولالعربية;
            }
            throw new Exception("Cannot unmarshal type OtherName");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (OtherName)untypedValue;
            switch (value)
            {
                case OtherName.AccordDeLibreÉchangeNordAméricain:
                    serializer.Serialize(writer, "Accord de Libre-échange Nord-Américain");
                    return;
                case OtherName.AlianzaDelPacífico:
                    serializer.Serialize(writer, "Alianza del Pacífico");
                    return;
                case OtherName.CaribischeGemeenschap:
                    serializer.Serialize(writer, "Caribische Gemeenschap");
                    return;
                case OtherName.CommunautéCaribéenne:
                    serializer.Serialize(writer, "Communauté Caribéenne");
                    return;
                case OtherName.ComunidadDelCaribe:
                    serializer.Serialize(writer, "Comunidad del Caribe");
                    return;
                case OtherName.JāmiAtAdDuwalAlArabīyah:
                    serializer.Serialize(writer, "Jāmiʻat ad-Duwal al-ʻArabīyah");
                    return;
                case OtherName.LeagueOfArabStates:
                    serializer.Serialize(writer, "League of Arab States");
                    return;
                case OtherName.SistemaDeLaIntegraciónCentroamericana:
                    serializer.Serialize(writer, "Sistema de la Integración Centroamericana,");
                    return;
                case OtherName.SouthAmericanUnion:
                    serializer.Serialize(writer, "South American Union");
                    return;
                case OtherName.TratadoDeLibreComercioDeAméricaDelNorte:
                    serializer.Serialize(writer, "Tratado de Libre Comercio de América del Norte");
                    return;
                case OtherName.UmojaWaAfrika:
                    serializer.Serialize(writer, "Umoja wa Afrika");
                    return;
                case OtherName.UnieVanZuidAmerikaanseNaties:
                    serializer.Serialize(writer, "Unie van Zuid-Amerikaanse Naties");
                    return;
                case OtherName.UnionAfricaine:
                    serializer.Serialize(writer, "Union africaine");
                    return;
                case OtherName.UniãoAfricana:
                    serializer.Serialize(writer, "União Africana");
                    return;
                case OtherName.UniãoDeNaçõesSulAmericanas:
                    serializer.Serialize(writer, "União de Nações Sul-Americanas");
                    return;
                case OtherName.UniónAfricana:
                    serializer.Serialize(writer, "Unión Africana");
                    return;
                case OtherName.UniónDeNacionesSuramericanas:
                    serializer.Serialize(writer, "Unión de Naciones Suramericanas");
                    return;
                case OtherName.الاتحادالأفريقي:
                    serializer.Serialize(writer, "الاتحاد الأفريقي");
                    return;
                case OtherName.جامعةالدولالعربية:
                    serializer.Serialize(writer, "جامعة الدول العربية");
                    return;
            }
            throw new Exception("Cannot marshal type OtherName");
        }

        public static readonly OtherNameConverter Singleton = new OtherNameConverter();
    }
}
