﻿using HRCoreCountriesRepository;
using QuickType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace XUnitTestDAL
{
    public class MongoDBLanguageRepositoryTest
    {
        private Dictionary<String, Language> _dictionnary = new Dictionary<String, Language>();
        private List<HRCountry> _cursor = new List<HRCountry>();
        private MongoDBLanguageRepository _repo = new MongoDBLanguageRepository(null, null);
        private HRCountry _country = new HRCountry();

        /// <summary>
        /// Test normal case.
        /// </summary>
        [Fact]
        public void FillUpDictionnary_With_Two_Differents_Languages_Expect_Two_Different_Languages()
        {
            _country.Languages = new Language[2];
            _country.Languages[0] = new Language() { Iso6391 = "AA"};
            _country.Languages[1] = new Language() { Iso6391 = "BB" };
            _cursor.Add(_country);
            _repo.FillUpDictionnary(_dictionnary, _cursor);
            Assert.NotNull(_dictionnary.Values);
            Assert.True(_dictionnary.Values.Count == 2);
            Assert.True(_dictionnary.Values.ToList<Language>()[0].Iso6391 == "AA");
            Assert.True(_dictionnary.Values.ToList<Language>()[1].Iso6391 == "BB");

        }
        /// <summary>
        /// Test that two languages different with case sensitive only (on is06391) are considered as one.
        /// </summary>
        [Fact]
        public void FillUpDictionnary_With_Two_Differents_Languages_On_Case_Sensitive_Expect_Two_Languages()
        {
            _country.Languages = new Language[2];
            _country.Languages[0] = new Language() { Iso6391 = "AA" };
            _country.Languages[1] = new Language() { Iso6391 = "aa" };
            _cursor.Add(_country);
            _repo.FillUpDictionnary(_dictionnary, _cursor);
            Assert.NotNull(_dictionnary.Values);
            Assert.True(_dictionnary.Values.Count == 2);
            Assert.True(_dictionnary.Values.ToList<Language>()[0].Iso6391 == "AA");
            Assert.True(_dictionnary.Values.ToList<Language>()[1].Iso6391 == "aa");
        }
        /// <summary>
        /// TODO
        /// </summary>
        [Fact]
        public void FillUpDictionnary_With_No_Language_Expect_No_Language()
        {
            _country.Languages = null;
            _cursor.Add(_country);
            _repo.FillUpDictionnary(_dictionnary, _cursor);
            Assert.NotNull(_dictionnary.Values);
            Assert.True(_dictionnary.Values.Count == 0);
        }
    }
}
